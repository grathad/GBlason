using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent a list of properties that are assigned to an element. 
    /// They are usually shared across multiple of them. And start with a <see cref="TokenNames.SharedKeyWord"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SharedProperties"/> := 
    /// <see cref="TokenNames.SharedKeyWord"/>? 
    /// (<see cref="TokenNames.SharedObjectReference"/> | <see cref="TokenNames.SharedPropertyAdverb"/>)* 
    /// <see cref="TokenNames.SharedProperty"/>+
    /// </para>
    /// </summary>
    internal class SharedPropertiesParser : ContainerParser
    {
        public SharedPropertiesParser(IParserPilot factory = null)
            : base(TokenNames.SharedProperties, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            Start(origin.Start);
            ParseOptional(TokenNames.SharedKeyWord);

            int safety = 0;
            while (LastPosition.Start < ParserPilot.LastPosition && safety++ < Configurations.GrammarMaxLoop)
            {
                var result = TryConsumeOr(LastPosition.Start,
                    TokenNames.SharedObjectReference,
                    TokenNames.SharedPropertyAdverb);

                if (result == null)
                {
                    break;
                }
                LastPosition = result.Position;
                CurrentCollection.Add(result.ResultToken);
            }

            ParseOneOrMore(TokenNames.SharedProperty);

            return End();
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}