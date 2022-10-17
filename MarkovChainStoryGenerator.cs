using System;
using SymbolTableLibrary;
using LinkedListLibrary;
using TreeTableLibrary;
using System.Collections.Generic;
using System.Diagnostics;
namespace MarkovStories
{
    public class MarkovEntry
    {
        private string key;
        private int count;
        public MyLinkedList<char> suffixes;
        public MarkovEntry(string k)
        {
            this.key = k;
            this.count = 0;
            suffixes = new MyLinkedList<char>();
        }
        public void AddSuffix(char ch)
        {
            this.suffixes.Add(ch);
            count++;
        }
        public string Key
        {
            get { return this.key; }
        }
        public override string ToString()
        {
            return $"key: {key} value: {this.suffixes}";
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            int n = Int32.Parse(args[1]); //second command line argument--length of states
            int l = Int32.Parse(args[2]); //third command line argument--length of entire story
            string text = File.ReadAllText(args[0]);
            MarkovEntry initialState = new MarkovEntry(getInitial(text, n));
            char initialSuffix = text[n];
            initialState.AddSuffix(initialSuffix);
            Stopwatch DictWatch = new Stopwatch();
            DictWatch.Start();
            SortedDictionary<string, MarkovEntry> DictStates = ParseTextDict(args[0], n);
            string DictStory = MakeDictStory(DictStates, l, n, initialState);
            Console.WriteLine("---------------------");
            Console.WriteLine("DotNet Sorted Dictionary \n");
            Console.WriteLine(DictStory);
            DictWatch.Stop();
            Stopwatch ListWatch = new Stopwatch();
            ListWatch.Start();
            ListSymbolTable<string, MarkovEntry> ListStates = ParseTextList(args[0], n);
            string ListStory = MakeListStory(ListStates, l, n, initialState);
            Console.WriteLine("---------------------");
            Console.WriteLine("Custom Linked List Symbol Table \n");
            Console.WriteLine(ListStory);
            ListWatch.Stop();
            Stopwatch TreeWatch = new Stopwatch();
            TreeWatch.Start();
            TreeTable<string, MarkovEntry> TreeStates = ParseTextTree(args[0], n);
            string TreeStory = MakeTreeStory(TreeStates, l, n, initialState);
            Console.WriteLine("---------------------");
            Console.WriteLine("Custom Binary Tree Symbol Table \n");
            Console.WriteLine(TreeStory + "\n\n");
            TreeWatch.Stop();
            int sourceLength = text.Length;
            Console.WriteLine($"The length of text file is {sourceLength} characters");
            float DictTime = DictWatch.ElapsedMilliseconds;
            Console.WriteLine($"The DotNet Sorted Dictionary took {DictTime} ms");
            float ListTime = ListWatch.ElapsedMilliseconds;
            Console.WriteLine($"The Custom Linked List Symbol Table took {ListTime} ms");
            float TreeTime = TreeWatch.ElapsedMilliseconds;
            Console.WriteLine($"The Custom Binary Tree Symbol Table took {TreeTime} ms");
        }
        public static SortedDictionary<string, MarkovEntry> ParseTextDict(string txt, int sublen)
        {
            SortedDictionary<string, MarkovEntry> db = new SortedDictionary<string, MarkovEntry>();
            string lines = File.ReadAllText(txt);
            for (int i = 0; i < lines.Length - (sublen - 1); i++) //first line length is 18 +1 too short ends before finishing line
            {
                char next = getSuffix(lines, i, sublen);
                string state = lines.Substring(i, sublen);
                if (!db.ContainsKey(state))
                {
                    MarkovEntry NewState = new MarkovEntry(state);
                    NewState.AddSuffix(next);
                    db.Add(state, NewState);
                }
                else
                {
                    db[state].AddSuffix(next);
                }
            }
            return db;
        }
        public static ListSymbolTable<string, MarkovEntry> ParseTextList(string txt, int sublen)
        {
            ListSymbolTable<string, MarkovEntry> db = new ListSymbolTable<string, MarkovEntry>();
            string lines = File.ReadAllText(txt);
            for (int i = 0; i < lines.Length - (sublen - 1); i++) //first line length is 18 +1 too short ends before finishing line
            {
                char next = getSuffix(lines, i, sublen);
                string state = lines.Substring(i, sublen);
                if (!db.Contains(state))
                {
                    MarkovEntry NewState = new MarkovEntry(state);
                    NewState.AddSuffix(next);
                    db.Add(state, NewState);
                }
                else
                {
                    db[state].AddSuffix(next);
                }
            }

            return db;
        }
        public static TreeTable<string, MarkovEntry> ParseTextTree(string txt, int sublen)
        {
            TreeTable<string, MarkovEntry> db = new TreeTable<string, MarkovEntry>();
            string lines = File.ReadAllText(txt);
            for (int i = 0; i < lines.Length - (sublen - 1); i++) //first line length is 18 +1 too short ends before finishing line
            {
                char next = getSuffix(lines, i, sublen);
                string state = lines.Substring(i, sublen);
                if (!db.Contains(state))
                {
                    MarkovEntry NewState = new MarkovEntry(state);
                    NewState.AddSuffix(next);
                    db.Add(state, NewState);
                }
                else
                {
                    db[state].AddSuffix(next);
                }
            }
            return db;
        }
        public static char RandomLetter(MarkovEntry state)
        {
            Random rnd = new Random();
            int ran = rnd.Next(0, state.suffixes.Count);
            return state.suffixes[ran];
        }
        public static string getInitial(string txt, int sublen)
        {
            return txt.Substring(0, sublen);
        }
        public static string MakeDictStory(SortedDictionary<string, MarkovEntry> db, int limit, int slength, MarkovEntry initial)
        {
            string story = "";
            string starting = initial.Key + initial.suffixes[0];
            story += starting;
            MarkovEntry state = initial;
            while (story.Length < limit)
            {
                if (story.Length == 0)
                {
                    string newState = story.Substring(story.Length, slength);
                    story += RandomLetter(db[newState]);
                }
                else
                {
                    string newState = story.Substring(story.Length - slength, slength);
                    story += RandomLetter(db[newState]);
                }
            }
            return story;
        }
        public static string MakeListStory(ListSymbolTable<string, MarkovEntry> db, int limit, int slength, MarkovEntry initial)
        {
            string story = "";
            string starting = initial.Key + initial.suffixes[0];
            story += starting;
            MarkovEntry state = initial;
            while (story.Length < limit)
            {
                if (story.Length == 0)
                {
                    string newState = story.Substring(story.Length, slength);
                    story += RandomLetter(db[newState]);
                }
                else
                {
                    try
                    {
                        string newState = story.Substring(story.Length - slength, slength);
                        story += RandomLetter(db[newState]);
                    }
                    catch (KeyNotFoundException)
                    {
                        string newState = story.Substring(story.Length - slength, slength);
                        story += RandomLetter(db[newState]);
                    }

                }
            }
            return story;
        }
        public static string MakeTreeStory(TreeTable<string, MarkovEntry> db, int limit, int slength, MarkovEntry initial)
        {
            string story = "";
            string starting = initial.Key + initial.suffixes[0];
            story += starting;
            MarkovEntry state = initial;
            while (story.Length < limit)
            {
                if (story.Length == 0)
                {
                    string newState = story.Substring(story.Length, slength);
                    story += RandomLetter(db[newState]);
                }
                else
                {
                    string newState = story.Substring(story.Length - slength, slength);
                    story += RandomLetter(db[newState]);
                }
            }
            return story;
        }
        public static char getSuffix(string text, int i, int sublen)
        {
            char next;
            if (text.Length == i + sublen - 1)
            {
                next = '\n';
            }
            else
            {
                try
                {
                    next = text[i + sublen];
                }
                catch (Exception)
                {
                    next = ' ';
                }
            }
            return next;
        }
    }
}