using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class ColouredNode<T> : Node<T>, IEquatable<ColouredNode<T>>
    {
        public bool colour;                     // Use true for red and false for black for rule 1

        public new ColouredNode<T> parent
        {
            get => (ColouredNode<T>)base.parent;
            set
            {
                base.parent = (Node<T>)value;
            }
        }
        public new ColouredNode<T> left
        {
            get => (ColouredNode<T>)base.left;
            set
            {
                base.left = (Node<T>)value;
            }
        }
        public new ColouredNode<T> right
        {
            get => (ColouredNode<T>)base.right;
            set
            {
                base.right = (Node<T>)value;
            }
        }

        public ColouredNode(T value) : base(value)
        {
            this.colour = true;
        }

        public ColouredNode(T value, ColouredNode<T> parent) : base(value, parent)
        {
            this.colour = true;
        }

        public bool Equals(ColouredNode<T> other)
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
}
