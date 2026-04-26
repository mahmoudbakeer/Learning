public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }


    public Employee(string name, string department, decimal salary)
    {
        Name = name;
        Department = department;
        Salary = salary;
    }
    public override string ToString()
    {
        return $"Name : {Name} - Department : {Department} - Salary : {Salary:C}";
    }
}

