using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame2
{
    /// <summary>
    /// Класс меню, обрабатывает первую форму
    /// </summary>
    public partial class Menu : Form
    {
        //private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        

        public Menu()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e) => this.Close();

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Width = 800; form.Height = 600;

            Game.Init(form);
            Game.Draw();
            form.Show();
        }
    }
}
