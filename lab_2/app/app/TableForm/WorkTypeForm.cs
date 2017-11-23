using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class WorkTypeForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox nameField;
        private readonly TextBox priceField;

        public WorkTypeForm(ListBox list, TextBox nameField, TextBox priceField)
        {
            this.list = list;
            this.nameField = nameField;
            this.priceField = priceField;
            Checker = new WorkTypeChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var work in db.work_type)
                    {
                        list.Items.Add($"{work.name}");
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
                    var work = (from w in db.work_type select w).ToArray()[index];
                    nameField.Text = work.name;
                    priceField.Text = work.price.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            nameField.ReadOnly = disable;
            priceField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            nameField.Text = "";
            priceField.Text = "0";
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
                    var work = (from w in db.work_type select w).ToArray()[index];
                    db.work_type.Remove(work);
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
            try
            {
                var price = decimal.Parse(priceField.Text);
                using (var db = new course_work_Entities())
                {
                    var work = new work_type()
                    {
                        name = nameField.Text,
                        price = price
                    };
                    db.work_type.Add(work);
                    db.SaveChanges();
                    Init();
                }
                list.SelectedIndex = list.Items.Count - 1;
            }
            catch (FormatException)
            {
                MessageBox.Show("Очікується число", "Помилка введення");
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
}

        public void SaveItem()
        {
            try
            {
                var index = list.SelectedIndex;
                using (var db = new course_work_Entities())
                {
                    var work = (from w in db.work_type select w).ToArray()[index];
                    work.name = nameField.Text;
                    work.price = decimal.Parse(priceField.Text);
                    db.SaveChanges();
                    Init();
                }
                list.SelectedIndex = index;
            }
            catch (FormatException)
            {
                MessageBox.Show("Очікується число", "Помилка введення");
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
