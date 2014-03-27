using Jinete.Controllers;
using Jinete.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Jinete.Test.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        Mock<ApplicationDbContext> _db;
        Mock<UserStore<ApplicationUser>> _us;
        UserManager<ApplicationUser> _um;
        AccountController _ac;

        [TestInitialize]
        public void Initialize()
        {
            _db = new Mock<ApplicationDbContext>();
            _us = new Mock<UserStore<ApplicationUser>>(_db.Object);
            _um = new UserManager<ApplicationUser>(_us.Object);
            _ac = new AccountController(_um);
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            AccountController controller = new AccountController(_um);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CanLogin()
        {
            //Arrange
            //Mock.Arrange(() => )


            LoginViewModel _lvm = new LoginViewModel
            {
                UserName = "",
                Password = "",
                RememberMe = false
            };

            string _ru = "~/Home/Index/";

            //Act
            var result = await _ac.Login(_lvm, _ru);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
