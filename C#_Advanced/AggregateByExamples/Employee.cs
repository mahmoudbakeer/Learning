


public class Employee
{
    public string Name { get; }
    public string Department { get; }
    public double Salary { get; }

    public Employee(string name , string department , double salary)
    {
        this.Name = name;
        this.Department = department;
        this.Salary = salary;
    }
}
