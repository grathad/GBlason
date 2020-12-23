using System.Collections.Generic;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a <see cref="TokenNames.SubPartNameList"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SubPartNameList"/> :=  
    /// <see cref="TokenNames.SubPartName"/>.  
    /// ((<see cref="TokenNames.LightSeparator"/>. <see cref="TokenNames.SubPartName"/>)* 
    /// <see cref="TokenNames.And"/> <see cref="TokenNames.SubPartName"/>.)?
    /// </para>
    /// </summary>
    /// <remarks>A list of subpart name is only a chain of words that matches the definition of a subpart name, that will share other properties at the parent parser level</remarks>
    internal class SubPartNameListParser : ContainerParser
    {
        public SubPartNameListParser(IParserPilot factory = null)
            : base(TokenNames.SubPartNameList, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //if no valid subpart group we stop
            var firstSubPartName = Parse(origin, TokenNames.SubPartName);
            if (firstSubPartName == null) { return null; }

            var lastName = firstSubPartName;
            var tempTree = new List<IToken>
            {
                lastName.ResultToken
            };

            //then we consume as many light separator followed by a symbol sub part name as we can
            while (lastName.Position.Start < ParserPilot.LastPosition)
            {
                //trying the coma
                var coma = Parse(lastName.Position, TokenNames.LightSeparator);
                if (coma == null)
                {
                    //no match we bail out
                    break;
                }
                //it should be followed by a symbol sub part
                var nextName = Parse(origin, TokenNames.SubPartName);
                if (nextName == null) { break; }
                //if we get there we are good to try another iteration in the loop
                lastName = nextName;
                //we attach the children so far in the temp tree
                tempTree.Add(coma.ResultToken);
                tempTree.Add(nextName.ResultToken);
                //saving the latest position of this parse in case we stop here
                origin = nextName.Position;
            }

            //then we try to consume the and sub part to complete
            var pand = Parse(origin, TokenNames.And);

            if (pand != null)
            {
                var p = Parse(pand.Position, TokenNames.SubPartName);
                if (p != null)
                {
                    tempTree.Add(pand.ResultToken);
                    tempTree.Add(p.ResultToken);
                    origin = p.Position;
                }
            }
            //ok this is a valid parse, we are good to attach everything for good
            foreach (var t in tempTree)
            {
                AttachChild(t);
            }

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}