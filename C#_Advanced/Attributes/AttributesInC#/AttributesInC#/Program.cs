using System;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

#pragma warning disable SYSLIB0011 // Ignore BinaryFormatter obsolete warning
/*
==========================================
What is an Attribute in C#?
==========================================

An attribute is a special piece of metadata (information about code) that you attach
to classes, methods, properties, fields, parameters, and other program elements.

It tells the compiler, the .NET runtime, or other frameworks something about the code.
By itself, an attribute usually does NOT change how the code executes. Instead, it
provides extra information that can be read and acted upon.

Attributes inherit from the System.Attribute class.

Syntax:
    [AttributeName]
    public class MyClass { }

The "Attribute" suffix is optional.
For example:
    [Serializable]
is actually using the SerializableAttribute class.

--------------------------------------------------

Why use Attributes?
-------------------

Attributes are used to describe code without modifying its implementation. They let
the compiler or frameworks perform special behavior automatically.

Examples:
- Mark a class as serializable.
- Ignore a field during serialization.
- Mark old code as obsolete.
- Execute a method only in DEBUG builds.
- Validate user input.
- Configure Entity Framework database mappings.
- Control JSON serialization.

--------------------------------------------------

When should you use Attributes?
-------------------------------

Use attributes whenever you want to provide additional information about your code
instead of writing extra logic yourself.

Common scenarios include:
- Serialization
- Validation
- Database mapping (Entity Framework)
- JSON/XML serialization
- Debugging
- Code analysis
- Creating your own custom metadata

--------------------------------------------------

How do Attributes work?
-----------------------

1. You attach an attribute to a code element.
2. The compiler stores that attribute as metadata in the compiled assembly.
3. During compilation or at runtime, the compiler, .NET runtime, or frameworks
   read the metadata (often using Reflection).
4. Based on the attribute, they perform a specific action.

Example:
    [Obsolete]
    void OldMethod() { }

The compiler reads the Obsolete attribute and displays a warning whenever
OldMethod() is used.

==========================================
*/
// =========================
// Serializable Demo
// =========================
[Serializable]
class Person
{
    public string Name;
    public int Age;

    // This field will NOT be serialized.
    [NonSerialized]
    public string Password;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("===== Serializable & NonSerialized =====\n");

        Person p = new Person
        {
            Name = "Mahmoud",
            Age = 21,
            Password = "123456"
        };

        Console.WriteLine("Before Serialization:");
        Console.WriteLine($"Name     : {p.Name}");
        Console.WriteLine($"Age      : {p.Age}");
        Console.WriteLine($"Password : {p.Password}");

        // Serialize object
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fs = new FileStream("person.bin", FileMode.Create))
        {
            formatter.Serialize(fs, p);
        }

        // Deserialize object
        Person restored;

        using (FileStream fs = new FileStream("person.bin", FileMode.Open))
        {
            restored = (Person)formatter.Deserialize(fs);
        }

        Console.WriteLine("\nAfter Deserialization:");
        Console.WriteLine($"Name     : {restored.Name}");
        Console.WriteLine($"Age      : {restored.Age}");
        Console.WriteLine($"Password : {restored.Password}");
        // Password is null because of [NonSerialized]

        Console.WriteLine("\n===== Conditional =====\n");

        Log("This message only appears in DEBUG mode.");

        Console.WriteLine("Main method continues normally.");

        Console.WriteLine("\n===== Obsolete =====\n");

        OldMethod();      // Generates a compiler warning
        NewMethod();
    }

    // =========================
    // Conditional Demo
    // =========================
    [Conditional("DEBUG")]
    static void Log(string message)
    {
        Console.WriteLine("DEBUG LOG: " + message);
    }

    // =========================
    // Obsolete Demo
    // =========================
    [Obsolete("OldMethod() is deprecated. Use NewMethod() instead.")]
    static void OldMethod()
    {
        Console.WriteLine("Old Method Executed");
    }

    static void NewMethod()
    {
        Console.WriteLine("New Method Executed");
    }
}
