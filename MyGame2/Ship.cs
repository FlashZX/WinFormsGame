using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MyGame2
{
    class Ship : BaseObject
    {
        private int _energy = 100;
        public int Energy => _energy;
        public static event Message MessageDie;
        public static event Message MessageWin;
        Bitmap image = new Bitmap("ship.png");

        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        public void EnergyHi(int n)
        {
            _energy += n;
            if (_energy > 100) _energy = 100;
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        public void Left()
        {
            if (Pos.X > 0) Pos.X = Pos.X - Dir.X;
        }
        public void Right()
        {
            if (Pos.X < Game.Width) Pos.X = Pos.X + Dir.X;
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }
        public void Win()
        {
            MessageWin?.Invoke();
        }

    }
}
