using System;
using System.Drawing;

namespace MyGame2
{
    /// <summary>
    /// Описывает пули
    /// </summary>
    class Bullet : BaseObject, IDisposable
    {
        Bitmap image = new Bitmap("rocket.png");

        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(image, Pos.X, Pos.Y);
        }
        public override void Update()
        {
            Pos.X = Pos.X + 8;
        }
        public void Dispose()
        {
            image.Dispose();
        }
    }
}
