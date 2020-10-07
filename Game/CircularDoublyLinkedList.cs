using System;
using System.Collections;
using System.Collections.Generic;


namespace Game
{
    public class CircularDoublyLinkedList<T>: IEnumerable<T>
    {
        Node<T> head;
        int count;
        public int Count { get { return count; } }
        public Node<T> Head { get; set; }
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
            {
                head = node;
                head.Next = node;
                head.Previous = node;
            }
            else
            {
                node.Previous = head.Previous;
                node.Next = head;
                head.Previous.Next = node;
                head.Previous = node;
            }
            count++;
        }

        public Node<T> Find(T item)
        {
            Node<T> node = FindNode(head, item);
            return node;

        }

        Node<T> FindNode(Node<T> node, T valueToCompare)
        {
            Node<T> result = null;
            if (Equals(node.Data, valueToCompare))
            {
                result = node;
            }
            else if (result == null && node.Next != head)
            {
                result = FindNode(node.Next, valueToCompare);
            }
            return result;
        }
    
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }
    }
}
