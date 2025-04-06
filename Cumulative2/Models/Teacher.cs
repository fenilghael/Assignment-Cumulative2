using System;
using System.Collections.Generic;

namespace Cumulative1.Models
{
    /// <summary>
    /// Represents a teacher in the school system.
    /// </summary>
    public class Teacher
    {
        // Properties of the Teacher entity
        public int TeacherId;
        public string FirstName;
        public string LastName;
        public string EmployeeId;
        public DateTime HireDate;
        public decimal SalaryAmount;
        public List<Class> AssignedClasses;
    }
}
