
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cumulative2.Models
{
    /// <summary>
    /// Represents a class in the school system.
    /// </summary>
    public class Subject
    {
        // Properties of the Subject entity
        public int SubjectId;
        public string SubjectCode;
        public int InstructorId;
        public DateTime StartDate;
        public DateTime EndDate;
        public string SubjectName;
    }
}
