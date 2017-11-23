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
    public partial class LoginForm : Form
    {

        private readonly ProgramData _programData = ProgramData.Instance;

        public LoginForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            userComboBox.Items.AddRange(User.Users());
            userComboBox.SelectedIndex = 0;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var userName = userComboBox.SelectedItem.ToString();
            var userPassword = passwordTextBox.Text;
            var user = User.LoginUser(userName, userPassword);
            _programData.CurrentUser = user;
            if (user == User.NullUser)
            {
                MessageBox.Show("Невірний логін або пароль", "Помилка");
            }
            else
            {
                ProgramData.Instance.CurrentUser = user;
                this.Hide();
                var form = new MainForm();
                form.Closed += (s, args) => this.Close();
                form.Show();
            }
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.Show();
            var error = (null as Checker.Checker).IsVisible(null);
        }
    }
}
