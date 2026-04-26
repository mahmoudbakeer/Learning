using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDvsT_SQL
{
    public class EmployeesReport
    {
        public List<Employee> employees { get; set; }
        private enLevel _Level;
        private string _LevelText;
        public enum enLevel { High , Medium , Low};
        public EmployeesReport(enLevel Level)
        {
            _Level = Level;
            employees = new List<Employee>();
        }

        public double GetAverageSalary()
        {
            if (!employees.Any()) return 0.0;
            return (Convert.ToDouble(employees.Average(x => x.Salary)));
        }

        public void Print()
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine($"The Catrgory is {_Level}");
            Console.WriteLine($"The number of employees is {employees.Count()}");
            Console.WriteLine($"The average salary is {this.GetAverageSalary()}");
            Console.WriteLine("=====================================================");
        }

    }
}
