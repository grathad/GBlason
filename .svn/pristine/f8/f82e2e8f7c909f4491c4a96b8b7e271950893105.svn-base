using System.Windows.Input;
using GBlason.ViewModel;

namespace GBlason.Control.MenuControl
{
    /// <summary>
    /// Interaction logic for RecentDocumentMenu.xaml
    /// </summary>
    public partial class RecentDocumentMenu
    {
        public RecentDocumentMenu()
        {
            InitializeComponent();
        }

        private void FlagRecentItemExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var recentItem = e.Parameter as RecentFileViewModel;
            if(recentItem != null)
                recentItem.IsFixed = !recentItem.IsFixed;
        }
    }
}
