using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectReflection
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Credit { get; set; }
        public DateTime? LastPayment { get; set; }
        public School School { get; set; }
        public Student Friend { get; set; }
    }

    public class School
    {
        public string Name { get; set; }
    }
}
