using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPIControllerTests
    {
        //class TestValuesService : IValuesService
        //{
        //    public void Add(string value)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public int Count()
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public bool Delete(int id)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public void Edit(int id, string value)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public IEnumerable<string> GetAll()
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public string GetById(int id)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [TestMethod]
        public void Index_returns_whith_DataValues()
        {
            var data = Enumerable.Range(1, 10)
                .Select(i => $"Value - {i}")
                .ToArray();

            var values_service_mock = new Mock<IValuesService>();
            values_service_mock.Setup(c => c.GetAll())
                .Returns(data);

            //var values_service = new TestValuesService();
            var controller = new WebAPIController(values_service_mock.Object);

            var result = controller.Index();

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            var i = 0;
            foreach (var actual_value in model)
            {
                var expected_value = data[i++];
                Assert.Equal(expected_value, actual_value);
            }

            values_service_mock.Verify(s => s.GetAll());
            values_service_mock.VerifyNoOtherCalls();
        }
    }
}
