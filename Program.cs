using System;
using System.Collections.Generic;

namespace Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!\n");
            RedBlackTree<int> myTree = new RedBlackTree<int>();

            int[] vals = { 10, 20, 1, 100, 200, 50, 75, 500, 750, 800};
            //int[] vals = { 10, 20, 1, 100, 200, 50, 75};

            foreach (int i in vals)
            {
                Console.WriteLine(String.Format("Inserting value {0} to Tree", i));
                myTree.insert(i);
            }

            myTree.printTree();


            IEnumerable<int> inOrder = myTree.inOrderTransversal();

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

// TODO: Unit testing (monte Carlo, other value types, critical functions (rotate, insert, delete, sorting)), Serializable, file saving loading, add heaps