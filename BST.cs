using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    class BST<T> : Tree<T>, IEnumerable<T>
    {

        public BST() : base() { }

        private void insert(Node<T> node, T value)
        {
            int comparison = Comparer<T>.Default.Compare(node.value, value);                                //for generic comparisons, returns 0 if ==, -ve if >, or +ve if <
            if (comparison == 0)
            {
                this.size -= 1;
                throw new DuplicateValueException(string.Format("Value: {0} already in tree", value));
            }
            else if (comparison > 0)
            {
                if (node.left == null)
                {
                    node.left = new Node<T>(value, node);
                    //this.insertCases(node.left);
                }
                else
                {
                    this.insert(node.left, value);
                }
            }
            else if (comparison < 0)
            {
                if (node.right == null)
                {
                    node.right = new Node<T>(value, node);
                    //this.insertCases(node.right);
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
            this.size += 1;
            if (this.root == null)
            {
                this.root = new Node<T>(value);
                return;
            }

            this.insert(this.root, value);
        }

        protected static Node<T> successor(Node<T> node)
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

        protected void deleteRecursive(Node<T> node)
        {

            if (node.left != null && node.right != null)            //node is internal node, swap value with successor and recurse 
            {
                Node<T> successor = BST<T>.successor(node);
                T temp = node.value;
                node.value = successor.value;
                successor.value = temp;
                this.deleteRecursive(successor);
            }
            else if (node.left != null)
            {
                node.value = node.left.value;
                node.left = null;
            }
            else if (node.right != null)
            {
                node.value = node.right.value;
                node.right = null;
            }
            else
            {
                if (node.parent.left == node)
                {
                    node.parent.left = null;
                }
                else
                {
                    node.parent.right = null;
                }
            }
            GC.Collect();                                   // Garbage collect deleted node

        }

        public override void delete(T value)
        {
            Node<T> node = this.find(this.root, value);

            if (node == null)
            {
                throw new NodeDoesNotExistException(String.Format("The tree does not contain value: {0}", value));
            }

            this.size -= 1;
            this.deleteRecursive(node);
        }

        protected Node<T> find(Node<T> node, T value)
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

        public override bool contains(T value)
        {
            /// returns bools if the tree contains the given value

            if (this.find(this.root, value) == null)
            {
                return false;
            }
            return true;
        }

        public virtual List<Node<T>> levelTranversal()
        {
            Queue<Node<T>> childeren = new Queue<Node<T>>();
            List<Node<T>> levelOrder = new List<Node<T>>();


            Node<T> currNode;

            childeren.Enqueue(this.root);
            while (childeren.Count > 0)
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
            get
            {
                foreach (ColouredNode<T> i in this.levelTranversal())
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

        // Interface implementation methods

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
