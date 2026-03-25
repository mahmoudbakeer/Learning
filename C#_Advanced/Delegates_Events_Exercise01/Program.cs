using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates_Events_Exercise01
{
    internal class Program
    {
        static void Main(string[] args)
        {   


            List<clsStudent> students = new List<clsStudent>();


            while (true)
            {
                clsStudent student = new clsStudent();


                Console.WriteLine($"Welcome Please Entere Your name : or 'done' if finished ");
                string name = Console.ReadLine();
                if(name is "done") break;
                Console.WriteLine($"Please Enter Your grades : (5 subjects)");
                List<int> grades = new List<int>();
                for (int i = 0; i < 5; i++) {

                    int grade = Convert.ToInt32(Console.ReadLine());
                    grades.Add(grade);
                }
                student.Name = name;
                student.Grades = grades;
                students.Add(student);
            }

            // Passing the function as parameter is strong application of delegation properties 
            // the lambda here represents inline function 

            // the parameters here all are delegates except the list of students 
            // first is Func delegate which is built in generic delegate 
            // second is the Predicate which is also built in delegate takes one argument and return true or false according to the logic
            // the third is a Action<> which is also a delegate that has no return type 
            Console.WriteLine($"Students Performance : ");
            GradingSystem.ShowStudentsPerformance(students,
                // Func<List<int> , double>  return the average of the list of integers
                (List<int> a) => (a.Sum() / a.Count),
                // Predicate<double> and check if the passed arg is greater or equal to 30
                (double x) => (x >= 30) ,
                // Action<clsStudent , double , bool>
                (clsStudent student,double grades, bool isPassed)=> {
                    Console.WriteLine($"The student {student.Name} has grade of {grades}");
                    Console.WriteLine($"The student {student.Name} status is  {(isPassed ? "Passed" : "Failed")}");
                }
                );
        }
    }
}
