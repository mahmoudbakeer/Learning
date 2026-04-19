public class Tree<T>
{
    // Best Practice: The top node of a tree is called the "Root"
    public TreeNode<T> Root { get; set; }

    public Tree(T value)
    {
        Root = new TreeNode<T>(value);
    }

    // Public method to start the search
    public TreeNode<T> Find(T value)
    {
        return FindRecursive(Root, value);
    }

    // Private recursive helper method to dig through all branches
    private TreeNode<T> FindRecursive(TreeNode<T> currentNode, T valueToFind)
    {
        // Base case: if the branch is empty, return null
        if (currentNode == null) return null;

        // If the current node matches, we found it!
        if (EqualityComparer<T>.Default.Equals(currentNode.Value, valueToFind))
        {
            return currentNode;
        }

        // If not, ask all the children to search their own branches
        foreach (var child in currentNode.Children)
        {
            var result = FindRecursive(child, valueToFind);

            // If one of the children found it deep down, pass that result back up
            if (result != null)
            {
                return result;
            }
        }

        // If we checked this whole branch and found nothing, return null
        return null;
    }

    public static void PrintTree(TreeNode<T> current, string indent = "")
    {
        if (current == null) return;

        // Indent comes BEFORE the value to push it to the right
        Console.WriteLine(indent + current.Value);

        foreach (var child in current.Children)
        {
            // Increase the indent for the next level deep
            PrintTree(child, indent + "  |- ");
        }
    }
}