﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class Employee
    {
        public int Id { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBorn { get; set; }
        public DateTime DateOfEmployment { get; set; }
    }

    //public record Employee2(int Id, string FirstName, string LastName, string Patronymic, int Age);
}