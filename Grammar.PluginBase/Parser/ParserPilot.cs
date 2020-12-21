using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Attributes;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Utils.Enum;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// Parse the list of keyword into a treeview structure composed of tokens.
    /// Manage all the calls and all the hierarchy in a centralized manner
    /// </summary>
    public class ParserPilot : IParserPilot
    {
        /// <summary>
        /// Default contructor of the parser pilot, intialize the values from the passed parameters.
        /// Initialize the assembly from the current one. And cache the available types to avoid slowing down the code in reflection logic
        /// </summary>
        /// <param name="errors">The list of errors already known (should be empty most of the time, or null)</param>
        /// <param name="factory">The factory to use to create the parsers</param>
        /// <param name="keyWords">The list of keywords to use to generate the tokens. Usually a result of the call of <see cref="Detector.DetectKeywords"/></param>
        public ParserPilot(
            IParserFactory factory,
            IList<ParsedKeyword> keyWords,
            IList<ParserError> errors = null)
        {
            Errors = errors ?? new List<ParserError>();
            KeyWords = keyWords ?? new List<ParsedKeyword>();
            Factory = factory ?? new DefaultParserFactory();
        }

        /// <summary>
        /// Contains the list of errors that the parser pilot encountered while executing the parsing.
        /// In case of async parsing this will be redundant with the error events
        /// </summary>
        public virtual IList<ParserError> Errors { get; }

        /// <summary>
        /// The list of keywords to parse, have to be provided so that the parser can work from that source
        /// </summary>
        internal virtual IList<ParsedKeyword> KeyWords { get; }

        /// <inheritdoc/>
        public virtual int LastPosition => KeyWords.Count - 1;

        /// <summary>
        /// This is the tree of all the calls made to all the parser attempt involved in creating the final token
        /// </summary>
        public virtual IParserTree CallTree { get; set; } = new ParserTree();

        internal virtual IParserFactory Factory { get; }

        /// <summary>
        /// Event triggered when a ParserNode (new or already created) is added to the ParserTree
        /// </summary>
        public event EventHandler<ParserTreeEventArgs> TreeChildAdded;

        protected virtual void OnTreeChildAdded(ParserTreeEventArgs e)
        {
            TreeChildAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Event triggered when a ParserNode value is changed at the parsing pilot level (so after the parsing itself is resolved)
        /// </summary>
        public event EventHandler<ParserNodeEventArgs> NodeChanged;

        protected virtual void OnNodeChanged(ParserNodeEventArgs e)
        {
            NodeChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public async Task<ITokenResult> ParseAsync(ITokenParsingPosition currentPosition = null, ParserBase parent = null, TokenNames token = TokenNames.Shield)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="Parse(ITokenParsingPosition,ParserBase,TokenNames)"/>
        ///  <summary>
        ///  Create the tree and return the element asked as the root.
        /// <remarks>The <paramref name="currentPosition"/> is NEVER altered in the function and will be returned as passed</remarks>
        ///  </summary>
        /// <param name="currentPosition">The current position in the source. Default to null (when it is starting as it is the root)</param>
        /// <param name="parent">The parent parser that is requesting for a parsing. Default to null (when starting as it is the root)</param>
        /// <param name="token">The token to try to read. Default to <see cref="Shield"/></param>
        /// <returns>The root element if the token parsing worked, or null if the reading was in error. Even if the result is not null it is possible that some errors occured. The <see cref="Errors"/> property will contain those</returns>
        public ITokenResult Parse(
            ITokenParsingPosition currentPosition = null,
            ParserBase parent = null,
            TokenNames token = TokenNames.Shield)
        {
            if (!IsInputValid(ref currentPosition, token))
            {
                return null;
            }

            //we check if the parser already exist in memory
            var parserNode = GetFromMemory(token, currentPosition.Start);
            if (ParserAlreadyExistAndExecutedNoReRun(
                parent,
                parserNode,
                out var tokenResult))
            {
                CallTree.AddChild(parent, parserNode);
                OnTreeChildAdded(new ParserTreeEventArgs { Tree = CallTree, NodeAdded = parserNode });
#if DEBUG
                Trace.TraceInformation(
                    $"{currentPosition.Start} > {parent?.Type ?? TokenNames.Undefined} > {token} > [Already Computed] > {parserNode.Result?.ResultToken?.GetNbLeaves() ?? 0} leaves found");
#endif
                return tokenResult;
            }


            //if not already used, then we create a new instance of parser. 2 options, container or leaf
            var parserInstance = NewParserInstance(token);
#if DEBUG
            Trace.TraceInformation(
                $"{currentPosition.Start} > {parent?.Type ?? TokenNames.Undefined} > {token} > Create new ");
#endif

            //this is a new parser node, we need to create it
            parserNode = new ParserNode(parserInstance, currentPosition);
            parserNode.Status = NodeParserStatus.Created;

            //here we run the PRE execution logic, based on the type attributes
            if (!RunPreExecution(parserNode.Parser, parent, currentPosition.Start))
            {
#if DEBUG
                Trace.TraceInformation($"{currentPosition.Start} > {parent?.Type ?? TokenNames.Undefined} > {token} > [Possible infinite loop error] > Stopping");
#endif
                return null;
            }

            //if we are allowed to continue the parsing we add the newly created parser in the tree of parsers
            CallTree.AddChild(parent, parserNode);
            OnTreeChildAdded(new ParserTreeEventArgs { Tree = CallTree, NodeAdded = parserNode });

            //then we finally try to consume the parser
            ITokenParsingPosition origin = new TokenParsingPosition(currentPosition);

            parserInstance.BenchmarkingWatch.Start();
            parserNode.Status = NodeParserStatus.Started;
            parserNode.Result = parserInstance.TryConsume(ref origin);
            parserInstance.BenchmarkingWatch.Stop();
            //we mark it as executed
            parserNode.Status = NodeParserStatus.Executed;
            OnNodeChanged(new ParserNodeEventArgs { NodeEdited = parserNode });

            //optional tracing
#if DEBUG
            var traceNbLeaves = parserNode.Result?.ResultToken?.GetNbLeaves() ?? 0;

            Trace.TraceInformation(
                traceNbLeaves > 0
                ? $"{currentPosition.Start} < {parent?.Type ?? TokenNames.Undefined} < {token} < [Success] < {traceNbLeaves} leaves found"
                : $"{currentPosition.Start} < {parent?.Type ?? TokenNames.Undefined} < {token} < No valid grammar found");
#endif
            return parserNode.Result;
        }

        /// <summary>
        /// Extraction of the logic that create a new parser instance for the requested parsing
        /// </summary>
        /// <param name="token">The token for the parser creation</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        /// When the factory fails to create the requested instance
        /// or when the created instance is null
        /// </exception>
        internal virtual ParserBase NewParserInstance(
            TokenNames token)
        {
            ParserBase parserInstance;
            try
            {
                parserInstance = Factory.CreateParser(token, this);
            }
            catch (Exception e)
            {
                throw new NotSupportedException(
                    $"We could not create a parser for the {token} because: {e.Message}",
                    e);
            }

            if (parserInstance == null)
            {
                throw new NotSupportedException(
                    $"The parser created for {token} is null");
            }
            return parserInstance;
        }

        /// <summary>
        /// Check if the requested parser is already in memory.
        /// Executed
        /// And check if the same parser can run again (based on <see cref="RunTreeMemory"/>)
        /// </summary>
        /// <param name="parent">The parser parent</param>
        /// <param name="parserNode">The parser node</param>
        /// <param name="tokenResult">The token result that was already executed (if a match is found) null if no match are found</param>
        /// <returns>True if already in memory, executed, and the <see cref="RunTreeMemory"/> is true</returns>
        internal virtual bool ParserAlreadyExistAndExecutedNoReRun(
            ParserBase parent,
            ParserNode parserNode,
            out ITokenResult tokenResult)
        {
            tokenResult = null;
            //if it does we return the same result as the one cached
            if (parserNode != null
                && parserNode.Status == NodeParserStatus.Executed
                && RunTreeMemory(parserNode))
            {
                //we found a parser node that already executed from the same position and got a result assigned already
                //so we use it rather than performing the same parse again
                //for traceability purpose if it is not on the same parent we attach it to its new parent
                //we likely will need a unique ID and change the equality in the parser node definition
                var getParentNode = CallTree.Get(parent);
                if (parserNode.Parent != getParentNode)
                {
                    CallTree.AddChild(parent, parserNode);
                }
                tokenResult = parserNode.Result;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check the status of the current pilot, and the input
        /// </summary>
        /// <param name="currentPosition">the current position (to check its validity)</param>
        /// <param name="token">the current token (to check its validity)</param>
        /// <returns>false if the state or the input is invalid, otherwise true</returns>
        internal virtual bool IsInputValid(
            ref ITokenParsingPosition currentPosition,
            TokenNames token)
        {
            if (Factory == null)
            {
                throw new NotSupportedException(nameof(Factory));
            }

            //those are logical statements without exceptions.
            //if there is a need for an exception they should be turned into default
            //validation as the input attribute level and always executed (and turned off for the exception of course)
            //for now they are part of the hard coded parsing logic
            if (KeyWords == null || !KeyWords.Any())
            {
                return false;
            }
            if (currentPosition == null)
            {
                currentPosition = TokenParsingPosition.DefaultStartingPosition;
            }
            if (currentPosition.Start > LastPosition)
            {
                return false;
            }
            //we check if a parser from the token name exists
            var parserType = Factory.GetParser(token);
            if (parserType == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Execute all the attribute for the current type, to check if the execution need to be continued (clean the code and add maintainability in the future rules per type)
        /// </summary>
        /// <param name="currentParser"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual bool RunPreExecution(ParserBase currentParser, ParserBase parent, int position)
        {
            if (currentParser == null)
            {
                return true;
            }
            var finalAttributes = currentParser.Type.Attributes().FindAttributes<PreParsingAttribute>();
            if (!(finalAttributes?.Any() ?? false))
            {
                //the list is empty, nothing to pre execute the original parsing can continue
                return true;
            }
            //we execute all the code in the given attributes, the first to return false will stop the pre execution
            return finalAttributes.All(preParsingAttribute => preParsingAttribute.Execute(CallTree, currentParser, parent, position));
        }

        /// <summary>
        /// Execute all the attribute for the current type, to check if the execution need to be continued (clean the code and add maintainability in the future rules per type)
        /// </summary>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        protected virtual bool RunTreeMemory(ParserNode currentNode)
        {
            if (currentNode?.Parser == null)
            {
                return true;
            }
            var finalAttributes = currentNode.Parser.Type.Attributes().FindAttributes<TreeMemoryAttribute>();
            if (!(finalAttributes?.Any() ?? false))
            {
                //the list is empty, nothing to pre execute the original parsing can continue
                return true;
            }
            //we execute all the code in the given attributes, the first to return false will stop the pre execution
            return finalAttributes.All(preParsingAttribute => preParsingAttribute.Execute(CallTree, currentNode));
        }

        /// <summary>
        /// <inheritdoc cref="GetKeyword(int)"/>
        /// </summary>
        /// <param name="position"><inheritdoc cref="GetKeyword(int)"/></param>
        /// <returns><inheritdoc cref="GetKeyword(int)"/></returns>
        public ParsedKeyword GetKeyword(int position)
        {
            if (position < 0 || position >= KeyWords.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return KeyWords.ElementAt(position);
        }

        /// <summary>
        /// <inheritdoc cref="GetRemainingKeywords(int)"/>
        /// </summary>
        /// <param name="position"><inheritdoc cref="GetRemainingKeywords(int)"/></param>
        /// <returns><inheritdoc cref="GetRemainingKeywords(int)"/></returns>
        public IList<ParsedKeyword> GetRemainingKeywords(int position)
        {
            if (position < 0 || position >= KeyWords.Count)
            {
                return new List<ParsedKeyword>();
            }
            return KeyWords.Skip(position).ToList();
        }

        /// <summary>
        /// Find the parser node in the call tree that match the current type name and position
        /// </summary>
        /// <param name="name">The name of the object to retrieve</param>
        /// <param name="positionStart">The position to look for in the filtering of the item</param>
        /// <returns></returns>
        protected virtual ParserNode GetFromMemory(TokenNames name, int positionStart)
        {
            if (positionStart < 0 || positionStart > LastPosition)
            {
                return null;
            }

            bool MemoryMatchLogic(ParserNode p)
            {
                return p?.Parser != null
                    && p.Parser.Type == name
                    && p.Position.Start == positionStart;
            }

            return CallTree.GetFirstOrDefault(MemoryMatchLogic);
        }

    }

    public class ParserTreeEventArgs : EventArgs
    {
        public IParserTree Tree { get; set; }
        public ParserNode NodeAdded { get; set; }
    }

    public class ParserNodeEventArgs : EventArgs
    {
        public ParserNode NodeEdited { get; set; }
    }

}
