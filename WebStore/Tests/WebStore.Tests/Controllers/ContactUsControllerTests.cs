using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class ContactUsControllerTests
    {
        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new ContactUsController();
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
