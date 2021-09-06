using System;
using System.Collections.Generic;

namespace Trees
{
    class Program
    {
        static void test1()
        {
            Console.WriteLine("Hello World!\n");
            RedBlackTree<int> myTree = new RedBlackTree<int>();

            int[] vals = { 10, 20, 1, 100, 200, 50, 75, 500, 750, 800 };
            //int[] vals = { 10, 20, 1, 100, 200, 50, 75};

            foreach (int i in vals)
            {
                Console.WriteLine(String.Format("Inserting value {0} to Tree", i));
                myTree.insert(i);
                myTree.printTree();
            }



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

        static void test2()
        {
            RedBlackTree<int> myTree = new RedBlackTree<int>();

            Random rnd = new Random();

            int len = rnd.Next(10, 30);

            for (int i = 0; i < len; i++)
            {
                int insertVal = rnd.Next(0,len*10);
                while (myTree.contains(insertVal))
                {
                   insertVal = rnd.Next(0, len * 10);
                }
                
                Console.WriteLine(String.Format("Inserting value {0} to Tree", insertVal));
                myTree.insert(insertVal);
                List<int> myList = new List<int>(myTree.inOrderTransversal());
                Console.WriteLine(string.Join(", ", myList));
                myTree.printTree();
            }
        }

        static void deleteTest()
        {
            RedBlackTree<int> myTree = new RedBlackTree<int>();

            Random rnd = new Random();

            int len = rnd.Next(10, 30);

            for (int i = 0; i < len; i++)
            {
                int insertVal = rnd.Next(0, len * 10);
                while (myTree.contains(insertVal))
                {
                    insertVal = rnd.Next(0, len * 10);
                }

                Console.WriteLine(String.Format("Inserting value {0} to Tree", insertVal));
                myTree.insert(insertVal);
            }

            List<int> myList = new List<int>(myTree.inOrderTransversal());
            Console.WriteLine(string.Join(", ", myList));
            myTree.printTree();

            int delLen = rnd.Next(1, (int)Math.Floor((double)(len/2)));

            for (int j = 0; j < delLen; j++)
            {
                int deleteitem = rnd.Next(myList.Count);
                Console.WriteLine(string.Format("Deleting item: {0}", deleteitem));
                myTree.delete(deleteitem);
                myList = new List<int>(myTree.inOrderTransversal());
                Console.WriteLine(string.Join(", ", myList));
                myTree.printTree();
            }
        }

        static void Main(string[] args)
        {

            test2();

        }
    }
}

// TODO: Unit testing (monte Carlo, other value types, critical functions (rotate, insert, delete, sorting)), Serializable, file saving loading, add heaps