using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Student s0 = new Student
            {
                ID = 99,
                Name = "Bob",
                Credit = 80m,
                LastPayment = DateTime.Now,
                School = null,
                Friend = null,
            };
            
            Student s1 = new Student
            {
                ID = 100,
                Name = "James",
                Credit = 100m,
                LastPayment = DateTime.Parse("2013-04-01 10:00"),
                School = null,
                Friend = null,
            };

            Student s1Mod = new Student
            {
                ID = 100,
                Name = "James Lee",
                Credit = 102m,
                LastPayment = DateTime.Now,
                School = new School
                {
                    Name = "Primary School",
                },
                Friend = s0,
            };

            var changes = ObjectReflection.GetChanges(s1, s1Mod,
                s => s.ID,
                s => s.Name,
                s => s.Credit,
                s => s.LastPayment,
                s => s.School.Name,
                s => s.Friend.Name,
                s => s.Friend.ID);

            changes.ForEach(c =>
            {
                Console.WriteLine($"{c.Field} has changed from '{c.OldValue}' to '{c.NewValue}'");
            });

            Console.ReadKey();
        }
    }
}
