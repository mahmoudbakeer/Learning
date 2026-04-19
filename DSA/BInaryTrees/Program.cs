// now lets study the binary tree 
// from now on this section which include the algorithms and problem solving
// will be different than the previous sections


// declare the binary tree
using BInaryTrees;

BinaryTree<int> bt = new BinaryTree<int>(1);

bt.insert(2);
bt.insert(3);
bt.insert(5);
bt.insert(6);
bt.insert(8);
bt.insert(9);
// tree nodes
Console.WriteLine("Tree  : ");
BinaryTree<int>.Print(bt.root);
Console.WriteLine();
// preorder traversal
bt.PreOrder();
// postorder traversal
bt.PostOrder();
// inorder traversal
bt.InOrder();
