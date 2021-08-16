using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTrees
{
    class RedBlackTree<T> : IEnumerable<T>
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
        private class Node<T>
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
        }

        private Node<T> root;

        public RedBlackTree()
        {
            this.root = null;
        }

        private Node<T> getUncle(Node<T> node)
        {
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
            switch (dir)
            {
                case RotationDirection.left:
                    if (node.parent == null)
                    {
                        this.root = node.right;
                        this.root.parent = null;
                        node.parent = this.root;
                        node.right = this.root.left;
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

        private void swapColour(Node<T> node1, Node<T> node2)
        {
            bool tempColour = node1.colour;
            node1.colour = node2.colour;
            node2.colour = tempColour;
        }


        private void insertCases(Node<T> node)
        {
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

            Node<T> uncle = this.getUncle(node);
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
            int comparison = Comparer<T>.Default.Compare(node.value, value);                                //for generic comparisons, returns 0 if ==, -ve if >, or +ve if <
            if (comparison == 0)
            {
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
            if (this.root == null)
            {
                this.root = new Node<T>(value);
                this.root.colour = false;                   //Root node should always be black
                return;
            }

            this.insert(this.root, value);
        }
       
        private Node<T> find(Node<T> node, T value)
        {
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
            if (this.find(this.root, value) == null)
            {
                return false;
            }
            return true;
        }

        private Node<T> successor(Node<T> node)
        {
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

        public void deleteNode(T value)
        {
            throw new NotImplementedException();
        }

        private List<T> levelTranversal()
        {
            Queue<Node<T>> childeren = new Queue<Node<T>>();
            List<T> levelOrder = new List<T>();


            Node<T> currNode;

            childeren.Enqueue(this.root);
            while(childeren.Count > 0)
            {
                currNode = childeren.Dequeue();
                levelOrder.Add(currNode.value);
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
                foreach (T i in this.levelTranversal())
                {
                    yield return i;
                }
            }
        }

        private void transverseInOrder(Node<T> node, List<T> ordered)
        {
            if (node == null)
            {
                return;
            }

            this.transverseInOrder(node.left, ordered);

            ordered.Add(node.value);

            this.transverseInOrder(node.right, ordered);
        }

        public List<T> inOrderTransversal()
        {
            List<T> ordered = new List<T>();
            this.transverseInOrder(this.root, ordered);
            return ordered;
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
        
    }
}
