using System;

namespace Trees
{
    public class Node<T> : IEquatable<Node<T>>
    {
        public Node<T> parent;
        public Node<T> left;
        public Node<T> right;
        public T value;

        public Node(T value)
        {
            this.value = value;
            this.left = null;
            this.right = null;
            this.parent = null;
        }
        public Node(T value, Node<T> parent)
        {
            this.value = value;
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
                return this.value.Equals(other.value);
            }
        }
    }
}
