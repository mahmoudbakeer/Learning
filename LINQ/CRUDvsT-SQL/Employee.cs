using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDvsT_SQL
{
    public class Employee
    {

        private enum enMode {  AddNew =  1 , Update = 2 };
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public int PerformanceRate { get; set; }
        private enMode _Mode; 
        public Employee()
        {
            ID = -1;
            Name = string.Empty;
            Department = string.Empty;
            Salary = 0;
            PerformanceRate = 0;
            _Mode = enMode.AddNew;
        }
        private Employee(int id , string name , string department , decimal salary , int performancerate)
        {
            ID = id;
            Name = name;
            Department = department;
            Salary = salary;
            PerformanceRate= performancerate;
            _Mode= enMode.Update;
        }
        private static DataTable GetAllEmployees()
        {
            return DAEmployee.GetEmployees();
        }
        public static List<Employee> GetListOfEmployees()
        {
            return Employee.GetAllEmployees().AsEnumerable()
                                        .Select(row => new Employee
                                        (
                                            row.Field<int>("ID"),
                                            row.Field<string>("Name"),
                                            row.Field<string>("Department"),
                                            Convert.ToDecimal(row["Salary"]),
                                            row.Field<int>("PerformanceRating")
                                        ))
                                        .ToList();
        }
        public bool Update()
        {
            return DAEmployee.UpdateEmployeeSalary(this);
        }
    }
}
