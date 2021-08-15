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
            myTree.addNode(750);
            myTree.addNode(7500);
            myTree.addNode(8000);
            List<int> inOrder = myTree.inOrderTransversal();

            Console.WriteLine("\nCreating List: ");
            foreach (int i in inOrder)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\nuisng IEnumerable: ");
            foreach (int i in myTree)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("\nBreadth Transversal: ");
            foreach (int i in myTree.breadthTransversal)
            {
                Console.WriteLine(i);
            }
        }
    }
}

// TODO: Unit testing, DFS/BFS, print in tree form