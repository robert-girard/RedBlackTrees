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
            private Node<T> leftChild;
            private Node<T> rightChild;
            private T value;
            private bool colour;

            public Node(T value)
            {
                this.value = value;
                this.colour = true;
                this.leftChild = null;
                this.rightChild = null;
            }
        }

        private Node<T> root;

        public RedBlackTree()
        {
            this.root = null;
        }

        public void addNode(T value)
        {
            
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
