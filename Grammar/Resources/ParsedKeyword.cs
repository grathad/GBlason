namespace Grammar
{
    /// <summary>
    /// Represent a key word that have been parsed
    /// </summary>
    public class ParsedKeyword
    {
        /// <summary>
        /// This the name that is given to a keyword that could not match any of the terms that are defined in the resources
        /// </summary>
        public const string NoKeyword = "NoKeyWord";

        /// <summary>
        /// The key that was detected for this keyword (as defined in the resources)
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The actual value of the keyword
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// The original position of the word in the given input
        /// </summary>
        internal int StartLocation { get; set; }
    }
}