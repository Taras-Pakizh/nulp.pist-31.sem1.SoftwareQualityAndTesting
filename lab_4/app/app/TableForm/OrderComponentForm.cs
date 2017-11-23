using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class OrderComponentForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly ComboBox componentField;
        private readonly ComboBox orderField;
        private readonly TextBox countField;

        public OrderComponentForm(ListBox list, ComboBox componentField, ComboBox orderField, TextBox countField)
        {
            this.list = list;
            this.componentField = componentField;
            this.orderField = orderField;
            this.countField = countField;
            Checker = new OrderComponentChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var orderComponent in db.order_component)
                    {
                        list.Items.Add($"[{orderComponent.ordr.creation_date.ToString("yyyy-MM-dd")}] {orderComponent.component1.name}");
                    }
                    componentField.Items.Clear();
                    foreach (var component in db.components)
                    {
                        componentField.Items.Add($"{component.name}");
                    }
                    orderField.Items.Clear();
                    foreach (var order in db.ordrs)
                    {
                        orderField.Items.Add($"{order.client1.person1.first_name} {order.client1.person1.last_name} ({order.creation_date.ToString("yyyy-MM-dd")})");
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
                    var orderComponent = (from oc in db.order_component select oc).ToArray()[index];
                    componentField.Text = orderComponent.component1.name;
                    orderField.Text = $"{orderComponent.ordr.client1.person1.first_name} {orderComponent.ordr.client1.person1.last_name} ({orderComponent.ordr.creation_date.ToString("yyyy-MM-dd")})";
                    countField.Text = orderComponent.component_count.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            componentField.Enabled = !disable;
            orderField.Enabled = !disable;
            countField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            if (componentField.Items.Count != 0) componentField.SelectedIndex = 0;
            if (orderField.Items.Count != 0) orderField.SelectedIndex = 0;
            countField.Text = "1";
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
                    var orderComponents = (from oc in db.order_component select oc).ToArray();
                    var orderComponent = orderComponents[index];
                    db.order_component.Remove(orderComponent);
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
                using (var db = new course_work_Entities())
                {
                    var orderComponent = new order_component()
                    {
                        component1 = (from c in db.components where c.name.Equals(componentField.Text) select c).First(),
                        ordr = (from o in db.ordrs select o).ToArray()[orderField.SelectedIndex],
                        component_count = int.Parse(countField.Text)
                    };
                    db.order_component.Add(orderComponent);
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
                    var orderComponents = (from oc in db.order_component select oc).ToArray();
                    var orderComponent = orderComponents[index];
                    orderComponent.component1 = (from c in db.components where c.name.Equals(componentField.Text) select c).First();
                    orderComponent.ordr = (from o in db.ordrs select o).ToArray()[orderField.SelectedIndex];
                    orderComponent.component_count = int.Parse(countField.Text);
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
