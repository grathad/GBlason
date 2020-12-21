using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Grammar;
using Grammar.English;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Format_Debugger
{
    public interface IServiceLayer
    {
        void Parse(string blazon);
    }

    public class ParserResult
    {
        public ContainerToken FormatTree { get; set; }


        public ParserPilot Pilot { get; set; }

        public List<ParsedKeyword> Keywords { get; set; }
    }

    public class ServiceLayer : IServiceLayer
    {
        public ViewModel VmReference { get; init; }

        protected Assembly TestAssembly { get; set; }

        protected Resources Resources { get; set; }

        protected Detector Detector { get; set; }

        protected ITokenParsingPosition Position { get; set; }

        protected string EntryText { get; set; }

        protected EnrichedParserNode PreviousNode { get; set; }

        public ServiceLayer(ViewModel vm)
        {
            VmReference = vm;
            TestAssembly = Assembly.GetAssembly(typeof(EnglishGrammar));
            Resources = new Resources(TestAssembly);
            Detector = new Detector(Errors, Resources);
            Position = TokenParsingPosition.DefaultStartingPosition;
        }

        protected List<ParserError> Errors { get; set; }
        public async void Parse(string blazon)
        {
            if (VmReference == null || string.IsNullOrEmpty(blazon))
            {
                return;
            }

            VmReference.ParsingNotInProgress = false;

            VmReference.Clear();

            var source = new MemoryStream(Encoding.UTF8.GetBytes(blazon));
            EntryText = blazon;

            var keyWords = await KeywordsDetectionAsync(source);

            VmReference.ResultKeywords = new ObservableCollection<ParsedKeyword>(keyWords.ToList());
            VmReference.DetectorBenchmarkTime = Detector.BenchmarkingWatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;

            var pilot = new ParserPilot(new DefaultParserFactory(TestAssembly), VmReference.ResultKeywords);
            pilot.TreeChildAdded += Pilot_TreeChildAdded;
            pilot.NodeChanged += Pilot_NodeChanged;

            _ = ParseAsync(pilot).ContinueWith((r) => EndParse(r.Result, pilot));
        }

        private void Pilot_NodeChanged(object sender, ParserNodeEventArgs e)
        {
            if (VmReference?.ResultCallStack == null)
            {
                return;
            }

            //find the corresponding element in the result call stack
            var root = VmReference.ResultCallStack.FirstOrDefault();
            if (root == null)
            {
                return;
            }
            else
            {
                var node = root.Find(e.NodeEdited);
                //updating the node information
                if (node != null)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        node.UpdateNode(e.NodeEdited, VmReference.ResultKeywords);
                    });
                }
            }
        }

        private void Pilot_TreeChildAdded(object sender, ParserTreeEventArgs e)
        {
            if (VmReference?.ResultCallStack == null)
            {
                return;
            }

            //find the corresponding element in the result call stack
            var root = VmReference.ResultCallStack.FirstOrDefault();
            if(PreviousNode != null)
            {
                PreviousNode.IsSelected = false;
            }
            var newNode = new EnrichedParserNode(e.NodeAdded, VmReference.ResultKeywords);
            if (root == null)
            {
                //we are adding the root
                Application.Current.Dispatcher.Invoke(delegate
                {
                    VmReference.ResultCallStack.Add(newNode);
                });
            }
            else
            {
                var parent = root.Find(e.NodeAdded.Parent);
                //adding the child to the parent and refreshing the UI
                if (parent != null)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        parent.EnrichedChildren.Add(newNode);
                    });

                }
            }
            PreviousNode = newNode;
        }

        private Task<IEnumerable<ParsedKeyword>> KeywordsDetectionAsync(MemoryStream source)
        {
            return Task.Run(() => Detector.DetectKeywords(source));
        }

        private Task<ITokenResult> ParseAsync(ParserPilot pilot)
        {
            return Task.Run(() => pilot.Parse(Position));
        }

        private void EndParse(ITokenResult result, ParserPilot pilot)
        {
            VmReference.Root = new ObservableCollection<ContainerToken> { result.ResultToken as ContainerToken };
            VmReference.ParsingNotInProgress = true;
            pilot.TreeChildAdded -= Pilot_TreeChildAdded;
            pilot.NodeChanged -= Pilot_NodeChanged;
            //removing the locked focus of the treeview after the completion
            PreviousNode.IsSelected = false;
        }
    }
}
