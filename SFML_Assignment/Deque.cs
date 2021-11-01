using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SFML_Assignment
{
    //Node class that represent data that is contained in the deque
    public class Node<T>
    {
        public Node<T> prevLink;
        public Node<T> nextLink;
        public T data;

        public Node(T value)
        {
            data = value;
            prevLink = null;
            nextLink = null;
        }
    }
    public class Deque<T>
    {
        public Node<T> front;
        public Node<T> rear;
        public int size;

        public Deque()
        {
            front = rear = null;
            size = 0;
        }

        //Method to check if deque is empty
        public bool isEmpty()
        {
            return (size == 0);
        }

        //Method to push data to the front of the deque
        public void pushFront(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if(front == null)
            {
                rear = front = newNode;
            }
            else
            {
                newNode.nextLink = front;
                front.prevLink = newNode;
                front = newNode;
            }
            size++;
        }

        //Method to push data to the rear of the deque
        public void pushRear(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if(rear == null)
            {
                front = rear = newNode;
            }
            else
            {
                newNode.prevLink = rear;
                rear.nextLink = newNode;
                rear = newNode;
            }
            size++;
        }

        //Method to remove data from the front of the deque
        public T popFront()
        {
            if (isEmpty())
            {
                Console.WriteLine("Deque is empty");
                throw new Exception("Deque is empty");
            }
            else
            {
                Node<T> temp = front;
                front = front.nextLink;

                if(front == null)
                {
                    rear = null;
                }
                else
                {
                    front.prevLink = null;
                }

                size--;
                return temp.data;
            }
        }

        //Method to remove data from the rear of the deque
        public T popRear()
        {
            if (isEmpty())
            {
                Console.WriteLine("Deque is empty");
                throw new Exception("Deque is empty");
            }
            else
            {
                Node<T> temp = rear;

                rear = rear.prevLink;

                if(rear == null)
                {
                    front = null;
                }
                else
                {
                    rear.nextLink = null;
                }

                size--;
                return temp.data;
            }
        }

        //Method to empty the deque
        public void clear()
        {
            rear = null;
            while (front != null)
            {
                Node<T> temp = front;
                front = front.nextLink;
            }
            size = 0;
        }

    }
}
