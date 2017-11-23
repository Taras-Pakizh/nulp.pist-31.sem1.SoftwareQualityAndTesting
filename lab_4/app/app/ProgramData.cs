using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    class ProgramData
    {
        public static readonly ProgramData Instance = new ProgramData();

        private ProgramData()
        {

        }

        // ---

        public User CurrentUser { set; get; }


    }
}
