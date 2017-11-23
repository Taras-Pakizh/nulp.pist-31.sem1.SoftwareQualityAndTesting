using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class ClientForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox firstNameField;
        private readonly TextBox lastNameField;
        private readonly TextBox phoneField;
        private readonly TextBox addressField;
        private readonly TextBox registrationDateField;

        public ClientForm(ListBox list, TextBox firstNameField, TextBox lastNameField, TextBox phoneField, TextBox addressField, TextBox registrationDateField)
        {
            this.list = list;
            this.firstNameField = firstNameField;
            this.lastNameField = lastNameField;
            this.phoneField = phoneField;
            this.addressField = addressField;
            this.registrationDateField = registrationDateField;
            Checker = new ClientChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var client in db.clients)
                    {
                        list.Items.Add($"{client.person1.first_name} {client.person1.last_name}");
                    }
                }
                if (list.Items.Count > 0) list.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void SelectedItem()
        {
            try {
                using (var db = new course_work_Entities())
                {
                    var index = list.SelectedIndex;
                    var client = (from c in db.clients select c).ToArray()[index];
                    firstNameField.Text = client.person1.first_name;
                    lastNameField.Text = client.person1.last_name;
                    phoneField.Text = client.person1.phone;
                    addressField.Text = client.person1.address;
                    registrationDateField.Text = client.registration_date.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            firstNameField.ReadOnly = disable;
            lastNameField.ReadOnly = disable;
            phoneField.ReadOnly = disable;
            addressField.ReadOnly = disable;
            registrationDateField.ReadOnly = true;
        }

        public void DefaultControlsValue()
        {
            firstNameField.Text = "";
            lastNameField.Text = "";
            phoneField.Text = "";
            addressField.Text = "";
            registrationDateField.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void NewItem()
        {
        }

        public void DeleteItem()
        {
            try {
                using (var db = new course_work_Entities())
                {
                    var index = list.SelectedIndex;
                    var clients = (from c in db.clients select c).ToArray();
                    var client = clients[index];
                    db.people.Remove(client.person1);
                    db.clients.Remove(client);
                    db.SaveChanges();
                    Init();
                    if (list.Items.Count != 0) list.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void EditItem()
        {
        }

        public void SaveNewItem()
        {
            try {
                using (var db = new course_work_Entities())
                {
                    var client = new client()
                    {
                        person1 = new person()
                        {
                            first_name = firstNameField.Text,
                            last_name = lastNameField.Text,
                            phone = phoneField.Text,
                            address = addressField.Text
                        },
                        registration_date = DateTime.Now
                    };
                    db.clients.Add(client);
                    db.SaveChanges();
                    Init();
                }
                list.SelectedIndex = list.Items.Count - 1;
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void SaveItem()
        {
            try {
                var index = list.SelectedIndex;
                using (var db = new course_work_Entities())
                {
                    var client = (from c in db.clients select c).ToArray()[index];
                    client.person1.first_name = firstNameField.Text;
                    client.person1.last_name = lastNameField.Text;
                    client.person1.phone = phoneField.Text;
                    client.person1.address = addressField.Text;
                    client.registration_date = DateTime.Now;
                    db.SaveChanges();
                    Init();
                }
                list.SelectedIndex = index;
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void Cancel()
        {
        }
    }
}
