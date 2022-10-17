using System;
using System.Collections;
using System.Collections.Generic;

namespace SymbolTableLibrary
{
    public class ListSymbolTable<K, V> : IEnumerable<K>
    {
        internal class Node<K, V>
        {
            public K key;
            public V value;
            public Node<K, V> next;

            public Node()
            {
                key = default;
                next = null;
            }

            public Node(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                this.next = null;
            }

            public override string ToString()
            {
                return $"{key}:{value}";
            }
        }
        //end of node class
        private Node<K, V> head;
        private Node<K, V> tail;
        private int count;

        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// creates a new symbol table
        /// </summary>
        public ListSymbolTable()
        {
            head = null;
            count = 0;
        }

        private Node<K,V> WalkToNode(int index)
        {
            if (index < 0 || index > count)
            {
                throw new IndexOutOfRangeException("The list is not that long");
            }
            Node<K, V> curr = head;
            for (int i = 0; i < index; i++)
            {
                curr = curr.next;
            }
            return curr;
        }

        private Node<K, V> WalkToNode(K key)
        {
            Node<K, V> curr = head;
            while(curr != null)
            {
                if (key.Equals(curr.key))
                {
                    return curr;
                }
                curr = curr.next;
            }
            throw new KeyNotFoundException($"Error: the key '{key}' is not in the symbol table):");
        }

        public void Add(K key, V value)
        {
            Node<K, V> newNode = new Node<K, V>(key, value);
            newNode.next = head;
            head = newNode;
            count++;
        }

        public void Remove(K key)
        {
            Node<K, V> prev = head;

            if(count > 0 && head.key.Equals(key))
            {
                Node<K, V> toRemove = head;
                head = head.next;
                toRemove.key = default;
                toRemove.value = default;
                toRemove.next = null;
                count--;
                return;
            }
            while (prev != null && prev.next != null)
            {
                //if node in front contains search key
                if (prev.next.key.Equals(key))
                {
                    Node<K, V> toRemove = prev.next;
                    prev.next = toRemove.next;
                    toRemove.key = default;
                    toRemove.value = default;
                    toRemove.next = null;
                    count--;
                    return;
                }
                prev = prev.next;
            }
        }

        public V this[K key]
        {
            get
            {
                Node<K, V> node = WalkToNode(key);
                if (node == null)
                {
                    string msg = $" ${key} could not be found in the symbol table";
                    throw new KeyNotFoundException(msg);
                }
                else
                {
                    return node.value;
                }
                
            }
            set
            {
                Node<K, V> node = WalkToNode(key);
                if(node == null)
                {
                    Add(key, value);
                }
                else
                {
                    node.value = value;
                }
                
            }
        }

        public bool Contains(K key)
        {
            try
            {
                Node<K, V> curr = WalkToNode(key);
                return true;
            }
            catch(KeyNotFoundException knfe)
            {
                return false;
            }
            
        }

        public void Clear()
        {
            this.head = null;
            count = 0;
        }

        public override string ToString()
        {
            return $"key: {this.head.key} value: {this.head.value}";
        }
        public IEnumerator<K> GetEnumerator()
        {
            Node<K, V> node = head;
            while (node != null)
            {
                yield return node.key;
                node = node.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

