using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grammar;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Token;

namespace Format_Debugger
{
    public class ViewModel : INotifyPropertyChanged
    {
        public void Clear()
        {
            ResultKeywords?.Clear();
            ResultCallStack?.Clear();
            ResultErrors?.Clear();
        }

        private bool _parsingNotInProgress = true;

        public bool ParsingNotInProgress
        {
            get => _parsingNotInProgress; set
            {
                _parsingNotInProgress = value;
                NotifiyPropertyChanged(nameof(ParsingNotInProgress));
            }
        }

        private ParserResult _meta;

        public ParserResult Meta
        {
            get => _meta;
            set
            {
                _meta = value;
                NotifiyPropertyChanged(nameof(Meta));
            }
        }

        private double _detectorBenchmarkTime;

        public double DetectorBenchmarkTime
        {
            get { return _detectorBenchmarkTime; }
            set
            {
                _detectorBenchmarkTime = value;
                NotifiyPropertyChanged(nameof(DetectorBenchmarkTime));
            }
        }

        private ObservableCollection<ContainerToken> _root = new ObservableCollection<ContainerToken>();

        public ObservableCollection<ContainerToken> Root
        {
            get => _root;
            set
            {
                _root = value;
                NotifiyPropertyChanged(nameof(Root));
            }
        }

        private ObservableCollection<EnrichedParserNode> _resultCallStack = new ObservableCollection<EnrichedParserNode>();

        public ObservableCollection<EnrichedParserNode> ResultCallStack
        {
            get => _resultCallStack;
            set
            {
                _resultCallStack = value;
                NotifiyPropertyChanged(nameof(ResultCallStack));
            }
        }

        private ObservableCollection<ParserError> _resultErrors = new ObservableCollection<ParserError>();

        public ObservableCollection<ParserError> ResultErrors
        {
            get => _resultErrors;
            set
            {
                _resultErrors = value;
                NotifiyPropertyChanged(nameof(ResultErrors));
            }
        }

        private ObservableCollection<ParsedKeyword> _resultKeywords = new ObservableCollection<ParsedKeyword>();

        public ObservableCollection<ParsedKeyword> ResultKeywords
        {
            get => _resultKeywords;
            set
            {
                _resultKeywords = value;
                NotifiyPropertyChanged(nameof(ResultKeywords));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class EnrichedParserNode : ParserNode, INotifyPropertyChanged
    {
        internal const string NoPositionText = "no position";
        internal const string NoTypeText = "no type";
        internal const string NoResultText = "no valid result";
        internal const string InExecution = "parsing in progress";
        internal const string Created = "node created";
        internal const string Executed = "parsing completed";
        internal const string Found = "found";

        /// <summary>
        /// WARNING deep clone constructor
        /// </summary>
        /// <param name="pn"></param>
        /// <param name="entryText"></param>
        public EnrichedParserNode(ParserNode currentRoot, IEnumerable<ParsedKeyword> keyWords) : base(currentRoot.Parser, currentRoot.Position)
        {
            EnrichedChildren = new ObservableCollection<EnrichedParserNode>();
            UpdateNode(currentRoot, keyWords);
            if(currentRoot.Children != null && currentRoot.Children.Any())
            {
                foreach(var child in currentRoot.Children)
                {
                    EnrichedChildren.Add(new EnrichedParserNode(child, keyWords));
                }
            }
        }

        public void UpdateNode(ParserNode currentRoot, IEnumerable<ParsedKeyword> keyWords)
        {
            if (currentRoot == null || keyWords == null)
            {
                return;
            }

            ExecutionTime = currentRoot.Parser.BenchmarkingWatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;
            PositionText = NoPositionText;
            if (currentRoot.Position != null)
            {
                PositionText = currentRoot.Position.Start.ToString();
            }
            ParserText = NoTypeText;
            if (currentRoot.Parser != null)
            {
                ParserText = currentRoot.Parser.Type.ToString();
            }
            switch (currentRoot.Status)
            {
                case NodeParserStatus.Created:
                    StatusText = Created;
                    break;
                case NodeParserStatus.Started:
                    StatusText = InExecution;
                    break;
                case NodeParserStatus.Executed:
                    StatusText = Executed;
                    break;
            }
            if (currentRoot.Status == NodeParserStatus.Executed)
            {
                if (currentRoot.Result?.ResultToken != null && currentRoot.Result?.Position != null && currentRoot.Position != null)
                {
                    IsInProgressOrSuccesfull = true;
                    ResultText = Found;
                    //the text used (position defined) is per keywords, not per string index
                    //getting the terms used
                    var strBuilder = new StringBuilder();
                    for (var i = currentRoot.Position?.Start ?? 0; i < (currentRoot.Result.Position?.Start ?? 0); i++)
                    {
                        strBuilder.Append(keyWords.ElementAt(i).Value).Append(' ');
                    }
                    Outcome = strBuilder.ToString();
                }
                else
                {
                    IsInProgressOrSuccesfull = false;
                    ResultText = NoResultText;
                    if (currentRoot.Errors?.Any() ?? false)
                    {
                        Outcome = currentRoot.Errors.First().Explanation;
                    }
                }
            }
        }

        /// <summary>
        /// Find the first enriched node in the tree that matches the regular one passed in parameter (using the unique id), this include this instance (root) if it matches
        /// </summary>
        /// <param name="node">The node to look for in the tree</param>
        /// <returns>The enriched node if any or null if none</returns>
        public EnrichedParserNode Find(ParserNode node)
        {
            if (node?.Parser == null || Parser == null)
            {
                return null;
            }
            if (Parser.UniqueId == node.Parser.UniqueId)
            {
                return this;
            }
            if (EnrichedChildren != null && EnrichedChildren.Any())
            {
                foreach (var child in EnrichedChildren)
                {
                    var result = child.Find(node);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        private double _executionTime;
        public double ExecutionTime
        {
            get { return _executionTime; }
            set
            {
                _executionTime = value;
                NotifiyPropertyChanged(nameof(ExecutionTime));
            }
        }

        private string _outcome;

        public string Outcome
        {
            get { return _outcome; }
            set
            {
                _outcome = value;
                NotifiyPropertyChanged(nameof(Outcome));
            }
        }

        private string _firstError;
        public string FirstError
        {
            get { return _firstError; }
            set
            {
                _firstError = value;
                NotifiyPropertyChanged(nameof(FirstError));
            }
        }

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                NotifiyPropertyChanged(nameof(StatusText));
            }
        }

        private string _resultText;
        public string ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                NotifiyPropertyChanged(nameof(ResultText));
            }
        }

        private bool _isSelected = true;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifiyPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private bool _isInProgressOrSuccesfull = true;

        public bool IsInProgressOrSuccesfull
        {
            get => _isInProgressOrSuccesfull;
            set
            {
                if (_isInProgressOrSuccesfull != value)
                {
                    _isInProgressOrSuccesfull = value;
                    NotifiyPropertyChanged(nameof(IsInProgressOrSuccesfull));
                }
            }
        }

        private string _positionText;
        public string PositionText
        {
            get { return _positionText; }
            set
            {
                _positionText = value;
                NotifiyPropertyChanged(nameof(PositionText));
            }
        }
        private string _parserText;
        public string ParserText
        {
            get { return _parserText; }
            set
            {
                _parserText = value;
                NotifiyPropertyChanged(nameof(ParserText));
            }
        }

        private ObservableCollection<EnrichedParserNode> _enrichedChildren;

        public ObservableCollection<EnrichedParserNode> EnrichedChildren
        {
            get { return _enrichedChildren; }
            set
            {
                _enrichedChildren = value;
                NotifiyPropertyChanged(nameof(EnrichedChildren));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }


}
