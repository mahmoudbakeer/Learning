using System;
using System.IO;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        Person person = new Person { Name = "Ahmad Ayman", Age = 21 };

        // --- Serialize manually ---
        using (BinaryWriter writer = new BinaryWriter(File.Open("person.bin", FileMode.Create)))
        {
            writer.Write(person.Name);
            writer.Write(person.Age);
        }

        // --- Deserialize manually ---
        using (BinaryReader reader = new BinaryReader(File.Open("person.bin", FileMode.Open)))
        {
            string name = reader.ReadString();
            int age = reader.ReadInt32();
            Person deserializedPerson = new Person { Name = name, Age = age };

            Console.WriteLine($"Name: {deserializedPerson.Name}, Age: {deserializedPerson.Age}");
        }

        Console.ReadKey();
    }
}