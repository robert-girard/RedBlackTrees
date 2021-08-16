using System;
using System.Collections.Generic;

namespace RedBlackTrees
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\n");
            RedBlackTree<int> myTree = new RedBlackTree<int>();

            int[] vals = { 10, 20, 1, 100, 200, 50, 75, 750, 7500, 8000 };
            
            foreach (int i in vals)
            {
                myTree.addNode(i);
                Console.WriteLine(String.Format("Added value {0} to Tree", i));
            }
            
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

            Console.WriteLine("\nThe tree conains 5: {0}", myTree.contains(5));
            Console.WriteLine("The tree conains 750: {0}", myTree.contains(750));
        }
    }
}

// TODO: Unit testing, DFS/BFS, print in tree form