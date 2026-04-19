using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BInaryTrees
{
    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> root { get; private set; }
        public BinaryTree(T value)
        {
           root = new BinaryTreeNode<T>(value); 
        }

        public void insert(T value)
        {
            if (root == null)
            {
                root = new BinaryTreeNode<T>(value);
                return;
            }
            else
            {
                Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
                queue.Enqueue(root);


                // insert level wise
                while (queue.Count > 0)
                {
                    var Node = queue.Dequeue();
                    if (Node.left == null)
                    {
                        Node.left = new BinaryTreeNode<T>(value);
                        return;
                    }
                    else
                    {
                        queue.Enqueue(Node.left);
                    }
                    if (Node.right == null)
                    {
                        Node.right = new BinaryTreeNode<T>(value);
                        return;
                    }
                    else
                    {
                        queue.Enqueue(Node.right);
                    }
                }
            }
        }
        public static void Print(BinaryTreeNode<T> node)
        {
            PrintTree(node, 0);
        }
        private static void PrintTree(BinaryTreeNode<T> root, int space)
        {
            int COUNT = 10;  // Distance between levels to adjust the visual representation
            if (root == null)
                return;


            space += COUNT;
            PrintTree(root.right, space); // Print right subtree first, then root, and left subtree last


            Console.WriteLine();
            for (int i = COUNT; i < space; i++)
                Console.Write(" ");
            Console.WriteLine(root.value); // Print the current node after space


            PrintTree(root.left, space); // Recur on the left child
        }

        private static void PreOrderTraversal(BinaryTreeNode<T> root)
        {
            if (root is null) return;
            Stack<BinaryTreeNode<T>> st = new Stack<BinaryTreeNode<T>>();
            st.Push(root);
            while (st.Count > 0)
            {
                var node = st.Pop();
                Console.Write($"{node.value} ");
                if (node.right is not null) st.Push(node.right);
                if (node.left is not null) st.Push(node.left);
            }
        }
        public void PreOrder()
        {
            Console.WriteLine("The PreOrder Traversal : ");
            PreOrderTraversal(this.root);
            Console.WriteLine();
        }
        private static void PostOrderTraversal(BinaryTreeNode<T> root)
        {
            if (root is null) return;
            Stack<BinaryTreeNode<T>> st = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = root;
            BinaryTreeNode<T> lastVisited = null;

            while(current is not null || st.Count > 0)
            {
                while(current is not null)
                {
                    st.Push(current);
                    current = current.left;
                }
                var peekNode = st.Peek();
                // here we are standing on a parent if this condition met
                if(peekNode.right is not null && peekNode.right != lastVisited)
                {
                    current = peekNode.right;
                }
                else
                {
                    var last = st.Pop();
                    Console.Write($"{last.value} ");
                    lastVisited = last;
                }
                
            }
        }
        public void PostOrder()
        {
            Console.WriteLine("The PostOrder Traversal : ");
            PostOrderTraversal(this.root);
            Console.WriteLine();
        }
        private static void InOrderTraversal(BinaryTreeNode<T> root)
        {
            if (root is null) return;
            Stack<BinaryTreeNode<T>> st = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = root;

            while(current is not null || st.Count > 0)
            {
                while(current is not null)
                {
                    st.Push(current);
                    current = current.left;
                }
                var node = st.Pop();
                Console.Write($"{node.value} ");
                current = node.right; // no problem if it is null
            }
        }
        public void InOrder()
        {
            Console.WriteLine("The InOrder Traversal : ");
            InOrderTraversal(this.root);
            Console.WriteLine();
        }
    }
}
