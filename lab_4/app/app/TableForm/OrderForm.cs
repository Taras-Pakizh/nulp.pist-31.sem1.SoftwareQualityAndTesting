using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class OrderForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly ComboBox clientField;
        private readonly ComboBox employeeField;
        private readonly ComboBox pcField;
        private readonly TextBox creationDateField;
        private readonly CheckBox isCompleteField;

        public OrderForm(ListBox list, ComboBox clientField, ComboBox employeeField, ComboBox pcField, TextBox creationDateField, CheckBox isCompleteField)
        {
            this.list = list;
            this.clientField = clientField;
            this.employeeField = employeeField;
            this.pcField = pcField;
            this.creationDateField = creationDateField;
            this.isCompleteField = isCompleteField;
            Checker = new OrderChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var order in db.ordrs)
                    {
                        var check = order.is_completed != 0 ? "x" : " ";
                        list.Items.Add($"[{check}] {order.client1.person1.first_name} {order.client1.person1.last_name} ({order.creation_date.ToString("yyyy-MM-dd")})");
                    }
                    clientField.Items.Clear();
                    foreach (var client in db.clients)
                    {
                        clientField.Items.Add($"{client.person1.first_name} {client.person1.last_name}");
                    }
                    employeeField.Items.Clear();
                    foreach (var employee in db.employees)
                    {
                        employeeField.Items.Add($"{employee.person1.first_name} {employee.person1.last_name}");
                    }
                    pcField.Items.Clear();
                    foreach (var pc in db.pcs)
                    {
                        pcField.Items.Add($"{pc.name}");
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
                    var order = (from o in db.ordrs select o).ToArray()[index];
                    clientField.Text = $"{order.client1.person1.first_name} {order.client1.person1.last_name}";
                    employeeField.Text = $"{order.employee1.person1.first_name} {order.employee1.person1.last_name}";
                    pcField.Text = order.pc1.name;
                    creationDateField.Text = order.creation_date.ToString("yyyy-MM-dd");
                    isCompleteField.Checked = order.is_completed != 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            clientField.Enabled = !disable;
            employeeField.Enabled = !disable;
            pcField.Enabled = !disable;
            creationDateField.ReadOnly = true;
            isCompleteField.Enabled = !disable;
        }

        public void DefaultControlsValue()
        {
            if (clientField.Items.Count != 0) clientField.SelectedIndex = 0;
            if (employeeField.Items.Count != 0) employeeField.SelectedIndex = 0;
            if (pcField.Items.Count != 0) pcField.SelectedIndex = 0;
            creationDateField.Text = DateTime.Now.ToString("yyyy-MM-dd");
            isCompleteField.Checked = false;
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
                    var orders = (from o in db.ordrs select o).ToArray();
                    var order = orders[index];
                    db.ordrs.Remove(order);
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
                    var order = new ordr()
                    {
                        client1 = (from c in db.clients where (c.person1.first_name + " " + c.person1.last_name).Equals(clientField.Text) select c).First(),
                        employee1 = (from e in db.employees where (e.person1.first_name + " " + e.person1.last_name).Equals(employeeField.Text) select e).First(),
                        pc1 = (from p in db.pcs where p.name.Equals(pcField.Text) select p).First(),
                        creation_date = DateTime.Now,
                        is_completed = isCompleteField.Checked ? 1 : 0
                    };
                    db.ordrs.Add(order);
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
                    var orders = (from o in db.ordrs select o).ToArray();
                    var order = orders[index];
                    order.client1 = (from c in db.clients where (c.person1.first_name + " " + c.person1.last_name).Equals(clientField.Text) select c).First();
                    order.employee1 = (from e in db.employees where (e.person1.first_name + " " + e.person1.last_name).Equals(employeeField.Text) select e).First();
                    order.pc1 = (from p in db.pcs where p.name.Equals(pcField.Text) select p).First();
                    order.creation_date = DateTime.Now;
                    order.is_completed = isCompleteField.Checked ? 1 : 0;
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
