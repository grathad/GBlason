using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ParserBase : IBenchmarker
    {
        /// <summary>
        /// Constructor for a parser base
        /// </summary>
        /// <param name="type">The current type of token expected to be parsed</param>
        /// <param name="parserPilot">The pilot that is used to handle the tree of parsers calls</param>
        protected ParserBase(TokenNames type, IParserPilot parserPilot)
        {
            ParserPilot = parserPilot ?? new ParserPilot(null, null);
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public IParserPilot ParserPilot { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual TokenNames Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ParserError> Error;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ITokenResult> Success;

        public Stopwatch BenchmarkingWatch { get; set; } = new Stopwatch();

        /// <summary>
        /// 
        /// </summary>
        protected List<IToken> Result = new List<IToken>();

        public Guid UniqueId { get; init; } = Guid.NewGuid();

        #region Parsing helpers

        /// <summary>
        /// Parse the given <paramref name="token"/> at the given <paramref name="currentPosition"/> using the currently defined <see cref="ParserPilot"/>
        /// <remarks>The <paramref name="currentPosition"/> is NEVER altered by calling this function and will be as provided after the execution</remarks>
        /// </summary>
        /// <param name="currentPosition">The position from which to parse. This won't be altered</param>
        /// <param name="token">The token that is meant to be parsed</param>
        /// <returns>A token result with the new position AND the resulting token, or null if the parse failed</returns>
        protected virtual ITokenResult Parse(
            ITokenParsingPosition currentPosition,
            TokenNames token = TokenNames.Shield)
        {
            var clonedPosition = currentPosition == null
                ? TokenParsingPosition.DefaultStartingPosition
                : new TokenParsingPosition(currentPosition);
            return ParserPilot.Parse(clonedPosition, this, token);
        }

        protected virtual bool Exist(int origin, TokenNames token)
        {
            return ParserPilot.Exist(origin, token);
        }

        /// <summary>
        /// Parse the given <paramref name="token"/> at the given <paramref name="currentPosition"/> using the currently defined <see cref="ParserPilot"/>
        /// </summary>
        /// <param name="currentPosition">The position from which to parse. This won't be altered</param>
        /// <param name="token">The token that is meant to be parsed</param>
        /// <returns>A token result with the new position AND the resulting token, or null if the parse failed</returns>
        protected virtual ITokenResult Parse(int currentPosition, TokenNames token = TokenNames.Shield)
        {
            var clonedPosition = new TokenParsingPosition { Start = currentPosition };
            return ParserPilot.Parse(clonedPosition, this, token);
        }

        #endregion

        #region abstract definition (contract)

        /// <summary>
        /// Try to consume the <see cref="ParsedKeyword"/> given in <paramref name="origin"/> and return the token that consumed the most of them
        /// <remarks>This version does change the value of the <paramref name="origin"/> to the latest position of the find returned in the token result</remarks>
        /// </summary>
        /// <param name="origin">The current position of the list of parsed key words to consume <see cref="ParsedKeyword"/></param>
        /// <returns>Token that have been created, or null if none could be</returns>
        public abstract ITokenResult TryConsume(ref ITokenParsingPosition origin);


        /// <summary>
        /// Try to consume the <see cref="ParsedKeyword"/> given in <paramref name="origin"/> and return the token that consumed the most of them
        /// <remarks>This version does change the value of the <paramref name="origin"/> to the latest position of the find returned in the token result</remarks>
        /// </summary>
        /// <param name="origin">The current position of the list of parsed key words to consume <see cref="ParsedKeyword"/></param>
        /// <returns>Token that have been created, or null if none could be</returns>
        public abstract Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin);

        /// <summary>
        /// Try to consume the <see cref="ParsedKeyword"/> given in <paramref name="origin"/> and return the token that consumed the most of them
        /// </summary>
        /// <param name="origin">The current position of the list of parsed key words to consume <see cref="ParsedKeyword"/></param>
        /// <returns>Token that have been created, or null if none could be, containing the final position of the finding</returns>
        public virtual ITokenResult TryConsume(int origin)
        {
            ITokenParsingPosition pos = new TokenParsingPosition { Start = origin };
            return TryConsume(ref pos);
        }

        protected List<IToken> CurrentCollection = new List<IToken>();

        protected ITokenParsingPosition LastPosition { get; set; } = new TokenParsingPosition();

        public void Start(int position)
        {
            LastPosition.Start = position;
        }

        protected abstract ITokenResult End();

        /// <summary>
        /// .
        /// </summary>
        /// <param name="token"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public virtual bool ParseMandatory(TokenNames token, ITokenParsingPosition origin = null)
        {
            if (origin != null)
            {
                LastPosition = origin;
            }
            var result = Parse(LastPosition, token);
            if (result?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Between, origin.Start);
            }
            LastPosition = result.Position;
            CurrentCollection.Add(result.ResultToken);
            return true;
        }

        /// <summary>
        /// ?
        /// </summary>
        /// <param name="token"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public virtual bool ParseOptional(TokenNames token, ITokenParsingPosition origin = null)
        {
            if (origin != null)
            {
                LastPosition = origin;
            }
            var result = Parse(LastPosition, token);
            if (result?.ResultToken == null)
            {
                return false;
            }
            LastPosition = result.Position;
            CurrentCollection.Add(result.ResultToken);
            return true;
        }

        #endregion

        #region Error helpers and management

        /// <summary>
        /// Collection hosting all the parsing error for the given parser
        /// </summary>
        public ICollection<ParserError> Errors { get; } = new List<ParserError>();

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorMandatoryTokenMissingMessage =
            "The mandatory token {0} was expected, instead {1} was present";

        /// <summary>
        /// To use when a token was expected and necessary but the parsing did not match the expectation
        /// </summary>
        /// <param name="mandatoryTokenExpected">The expected and mandatory type of token</param>
        /// <param name="currentPosition"></param>
        protected void ErrorMandatoryTokenMissing(TokenNames mandatoryTokenExpected, int currentPosition)
        {
            ParserError error;
            try
            {
                var positionStart = ParserPilot.GetKeyword(currentPosition);
                error = new ParserError(positionStart.StartLocation, string.Format((string)ErrorMandatoryTokenMissingMessage, (object)mandatoryTokenExpected, positionStart.Value));
            }
            catch (IndexOutOfRangeException ioore)
            {
                error = new ParserError(0, string.Format((string)ErrorMandatoryTokenMissingMessage, (object)mandatoryTokenExpected, ioore.Message));
            }
            Errors.Add(error);
        }

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorNoOptionFoundMessage =
            "None of the options available for parsing a token {0} were matched. {1}";

        /// <summary>
        /// Error used when the parser return null because none of the options for parsing are matched (only used in the leaves for now)
        /// </summary>
        protected void ErrorNoOptionFound(int currentPosition)
        {
            ParserError error;
            try
            {
                var positionStart = ParserPilot.GetKeyword(currentPosition);
                error = new ParserError(positionStart.StartLocation,
                    string.Format((string)ErrorNoOptionFoundMessage, Type, null));
            }
            catch (NullReferenceException nrex)
            {
                error = new ParserError(0, string.Format((string)ErrorNoOptionFoundMessage, Type, nrex.Message));
            }
            catch (IndexOutOfRangeException ioore)
            {
                error = new ParserError(0, string.Format((string)ErrorNoOptionFoundMessage, Type, ioore.Message));
            }
            Errors.Add(error);
        }

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorOptionalTokenMissingMessage =
            "The expected token {0} was not found";

        /// <summary>
        /// To use when a token was expected and optional but the parsing did not match the expectation
        /// </summary>
        /// <param name="optionalTokenExpected">The expected and optional type of token</param>
        /// <param name="currentPosition"></param>
        protected void ErrorOptionalTokenMissing(TokenNames optionalTokenExpected, int currentPosition)
        {
            ParserError error;
            try
            {
                var positionStart = ParserPilot.GetKeyword(currentPosition);
                error = new ParserError(positionStart.StartLocation, string.Format((string)ErrorOptionalTokenMissingMessage, (object)optionalTokenExpected));
            }
            catch (IndexOutOfRangeException ioore)
            {
                error = new ParserError(0, string.Format((string)ErrorMandatoryTokenMissingMessage, (object)optionalTokenExpected, ioore.Message));
            }
            OnError(error);
        }

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorNoTokenKeywordsMessage = "Impossible to find the keywords for the {0} tokens";

        /// <summary>
        /// This error is chosen when a parser try to get the list of key words (<see cref="Resources"/>) but can not find the expected ones
        /// </summary>
        protected void ErrorNoTokenKeywords(int currentPosition)
        {
            ParserError error;
            try
            {
                var positionStart = ParserPilot.GetKeyword(currentPosition);
                error = new ParserError(positionStart.StartLocation, string.Format((string)ErrorNoTokenKeywordsMessage, (object)Type));
            }
            catch (IndexOutOfRangeException)
            {
                error = new ParserError(0, string.Format((string)ErrorNoTokenKeywordsMessage, (object)Type));
            }
            OnError(error);
        }

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorNotEnoughChildrenMessage = "Impossible to create a {0} there are not enough children to be valid";

        /// <summary>
        /// This error is chosen when a parser try to get the list of key words (<see cref="Resources"/>) but can not find the expected ones
        /// </summary>
        protected void ErrorNotEnoughChildren(int currentPosition)
        {
            ParserError error;
            try
            {
                var positionStart = ParserPilot.GetKeyword(currentPosition);
                error = new ParserError(positionStart.StartLocation, string.Format((string)ErrorNotEnoughChildrenMessage, (object)Type));
            }
            catch (IndexOutOfRangeException)
            {
                error = new ParserError(0, string.Format((string)ErrorNotEnoughChildrenMessage, (object)Type));
            }
            OnError(error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnError(ParserError e)
        {
            Error?.Invoke(this, e);
        }
        #endregion

    }
}