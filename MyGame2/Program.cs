using System;
using System.Windows.Forms;

namespace MyGame2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Width = 1000; form.Height = 600;

            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}