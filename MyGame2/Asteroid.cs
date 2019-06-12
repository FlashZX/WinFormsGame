using System;
using System.Drawing;

namespace MyGame2
{
    class Asteroid : BaseObject, IDisposable
    {
        Bitmap image = new Bitmap("ast.png");

        public int Power { get; set; }

        Random rnd = new Random();
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 2;
        }
        /// <summary>
        /// Выводит спрайт астеройдов
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y);
        }
        //Метод Update мы можем переопределить или воспользоваться реализацией базового класса.
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
        public void Dispose()
        {
            image.Dispose();
        }
    }
}
