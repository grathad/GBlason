using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace Grammar
{
    /// <summary>
    /// Helper method for keyword to identify (mostly for error management) the key that is found versus the key that was expected
    /// </summary>
    public class FoundExpected
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="found"></param>
        /// <param name="expected"></param>
        public FoundExpected(ParsedKeyword found, string expected)
            : this(
                  new List<ParsedKeyword> { found },
                  new List<IEnumerable<string>> { new List<string> { expected } })
        {
        }

        /// <summary>
        /// Create a text version of the list of expected key words separating the words with "," and "or"
        /// </summary>
        /// <param name="found">The found keyword</param>
        /// <param name="expected">The expected word</param>
        public FoundExpected(ParsedKeyword found, IEnumerable<IEnumerable<string>> expected)
            : this(
                  new List<ParsedKeyword> { found },
                  expected)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="found"></param>
        /// <param name="expected"></param>
        public FoundExpected(IEnumerable<ParsedKeyword> found, IEnumerable<IEnumerable<string>> expected)
        {
            found.ToList();
            if (expected == null)
            {
                Expected = null;
                return;
            }
            var expectedList = expected.ToList();
            var idx = 0;
            foreach (var expect in expectedList)
            {
                Expected += expect.Aggregate((a, b) => a + " " + b);
                if (++idx == expectedList.Count - 1)
                {
                    Expected += " or ";
                }
                else if (idx != expectedList.Count)
                {
                    Expected += ", ";
                }
            }
        }
        
        //public string FoundString
        //{
        //    get
        //    {
        //        return Enumerable.Aggregate<string>(Found.Select(keyword => keyword.Value), (a, b) => a + " " + b);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public string Expected { get; }
    }
}