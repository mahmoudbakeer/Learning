List<int> list1 = [1, 2, 3, 4];
List<int> list2 = [5, 6 , 7 , 8, 1 , 2];

// the concat will concatinate any two collection of same type into one new collection
var res = list1.Concat(list2);
Console.WriteLine("The Concatinate of two list is : ");
foreach (var item in res)
{
    Console.Write($"{item} ");
}
Console.WriteLine();
// the union will add two collection of same type togather excluding the duplicated values in two of the collection
Console.WriteLine("The Union of two list is : ");
var UnionRes = list1.Union(list2);
foreach (var item in UnionRes)
{
    Console.Write($"{item} ");
}
Console.WriteLine();
// the UnionBy extenstion is used to to union the two collection but it will combare between the elements based on KeySelector
// means that the if we have two collection of records , class you can specify the comparison parmameters
List<Pet> Pets1 = new List<Pet>() { 
    new Pet(1 , "Hory", "Cat"),    
    new Pet(4 , "Lucy" , "Dog")
};
List<Pet> Pets2 = new List<Pet>()
{
    new Pet(3 , "Max", "Dog"),
    new Pet(1 , "Hory", "Cat")    
};
var PetsResult = Pets1.UnionBy(Pets2 , p => new {p.ID , p.Name});
Console.WriteLine("The result of UninonBy ");
foreach (var item in PetsResult)
{
    Console.WriteLine($"{item.ID} - {item.Name} - {item.Type}");
}
public class Pet
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int ID { get; set; }
    public Pet(int id , string name , string type)
    {
       Name = name;
        Type = type;
        ID = id;
    }
}
