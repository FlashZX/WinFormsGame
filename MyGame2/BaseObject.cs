//Насыров Игорь, Пермь. C#_2 ДЗ № - 2.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MyGame2
{
    /// <summary>
    /// Класс для создания основных обьектов, выводимых в игре.
    /// </summary>
    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        public delegate void Message();

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        /// <summary>
        /// Абстрактный метод. Переопределяется в наследниках
        /// </summary>
        public abstract void Draw();
        /// <summary>
        /// Абстрактный метод. Переопределяется в наследниках
        /// </summary>
        public abstract void Update();

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
