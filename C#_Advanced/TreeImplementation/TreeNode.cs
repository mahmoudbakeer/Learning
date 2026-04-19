public class TreeNode<T>
{
    public T Value { get; set; }
    public List<TreeNode<T>> Children { get; set; }

    public TreeNode(T value)
    {
        Value = value;
        Children = new List<TreeNode<T>>();
    }

    public void Add(T value)
    {
        Children.Add(new TreeNode<T>(value));
    }
}