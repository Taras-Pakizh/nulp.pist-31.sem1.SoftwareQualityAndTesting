using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.Permission;

namespace app.Checker
{
    public class StatisticChecker : Checker
    {
        public bool IsVisible(User user)
        {
            return user.Check(ADMIN_ACCESS) || user.Check(ACCOUNTANT_ACCESS) || user.Check(LOGISTICS_ACCESS);
        }

        public bool Add(User user)
        {
            return false;
        }

        public bool Delete(User user)
        {
            return false;
        }

        public bool Edit(User user)
        {
            return false;
        }
    }
}
