using System;

namespace Utils.IO
{
    /// <summary>
    /// Wrapper for the <see cref="System.IO.Path"/>
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 
        /// </summary>
        public static Func<string, string> GetDirectoryName = System.IO.Path.GetDirectoryName;

        /// <summary>
        /// 
        /// </summary>
        public static Func<string, string, string> Combine = System.IO.Path.Combine;
    }
}
