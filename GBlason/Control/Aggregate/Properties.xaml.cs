using System.Windows;
using System.Windows.Input;
using GBlason.Common.CustomCommand;

namespace GBlason.Control.Aggregate
{
    /// <summary>
    /// Interaction logic for Properties.xaml
    /// </summary>
    public partial class Properties
    {
        public Properties()
        {
            InitializeComponent();
        }

        private void CommandBindingCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShapeChangingExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
            var command = e.Command as ChangeShape;
            if(command!=null)
                command.Execute(e.Parameter, e.Source as IInputElement);
        }
    }
}
