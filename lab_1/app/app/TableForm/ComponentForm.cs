using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class ComponentForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox nameField;
        private readonly TextBox descriptionField;
        private readonly ComboBox categoryField;
        private readonly ComboBox producerField;
        private readonly TextBox priceField;

        public ComponentForm(ListBox list, TextBox nameField, TextBox descriptionField, ComboBox categoryField, ComboBox producerField, TextBox priceField)
        {
            this.list = list;
            this.nameField = nameField;
            this.descriptionField = descriptionField;
            this.categoryField = categoryField;
            this.producerField = producerField;
            this.priceField = priceField;
            Checker = new ComponentChecker();
        }

        public void Init()
        {
            try
            {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var component in db.components)
                    {
                        list.Items.Add($"{component.name} ({component.description})");
                    }
                    categoryField.Items.Clear();
                    foreach (var category in db.categories)
                    {
                        categoryField.Items.Add($"{category.name}");
                    }
                    producerField.Items.Clear();
                    foreach (var producer in db.producers)
                    {
                        producerField.Items.Add($"{producer.name}");
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
                    var component = (from c in db.components select c).ToArray()[index];
                    nameField.Text = component.name;
                    descriptionField.Text = component.description;
                    categoryField.Text = component.category1.name;
                    producerField.Text = component.producer1.name;
                    priceField.Text = component.price.ToString();
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
            categoryField.Enabled = !disable;
            producerField.Enabled = !disable;
            priceField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            nameField.Text = "";
            descriptionField.Text = "";
            if (categoryField.Items.Count != 0) categoryField.SelectedIndex = 0;
            if (producerField.Items.Count != 0) producerField.SelectedIndex = 0;
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
                    var components = (from c in db.components select c).ToArray();
                    var component = components[index];
                    var deleteFlag = true;
                    foreach (var c in components)
                    {
                        if (!c.Equals(component) && c.producer1.name.Equals(component.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.producers.Remove(component.producer1);
                    }
                    db.components.Remove(component);
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
                    var s = from p in db.producers where p.name.Equals(producerField.Text) select p;
                    producer producer;
                    if (!s.Any())
                    {
                        producer = new producer()
                        {
                            name = producerField.Text,
                        };
                        db.producers.Add(producer);
                        db.SaveChanges();
                    }
                    else
                    {
                        producer = s.First();
                    }
                    var component = new component()
                    {
                        name = nameField.Text,
                        description = descriptionField.Text,
                        category1 = (from c in db.categories where c.name.Equals(categoryField.Text) select c).First(),
                        producer1 = producer,
                        price = decimal.Parse(priceField.Text)
                    };
                    db.components.Add(component);
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
            try {
                var index = list.SelectedIndex;
                using (var db = new course_work_Entities())
                {
                    var s = from p in db.producers where p.name.Equals(producerField.Text) select p;
                    producer producer;
                    if (!s.Any())
                    {
                        producer = new producer()
                        {
                            name = producerField.Text,
                        };
                        db.producers.Add(producer);
                        db.SaveChanges();
                    }
                    else
                    {
                        producer = s.First();
                    }
                    var components = (from c in db.components select c).ToArray();
                    var component = components[index];
                    var deleteFlag = true;
                    foreach (var c in components)
                    {
                        if (!c.Equals(component) && c.producer1.name.Equals(component.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    var pcs = (from p in db.pcs select p).ToArray();
                    foreach (var p in pcs)
                    {
                        if (p.producer1.name.Equals(component.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.producers.Remove(component.producer1);
                    }
                    component.name = nameField.Text;
                    component.description = descriptionField.Text;
                    component.category1 = (from c in db.categories where c.name.Equals(categoryField.Text) select c).First();
                    component.producer1 = producer;
                    component.price = decimal.Parse(priceField.Text);
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
