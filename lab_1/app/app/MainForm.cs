using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.TableForm;

namespace app
{
    public partial class MainForm : Form
    {
        private readonly ProgramData programData = ProgramData.Instance;

        private bool newItemFlag = true;

        private readonly TableForm.TableForm categoryForm;
        private readonly TableForm.TableForm firmForm;
        private readonly TableForm.TableForm workTypeForm;
        private readonly TableForm.TableForm employeeForm;
        private readonly TableForm.TableForm clientForm;
        private readonly TableForm.TableForm componentForm;
        private readonly TableForm.TableForm pcForm;
        private readonly TableForm.TableForm orderForm;
        private readonly TableForm.TableForm orderComponentForm;
        private readonly TableForm.TableForm componentOnFirmForm;
        private readonly TableForm.TableForm componentInstallForm;
        private readonly TableForm.TableForm statisticForm;

        private TableForm.TableForm currentForm;
        private Checker.Checker currentChecker;

        public MainForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            categoryForm = new CategoryForm(list, nameCategoryField, descriptionCategoryField);
            firmForm = new FirmForm(list, cityFirmComboBox, addressFirmField, phoneFirmField);
            workTypeForm = new WorkTypeForm(list, nameWorkTypeField, priceWorkTypeField);
            employeeForm = new EmployeeForm(list, firstNameEmployeeField, secondNameEmployeeField, phoneEmployeeField, addressEmployeeField, positionEmployeeField, firmEmployeeField, experienceEmployeeField);
            clientForm = new ClientForm(list, firstNameClientField, lastNameClientField, phoneClientField, addressClientField, registrationDateClientField);
            componentForm = new ComponentForm(list, nameComponentField, descriptionComponentField, categoryComponentField, producerComponentField, priceComponentField);
            pcForm = new PCForm(list, namePCField, descriptionPCField, pcTypePCField, producerPCField);
            orderForm = new OrderForm(list, clientOrderField, employeeOrderField, pcOrderField, creationDateOrderField, isCompleteOrderField);
            orderComponentForm = new OrderComponentForm(list, componentOrderComponentField, orderOrderComponentField, countOrderComponentField);
            componentOnFirmForm = new ComponentOnFirmForm(list, componentComponentOnFirmField, firmComponentOnFirmField, countComponentOnFirmField);
            componentInstallForm = new ComponentInstallForm(list, orderComponentComponentInstallField, componentOnFirmComponentInstallField, employeeComponentInstallField, workTypeComponentInstallField, statusComponentInstallField, endWorkDateComponentInstallField, transmissionDateComponentInstallField, beginDateWarrantyComponentInstallField, durationWarrantyComponentInstallField);
            statisticForm = new StatisticForm(list, firmCountStatisticField, workTypeCountStatisticField, employeeCountStatisticField, clientCountStatisticField, componentCountStatisticField, componentPriceStatisticField, orderCountStatisticField, orderPriceStatisticField);

            var user = programData.CurrentUser;
            if (!categoryForm.Checker.IsVisible(user)) categoryTabPage.Dispose();
            if (!firmForm.Checker.IsVisible(user)) firmTabPage.Dispose();
            if (!workTypeForm.Checker.IsVisible(user)) workTypeTabPage.Dispose();
            if (!employeeForm.Checker.IsVisible(user)) employeeTabPage.Dispose();
            if (!clientForm.Checker.IsVisible(user)) clientTabPage.Dispose();
            if (!componentForm.Checker.IsVisible(user)) componentTabPage.Dispose();
            if (!pcForm.Checker.IsVisible(user)) pcTabPage.Dispose();
            if (!orderForm.Checker.IsVisible(user)) orderTabPage.Dispose();
            if (!orderComponentForm.Checker.IsVisible(user)) orderComponentTabPage.Dispose();
            if (!componentOnFirmForm.Checker.IsVisible(user)) componentOnFirmTabPage.Dispose();
            if (!componentInstallForm.Checker.IsVisible(user)) componentInstallTabPage.Dispose();
            if (!statisticForm.Checker.IsVisible(user)) statisticTabPage.Dispose();

            tabControl1.SelectedTab = managementTabPage;
            managementTabControl_SelectedIndexChanged(new object(), EventArgs.Empty);
            EnableButton(true, true, true, false, false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            labelUsername.Text = "Ви зайшли під іменем: " + programData.CurrentUser.Name;
        }

        private void labelUsername_Click(object sender, EventArgs e)
        {
            ProgramData.Instance.CurrentUser = User.NullUser;
            this.Hide();
            var form = new LoginForm();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void managementTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (managementTabControl.SelectedTab == categoryTabPage) currentForm = categoryForm;
            else if (managementTabControl.SelectedTab == firmTabPage) currentForm = firmForm;
            else if (managementTabControl.SelectedTab == workTypeTabPage) currentForm = workTypeForm;
            else if (managementTabControl.SelectedTab == employeeTabPage) currentForm = employeeForm;
            else if (managementTabControl.SelectedTab == clientTabPage) currentForm = clientForm;
            else if (managementTabControl.SelectedTab == componentTabPage) currentForm = componentForm;
            else if (managementTabControl.SelectedTab == pcTabPage) currentForm = pcForm;
            else if (managementTabControl.SelectedTab == orderTabPage) currentForm = orderForm;
            else if (managementTabControl.SelectedTab == orderComponentTabPage) currentForm = orderComponentForm;
            else if (managementTabControl.SelectedTab == componentOnFirmTabPage) currentForm = componentOnFirmForm;
            else if (managementTabControl.SelectedTab == componentInstallTabPage) currentForm = componentInstallForm;
            else if (managementTabControl.SelectedTab == statisticTabPage) currentForm = statisticForm;
            currentChecker = currentForm.Checker;
            EnableButton(true, true, true, false, false);
            ReadOnlyControls(true);
            currentForm.Init();
        }

        private void list_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            newItemFlag = false;
            EnableButton(true, true, true, false, false);
            ReadOnlyControls(true);
            currentForm.SelectedItem();
        }



        private void EnableButton(bool newButton, bool deleteButton, bool editButton, bool okButton, bool cancelButton)
        {
            var user = programData.CurrentUser;
            this.newButton.Enabled = newButton && currentChecker.Add(user);
            this.deleteButton.Enabled = deleteButton && currentChecker.Delete(user);
            this.editButton.Enabled = editButton && currentChecker.Edit(user);
            this.saveButton.Enabled = okButton && (currentChecker.Add(user) || currentChecker.Edit(user));
            this.cancelButton.Enabled = cancelButton && (currentChecker.Add(user) || currentChecker.Edit(user));
        }

        private void ReadOnlyControls(bool disable)
        {
            currentForm.ReadOnlyControls(disable);
        }

        private void DefaultControlsValue()
        {
            currentForm.DefaultControlsValue();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            newItemFlag = true;
            EnableButton(true, false, false, true, true);
            ReadOnlyControls(false);
            DefaultControlsValue();
            currentForm.NewItem();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            EnableButton(true, false, false, true, true);
            ReadOnlyControls(false);
            DefaultControlsValue();
            currentForm.DeleteItem();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            EnableButton(true, true, false, true, true);
            ReadOnlyControls(false);
            currentForm.EditItem();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            EnableButton(true, true, true, false, false);
            ReadOnlyControls(true);
            if (newItemFlag)
            {
                currentForm.SaveNewItem();
            }
            else
            {
                currentForm.SaveItem();
            }
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (newItemFlag)
            {
                DefaultControlsValue();
            }
            else
            {
                EnableButton(true, true, true, false, false);
                ReadOnlyControls(true);
            }
            currentForm.Cancel();
        }

        private void saveStatisticButton_Click(object sender, EventArgs e)
        {
            var error = (null as Checker.Checker).IsVisible(null);
            var text = "";
            
            using (var db = new course_work_Entities())
            {
                decimal count = 0;
                text += $" --- Статистика компонувальника комп'ютера --- \n";
                text += $"Дата: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n";
                text += $"Тип користувача: {programData.CurrentUser.Name}\n";
                text += $"--------------------------------------\n";
                text += $"Фірми :\n";
                text += $"кількість : {db.firms.Count()}\n";
                foreach (var v in db.firms)
                {
                    text += $"\t{v.city1.name} {v.address} {v.phone}\n";
                }
                text += $"Типи робіт :\n";
                text += $"кількість робіт : {db.work_type.Count()}\n";
                foreach (var v in db.work_type)
                {
                    text += $"\t{v.name} {v.price}\n";
                }
                text += $"Працівники :\n";
                text += $"кількість : {db.employees.Count()}\n";
                foreach (var v in db.employees)
                {
                    text += $"\t{v.person1.first_name} {v.person1.last_name}\n";
                }
                text += $"Клієнти :\n";
                text += $"кількість : {db.clients.Count()}\n";
                foreach (var v in db.clients)
                {
                    text += $"\t{v.person1.first_name} {v.person1.last_name}\n";
                }
                text += $"Компоненти :\n";
                text += $"кількість різних : {db.components.Count()}\n";
                count = 0;
                foreach (var cof in db.component_on_firm)
                {
                    count += cof.component_count;
                }
                text += $"кількість разом : {count}\n";
                count = 0;
                foreach (var cof in db.component_on_firm)
                {
                    count += (cof.component_count * cof.component1.price);
                }
                text += $"загальна вартість : {count}\n";
                text += $"Замовлення :\n";
                text += $"кількість : {db.ordrs.Count()}\n";
                count = 0;
                foreach (var ci in db.component_install)
                {
                    count += (ci.work_type1.price + ci.order_component1.component1.price) * ci.order_component1.component_count;
                }
                text += $"загальна вартість : {count}\n";
                text += $"--------------------------------------\n";
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FileName = $"Статистика [{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}]";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, text);
            }
        }
    }
}
