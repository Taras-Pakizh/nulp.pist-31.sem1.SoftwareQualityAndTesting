using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class ComponentOnFirmForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly ComboBox componentField;
        private readonly ComboBox firmField;
        private readonly TextBox countField;

        public ComponentOnFirmForm(ListBox list, ComboBox componentField, ComboBox firmField, TextBox countField)
        {
            this.list = list;
            this.componentField = componentField;
            this.firmField = firmField;
            this.countField = countField;
            Checker = new ComponentOnFirmChecker();
        }

        public void Init()
        {
            try {
            list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var componentOnFirm in db.component_on_firm)
                    {
                        list.Items.Add($"[{componentOnFirm.firm1.address}] {componentOnFirm.component1.name}");
                    }
                    componentField.Items.Clear();
                    foreach (var component in db.components)
                    {
                        componentField.Items.Add($"{component.name}");
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
                    var componentOnFirm = (from cof in db.component_on_firm select cof).ToArray()[index];
                    componentField.Text = componentOnFirm.component1.name;
                    firmField.Text = componentOnFirm.firm1.address;
                    countField.Text = componentOnFirm.component_count.ToString();
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
            firmField.Enabled = !disable;
            countField.ReadOnly = disable;
        }

        public void DefaultControlsValue()
        {
            if (componentField.Items.Count != 0) componentField.SelectedIndex = 0;
            if (firmField.Items.Count != 0) firmField.SelectedIndex = 0;
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
                    var componentsOnFirm = (from cof in db.component_on_firm select cof).ToArray();
                    var componentOnFirm = componentsOnFirm[index];
                    db.component_on_firm.Remove(componentOnFirm);
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
                    var componentOnFirm = new component_on_firm()
                    {
                        component1 = (from c in db.components where c.name.Equals(componentField.Text) select c).First(),
                        firm1 = (from f in db.firms where f.address.Equals(firmField.Text) select f).First(),
                        component_count = int.Parse(countField.Text)
                    };
                    db.component_on_firm.Add(componentOnFirm);
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
                    var componentsOnFirm = (from cof in db.component_on_firm select cof).ToArray();
                    var componentOnFirm = componentsOnFirm[index];
                    componentOnFirm.component1 = (from c in db.components where c.name.Equals(componentField.Text) select c).First();
                    componentOnFirm.firm1 = (from f in db.firms where f.address.Equals(firmField.Text) select f).First();
                    componentOnFirm.component_count = int.Parse(countField.Text);
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
