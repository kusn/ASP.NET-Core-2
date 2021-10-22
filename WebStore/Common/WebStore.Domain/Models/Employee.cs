using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    /// <summary>Информация  о сотруднике</summary>
    public class Employee
    {
        /// <summary>Идентификатор сотрудника</summary>
        public int Id { get; set;}

        /// <summary>Имя</summary>
        public string FirstName { get; set; }

        /// <summary>Фамилия</summary>
        public string LastName { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        /// <summary>Возраст</summary>
        public int Age { get; set; }

        /// <summary>Дата рождения</summary>
        public DateTime DateOfBorn { get; set; }

        /// <summary>Дата устройства на работу</summary>
        public DateTime DateOfEmployment { get; set; }

        public override string ToString()
        {
            return $"[{Id} ]{LastName} {FirstName} {Patronymic} ({Age})";
        }
    }

    //public record Employee2(int Id, string FirstName, string LastName, string Patronymic, int Age);
}
