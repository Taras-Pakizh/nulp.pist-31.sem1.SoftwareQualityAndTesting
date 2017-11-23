using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.Permission;

namespace app.Checker
{
    class FirmChecker : Checker
    {
        public bool IsVisible(User user)
        {
            return user.Check(ADMIN_ACCESS) || user.Check(MANAGEMENT_ACCESS) || user.Check(LOGISTICS_ACCESS);
        }

        public bool Add(User user)
        {
            return user.Check(ADMIN_ACCESS) || user.Check(MANAGER_ACCESS);
        }

        public bool Delete(User user)
        {
            return user.Check(ADMIN_ACCESS) || user.Check(MANAGER_ACCESS);
        }

        public bool Edit(User user)
        {
            return user.Check(ADMIN_ACCESS) || user.Check(MANAGER_ACCESS);
        }
    }
}
