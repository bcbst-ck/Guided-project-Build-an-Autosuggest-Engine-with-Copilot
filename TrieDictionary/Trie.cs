/// <summary>
/// Represents a Trie (prefix tree) data structure.
/// </summary>
public class Trie
{
    private TrieNode root = new TrieNode();

    /// <summary>
    /// Initializes a new instance of the <see cref="Trie"/> class.
    /// </summary>
    public Trie()
    {
    }

    /// <summary>
    /// Inserts a word into the Trie.
    /// </summary>
    public bool Insert(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                current.Children[c] = new TrieNode(c);
            }
            current = current.Children[c];
        }
        if (current.IsEndOfWord)
        {
            return false; // Word already exists
        }
        current.IsEndOfWord = true;
        return true;
    }

    /// <summary>
    /// Searches for a word in the Trie.
    /// </summary>
    public bool Search(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                return false;
            }
            current = current.Children[c];
        }
        return current.IsEndOfWord;
    }

    /// <summary>
    /// Deletes a word from the Trie.
    /// </summary>
    public bool Delete(string word)
    {
        return _delete(root, word, 0);
    }

    // Helper method to delete a word from the trie by recursively removing its nodes
    private bool _delete(TrieNode root, string word, int index)
    {
        // Base case: we've reached the end of the word
        if (index == word.Length)
        {
            // If the current node is not the end of a word, we can't delete it
            if (!root.IsEndOfWord)
            {
                return false;
            }
            // Otherwise, we can delete it by setting the end of word flag to false
            root.IsEndOfWord = false;
            // And returning true to indicate that the node can be deleted
            return true;
        }
        // Recursive case: we haven't reached the end of the word yet
        char currentChar = word[index];
        // If the current node doesn't have a child with the current character, we can't delete it
        if (!root.HasChild(currentChar))
        {
            return false;
        }
        // Otherwise, we can delete it if its child can be deleted
        bool canDelete = _delete(root.Children[currentChar], word, index + 1);
        // If the child can be deleted and the current node is not the end of a word, we can delete it
        if (canDelete && !root.IsEndOfWord)
        {
            root.Children.Remove(currentChar);
            return true;
        }
        // Otherwise, we can't delete it
        return false;
    }

    /// <summary>
    /// Provides auto-suggestions based on a given prefix.
    /// </summary>
    public List<string> AutoSuggest(string prefix)
    {
        TrieNode current = root;
        foreach (char c in prefix)
        {
            if (!current.HasChild(c))
            {
                return new List<string>(); // No suggestions
            }
            current = current.Children[c];
        }
        return GetAllWordsWithPrefix(current, prefix);
    }

    /// <summary>
    /// Retrieves all words in the Trie that start with a given prefix.
    /// </summary>
    private List<string> GetAllWordsWithPrefix(TrieNode node, string prefix)
    {
        List<string> words = new List<string>();
        if (node.IsEndOfWord)
        {
            words.Add(prefix);
        }
        foreach (var child in node.Children)
        {
            words.AddRange(GetAllWordsWithPrefix(child.Value, prefix + child.Key));
        }
        return words;
    }

    /// <summary>
    /// Retrieves all words stored in the Trie.
    /// </summary>
    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    /// <summary>
    /// Prints the structure of the Trie to the console.
    /// </summary>
    public void PrintTrieStructure()
    {
        _printTrieNodes(root);
    }

    /// <summary>
    /// Recursively prints the nodes of the Trie.
    /// </summary>
    private void _printTrieNodes(TrieNode node, string format = " ", bool isLastChild = true)
    {
        Console.WriteLine(format + (isLastChild ? "└── " : "├── ") + node._value);
        format += isLastChild ? "    " : "│   ";
        var children = node.Children.Values.ToList();
        for (int i = 0; i < children.Count; i++)
        {
            _printTrieNodes(children[i], format, i == children.Count - 1);
        }
    }

    /// <summary>
    /// Provides spelling suggestions for a given word based on the Levenshtein distance.
    /// </summary>
    public List<string> GetSpellingSuggestions(string word)
    {
        List<string> suggestions = new List<string>();
        foreach (var w in GetAllWords())
        {
            if (LevenshteinDistance(word, w) <= 2)
            {
                suggestions.Add(w);
            }
        }
        return suggestions;
    }

    /// <summary>
    /// Computes the Levenshtein distance between two strings.
    /// </summary>
    private int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[] d = new int[n + 1];

        for (int j = 0; j <= n; j++)
        {
            d[j] = j;
        }

        for (int i = 1; i <= m; i++)
        {
            int prev = d[0];
            d[0] = i;
            for (int j = 1; j <= n; j++)
            {
                int temp = d[j];
                d[j] = Math.Min(Math.Min(d[j] + 1, d[j - 1] + 1), prev + (s[i - 1] == t[j - 1] ? 0 : 1));
                prev = temp;
            }
        }

        return d[n];
    }
}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }
    public char _value;

    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}
