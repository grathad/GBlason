using System.Collections.Generic;
using System.Linq;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// This is a decorator around an actual parser, used to control the tree of parsing calls (root / children / leaves)
    /// Contains helper function to help represent the parser it contains
    /// </summary>
    public class ParserNode
    {
        /// <summary>
        /// Create a new node, with the corresponding parser <paramref name="pb"/> starting at a defined position <paramref name="pos"/>
        /// </summary>
        /// <param name="pb">The parser that will be included in this node</param>
        /// <param name="pos">The position at which the parser will start parsing</param>
        public ParserNode(ParserBase pb, ITokenParsingPosition pos)
        {
            Parser = pb;
            Position = pos;
            if (Parser != null)
            {
                Parser.Error += ParserOnError;
            }
        }

        /// <summary>
        /// Save the Errors raised by the <see cref="Parser"/> in this node
        /// </summary>
        /// <param name="sender">The sended of the error event</param>
        /// <param name="parserError">The actual error</param>
        internal void ParserOnError(object sender, ParserError parserError)
        {
            Errors.Add(parserError);
        }

        /// <summary>
        /// Since we are listening to events, we need to clean up the reference to avoid memory leaks
        /// </summary>
        ~ParserNode()
        {
            if (Parser != null)
            {
                Parser.Error -= ParserOnError;
            }
        }

        /// <summary>
        /// The parser that is represented by this node in the parsing tree
        /// </summary>
        public virtual ParserBase Parser { get; }

        /// <summary>
        /// The starting position of this parsing node execution
        /// The result position is in the <see cref="Result"/> <see cref="Position"/>
        /// </summary>
        public virtual ITokenParsingPosition Position { get; }

        /// <summary>
        /// The list of errors that have been raised by the parsing effort
        /// </summary>
        public virtual IList<ParserError> Errors { get; } = new List<ParserError>();

        /// <summary>
        /// The list of Children that are created during the parsing effort, and participate in the creation of the <see cref="ParserTree"/>
        /// </summary>
        public virtual IList<ParserNode> Children { get; } = new List<ParserNode>();

        /// <summary>
        /// The actual result of the parsing effort
        /// </summary>
        public virtual ITokenResult Result { get; set; }

        /// <summary>
        /// The parent for this node, participate in the definition of the <see cref="ParserTree"/>
        /// </summary>
        public virtual ParserNode Parent { get; set; }

        /// <summary>
        /// Flag to know if the parser did execute or is only created and pending for the result of its children parsers
        /// </summary>
        public virtual NodeParserStatus Status { get; set; }

        /// <summary>
        /// True if the parser contained in this node succesfully found a result, otherwise false
        /// </summary>
        public virtual bool ValidResult
        {
            get
            {
                return Result?.ResultToken != null && Result?.Position != null && Position != null;
            }
        }

        internal const string NoPositionText = "no position";
        internal const string NoTypeText = "no type";
        internal const string NoResultText = "no valid result";
        internal const string InExecution = "parsing in progress";
        internal const string Created = "node created";
        internal const string Executed = "parsing completed";

        /// <summary>
        /// Define a easy to read result from a parser node for debug purposes
        /// </summary>
        /// <returns>A string version of the node extracted from the parsing execution / status</returns>
        public override string ToString()
        {
            var position = NoPositionText;
            if (Position != null)
            {
                position = Position.Start.ToString();
            }
            var type = NoTypeText;
            if (Parser != null)
            {
                type = Parser.Type.ToString();
            }
            var statusMessage = string.Empty;
            switch (Status)
            {
                case NodeParserStatus.Created:
                    statusMessage = Created;
                    break;
                case NodeParserStatus.Started:
                    statusMessage = InExecution;
                    break;
                case NodeParserStatus.Executed:
                    statusMessage = InExecution;
                    break;

            }
            var resultMessage = string.Empty;

            if (Result?.ResultToken != null && Result?.Position != null && Position != null)
            {
                resultMessage =
                    $"found using {Result.Position.Start - Position.Start} key words";
            }
            else if (Errors?.Any() ?? false)
            {
                resultMessage = $"{Errors.Count} error(s)";
            }

            var totalString = $"{position},{type},{statusMessage} {resultMessage}";
            return totalString;
        }
    }

    public enum NodeParserStatus
    {
        Created = 0,
        Started = 1,
        Executed = 2
    }
}