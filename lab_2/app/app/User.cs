using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static app.Permission;

namespace app
{
    public class User
    {

        public static readonly User NullUser = new User("", "", new List<Permission>());

        private static readonly List<User> UserList = new List<User>();

        public static string[] Users()
        {
            return UserList.Select(u => u._login).ToArray();
        }

        static User()
        {
            UserList.Add(new User("admin", "admin", new List<Permission>() { LOGISTICS_ACCESS, MANAGEMENT_ACCESS, ADMIN_ACCESS }));
            UserList.Add(new User("logist", "logist", new List<Permission>() { LOGISTICS_ACCESS }));
            UserList.Add(new User("manager", "manager", new List<Permission>() { MANAGEMENT_ACCESS, MANAGER_ACCESS }));
            UserList.Add(new User("employee", "employee", new List<Permission>() { MANAGEMENT_ACCESS, EMPLOYEE_ACCESS }));
            UserList.Add(new User("accountant", "accountant", new List<Permission>() { MANAGEMENT_ACCESS, ACCOUNTANT_ACCESS }));
        }

        public string Name { get { return _login; } }
        readonly string _login;
        private readonly string _password;
        private readonly List<Permission> _permissions;

        private User(string login, string password, List<Permission> permissions)
        {
            this._login = login;
            this._password = password;
            this._permissions = permissions;
        }

        public static User LoginUser(string login, string password)
        {
            var current = NullUser;
            foreach (var u in UserList)
            {
                if (u._login == login && u._password == password)
                {
                    current = u;
                }
            }
            return current;
        }

        public bool Check(Permission permission)
        {
            return _permissions.Contains(permission);
        }
    }
}
