using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]    
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData employeesData)
        {
            _EmployeesData = employeesData;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _EmployeesData.GetAll();
            return Ok(employees);
        }
    }
}
