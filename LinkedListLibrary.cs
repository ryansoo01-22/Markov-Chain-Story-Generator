using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedListLibrary
{
    public class MyLinkedList<T> : IEnumerable<T>
    {
        internal class Node<T>
        {
            public T data;
            public Node<T> next;

            public Node()
            {
                data = default;
                next = null;
            }

            public Node(T initialValue)
            {
                data = initialValue;
                next = null;
            }
        }//end of node class
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public MyLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Add(T item)
        {
            Node<T> myNode = new Node<T>(item);
            if (head == null && tail == null) // nothing in list
            {
                head = myNode;
                tail = myNode;

            }
            else
            {
                tail.next = myNode;
                tail = myNode;
                myNode.next = null;

            }
            count++;
        }

        public int IndexOf(T item, int startAt = 0)
        {
            int index = startAt;
            Node<T> curr = WalkToIndex(startAt);
            for (int i = 0; i < count; i++)
            {
                if (curr.data.Equals(item))
                {
                    return index;
                }
                curr = curr.next;
                index++;
            }
            return -1;
        }

        private Node<T> WalkToIndex(int index)
        {
            if (index < 0 || index > count)
            {
                throw new IndexOutOfRangeException("The list is not that long");
            }
            Node<T> curr = head;
            for (int i = 0; i < index; i++)
            {
                curr = curr.next;
            }
            return curr;
        }

        public void Insert(T item, int index)
        {
            Node<T> CurrNode = WalkToIndex(index - 1);
            Node<T> NewNode = new Node<T>(item);
            Node<T> AfterNewNode = CurrNode.next;
            CurrNode.next = NewNode;
            NewNode.next = AfterNewNode;
            count++;
        }

        public void RemoveAt(int index)
        {

            if (index != 0)
            {
                Node<T> curr = WalkToIndex(index - 1);
                Node<T> AfterRemNode = curr.next.next;
                curr.next = AfterRemNode;
                count--;
            }
            else
            {
                head = WalkToIndex(index).next;
                count--;
            }
        }

        public void Clear()
        {
            this.head = null;
            this.tail = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            if (IndexOf(item) == -1)
            {
                return false;
            }
            else
            {
                return true;
            };
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> curr = head;
            while (curr != null)
            {
                yield return curr.data;
                curr = curr.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                Node<T> curr = WalkToIndex(index);
                return curr.data;
            }
            set
            {
                Node<T> curr = WalkToIndex(index);
                curr.data = value;
            }
        }

        public T[] ToArray()
        {
            Node<T> curr = head;
            T[] listArr = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                listArr[i] = curr.data;
                curr = curr.next;
            }
            return listArr;
        }

        public override string ToString()
        {
            string MyString = "";
            Node<T> curr = head;
            for (int i = 0; i < this.Count; i++)
            {
                MyString += curr.data;
                MyString += " ";
                curr = curr.next;
            }
            return MyString;
        }

    }

}

