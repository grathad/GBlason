using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Try to parse the <see cref="ParsedKeyword"/> into a <see cref="Charge"/>
    /// </summary>
    /// <example>
    /// Grammar:
    /// <see cref="TokenNames.PositionnedCharges"/> := <see cref="TokenNames.SimplePositionnedCharges"/> 
    ///                                             | <see cref="TokenNames.ComplexPositionnedCharges"/>
    /// </example>
    internal class PositionnedChargesParser : ContainerParser
    {
        public PositionnedChargesParser(IParserPilot factory = null)
            : base(TokenNames.PositionnedCharges, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var longestOption = Parse(origin, TokenNames.SimplePositionnedCharges);

            //todo: if a complex positionned charge can be included in a complex positionned charge
            //some position can contain others, some can't like a list in a list is not possible, it is just a longer list
            //a on X, Y can be potentially : on X, on Y, Z
            //we need the absolute list of choices and split them in all potential cases and grammars



            //we will need a lot of refactoring, potentially another layer, and see how it is blazonned, for now we only support one level
            //so if we find ourselves again, we stop

            var complex = Parse(origin, TokenNames.ComplexPositionnedCharges);
            if (longestOption == null ||
                complex != null)
            {
                longestOption = complex;
            }

            if (longestOption == null)
            {
                //ErrorWrongChild(parsedKeywords.First());
                return null;
            }
            AttachChild(longestOption.ResultToken);
            origin = longestOption.Position;
            return CurrentToken.AsTokenResult(longestOption);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}