using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_1_Avancerad.NET.Models
{
    internal class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
