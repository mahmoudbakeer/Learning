using System.Data;
using System.Diagnostics;
using static CRUDvsT_SQL.EmployeesReport;

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
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("The Updation on salary Using TSQL operations: ");
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            ReportUsingTsql();
            stopwatch1.Stop();
            Console.WriteLine($"The Time taken is : {stopwatch1.ElapsedMilliseconds} ms");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("The Updation on salary Using c# crud operations: ");
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            ReportUsingCSharp();
            stopwatch2.Stop();
            Console.WriteLine($"The Time taken is : {stopwatch2.ElapsedMilliseconds} ms");
            //// first connection take much more time than the rest
            //Console.WriteLine("The Updation on salary Using TSQL operations: ");
            //Stopwatch stopwatch1 = new Stopwatch();
            //stopwatch1.Start();
            //UpdateUsingTSql();
            //stopwatch1.Stop();
            //Console.WriteLine($"The Time taken is : {stopwatch1.ElapsedMilliseconds} ms");
            //Console.WriteLine("The Updation on salary Using c# crud operations: ");
            //Stopwatch stopwatch2 = new Stopwatch();
            //stopwatch2.Start();
            //UpdateUsingCSharp();
            //stopwatch2.Stop();
            //Console.WriteLine($"The Time taken is : {stopwatch2.ElapsedMilliseconds} ms");

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
        public static void ReportUsingCSharp()
        {
            EmployeesReport Lowreport = new EmployeesReport(EmployeesReport.enLevel.Low);
            EmployeesReport Medreport = new EmployeesReport(EmployeesReport.enLevel.Medium);
            EmployeesReport Highreport = new EmployeesReport(EmployeesReport.enLevel.High);
            List<Employee> employees = Employee.GetListOfEmployees();
            foreach (Employee employee in employees)
            {
                if(employee.PerformanceRate >= 90 ) Highreport.employees.Add(employee);
                else if (employee.PerformanceRate >= 60) Medreport.employees.Add(employee);
                else Lowreport.employees.Add(employee);
            }

            Lowreport.Print();
            Console.WriteLine();
            Medreport.Print();
            Console.WriteLine();
            Highreport.Print();
        }
        public static void ReportUsingTsql ()
        {
            DataTable dt = DAEmployee.GetValues();
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine();
                Console.WriteLine("=====================================================");
                Console.WriteLine($"The Category is {row["PerformanceCategory"]}");
                Console.WriteLine($"The number of employees is {row["NumberOfEmployees"]}");
                Console.WriteLine($"The average salary is {row["AvgSalary"]}");
                Console.WriteLine("=====================================================");
                Console.WriteLine();

            }

        }
    }
}
