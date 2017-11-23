using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class EmployeeForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox firstNameField;
        private readonly TextBox secondNameField;
        private readonly TextBox phoneField;
        private readonly TextBox addressField;
        private readonly ComboBox positionField;
        private readonly ComboBox firmField;
        private readonly TextBox experienceField;

        public EmployeeForm(ListBox list, TextBox firstNameField, TextBox secondNameField, TextBox phoneField, TextBox addressField, ComboBox positionField, ComboBox firmField, TextBox experienceField)
        {
            this.list = list;
            this.firstNameField = firstNameField;
            this.secondNameField = secondNameField;
            this.phoneField = phoneField;
            this.addressField = addressField;
            this.positionField = positionField;
            this.firmField = firmField;
            this.experienceField = experienceField;
            Checker = new EmployeeChecker();
        }

        public void Init()
        {
            try {
            list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var employee in db.employees)
                    {
                        list.Items.Add($"{employee.person1.first_name} {employee.person1.last_name}");
                    }
                    positionField.Items.Clear();
                    foreach (var position in db.positions)
                    {
                        positionField.Items.Add($"{position.name}");
                    }
                    firmField.Items.Clear();
                    foreach (var firm in db.firms)
                    {
                        firmField.Items.Add($"{firm.address}");
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
                    var employee = (from e in db.employees select e).ToArray()[index];
                    firstNameField.Text = employee.person1.first_name;
                    secondNameField.Text = employee.person1.last_name;
                    phoneField.Text = employee.person1.phone;
                    addressField.Text = employee.person1.address;
                    positionField.Text = employee.position1.name;
                    firmField.Text = employee.firm1.address;
                    experienceField.Text = employee.experience.ToString();
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
            secondNameField.ReadOnly = disable;
            phoneField.ReadOnly = disable;
            addressField.ReadOnly = disable;
            positionField.Enabled = !disable;
            firmField.Enabled = !disable;
            experienceField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            firstNameField.Text = "";
            secondNameField.Text = "";
            phoneField.Text = "";
            addressField.Text = "";
            if (positionField.Items.Count != 0) positionField.SelectedIndex = 0;
            if (firmField.Items.Count != 0) firmField.SelectedIndex = 0;
            experienceField.Text = "";
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
                    var employees = (from e in db.employees select e).ToArray();
                    var employee = employees[index];
                    var deleteFlag = true;
                    foreach (var e in employees)
                    {
                        if (!e.Equals(employees) && e.position1.name.Equals(employee.position1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.positions.Remove(employee.position1);
                    }
                    db.people.Remove(employee.person1);
                    db.employees.Remove(employee);
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
                    var s = from p in db.positions where p.name.Equals(positionField.Text) select p;
                    position position;
                    if (!s.Any())
                    {
                        position = new position()
                        {
                            name = positionField.Text,
                        };
                        db.positions.Add(position);
                        db.SaveChanges();
                    }
                    else
                    {
                        position = s.First();
                    }
                    var employee = new employee ()
                    {
                        person1 = new person()
                        {
                            first_name = firstNameField.Text,
                            last_name = secondNameField.Text,
                            phone = phoneField.Text,
                            address = addressField.Text
                        },
                        position1 = position,
                        firm1 = (from f in db.firms where f.address.Equals(firmField.Text) select f).First(),
                        experience = int.Parse(experienceField.Text)
                    };
                    db.employees.Add(employee);
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
                    var s = from p in db.positions where p.name.Equals(positionField.Text) select p;
                    position position;
                    if (!s.Any())
                    {
                        position = new position()
                        {
                            name = positionField.Text,
                        };
                        db.positions.Add(position);
                        db.SaveChanges();
                    }
                    else
                    {
                        position = s.First();
                    }
                    var employees = (from e in db.employees select e).ToArray();
                    var employee = employees[index];
                    var deleteFlag = true;
                    foreach (var e in employees)
                    {
                        if (!e.Equals(employee) && e.position1.name.Equals(employee.position1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.positions.Remove(employee.position1);
                    }
                    employee.person1.first_name = firstNameField.Text;
                    employee.person1.last_name = secondNameField.Text;
                    employee.person1.phone = phoneField.Text;
                    employee.person1.address = addressField.Text;
                    employee.position1 = position;
                    employee.firm1 = (from f in db.firms where f.address.Equals(firmField.Text) select f).First();
                    employee.experience = int.Parse(experienceField.Text);
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
