using System;
using System.IO;

namespace Utils.IO
{
    /// <summary>
    /// Wrapper for <see cref="System.IO.Directory"/>
    /// Used to mock the directory behavior for unit testing purpose
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// Wrapper for <see cref="System.IO.Directory.Exists"/>
        /// </summary>
        public static Func<string,bool> Exists = System.IO.Directory.Exists;
        /// <summary>
        /// Wrapper for <see cref="System.IO.Directory.GetFiles(string, string, SearchOption)"/>
        /// </summary>
        public static Func<string, string, SearchOption, string[]> GetFiles = System.IO.Directory.GetFiles;
    }
}
