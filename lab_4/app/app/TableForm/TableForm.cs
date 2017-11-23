using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.TableForm
{
    public interface TableForm
    {

        Checker.Checker Checker { get; }

        void Init();

        void SelectedItem();

        void ReadOnlyControls(bool disable);

        void DefaultControlsValue();

        void NewItem();

        void DeleteItem();

        void EditItem();

        void SaveNewItem();

        void SaveItem();

        void Cancel();

    }

}
