using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class CategoryForm : TableForm
    {

        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox nameField;
        private readonly TextBox descriptionField;

        public CategoryForm(ListBox list, TextBox nameField, TextBox descriptionField)
        {
            this.list = list;
            this.nameField = nameField;
            this.descriptionField = descriptionField;
            Checker = new CategoryChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var category in db.categories)
                    {
                        list.Items.Add($"{category.name} ({category.description})");
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
                    var category = (from c in db.categories select c).ToArray()[index];
                    nameField.Text = category.name;
                    descriptionField.Text = category.description;
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
            descriptionField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            nameField.Text = "";
            descriptionField.Text = "";
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
                    var category = (from c in db.categories select c).ToArray()[index];
                    db.categories.Remove(category);
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
                    var category = new category()
                    {
                        name = nameField.Text,
                        description = descriptionField.Text
                    };
                    db.categories.Add(category);
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
                    var category = (from c in db.categories select c).ToArray()[index];
                    category.name = nameField.Text;
                    category.description = descriptionField.Text;
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
