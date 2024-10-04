using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownRender
{
    class Program
    {
        public class Node<T>
        {
            public T Value;
            public Node<T> Next;

            public Node(T value)
            {
                Value = value;
                Next = null;
            }
        }

        public class Stack<T>
        {
            private Node<T> top;

            public Stack()
            {
                top = null; 
            }

            public void Push(T item)
            {
                Node<T> newNode = new Node<T>(item);
                newNode.Next = top; 
                top = newNode; 
            }

            public T Pop()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("Stack is empty.");

                T value = top.Value; 
                top = top.Next; 
                return value; 
            }

            public T Peek()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("Stack is empty.");

                return top.Value; 
            }

            public bool IsEmpty()
            {
                return top == null; 
            }

            public int Count
            {
                get
                {
                    int count = 0;
                    Node<T> current = top;
                    while (current != null)
                    {
                        count++;
                        current = current.Next;
                    }
                    return count;
                }
            }
        }
    }
}
