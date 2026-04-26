
using System.Collections;

Hashtable table = [];
// adding element
table.Add("FirstKey", "FirstValue");
table.Add("SecondKey", "SecondValue");

// accessing the element
Console.WriteLine($"The Key : {"FirstKey"} - The Value : {table["FirstKey"]}");


// removing the element
table.Remove("FirstKey");


