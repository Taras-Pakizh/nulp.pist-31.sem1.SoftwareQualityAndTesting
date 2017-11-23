using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            var text = "Програма для сервісу компонування комп'ютера\n" +
                       "------------------------------------------------------------------------------------------------------\n" +
                       "\n" +
                       "Автори:\n" +
                       "\tДубень Богдан студент НУ ЛП групи ПІст - 21\n" +
                       "\tМикуланинець Іван студент НУ ЛП групи ПІст - 21\n" +
                       "\n" +
                       "Контактні дані:\n" +
                       "\temail: feodott @gmail.com\n" +
                       "\n" +
                       "Версія програми: 1.0.1\n" +
                       "\n" +
                       "------------------------------------------------------------------------------------------------------\n";
            InitializeComponent();
            label1.Text = text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
