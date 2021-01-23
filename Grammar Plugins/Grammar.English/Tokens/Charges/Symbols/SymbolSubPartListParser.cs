using System.Collections.Generic;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a <see cref="TokenNames.SymbolSubPartList"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolSubPartList"/> :=  
    /// <see cref="TokenNames.SymbolSubPartGroup"/>.  
    /// ((<see cref="TokenNames.LightSeparator"/>. <see cref="TokenNames.SymbolSubPartGroup"/>)* 
    /// <see cref="TokenNames.And"/> <see cref="TokenNames.SymbolSubPartGroup"/>.)?
    /// </para>
    /// </summary>
    /// <remarks>A list of subpart is not responsible for any shared properties of the component of the list, thus each group define its own tincture and shared properties, if the group have only one shared tincture for example
    /// then the list will be only including one group</remarks>
    internal class SymbolSubPartListParser : ContainerParser
    {
        public SymbolSubPartListParser(IParserPilot factory = null)
            : base(TokenNames.SymbolSubPartList, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //if no valid subpart group we stop
            var firstSubPartGroup = Parse(origin, TokenNames.SymbolSubPartGroup);
            if (firstSubPartGroup == null) {
                return null; 
            }
            if(firstSubPartGroup.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SymbolSubPartGroup, origin.Start);
                return null;
            }

            var lastGroup = firstSubPartGroup;
            var tempTree = new List<IToken>();
            origin = lastGroup.Position;
            tempTree.Add(lastGroup.ResultToken);

            //then we consume as many light seprator followed by a symbol sub part as we can
            while (lastGroup.Position.Start < ParserPilot.LastPosition)
            {
                //trying the coma
                var coma = Parse(lastGroup.Position, TokenNames.LightSeparator);
                if (coma == null)
                {
                    //no match we bail out
                    break;
                }
               
                //it should be followed by a symbol sub part
                var nextGroup = Parse(coma.Position, TokenNames.SymbolSubPartGroup);
                if (nextGroup == null) { break; }
                //if we get there we are good to try another iteration in the loop
                //we attach the children so far in the temp tree
                lastGroup = nextGroup;
                tempTree.Add(coma.ResultToken);
                tempTree.Add(nextGroup.ResultToken);
                //saving the latest position of this parse in case we stop here
                origin = lastGroup.Position;
            }

            //then we try to consume the and sub part to complete
            var pand = Parse(origin, TokenNames.And);

            if (pand != null)
            {
                var p = Parse(pand.Position, TokenNames.SymbolSubPartGroup);
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