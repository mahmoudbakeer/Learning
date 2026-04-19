Tree<string> CompanyTree = new Tree<string>("CEO");

// Add direct children to the Root
CompanyTree.Root.Add("CMO");
CompanyTree.Root.Add("CTO");

// Find existing nodes and add children to them
CompanyTree.Find("CMO").Add("Marketing Manager");
CompanyTree.Find("CTO").Add("Developer");

// Print tree
Console.WriteLine("Company Hierarchy:");
Tree<string>.PrintTree(CompanyTree.Root);

Console.ReadKey();