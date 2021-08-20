using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTrees
{
    class RedBlackTree<T> : IEnumerable<T>, IEquatable<RedBlackTree<T>>
    {
        /* RadBlackTree Rules
         * 
         * 1. Every node has a colour either red or black.
         * 2. The root of the tree is always black.
         * 3. There are no two adjacent red nodes (A red node cannot have a red parent or red child).
         * 4. Every path from a node(including root) to any of its descendants NULL nodes has the same number of black nodes.
         * 
        */

        private enum RotationDirection
        {
            left,
            right
        }
        private class Node<T> : IEquatable<Node<T>>
        {
            public Node<T> parent;
            public Node<T> left;
            public Node<T> right;
            public T value;
            public bool colour;                     // Use true for red and false for black for rule 1

            public Node(T value)
            {
                this.value = value;
                this.colour = true;
                this.left = null;
                this.right = null;
                this.parent = null;
            }
            public Node(T value, Node<T> parent)
            {
                this.value = value;
                this.colour = true;
                this.left = null;
                this.right = null;
                this.parent = parent;
            }

            public bool Equals(Node<T> other)
            {
                if (other == null)
                {
                    return false;
                }
                else 
                {
                    return (this.colour == other.colour && this.value.Equals(other.value));
                }
            }
        }

        private Node<T> root;

        public int size { get; private set; }

        public RedBlackTree()
        {
            this.root = null;
            this.size = 0;
        }

        private static Node<T> getUncle(Node<T> node)
        {
            /// finds and returns the uncle node of the provided node or null of none exist

            Node<T> parent;
            if (node.parent == null)
            {
                return null;                                // I dont think this should ever happen maybe make exception?
            }

            parent = node.parent;

            if (parent.parent == null)
            {
                return null;                                // I dont think this should ever happen maybe make exception?
            } 
            else if (parent == parent.parent.left)
            {
                return parent.parent.right;
            }
            else
            {
                return parent.parent.left;
            }
        }

        private void rotate(Node<T> node, RotationDirection dir)
        {
            /// perfomrs rotation on the given node in provided direction

            switch (dir)
            {
                case RotationDirection.left:
                    if (node.parent == null)
                    {
                        this.root = node.right;
                        this.root.parent = null;
                        node.parent = this.root;
                        node.right = this.root.left;
                        this.root.left.parent = node;
                        this.root.left = node;
                    } 
                    else
                    {
                        if (node.parent.left == node)
                        {
                            node.parent.left = node.right;
                        }
                        else
                        {
                            node.parent.right = node.right;
                        }
                        node.right.parent = node.parent;
                        node.parent = node.right;
                        node.right = node.right.left;
                        node.parent.left = node;
                    }
                    break;
                case RotationDirection.right:
                    if (node.parent == null)
                    {
                        this.root = node.left;
                        this.root.parent = null;
                        node.parent = this.root;
                        node.left = this.root.right;
                        this.root.right.parent = node;
                        this.root.right = node;
                    }
                    else
                    {
                        if (node.parent.left == node)
                        {
                            node.parent.left = node.left;
                        }
                        else
                        {
                            node.parent.right = node.left;
                        }
                        node.left.parent = node.parent;
                        node.parent = node.left;
                        node.left = node.left.right;
                        node.parent.left = node;
                    }
                    break;
            }
        }

        private static void swapColour(Node<T> node1, Node<T> node2)
        {
            bool tempColour = node1.colour;
            node1.colour = node2.colour;
            node2.colour = tempColour;
        }

        private void insertCases(Node<T> node)
        {
            /// checks the case for a given inserted node and performs rotation/colour swap as per RedBlack Tree Algorithm

            if (node == null)
            {
                return;
            }
            else if (node.parent == null)
            {
                node.colour = false;                        //Root node should always be black
                return;
            }
            else if (node.parent.colour == false)
            {
                return;                                     //if parent is red then you are done
            }

            Node<T> uncle = getUncle(node);
            bool uncleColour;
            if (uncle == null)
            {
                uncleColour = false;
            }
            else
            {
                uncleColour = uncle.colour;
            }


            if (uncleColour == true)
            {
                node.parent.colour = false;
                uncle.colour = false;
                node.parent.parent.colour = true;
                this.insertCases(node.parent.parent);
            }
            else
            {
                if (node == node.parent.left)
                {
                    if (node.parent == node.parent.parent.left)
                    {
                        swapColour(node.parent, node.parent.parent);
                        this.rotate(node.parent.parent, RotationDirection.right);
                    }
                    else
                    {
                        this.rotate(node.parent, RotationDirection.right);
                        swapColour(node.parent, node.parent.parent);
                        this.rotate(node.parent.parent, RotationDirection.left);
                    }
                }
                else
                {
                    if (node.parent == node.parent.parent.left)
                    {
                        this.rotate(node.parent, RotationDirection.left);
                        swapColour(node.parent, node.parent.parent);
                        this.rotate(node.parent.parent, RotationDirection.right);

                    }
                    else
                    {
                        swapColour(node.parent, node.parent.parent);
                        this.rotate(node.parent.parent, RotationDirection.left);
                    }
                }
                this.insertCases(node.parent.parent);
            }

        }

        private void insert(Node<T> node, T value)
        {
            /// performs BST like insertion and calls the helper funciton insertCases to handle RedBlack specific updates including rotations/colour changes

            this.size += 1;
            int comparison = Comparer<T>.Default.Compare(node.value, value);                                //for generic comparisons, returns 0 if ==, -ve if >, or +ve if <
            if (comparison == 0)
            {
                this.size -= 1;
                throw new DuplicateValueException(string.Format("Value: {0} already in tree", value));
            } else if (comparison > 0)
            {
                if (node.left == null)
                {
                    node.left = new Node<T>(value, node);
                    this.insertCases(node.left);
                } else
                {
                    this.insert(node.left, value);
                }
            } else if (comparison < 0)
            {
                if (node.right == null)
                {
                    node.right = new Node<T>(value, node);
                    this.insertCases(node.right);
                }
                else
                {
                    this.insert(node.right, value);
                }
            }

        }

        public void addNode(T value)
        {
            /// permits user to add a value to the tree

            if (this.root == null)
            {
                this.root = new Node<T>(value);
                this.root.colour = false;                   //Root node should always be black
                this.size = 1;
                return;
            }

            this.insert(this.root, value);
        }
       
        private Node<T> find(Node<T> node, T value)
        {
            /// recursively search tree for the provided value and returns it if found, else returns null

            if (node == null)
            {
                return null;
            }

            int comparison = Comparer<T>.Default.Compare(node.value, value);

            if (comparison == 0)
            {
                return node;
            }
            else if (comparison > 0)
            {
                return this.find(node.left, value);
            }
            else
            {
                return this.find(node.right, value);
            }
        }

        public bool contains(T value)
        {
            /// returns bools if the tree contains the given value
         
            if (this.find(this.root, value) == null)
            {
                return false;
            }
            return true;
        }

        private static Node<T> successor(Node<T> node)
        {
            /// returns the successor node, next largest valued node, of the given node
            
            Node<T> currNode = node.right;
            if (currNode == null)
            {
                return null;
            }

            while (currNode.left != null)
            {
                currNode = currNode.left;
            }

            return currNode;
        }

        private static Node<T> sibling(Node<T> node)
        {
            if (node.parent == null)
            {
                throw new NodeDoesNotExistException("Node does not have parent so cannot have sibing");
            }
            if (node == node.parent.left)
            {
                return node.parent.right;
            }
            else
            {
                return node.parent.left;
            }
        }

        private static bool colour(Node<T> node)
        {
            if (node == null)
            {
                return false;
            }
            else
            {
                return node.colour;
            }
        }

        private void doubleBlackCases(Node<T> node)
        {
            if (node == null)
            {
                return;
            }
            else if (node.parent == null)
            {
                node.colour = false;
                return;
            }

            Node<T> sibling = RedBlackTree<T>.sibling(node);
            if (sibling == null)
            {
                doubleBlackCases(node.parent);
            }
            else if (sibling.colour == false)                                                
            {
                if (colour(sibling.left) == true || colour(sibling.right) == true)  // case 3.2.a
                {
                    if (sibling.parent.left == sibling)
                    {
                        if (colour(sibling.left) == true)                           // case 3.2.a.i
                        {
                            sibling.left.colour = false;
                            this.rotate(node.parent, RotationDirection.right);
                        }
                        else                                                        // case 3.2.a.ii
                        {
                            sibling.right.colour = false;
                            this.rotate(sibling, RotationDirection.left);
                            this.rotate(node.parent, RotationDirection.right);
                        }
                    }
                    else
                    {
                        if (colour(sibling.right) == true)                          // case 3.2.a.iii
                        {
                            sibling.right.colour = false;
                            this.rotate(node.parent, RotationDirection.left);
                        }
                        else                                                        // case 3.2.a.iv
                        {
                            sibling.left.colour = false;
                            this.rotate(sibling, RotationDirection.right);
                            this.rotate(node.parent, RotationDirection.left);
                        }

                    }
                }
                else                                                                // case 3.2.b
                {
                    sibling.colour = true;
                    if (node.parent.colour == true)
                    {
                        doubleBlackCases(node.parent);
                    }
                }
            }
            else                                                                    // case 3.2.c
            {
                sibling.colour = false;
                sibling.parent.colour = true;
                if (sibling.parent.left == sibling)                                 // case 3.2.c.i
                {
                    this.rotate(node.parent, RotationDirection.right);
                }
                else                                                                // case 3.2.c.ii
                {
                    this.rotate(node.parent, RotationDirection.left);
                }
                doubleBlackCases(node);
            }

        }

        private void deleteRecursive(Node<T> node)
        {

            if (node.left != null && node.right != null)            //node is internal node, swap value with successor and recurse 
            {
                Node<T> successor = RedBlackTree<T>.successor(node);
                T temp = node.value;
                node.value = successor.value;
                successor.value = temp;
                this.deleteRecursive(successor);
            }

            if (node.left != null)
            {
                if (node.colour || node.left.colour)                //both wont be true since true is red and redBlack trees dont have 2 reds as parent/child
                {
                    node.value = node.left.value;
                    node.colour = false;
                    node.left = null;
                    GC.Collect();                                   // Garbage collect deleted node
                }
                else
                {
                    this.doubleBlackCases(node);
                }
            }
            else if (node.right != null)
            {
                if (node.colour || node.left.colour)                //both wont be true since true is red and redBlack trees dont have 2 reds as parent/child
                {
                    node.value = node.right.value;
                    node.colour = false;
                    node.right = null;
                    GC.Collect();                                   // Garbage collect deleted node
                }
                else
                {
                    this.doubleBlackCases(node);
                }
            }
            else
            {
                this.doubleBlackCases(node);
            }

        }

        public void deleteNode(T value)
        {
            Node<T> node = this.find(this.root, value);

            if (node == null)
            {
                throw new NodeDoesNotExistException(String.Format("The tree does not contain value: {0}", value));
            }

            this.size -= 1;
            this.deleteRecursive(node);
        }

        private List<Node<T>> levelTranversal()
        {
            Queue<Node<T>> childeren = new Queue<Node<T>>();
            List<Node<T>> levelOrder = new List<Node<T>>();


            Node<T> currNode;

            childeren.Enqueue(this.root);
            while(childeren.Count > 0)
            {
                currNode = childeren.Dequeue();
                levelOrder.Add(currNode);
                if (currNode.left != null)
                {
                    childeren.Enqueue(currNode.left);
                }
                if (currNode.right != null)
                {
                    childeren.Enqueue(currNode.right);
                }
            }
            return levelOrder;

        }

        public IEnumerable<T> breadthTransversal
        {
            get {
                foreach (Node<T> i in this.levelTranversal())
                {
                    yield return i.value;
                }
            }
        }

        private void transverseInOrder(Node<T> node, List<Node<T>> ordered)
        {
            if (node == null)
            {
                return;
            }

            this.transverseInOrder(node.left, ordered);

            ordered.Add(node);

            this.transverseInOrder(node.right, ordered);
        }

        public IEnumerable<T> inOrderTransversal()
        {
            List<Node<T>> ordered = new List<Node<T>>();
            this.transverseInOrder(this.root, ordered);
            return ordered.Select(i => i.value);
        }

        
        public IEnumerator<T> GetEnumerator()
        {
            if (this.root == null)
            {
                yield break;
            }

            foreach (T i in this.inOrderTransversal())
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private HashSet<Node<T>> getNodes()
        {
            return getNodes(this.root);
        }

        private HashSet<Node<T>> getNodes(Node<T> node)
        {
            HashSet<Node<T>> temp;
            if (node.left == null && node.right == null)
            {
                temp = new HashSet<Node<T>>();
                temp.Add(node);
                return temp;
            }
            else if (node.left == null)
            {
                temp = getNodes(node.right);
                temp.Add(node);
                return temp;
            }
            else if (node.right == null)
            {
                temp = getNodes(node.left);
                temp.Add(node);
                return temp;
            }
            else
            {
                temp = getNodes(node.right);
                temp.Add(node);
                temp.UnionWith(getNodes(node.left));
                return temp;
            }
        }

        private static void printAtColoured(String value, int row, int col, bool colour)
        {
            Console.SetCursorPosition(col, row);
            if (colour)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void printTree()
        {
            int maxHeight = (int)Math.Ceiling(Math.Log2(this.size + 1));
            int maxLeaves = (int)Math.Pow(2, maxHeight -1 );
            int nodeLength = 0;
            foreach (T i in this.breadthTransversal)
            {
                int len = i.ToString().Length;
                nodeLength = nodeLength >= len ? nodeLength : len;
            }

            int maxBaseWidth = maxLeaves * nodeLength + (maxLeaves - 1);

            Dictionary<T, (int row, int col, bool colour)> nodeLocationColour = new Dictionary<T, (int row, int col, bool colour)>();  // tuble is: row, col, colour
            nodeLocationColour[this.root.value] = (0, (int)(maxBaseWidth/2), this.root.colour);
            foreach (Node<T> i in this.levelTranversal())
            {
                if (i.parent != null)
                {
                    int row, col, offset;
                    row = nodeLocationColour[i.parent.value].row + 1;
                    col = nodeLocationColour[i.parent.value].col;
                    offset = (int)Math.Ceiling((maxBaseWidth / Math.Pow(2, row + 1)));
                    if (i.parent.right == i)
                    {
                        col += offset;
                    }
                    else
                    {
                        col -= offset;
                    }
                    nodeLocationColour[i.value] = (row, col, i.colour);
                }
            }

            var pos =  Console.GetCursorPosition();
            foreach (var i in nodeLocationColour)
            {
                //Console.WriteLine("Value: {0}, row: {1}, col: {2}, colour: {3}", i.Key.ToString(), i.Value.row, i.Value.col, i.Value.colour);
                printAtColoured(i.Key.ToString(), pos.Top + i.Value.row, pos.Left + i.Value.col, i.Value.colour);
            }
            Console.WriteLine(" ");

        }

        private List<Node<T>> getLeaves()
        {
            if (this.size > 0)
            {
                return this.getLeaves(this.root);
            }
            else
            {
                return new List<Node<T>>();
            }
        }

        private List<Node<T>> getLeaves(Node<T> node)
        {
            List<Node<T>> leaves = new List<Node<T>>();

            if (node.left == null && node.right == null)
            { 
                leaves.Add(node);
                return leaves;
            }
            else if (node.left != null)
            {
                leaves.Concat(getLeaves(node.left));
            }
            else if (node.right != null)
            {
                leaves.Concat(getLeaves(node.right));
            }
            return leaves;

        }

        private static bool parentComparer(Node<T> node1, Node<T> node2)
        {
            if (node1.parent == null && node2.parent == null)
            {
                return node1.Equals(node2);
            }    
            else if (node1.parent == null || node2.parent == null)
            {
                return false;
            }
            else if (node1.Equals(node2))
            {
                return parentComparer(node1.parent, node2.parent);
            }
            else
            {
                return false;
            }
        }

        private static bool isLeaf(Node<T> node)
        {
            if (node.left == null && node.right == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool treeComparer(Node<T> node1, Node<T> node2)
        {
            if (isLeaf(node1) && isLeaf(node2))
            {
                if  (node1.Equals(node2))
                {
                    return parentComparer(node1, node2);
                }
                else
                {
                    return false;
                }
            }
            else if (isLeaf(node1) && isLeaf(node2))
            {
                return false;
            }
            else if (!treeComparer(node1.left, node2.left))
            {
                return false;
            }
            else if (!treeComparer(node1.right, node2.right))
            {
                return false;
            }
            else if (node1.Equals(node2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(RedBlackTree<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return treeComparer(this.root, other.root);
        }
    }
}
