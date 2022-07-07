namespace Grammar
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyWordMatch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <param name="raw"></param>
        public KeyWordMatch(string k, string v, string raw)
        {
            Key = k;
            Value = v;
            Raw = raw;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Raw { get; }
    }
}