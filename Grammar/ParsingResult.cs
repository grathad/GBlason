using System.Collections.Generic;
using Format.Elements;

namespace Grammar
{
    /// <summary>
    /// Represent the result of a parsing.
    /// The parsing in the engine is meant to receive, as an input a string (or stream)
    /// And provide, as an output, a <see cref="BaseElement"/>
    /// The <see cref="Format"/> represent the root of the parsing that is being done 
    /// (the parsing might start at another level than a shield for example) 
    /// <para>
    /// The output of a parsing can optionally include errors and warnings
    /// </para>
    /// </summary>
    public class ParsingResult
    {
        /// <summary>
        /// Create a new result from the root of the exepcted format
        /// </summary>
        /// <param name="format"></param>
        public ParsingResult(BaseElement format)
        {
            Format = format;
        }
        
        /// <summary>
        /// The format extracted from the blazon after the parsing was completed
        /// </summary>
        public BaseElement Format { get; }
    }
}