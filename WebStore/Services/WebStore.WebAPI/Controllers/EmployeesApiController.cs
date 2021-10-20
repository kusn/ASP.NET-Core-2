﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(WebAPIAdresses.Empolyees)]    
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var id = _EmployeesData.Add(employee);
            return CreatedAtAction(nameof(GetById), new { id }, employee);
            //return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _EmployeesData.Update(employee);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _EmployeesData.Delete(id);
            return result ? Ok(result) : NotFound(result);
        }
    }
}
