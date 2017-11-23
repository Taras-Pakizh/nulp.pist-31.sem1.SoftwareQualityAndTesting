using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class FirmForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly ComboBox cityField;
        private readonly TextBox addressField;
        private readonly TextBox phoneField;

        public FirmForm(ListBox list, ComboBox cityField, TextBox addressField, TextBox phoneField)
        {
            this.list = list;
            this.cityField = cityField;
            this.addressField = addressField;
            this.phoneField = phoneField;
            Checker = new FirmChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var firm in db.firms)
                    {
                        list.Items.Add($"{firm.address}");
                    }
                    cityField.Items.Clear();
                    foreach (var city in db.cities)
                    {
                        cityField.Items.Add($"{city.name}");
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
                    var firm = (from c in db.firms select c).ToArray()[index];
                    cityField.Text = firm.city1.name;
                    addressField.Text = firm.address;
                    phoneField.Text = firm.phone;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            cityField.Enabled = !disable;
            addressField.ReadOnly = disable;
            phoneField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            if (cityField.Items.Count != 0) cityField.SelectedIndex = 0;
            addressField.Text = "";
            phoneField.Text = "";
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
                    var firms = (from c in db.firms select c).ToArray();
                    var firm = firms[index];
                    var deleteFlag = true;
                    foreach (var f in firms)
                    {
                        if (!f.Equals(firm) && f.city1.name.Equals(firm.city1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.cities.Remove(firm.city1);
                    }
                    db.firms.Remove(firm);
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
                    var s = from f in db.cities where f.name.Equals(cityField.Text) select f;
                    city city;
                    if (!s.Any())
                    {
                        city = new city()
                        {
                            name = cityField.Text,
                        };
                        db.cities.Add(city);
                        db.SaveChanges();
                    }
                    else
                    {
                        city = s.First();
                    }
                    var firm = new firm()
                    {
                        city1 = city,
                        address = addressField.Text,
                        phone = phoneField.Text
                    };
                    db.firms.Add(firm);
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
                    var s = from f in db.cities where f.name.Equals(cityField.Text) select f;
                    city city;
                    if (!s.Any())
                    {
                        city = new city()
                        {
                            name = cityField.Text,
                        };
                        db.cities.Add(city);
                        db.SaveChanges();
                    }
                    else
                    {
                        city = s.First();
                    }
                    var firms = (from f in db.firms select f).ToArray();
                    var firm = firms[index];
                    var deleteFlag = true;
                    foreach (var f in firms)
                    {
                        if (!f.Equals(firm) && f.city1.name.Equals(firm.city1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.cities.Remove(firm.city1);
                    }
                    firm.city1 = city;
                    firm.address = addressField.Text;
                    firm.phone = phoneField.Text;
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
