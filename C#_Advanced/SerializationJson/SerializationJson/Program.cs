using System;
using System.IO;
// in some older .net versions it will throw an error because you don't have a refrence to Serialization library on you project
// right click on you project SerializationJson and add -> refrences -> search on Serialization and add it
using System.Runtime.Serialization.Json;


[Serializable]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}


class Program
{
    static void Main()
    {
        // Create an instance of the Person class
        Person person = new Person { Name = "Mohammed Abu-Hadhoud", Age = 30 };


        // JSON serialization
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Person));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.WriteObject(stream, person);
            string jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());


            // Save the JSON string to a file (optional)
            File.WriteAllText("person.json", jsonString);
        }


        // Deserialize the object back
        using (FileStream stream = new FileStream("person.json", FileMode.Open))
        {
            Person deserializedPerson = (Person)serializer.ReadObject(stream);
            Console.WriteLine($"Name: {deserializedPerson.Name}, Age: {deserializedPerson.Age}");
        }
    }
}