using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTrees
{
    class RedBlackTree<T>
    {
        private class Node<T>
        {
            public Node<T> parent;
            public Node<T> left;
            public Node<T> right;
            public T value;
            public bool colour;

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

        private void cases(Node<T> node)
        {

        }

        private void insert(Node<T> node, T value)
        {
            int comparison = Comparer<T>.Default.Compare(node.value, value);                                //for generic comparisons, returns 0 if ==, -ve if <, or +ve if >
            if (comparison == 0)
            {
                throw new DuplicateValueException(string.Format("Value: {0} already in tree", value));
            } else if (comparison < 0)
            {
                if (node.left == null)
                {
                    node.left = new Node<T>(value, node);
                    this.cases(node.left);
                } else
                {
                    this.insert(node.left, value);
                }
            } else if (comparison > 0)
            {
                if (node.right == null)
                {
                    node.right = new Node<T>(value, node);
                    this.cases(node.right);
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
                return;
            }

            this.insert(this.root, value);   
        }

        public void deleteNode(T value)
        {

        }

        public List<T> DFS()
        {
            return new List<T>();
        }

        public List<T> BFS()
        {
            return new List<T>();
        }

        public List<T> inOrderTransversal()
        {
            return new List<T>();
        }

    }
}
