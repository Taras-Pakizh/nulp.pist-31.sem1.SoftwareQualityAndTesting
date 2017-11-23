using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class ComponentInstallForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly ComboBox orderComponentField;
        private readonly ComboBox componentOnFirmField;
        private readonly ComboBox employeeField;
        private readonly ComboBox workTypeField;
        private readonly ComboBox statusField;
        private readonly DateTimePicker endWorkDateField;
        private readonly TextBox transmissionDateField;
        private readonly TextBox beginDateWarrantyField;
        private readonly TextBox durationWarrantyField;

        public ComponentInstallForm(ListBox list, ComboBox orderComponentField, ComboBox componentOnFirmField, ComboBox employeeField, ComboBox workTypeField, ComboBox statusField, DateTimePicker endWorkDateField, TextBox transmissionDateField, TextBox beginDateWarrantyField, TextBox durationWarrantyField)
        {
            this.list = list;
            this.orderComponentField = orderComponentField;
            this.componentOnFirmField = componentOnFirmField;
            this.employeeField = employeeField;
            this.workTypeField = workTypeField;
            this.statusField = statusField;
            this.endWorkDateField = endWorkDateField;
            this.transmissionDateField = transmissionDateField;
            this.beginDateWarrantyField = beginDateWarrantyField;
            this.durationWarrantyField = durationWarrantyField;
            Checker = new ComponentInstallChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var componentInstall in db.component_install)
                    {
                        list.Items.Add($"[{componentInstall.status}] {componentInstall.order_component1.component1.name} {componentInstall.transmission_component_install1.transmission_date.ToString("yyyy-MM-dd")}");
                    }
                    orderComponentField.Items.Clear();
                    foreach (var orderComponent in db.order_component)
                    {
                        orderComponentField.Items.Add($"[{orderComponent.ordr.creation_date.ToString("yyyy-MM-dd")}] {orderComponent.component1.name}");
                    }
                    componentOnFirmField.Items.Clear();
                    foreach (var componentOnFirm in db.component_on_firm)
                    {
                        componentOnFirmField.Items.Add($"[{componentOnFirm.firm1.address}] {componentOnFirm.component1.name}");
                    }
                    employeeField.Items.Clear();
                    foreach (var employee in db.employees)
                    {
                        employeeField.Items.Add($"{employee.person1.first_name} {employee.person1.last_name}");
                    }
                    workTypeField.Items.Clear();
                    foreach (var workType in db.work_type)
                    {
                        workTypeField.Items.Add($"{workType.name}");
                    }
                    statusField.Items.Clear();
                    foreach (var componentInstall in db.component_install)
                    {
                        if (!statusField.Items.Contains(componentInstall.status))
                        {
                            statusField.Items.Add($"{componentInstall.status}");
                        }
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
                    var componentInstall = (from ci in db.component_install select ci).ToArray()[index];
                    orderComponentField.Text = $"[{componentInstall.order_component1.ordr.creation_date.ToString("yyyy-MM-dd")}] {componentInstall.order_component1.component1.name}";
                    componentOnFirmField.Text = $"[{componentInstall.transmission_component_install1.component_on_firm1.firm1.address}] {componentInstall.transmission_component_install1.component_on_firm1.component1.name}";
                    employeeField.Text = $"{componentInstall.employee1.person1.first_name} {componentInstall.employee1.person1.last_name}";
                    workTypeField.Text = componentInstall.work_type1.name;
                    statusField.Text = componentInstall.status;
                    endWorkDateField.Value = componentInstall.end_work_date;
                    transmissionDateField.Text = componentInstall.transmission_component_install1.transmission_date.ToString("yyyy-MM-dd");
                    beginDateWarrantyField.Text = componentInstall.warranty1.begin_date.ToString("yyyy-MM-dd");
                    durationWarrantyField.Text = componentInstall.warranty1.duration.ToString();
                    }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void ReadOnlyControls(bool disable)
        {
            orderComponentField.Enabled = !disable;
            componentOnFirmField.Enabled = !disable;
            employeeField.Enabled = !disable;
            workTypeField.Enabled = !disable;
            statusField.Enabled = !disable;
            endWorkDateField.Enabled = !disable;
            transmissionDateField.ReadOnly = true;
            beginDateWarrantyField.ReadOnly = true;
            durationWarrantyField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            if (orderComponentField.Items.Count != 0) orderComponentField.SelectedIndex = 0;
            if (componentOnFirmField.Items.Count != 0) componentOnFirmField.SelectedIndex = 0;
            if (employeeField.Items.Count != 0) employeeField.SelectedIndex = 0;
            if (workTypeField.Items.Count != 0) workTypeField.SelectedIndex = 0;
            if (statusField.Items.Count != 0) statusField.SelectedIndex = 0;
            transmissionDateField.Text = DateTime.Now.ToString("yyyy-MM-dd");
            beginDateWarrantyField.Text = DateTime.Now.ToString("yyyy-MM-dd");
            durationWarrantyField.Text = "12";
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
                    var componentsInstall = (from ci in db.component_install select ci).ToArray();
                    var componentInstall = componentsInstall[index];
                    db.warranties.Remove(componentInstall.warranty1);
                    db.component_install.Remove(componentInstall);
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
                    var componentInstall = new component_install()
                    {
                        order_component1 = (from oc in db.order_component select oc).ToArray()[orderComponentField.SelectedIndex],
                        work_type1 = (from wt in db.work_type select wt).ToArray()[workTypeField.SelectedIndex],
                        status = statusField.Text,
                        employee1 = (from e in db.employees select e).ToArray()[employeeField.SelectedIndex],
                        end_work_date = endWorkDateField.Value,
                        warranty1 = new warranty()
                        {
                            begin_date = DateTime.Now,
                            duration = int.Parse(durationWarrantyField.Text)
                        },
                        transmission_component_install1 = new transmission_component_install()
                        {
                            component_on_firm1 = (from cof in db.component_on_firm select cof).ToArray()[componentOnFirmField.SelectedIndex],
                            transmission_date = DateTime.Now
                        }
                    };
                    componentInstall.transmission_component_install1.component_on_firm1.component_count -=
                        componentInstall.order_component1.component_count;
                    db.component_install.Add(componentInstall);
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
                    var componentsInstall = (from ci in db.component_install select ci).ToArray();
                    var componentInstall = componentsInstall[index];
                    componentInstall.order_component1 = (from oc in db.order_component select oc).ToArray()[orderComponentField.SelectedIndex];
                    componentInstall.work_type1 = (from wt in db.work_type select wt).ToArray()[workTypeField.SelectedIndex];
                    componentInstall.status = statusField.Text;
                    componentInstall.employee1 = (from e in db.employees select e).ToArray()[employeeField.SelectedIndex];
                    componentInstall.end_work_date = endWorkDateField.Value;
                    componentInstall.warranty1.begin_date = DateTime.Now;
                    componentInstall.warranty1.duration = int.Parse(durationWarrantyField.Text);
                    componentInstall.transmission_component_install1.component_on_firm1 = (from cof in db.component_on_firm select cof).ToArray()[componentOnFirmField.SelectedIndex];
                    componentInstall.transmission_component_install1.transmission_date = DateTime.Now;
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
