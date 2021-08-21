using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    class RedBlackTree<T> : BST<T>, IEquatable<RedBlackTree<T>>
    {
        /* RadBlackTree Rules
         * 
         * 1. Every node has a colour either red or black.
         * 2. The root of the tree is always black.
         * 3. There are no two adjacent red nodes (A red node cannot have a red parent or red child).
         * 4. Every path from a node(including root) to any of its descendants NULL nodes has the same number of black nodes.
         * 
        */

        protected new ColouredNode<T> root
        {
            get => (ColouredNode<T>)base.root;
            set
            {
                base.root = (Node<T>)value;
            }
        }

        public RedBlackTree()
        {
            this.root = null;
            this.size = 0;
        }


        private static void swapColour(ColouredNode<T> node1, ColouredNode<T> node2)
        {
            bool tempColour = node1.colour;
            node1.colour = node2.colour;
            node2.colour = tempColour;
        }

        private void insertCases(ColouredNode<T> node)
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

            ColouredNode<T> uncle = (ColouredNode<T>)getUncle(node);
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

        private void insert(ColouredNode<T> node, T value)
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
                    node.left = new ColouredNode<T>(value, node);
                    this.insertCases(node.left);
                } else
                {
                    this.insert(node.left, value);
                }
            } else if (comparison < 0)
            {
                if (node.right == null)
                {
                    node.right = new ColouredNode<T>(value, node);
                    this.insertCases(node.right);
                }
                else
                {
                    this.insert(node.right, value);
                }
            }

        }

        public override void insert(T value)
        {
            /// permits user to add a value to the tree

            if (this.root == null)
            {
                this.root = new ColouredNode<T>(value);
                this.root.colour = false;                   //Root node should always be black
                this.size = 1;
                return;
            }

            this.insert(this.root, value);
        }

        private static bool colour(ColouredNode<T> node)
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

        protected ColouredNode<T> sibling(ColouredNode<T> node)
        {
            return (ColouredNode<T>)base.sibling((Node<T>)node);
        }

        private void doubleBlackCases(ColouredNode<T> node)
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

            ColouredNode<T> sibling = this.sibling(node);
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

        protected void deleteRecursive(ColouredNode<T> node)
        {

            if (node.left != null && node.right != null)            //node is internal node, swap value with successor and recurse 
            {
                ColouredNode<T> successor = (ColouredNode<T>)RedBlackTree<T>.successor((Node<T>)node);
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

        public override void delete(T value)
        {
            ColouredNode<T> node = (ColouredNode<T>)this.find((Node<T>)this.root, value);

            if (node == null)
            {
                throw new NodeDoesNotExistException(String.Format("The tree does not contain value: {0}", value));
            }

            this.size -= 1;
            this.deleteRecursive(node);
        }

        private HashSet<ColouredNode<T>> getNodes()
        {
            return getNodes(this.root);
        }

        private HashSet<ColouredNode<T>> getNodes(ColouredNode<T> node)
        {
            HashSet<ColouredNode<T>> temp;
            if (node.left == null && node.right == null)
            {
                temp = new HashSet<ColouredNode<T>>();
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
            foreach (ColouredNode<T> i in this.levelTranversal())
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


        private static bool parentComparer(ColouredNode<T> node1, ColouredNode<T> node2)
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


        private static bool treeComparer(ColouredNode<T> node1, ColouredNode<T> node2)
        {
            if (Tree<T>.isLeaf(node1) && isLeaf(node2))
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

        // Interface implementation methods

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
