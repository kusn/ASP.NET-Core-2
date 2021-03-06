using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client, WebAPIAddresses.Empolyees)
        {

        }

        public int Add(Employee employee)
        {
            var response = Post(Address, employee);
            var added_employee = response.Content.ReadFromJsonAsync<Employee>().Result;
            if (added_employee is null)
                return -1;
            var id = added_employee.Id;
            return id;
        }

        public bool Delete(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return success;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = Get<IEnumerable<Employee>>(Address);
            return employees;
        }

        public Employee GetById(int id)
        {
            var result = Get<Employee>($"{Address}/{id}");
            return result;
        }

        public void Update(Employee employee)
        {
            Put(Address, employee);
        }
    }
}
