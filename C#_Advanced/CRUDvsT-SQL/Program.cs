using System.Data;
using System.Diagnostics;

namespace CRUDvsT_SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DAEmployee.openFirstConnection(); 
            stopwatch.Stop();
            Console.WriteLine($"The Time taken to initialize the connection pool is : {stopwatch.ElapsedMilliseconds} ms");
            // first connection take much more time than the rest
            Console.WriteLine("The Updation on salary Using TSQL operations: ");
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            UpdateUsingTSql();
            stopwatch1.Stop();
            Console.WriteLine($"The Time taken is : {stopwatch1.ElapsedMilliseconds} ms");
            Console.WriteLine("The Updation on salary Using c# crud operations: ");
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            UpdateUsingCSharp();
            stopwatch2.Stop();
            Console.WriteLine($"The Time taken is : {stopwatch2.ElapsedMilliseconds} ms");
        
        }

        public static void UpdateUsingCSharp()
        {
            List<Employee> employees = Employee.GetListOfEmployees();
            int cnt = 0;
            foreach (Employee employee in employees)
            {
                if(employee.Department == "HR")
                {
                    if(employee.PerformanceRate > 90) employee.Salary = employee.Salary * 1.15m;
                    else if(employee.PerformanceRate > 80) employee.Salary = employee.Salary * 1.1m;
                    else if(employee.PerformanceRate > 50 ) employee.Salary = employee.Salary * 1.05m;
                    
                }
                else if(employee.Department == "Sale")
                {
                    if (employee.PerformanceRate > 90) employee.Salary = employee.Salary * 1.1m;
                    else if (employee.PerformanceRate > 80) employee.Salary = employee.Salary * 1.08m;
                    else if (employee.PerformanceRate > 50) employee.Salary = employee.Salary * 1.04m;
                }
                else 
                {
                    if (employee.PerformanceRate > 90) employee.Salary = employee.Salary * 1.08m;
                    else if (employee.PerformanceRate > 80) employee.Salary = employee.Salary * 1.06m;
                    else if (employee.PerformanceRate > 50) employee.Salary = employee.Salary * 1.03m;
                }
               cnt = employee.Update() ? ++cnt : cnt;
            }
            Console.WriteLine($"The rows affected {cnt}");
        }
        public static void UpdateUsingTSql()
        {
            int rows = DAEmployee.UpdateEmployeesSalaryUsingTSQL();
            Console.WriteLine($"The rows affected {rows}");
        }
    }
}
