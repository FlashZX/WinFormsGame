using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame2
{
    /// <summary>
    /// Основная механика игры
    /// </summary>
    class Game
    {
        public static Random rnd = new Random();
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        private static Timer _timer = new Timer { Interval = 50 };
        
        //Свойства Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        /// <summary>
        /// Инициализация обьектов текущей формы
        /// </summary>
        /// <param name="form"></param>
        public static void Init(System.Windows.Forms.Form form)
        {
            //Граф.устройсто для вывода графики
            Graphics g;
            //Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            // Создаем объект (поверхность рисования) и связываем его с формой
            g = form.CreateGraphics();
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Ship.MessageDie += Finish;
            Ship.MessageWin += Win;
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Load();
        }
        /// <summary>
        /// Методы выполняемые за один "тик" таймера
        /// </summary>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        //Создадим обьекты
        private static int astnum;
        private static BaseObject[] _objs;
        private static Ship _ship;
        private static AidKit[] _aid;
        private static List<Bullet> _bullets = new List<Bullet>();
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        /// <summary>
        /// Загружаем игровые обьекты
        /// </summary>
        public static void Load()
        {
            _objs = new BaseObject[30];
            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(26, 25));

            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50), r1 = rnd.Next(1, 5);
                _objs[i] = new Star(new Point(-r, rnd.Next(0, Height)), new
                Point(-r, r), new Size(rnd.Next(r1, r1), rnd.Next(r1, r1)));
            }
            for (var i = 0; i < rnd.Next(65,100); i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids.Add(new Asteroid(new Point(1400, rnd.Next(0, Height - 30)),
                new Point(-r / 5, r), new Size(30, 32)));
            }
            astnum = _asteroids.Count;
            _aid = new AidKit[rnd.Next(2, 5)];
            for (var i = 0; i < _aid.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _aid[i] = new AidKit(new Point(1500, rnd.Next(0, Height - 10)),
                new Point(-r / 5, r), new Size(16, 24));
            }
        }
        static int score = 0;
        /// <summary>
        /// Выводим игровые обьекты
        /// </summary>
        public static void Draw()
        {
            //Проверяем вывод графики
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
                a?.Draw();
            foreach (Bullet b in _bullets)
                b?.Draw();
            //Если здоровья меньше 85ти, даем аптечки
            if (_ship.Energy < 85)
            {
                foreach (AidKit a in _aid)
                    a?.Draw();
            }
            _ship?.Draw();
            Buffer.Graphics.DrawString("Energy: " + _ship.Energy + "\nAsteroids left: " 
                + astnum + "\nScore: " + score, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Render();
        }
        /// <summary>
        /// Обновляем состояния перечисленных в методе обьектов
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (Bullet b in _bullets) b?.Update();
            if (astnum == 0) _ship.Win();

            for (var n = 0; n < _aid.Length; n++)
            {
                if (_aid[n] == null) continue;
                _aid[n].Update();
                if (_ship != null && _ship.Collision(_aid[n]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _aid[n] = null;
                    if (_ship.Energy < 100)
                    {
                        var rnd = new Random();
                        _ship?.EnergyHi(rnd.Next(5, 15));
                        System.Media.SystemSounds.Exclamation.Play();
                    }
                    continue;
                }
                if (!_ship.Collision(_aid[n])) continue;
            }
            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                var rnd = new Random();
                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                         System.Media.SystemSounds.Hand.Play();

                        _bullets[j].Dispose();//удаляем ресурсы пули
                        _bullets.RemoveAt(j);
                        _asteroids[i] = null;
                        astnum--; score += rnd.Next(5, 15);
                        j--;
                    }
                }
                if (_asteroids[i] == null || !_ship.Collision(_asteroids[i])) continue;
                _ship.EnergyLow(rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.Energy <= 0) _ship.Die();
            }
            //Если пули вылетают за экран, подчищаем ресурсы
            for (var i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i].Rect.X >= Game.Width)
                {
                    _bullets[i].Dispose();
                    _bullets.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Обрабатываем нажатие клавиш
        /// </summary>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) _bullets.Add(new Bullet(new Point(_ship.Rect.X
                + 40, _ship.Rect.Y + 15), new Point(4, 0), new Size(4, 4)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
            if (e.KeyCode == Keys.Left) _ship.Left();
            if (e.KeyCode == Keys.Right) _ship.Right();
        } 
        /// <summary>
        /// Концовка если проиграли
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif,
            60), Brushes.White, Width / 4 + 25, Height / 3);
            Buffer.Render();
        }
        /// <summary>
        /// Концовка если выиграли
        /// </summary>
        public static void Win()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("You Win!", new Font(FontFamily.GenericSansSerif,
            60, FontStyle.Underline), Brushes.White, Width / 3, Height / 3);
            Buffer.Render();
        }
    }
}       