namespace MyFirstWebApp.Entities.Repositories
{
    public class EmployeeRepo
    {
        private static List<Employee> employees = new List<Employee>()
        {
            new Employee(1, "Hamed", "Developer", 2000),
            new Employee(2, "Mahmoud", "Saler", 1000),
            new Employee(3, "Sohael", "Engineer", 2000),
        };

        public static List<Employee> GetEmployees() => employees;

        public static void AddEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                employees.Add(employee);
            }
        }

        public static bool UpdateEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                var ExistEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);
                if (ExistEmployee is not null)
                {
                    ExistEmployee.Name = employee.Name;
                    ExistEmployee.Salary = employee.Salary;
                    ExistEmployee.Position = employee.Position;
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteEmployee(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
            {
                employees.Remove(employee);
                return true;
            }
            return false;
        }
    }
}
