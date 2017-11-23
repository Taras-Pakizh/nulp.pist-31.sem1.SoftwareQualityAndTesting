using Microsoft.VisualStudio.TestTools.UnitTesting;
using app.Checker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Checker.Tests
{
    [TestClass()]
    public class CheckerTests
    {

        [TestMethod()]
        public void IsVisibleTestPositive()
        {
            var checker = new StatisticChecker();
            var user = User.LoginUser("admin", "admin");
            Assert.IsTrue(checker.IsVisible(user));
        }

        [TestMethod()]
        public void IsVisibleTestNegative()
        {
            var checker = new StatisticChecker();
            var user = User.NullUser;
            Assert.IsFalse(checker.IsVisible(user));
        }

        [TestMethod()]
        public void AddTestPositive()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("admin", "admin");
            Assert.IsTrue(checker.Add(user));
        }

        [TestMethod()]
        public void AddTestNegative()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("employee", "employee");
            Assert.IsFalse(checker.Add(user));
        }

        [TestMethod()]
        public void DeleteTestPositive()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("admin", "admin");
            Assert.IsTrue(checker.Delete(user));
        }

        [TestMethod()]
        public void DeleteTestNegative()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("accountant", "accountant");
            Assert.IsFalse(checker.Delete(user));
        }

        [TestMethod()]
        public void EditTestPositive()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("admin", "admin");
            Assert.IsTrue(checker.Edit(user));
        }

        [TestMethod()]
        public void EditTestNegative()
        {
            var checker = new FirmChecker();
            var user = User.LoginUser("accountant", "accountant");
            Assert.IsFalse(checker.Edit(user));
        }

        [TestMethod()]
        public void UserTestPositive()
        {
            var user = User.LoginUser("admin", "admin");
            Assert.AreNotEqual(User.NullUser, user);
        }

        [TestMethod()]
        public void UserTestNegative()
        {
            var user = User.LoginUser("adin", "amin");
            Assert.ReferenceEquals(User.NullUser, user);
        }
    }
}