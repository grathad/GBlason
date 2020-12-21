using System.Threading.Tasks;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// This class represent an element in the grammar that only group a "or" based choices between mutliple type of children
    /// </summary>
    public class ContainerOrParser : ContainerParser
    {
        internal readonly TokenNames[] Options;

        /// <summary>
        /// Create a new instance of the generic parser for token representing an "or"
        /// </summary>
        /// <param name="type">the token type (parent token)</param>
        /// <param name="pilot">the pilot used for the parsing</param>
        /// <param name="options">the options of children (child1 or child2 or ... childn)</param>
        public ContainerOrParser(
            TokenNames type,
            IParserPilot pilot,
            params TokenNames[] options)
            : base(type, pilot)
        {
            Options = options;
        }

        /// <summary>
        /// Try consume the token and its children based on the options available.
        /// Attach the result to the parent, and return it
        /// </summary>
        /// <param name="origin">the position from which to parse</param>
        /// <returns>Null if no match are found, or the current token with the child attached</returns>
        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin, Options);

            if (result != null)
            {
                AttachChild(result.ResultToken);
            }
            
            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}