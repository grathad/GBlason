using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using MahApps.Metro.Controls;

namespace Format_Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            Service = new ServiceLayer(Context);
            parseButton.Focus();
        }

        public ViewModel Context { get; set; } = new ViewModel();

        public IServiceLayer Service { get; set; }

        private void parseButton_Click(object sender, RoutedEventArgs e)
        {
            //get the input
            var text = blazonText.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            //Ask the communication layer to send the request to the parsing service
            //should be injected, just doing a mockup for now
            try
            {               
                Service.Parse(text);
                //if (result?.Pilot == null)
                //{
                //    return;
                //}
                ////First version is synchronous
                //Context.Meta = result;
                ////will need more work
                //Context.ResultCallStack = new ObservableCollection<EnrichedParserNode> { new EnrichedParserNode(result, text, result.Pilot.CallTree.Root) };
                //Context.Root = new ObservableCollection<ContainerToken> { result.FormatTree };
                //Context.ResultErrors = new ObservableCollection<Grammar.ParserError>(result.Pilot.Errors);
                //Context.ResultKeywords = new ObservableCollection<ParsedKeyword>(result.Keywords);
            }
            catch (Exception ex)
            {
                var debug = ex;
            }
        }

        private void AttachResultChildren(ResultNode parent, List<IToken> children)
        {
            if (parent == null || children == null || !children.Any())
            {
                return;
            }
            foreach (var child in children)
            {
                var adult = child as ContainerToken;
                if (adult != null)
                {
                    var childContainer = new ResultNode { Name = child.ToString() };
                    parent.Children.Add(childContainer);
                    AttachResultChildren(childContainer, adult.Children);
                }
                else
                {
                    var childContainer = new ResultNode { Name = child.Type.ToString() };
                    parent.Children.Add(childContainer);
                    childContainer.Children.Add(new ResultItem { Name = child.ToString() });
                }
            }
        }

        private void callStackTreeview_Selected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            item?.BringIntoView();

            // prevent this event bubbling up to any parent nodes
            e.Handled = true;
        }
    }

    public interface IResult
    {
        string Name { get; set; }
    }

    public class ResultNode : ResultItem, IResult
    {
        public ObservableCollection<IResult> Children { get; set; } = new ObservableCollection<IResult>();
    }

    public class ResultItem : IResult
    {
        public string Name { get; set; }
    }
}
