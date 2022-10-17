using System;

namespace TreeTableLibrary
{
    public class TreeTable<K, V> : IEnumerable<K> where K : IComparable<K>
    {
        class Node<K, V>
        {
            public K key;
            public V value;
            public Node<K, V> L;
            public Node<K, V> R;
            public int count;

            public Node(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                this.L = null;
                this.R = null;
                this.count = 1;
            }

            public override string ToString()
            {
                string Lchild = "NULL";
                if (L != null)
                {
                    Lchild += $"{L.key} <- ";
                }

                string Rchild = "NULL";
                if (L != null)
                {
                    Rchild += $"{R.key} <- ";
                }

                return $"{Lchild} <- {key}:{value} -> {Rchild}";
            }
        }
        private Node<K, V> root;

        public TreeTable()
        {
            root = null;
        }

        public int Count
        {
            get
            {
                if (root == null)
                {
                    return 0;
                }
                else
                {
                    return root.count;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                Node<K, V> node = WalkToNode(key, root);
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
                Node<K, V> node = WalkToNode(key, root);
                if (node == null)
                {
                    Add(key, value);
                }
                else
                {
                    node.value = value;
                }

            }
        }
        public void Add(K key, V value)
        {
            //handles all situations including an empty tree
            root = Add(key, value, root);
        }

        private Node<K, V> Add(K key, V value, Node<K, V> subroot)
        {
            if (subroot == null)
            {
                return new Node<K, V>(key, value);
            }

            int compare = key.CompareTo(subroot.key);

            if (compare == -1) //-1 for <
            {
                subroot.L = Add(key, value, subroot.L);
            }
            else if (compare == +1) // +1 for >
            {
                subroot.R = Add(key, value, subroot.R);
            }
            else // 0 for =
            {
                throw new ArgumentException($"A node with key '{key}' already exists in the symbol table");
            }

            subroot.count++;
            return subroot;
        }
        public K Max()
        {
            return Max(root);
        }

        private K Max(Node<K, V> root)
        {
            if (root.R != null)
            {
                return Max(root.R);
            }
            else
            {
                return root.R.key;
            }
        }

        public K Min()
        {
            return Min(root);
        }

        private K Min(Node<K, V> root)
        {
            if (root.L != null)
            {
                return Max(root.L);
            }
            else
            {
                return root.L.key;
            }
        }
        private Node<K, V> WalkToNode(K nodeKey, Node<K, V> subroot)
        {
            if (subroot == null)
            {
                return null;
            }
            int compare = nodeKey.CompareTo(subroot.key);
            if (compare == -1)
            {
                return WalkToNode(nodeKey, subroot.L);
            }
            else if (compare == +1)
            {
                return WalkToNode(nodeKey, subroot.R);
            }
            else
            {
                return subroot;
            }
        }

        public K Predecessor(K fromKey)
        {
            Node<K, V> curr = WalkToNode(fromKey, root);
            if (curr == null)
            {
                throw new ArgumentException($"Error: {fromKey} does not exist");
            }
            if (curr.L == null)
            {
                throw new InvalidOperationException($"Error: {fromKey} does not have a predecessor");
            }
            if (curr.L != null && curr.L.R == null)
            {
                return curr.L.key;
            }
            else
            {
                return Max(curr.L);
            }
        }

        public K Successor(K fromKey)
        {
            Node<K, V> curr = WalkToNode(fromKey, root);
            if (curr == null)
            {
                throw new ArgumentException($"Error: {fromKey} does not exist");
            }
            if (curr.R == null)
            {
                throw new InvalidOperationException($"Error: {fromKey} does not have a predecessor");
            }
            if (curr.R != null && curr.R.L == null)
            {
                return curr.R.key;
            }
            else
            {
                return Max(curr.R);
            }
        }
        public void PrintKeysInOrder()
        {
            PrintKeysInOrder(root);
        }

        private void PrintKeysInOrder(Node<K, V> subroot)
        {
            if (subroot != null)
            {
                PrintKeysInOrder(subroot.L);
                Console.WriteLine(subroot.key);
                PrintKeysInOrder(subroot.R);
            }
        }
        private System.Collections.Generic.IEnumerable<K> GetEnumerator(Node<K, V> subroot)
        {
            if (subroot != null)
            {
                foreach (K key in GetEnumerator(subroot.L)) yield return key;
                yield return subroot.key;
                foreach (K key in GetEnumerator(subroot.R)) yield return key;
            }

        }

        public System.Collections.Generic.IEnumerator<K> GetEnumerator()
        {
            foreach (K key in GetEnumerator(root))
            {
                yield return key;

            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


