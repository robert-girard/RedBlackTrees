using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Trees
{
    public abstract class Tree<T>
    {
        protected enum RotationDirection
        {
            left,
            right
        }

        protected Node<T> root;

        public int size { get; protected set; }

        public Tree()
        {
            this.root = null;
            this.size = 0;
            Console.SetBufferSize(Console.BufferWidth, 500);
        }

        public abstract void insert(T value);

        public abstract void delete(T value);

        public abstract bool contains(T value);

        protected static Node<T> getUncle(Node<T> node)
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

        protected Node<T> sibling(Node<T> node)
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

        protected List<Node<T>> getLeaves()
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

        protected List<Node<T>> getLeaves(Node<T> node)
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

        protected void rotate(Node<T> node, RotationDirection dir)
        {
            /// perfomrs rotation on the given node in provided direction

            switch (dir)
            {
                case RotationDirection.left:
                    if (node.parent == null)
                    {
                        this.root = node.right;
                    }
                    else if (node.parent.left == node)
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
                    break;
                case RotationDirection.right:
                    if (node.parent == null)
                    {
                        this.root = node.left;
                    }
                    else if (node.parent.left == node)
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
                    node.parent.right = node;
                    break;
            }
        }

        protected static bool isLeaf(Node<T> node)
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

    }
}