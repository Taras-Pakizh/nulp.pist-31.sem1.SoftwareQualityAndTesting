using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app.Checker;

namespace app.TableForm
{
    class PCForm : TableForm
    {
        public Checker.Checker Checker { get; }

        private readonly ListBox list;
        private readonly TextBox nameField;
        private readonly TextBox descriptionField;
        private readonly ComboBox pcTypeField;
        private readonly ComboBox producerField;

        public PCForm(ListBox list, TextBox nameField, TextBox descriptionField, ComboBox pcTypeField, ComboBox producerField)
        {
            this.list = list;
            this.nameField = nameField;
            this.descriptionField = descriptionField;
            this.pcTypeField = pcTypeField;
            this.producerField = producerField;
            Checker = new PCChecker();
        }

        public void Init()
        {
            try {
                list.Items.Clear();
                using (var db = new course_work_Entities())
                {
                    foreach (var pc in db.pcs)
                    {
                        list.Items.Add($"{pc.name} ({pc.description})");
                    }
                    pcTypeField.Items.Clear();
                    foreach (var pcType in db.pc_type)
                    {
                        pcTypeField.Items.Add($"{pcType.name}");
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
                    var pc = (from p in db.pcs select p).ToArray()[index];
                    nameField.Text = pc.name;
                    descriptionField.Text = pc.description;
                    pcTypeField.Text = pc.pc_type1.name;
                    producerField.Text = pc.producer1.name;
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
            pcTypeField.Enabled = !disable;
            producerField.Enabled = !disable;
        }

        public void DefaultControlsValue()
        {
            nameField.Text = "";
            descriptionField.Text = "";
            if (pcTypeField.Items.Count != 0) pcTypeField.SelectedIndex = 0;
            if (producerField.Items.Count != 0) producerField.SelectedIndex = 0;
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
                    var pcs = (from p in db.pcs select p).ToArray();
                    var pc = pcs[index];
                    var deleteFlag = true;
                    foreach (var p in pcs)
                    {
                        if (!p.Equals(pc) && p.pc_type1.name.Equals(pc.pc_type1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.pc_type.Remove(pc.pc_type1);
                    }
                    deleteFlag = true;
                    foreach (var p in pcs)
                    {
                        if (!p.Equals(pc) && p.producer1.name.Equals(pc.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    var components = (from c in db.components select c).ToArray();
                    foreach (var c in components)
                    {
                        if (c.producer1.name.Equals(pc.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.producers.Remove(pc.producer1);
                    }
                    db.pcs.Remove(pc);
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
                    var pcTypeS = from p in db.pc_type where p.name.Equals(pcTypeField.Text) select p;
                    pc_type pcType;
                    if (!pcTypeS.Any())
                    {
                        pcType = new pc_type()
                        {
                            name = pcTypeField.Text,
                        };
                        db.pc_type.Add(pcType);
                        db.SaveChanges();
                    }
                    else
                    {
                        pcType = pcTypeS.First();
                    }
                    var producerS = from p in db.producers where p.name.Equals(producerField.Text) select p;
                    producer producer;
                    if (!producerS.Any())
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
                        producer = producerS.First();
                    }
                    var pc = new pc()
                    {
                        name = nameField.Text,
                        description = descriptionField.Text,
                        pc_type1 = pcType,
                        producer1 = producer
                    };
                    db.pcs.Add(pc);
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
                    var pcTypeS = from p in db.pc_type where p.name.Equals(pcTypeField.Text) select p;
                    pc_type pcType;
                    if (!pcTypeS.Any())
                    {
                        pcType = new pc_type()
                        {
                            name = pcTypeField.Text,
                        };
                        db.pc_type.Add(pcType);
                        db.SaveChanges();
                    }
                    else
                    {
                        pcType = pcTypeS.First();
                    }
                    var producerS = from p in db.producers where p.name.Equals(producerField.Text) select p;
                    producer producer;
                    if (!producerS.Any())
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
                        producer = producerS.First();
                    }
                    var pcs = (from p in db.pcs select p).ToArray();
                    var pc = pcs[index];
                    var deleteFlag = true;
                    foreach (var p in pcs)
                    {
                        if (!p.Equals(pc) && p.pc_type1.name.Equals(pc.pc_type1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.pc_type.Remove(pc.pc_type1);
                    }
                    deleteFlag = true;
                    foreach (var p in pcs)
                    {
                        if (!p.Equals(pc) && p.producer1.name.Equals(pc.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    var components = (from c in db.components select c).ToArray();
                    foreach (var c in components)
                    {
                        if (c.producer1.name.Equals(pc.producer1.name))
                        {
                            deleteFlag = false;
                        }
                    }
                    if (deleteFlag)
                    {
                        db.producers.Remove(pc.producer1);
                    }
                    pc.name = nameField.Text;
                    pc.description = descriptionField.Text;
                    pc.pc_type1 = pcType;
                    pc.producer1 = producer;
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
