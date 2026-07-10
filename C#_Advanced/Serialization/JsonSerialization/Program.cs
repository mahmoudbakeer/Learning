using System.CodeDom.Compiler;
using System.Text.Json;
using System.Xml.Serialization;

/*
=========================================================================================
                                    SERIALIZATION
=========================================================================================

What is Serialization?
----------------------
Serialization is the process of converting an object in memory into a format that can
be stored or transmitted. The serialized data can later be reconstructed back into the
original object using deserialization.

Object in Memory
       │
       ▼
Serialization
       │
       ▼
JSON / XML / Binary / Other Formats
       │
       ▼
Store in File, Database, or Send over Network

Deserialization is the reverse process.

JSON / XML / Binary
       │
       ▼
Deserialization
       │
       ▼
Object in Memory


=========================================================================================
Why do we use Serialization?
=========================================================================================

Serialization is useful whenever an object needs to leave the application's memory.

Common use cases include:

1. Saving application data to a file.
2. Reading objects back from files.
3. Sending objects through a network.
4. Communicating with Web APIs.
5. Storing objects inside databases.
6. Caching objects.
7. Logging object data.

Without serialization, only the running program can understand an object because it
exists only in RAM.


=========================================================================================
Types of Serialization
=========================================================================================

1. JSON Serialization
---------------------
Converts an object into JSON.

Example:

{
    "Name": "Mahmoud",
    "Age": 23
}

Advantages:
- Small size.
- Human readable.
- Fast.
- Standard format for REST APIs.
- Supported by almost every programming language.

Used for:
- ASP.NET Core Web APIs
- Mobile applications
- JavaScript applications
- Configuration files
- Data exchange over HTTP


2. XML Serialization
--------------------
Converts an object into XML.

Example:

<Person>
    <Name>Mahmoud</Name>
    <Age>23</Age>
</Person>

Advantages:
- Human readable.
- Supports attributes, namespaces, and schemas.
- Widely used by legacy systems.

Used for:
- SOAP Web Services
- Legacy enterprise applications
- Configuration files
- Systems requiring XML schemas (XSD)


3. Binary Serialization (Legacy)
--------------------------------
Converts an object into binary bytes.

Advantages:
- Very compact.
- Faster than text formats.

Disadvantages:
- Not human readable.
- Not recommended for modern .NET applications.
- BinaryFormatter is obsolete due to security risks.

Used only in very specific scenarios where a custom binary protocol is required.


=========================================================================================
When should each type be used?
=========================================================================================

JSON
----
✔ REST APIs
✔ Modern web applications
✔ Mobile apps
✔ Configuration files
✔ Cross-platform communication

XML
---
✔ Legacy systems
✔ SOAP services
✔ Applications requiring XML schemas
✔ Some enterprise integrations

Binary
------
✔ Custom binary protocols
✔ High-performance internal communication
✔ Rarely used in modern applications


=========================================================================================
About this program
=========================================================================================

This example demonstrates JSON Serialization using the built-in
System.Text.Json library.

Program Flow

Create Person Object
        │
        ▼
Serialize to JSON String
        │
        ▼
Print JSON
        │
        ▼
Deserialize JSON
        │
        ▼
Create a New Person Object
        │
        ▼
Print Object
*/
internal class Program
{
    private static void Main(string[] args)
    {
        Person p = new Person { Name = "Mahmoud", Age = 23 };

        string jsonstr = SerializeObjectToJson(p);

        Console.WriteLine();
        Console.WriteLine(jsonstr);
        Console.WriteLine();
        Person newp = DeserializeFromJson(jsonstr);
        newp.Print();
        Console.WriteLine();
    }

    private static string SerializeObjectToJson(Person p)
    {
        return JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
    }

    private static Person DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<Person>(json);
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public void Print()
    {
        Console.WriteLine($"Name is {Name}, Age is {Age}");
    }
}
