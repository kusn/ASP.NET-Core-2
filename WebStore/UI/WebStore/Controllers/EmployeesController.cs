using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;
using Microsoft.Extensions.Logging;
using WebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Controllers
{
    //[Route("Staff/[action]/{id?}")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;


        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {            
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        //[Route("~employees/all")]
        public IActionResult Index()                        // http://localhost:5000/Home/Employees
        {
            return View(_EmployeesData.GetAll());
        }

        //[Route("~employees/info-{id}")]
        public IActionResult Details(int id)            // http://localhost:5000/Home/Details/id
        {
            var employee = _EmployeesData.GetById(id);

            if (employee is null) return NotFound();


            return View(employee);
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }

        #region Delete
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateOfBorn = employee.DateOfBorn,
                DateOfEmployment = employee.DateOfEmployment,
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)               // // http://localhost:5000/Home/Edit/id
        {
            if (id is null)
                return View(new EmployeeViewModel());
            
            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateOfBorn = employee.DateOfBorn,
                DateOfEmployment = employee.DateOfEmployment,
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel viewModel)
        {
            if (viewModel.LastName == "Асама" && viewModel.Name == "Бин" && viewModel.Patronymic == "Ладан")
                ModelState.AddModelError("", "Террористов не берём!");
            
            if(!ModelState.IsValid) return View(viewModel);

            var employee = new Employee
            {
                Id = viewModel.Id,
                FirstName = viewModel.Name,
                LastName = viewModel.LastName,
                Patronymic = viewModel.Patronymic,
                Age = viewModel.Age,
                DateOfBorn = viewModel.DateOfBorn,
                DateOfEmployment = viewModel.DateOfEmployment,
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
