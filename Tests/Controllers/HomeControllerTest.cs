using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Web.Controllers;

namespace Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ThankYou()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.ThankYou() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TooManyUsers()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.TooManyUsers() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
