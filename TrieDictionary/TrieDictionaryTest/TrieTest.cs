using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TrieDictionaryTest
{
    [TestClass]
    public class TrieTest
    {
        private Trie dictionary;

        [TestInitialize]
        public void Setup()
        {
            dictionary = new Trie();
        }

        [TestMethod]
        public void TestInsert()
        {
            bool inserted = dictionary.Insert("test");
            Assert.IsTrue(inserted);
            Assert.IsTrue(dictionary.Search("test"));
        }

        [TestMethod]
        public void TestInsertDuplicate()
        {
            dictionary.Insert("test");
            bool inserted = dictionary.Insert("test");
            Assert.IsFalse(inserted);
        }

        [TestMethod]
        public void TestDelete()
        {
            dictionary.Insert("test");
            bool deleted = dictionary.Delete("test");
            Assert.IsTrue(deleted);
            Assert.IsFalse(dictionary.Search("test"));
        }

        [TestMethod]
        public void TestDeleteNonExistent()
        {
            bool deleted = dictionary.Delete("test");
            Assert.IsFalse(deleted);
        }

        [TestMethod]
        public void TestDeletePrefix()
        {
            dictionary.Insert("test");
            dictionary.Insert("testing");
            bool deleted = dictionary.Delete("test");
            Assert.IsTrue(deleted);
            Assert.IsFalse(dictionary.Search("test"));
            Assert.IsTrue(dictionary.Search("testing"));
        }

        [TestMethod]
        public void TestAutoSuggest()
        {
            dictionary.Insert("catastrophe");
            dictionary.Insert("catatonic");
            dictionary.Insert("caterpillar");
            List<string> suggestions = dictionary.AutoSuggest("cat");
            CollectionAssert.AreEqual(new List<string> { "catastrophe", "catatonic", "caterpillar" }, suggestions);
        }

        [TestMethod]
        public void TestGetSpellingSuggestions()
        {
            dictionary.Insert("cat");
            dictionary.Insert("caterpillar");
            dictionary.Insert("catastrophe");
            List<string> suggestions = dictionary.GetSpellingSuggestions("caterpiller");
            CollectionAssert.AreEqual(new List<string> { "caterpillar" }, suggestions);
        }
    }
}
