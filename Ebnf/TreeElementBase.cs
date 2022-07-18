namespace Ebnf
{
    public class TreeElementBase
    {
        /// <summary>
        /// The representation of the rule, either the given name in the grammar or a constructed one for groups
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the rules included in this node are optional
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// If the rules included in this node are meant to be repeated more than once (compatible with optional)
        /// </summary>
        public bool IsRepetition { get; set; } = false;

        /// <summary>
        /// If only one of the rules included in this node is meant to be resolved
        /// </summary>
        public bool IsAlternation { get; set; } = false;

        public bool IsGroup { get; set; } = false;

        public bool IsLeaf { get; set; } = false;

        /// <summary>
        /// The raw content of the rule (text representation from the grammar)
        /// </summary>
        public string RulesContent { get; set; }
    }
}