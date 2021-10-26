using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Status_with_code_404_Returns_View()
        {
            //A A A = Arrange - Act - Assert
            
            #region Arrange

            const string code = "404";
            const string expected_url = "/NotFound/Index";
            var controller = new HomeController();

            #endregion

            #region Act

            var result = controller.Status(code);

            #endregion

            #region Assert

            var redirect = Assert.IsType<RedirectResult>(result);

            var actual_url = redirect.Url;

            Assert.Equal(expected_url, actual_url);

            #endregion
        }

        [TestMethod]
        [DataRow("123")]
        [DataRow("QWE")]
        public void Status_with_code_Returns_View(string code)
        {
            //const string code = "123";
            string expected_contetnt = "Статусный код: " + code;
            var controller = new HomeController();

            var result = controller.Status(code);

            var content_result = Assert.IsType<ContentResult>(result);

            var actual_content = content_result.Content;

            Assert.Equal(expected_contetnt, actual_content);
            //AssertFailedException
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Status_thrown_ArgumentNullException_when_code_is_null_1()
        {
            var controller = new HomeController();
            
            _ = controller.Status(null);
        }

        [TestMethod]
        public void Status_thrown_ArgumentNullException_when_code_is_null_2()
        {
            const string expected_parameter_name = "Code";
            var controller = new HomeController();

            Exception exception = null;
            try
            {
                _ = controller.Status(null);
            }
            catch (ArgumentNullException e)
            {

                exception = e;
            }

            var actual_exception = Assert.IsType<ArgumentNullException>(exception);
            var actual_parameter_name = actual_exception.ParamName;
            Assert.Equal(expected_parameter_name, actual_parameter_name);
        }

        [TestMethod]
        public void Status_thrown_ArgumentNullException_when_code_is_null_3()
        {
            const string expected_parameter_name = "Code";
            var controller = new HomeController();

            var actual_exception = Assert.Throws<ArgumentNullException>(() => controller.Status(null));

            var actual_parameter_name = actual_exception.ParamName;

            Assert.Equal(expected_parameter_name, actual_parameter_name);
        }
    }
}
