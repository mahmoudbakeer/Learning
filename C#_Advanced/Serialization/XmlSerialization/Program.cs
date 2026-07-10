// See https://aka.ms/new-console-template for more information
using System.Xml;
using System.Xml.Serialization;

public class Program
{
    public static void Main()
    {
        var person = new Person("Mahmoud", 22);
        string xmlContent = XmlSerialized(person);
        Console.WriteLine();
        Console.WriteLine(xmlContent);
        Console.WriteLine();
        File.WriteAllText("XMLContent.xml", xmlContent);
        string xml = File.ReadAllText("XMLContent.xml");
        Person newxml = XmlDeserializer(xml);
        newxml.Print();
        Console.WriteLine();
    }

    private static Person XmlDeserializer(string xml)
    {
        Person p;

        XmlSerializer ser = new XmlSerializer(typeof(Person));
        using (TextReader sr = new StringReader(xml))
        {
            p = ser.Deserialize(sr) as Person;
        }
        return p;
    }

    private static string XmlSerialized(Person p)
    {
        string result = "";
        var XmlSer = new XmlSerializer(typeof(Person));
        using (var sw = new StringWriter())
        {
            using (var writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
            {
                XmlSer.Serialize(writer, p);
                result = sw.ToString();
            }
        }

        return result;
    }
}

[Serializable]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person() { }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Print()
    {
        Console.WriteLine($"Person Name is {Name} and Age is {Age}");
    }
}
