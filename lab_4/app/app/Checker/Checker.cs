using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Checker
{
    public interface Checker
    {

        bool IsVisible(User user);

        bool Add(User user);

        bool Delete(User user);

        bool Edit(User user);

    }
}
