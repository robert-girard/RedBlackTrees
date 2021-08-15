using System;
using System.Collections.Generic;

namespace RedBlackTrees
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RedBlackTree<int> myTree = new RedBlackTree<int>();
            myTree.addNode(10);
            myTree.addNode(20);
            myTree.addNode(1);
            myTree.addNode(100);
            myTree.addNode(200);
            myTree.addNode(50);
            myTree.addNode(75);
            List<int> inOrder = myTree.inOrderTransversal();

            foreach (int i in inOrder)
            {
                Console.WriteLine(i);
            }
        }
    }
}