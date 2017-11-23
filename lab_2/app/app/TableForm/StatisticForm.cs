using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class StatisticForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox firmCountField;
        private readonly TextBox workTypeCountField;
        private readonly TextBox employeeCountField;
        private readonly TextBox clientCountField;
        private readonly TextBox componentCountField;
        private readonly TextBox componentPriceField;
        private readonly TextBox orderCountField;
        private readonly TextBox orderPriceField;

        public StatisticForm(ListBox list, TextBox firmCountField, TextBox workTypeCountField, TextBox employeeCountField, TextBox clientCountField, TextBox componentCountField, TextBox componentPriceField, TextBox orderCountField, TextBox orderPriceField)
        {
            this.list = list;
            this.firmCountField = firmCountField;
            this.workTypeCountField = workTypeCountField;
            this.employeeCountField = employeeCountField;
            this.clientCountField = clientCountField;
            this.componentCountField = componentCountField;
            this.componentPriceField = componentPriceField;
            this.orderCountField = orderCountField;
            this.orderPriceField = orderPriceField;
            Checker = new StatisticChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    firmCountField.Text = db.firms.Count().ToString();
                    workTypeCountField.Text = db.work_type.Count().ToString();
                    employeeCountField.Text = db.employees.Count().ToString();
                    clientCountField.Text = db.clients.Count().ToString();
                    decimal count = 0;
                    foreach (var cof in db.component_on_firm)
                    {
                        count += cof.component_count;
                    }
                    componentCountField.Text = count.ToString();
                    count = 0;
                    foreach (var cof in db.component_on_firm)
                    {
                        count += (cof.component_count * cof.component1.price);
                    }
                    componentPriceField.Text = count.ToString();
                    orderCountField.Text = db.ordrs.Count().ToString();
                    count = 0;
                    foreach (var ci in db.component_install)
                    {
                        count += (ci.work_type1.price + ci.order_component1.component1.price) * ci.order_component1.component_count;
                    }
                    orderPriceField.Text = count.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Щось пішло не так", "Помилка");
            }
        }

        public void SelectedItem()
        {
        }

        public void ReadOnlyControls(bool disable)
        {
            firmCountField.ReadOnly = true;
            workTypeCountField.ReadOnly = true;
            employeeCountField.ReadOnly = true;
            clientCountField.ReadOnly = true;
            componentCountField.ReadOnly = true;
            componentPriceField.ReadOnly = true;
            orderCountField.ReadOnly = true;
            orderPriceField.ReadOnly = true;
        }

        public void DefaultControlsValue()
        {
            firmCountField.Text = "0";
            workTypeCountField.Text = "0";
            employeeCountField.Text = "0";
            clientCountField.Text = "0";
            componentCountField.Text = "0";
            componentPriceField.Text = "0";
            orderCountField.Text = "0";
            orderPriceField.Text = "0";
        }

        public void NewItem()
        {
        }

        public void DeleteItem()
        {
        }

        public void EditItem()
        {
        }

        public void SaveNewItem()
        {
        }

        public void SaveItem()
        {
        }

        public void Cancel()
        {
        }
    }
}
