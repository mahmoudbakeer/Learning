using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BInaryTrees
{
    public class BinaryTreeNode<T>
    {
        public T value { get; set; }
        public BinaryTreeNode<T> left { get; set; }
        public BinaryTreeNode<T> right { get; set; }
        public BinaryTreeNode(T value)
        {
            this.value = value;
            left = null;
            right = null;
        }
    }
}
