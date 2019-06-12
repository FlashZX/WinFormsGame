using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MyGame2
{
    /// <summary>
    /// Описывает аптечки
    /// </summary>
    class AidKit : BaseObject
    {
        Bitmap image = new Bitmap("powup.png");

        public int Power { get; set; }
        public AidKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        /// <summary>
        /// Выводит спрайт аптечек
        /// </summary>
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

    }
}
