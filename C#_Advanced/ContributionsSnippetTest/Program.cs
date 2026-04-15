// <Snippet207>
 static void UnionByKeySelectorExample()
{
    (int ProductId, string Name , decimal Price)[] localProducts =
    {
                    (101, "Laptop", 1000m),                 
                    (102, "Mouse", 100m),
                    (103, "Keyboard", 120m)
                };

    (int ProductId, string Name, decimal Price)[] warehouseProducts =
    {
                    (102, "Mouse", 120m),      // Duplicate ProductId (already in local)
                    (104, "Monitor", 800m),
                    (101, "Laptop", 1000m)     // Duplicate ProductId (already in local)
                };
    var combinedProducts =
        localProducts.UnionBy(
            warehouseProducts,
            product => product.ProductId
        );

    foreach (var product in combinedProducts)
    {
        Console.WriteLine($"{product.ProductId}: {product.Name} - ${product.Price}");
    }

    /*
    This code produces the following output:

    101: Laptop - $1000
    102: Mouse - $100
    103: Keyboard - $120
    104: Monitor - $800
    */
}
// </Snippet207>

// <Snippet208>
 static void UnionByComparerExample()
{
    (string Email, string FullName)[] marketingList =
    {
                    ("Mahmoud.Doe@example.com", "Mahmoud Doe"),
                    ("alice.smith@example.com", "Alice Smith")
                };

    (string Email, string FullName)[] salesList =
    {
                    ("ALICE.SMITH@EXAMPLE.COM", "Alice S."), // Duplicate email, different casing
                    ("Sara.jones@example.com", "Sara Jones")
                };

    var combinedList =
        marketingList.UnionBy(
            salesList,
            contact => contact.Email,
            StringComparer.OrdinalIgnoreCase
        );

    foreach (var contact in combinedList)
    {
        Console.WriteLine($"{contact.FullName} ({contact.Email})");
    }

    /*
    This code produces the following output:

    Mahmoud Doe (Mahmoud.Doe@example.com)
    Alice Smith (alice.smith@example.com)
    Sara Jones (Sara.jones@example.com)
    */
}
// </Snippet208>

UnionByKeySelectorExample();
