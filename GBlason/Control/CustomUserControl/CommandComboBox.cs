using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GBlason.Control.CustomUserControl
{
    public class CommandComboBox : ComboBox, ICommandSource
    {
        static CommandComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandComboBox), new FrameworkPropertyMetadata(typeof(CommandComboBox)));
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (Command == null) return;
            var rCommand = Command as RoutedCommand;
            if (rCommand == null) Command.Execute(CommandParameter);
            else rCommand.Execute(CommandParameter, CommandTarget);
        }

        #region CommandProperty

        /// <summary>
        /// The command property enabling binding of command
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(CommandComboBox),
            new PropertyMetadata(null, CommandChanged));

        /// <summary>
        /// Gets the command that will be executed when the command source is invoked.
        /// </summary>
        /// <returns>The command that will be executed when the command source is invoked.</returns>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Fired when the command is changed
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cs = (CommandComboBox)d;
            cs.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

       
        /// <summary>
        /// Add a new command to the Command Property.
        /// </summary>
        /// <param name="oldCommand">The old command.</param>
        /// <param name="newCommand">The new command.</param>
        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            // If oldCommand is not null, then we need to remove the handlers.
            if (oldCommand != null)
            {
                RemoveCommand(oldCommand, newCommand);
            }
            AddCommand(oldCommand, newCommand);
        }

        /// <summary>
        /// Remove an old command from the Command Property.
        /// </summary>
        /// <param name="oldCommand">The old command.</param>
        /// <param name="newCommand">The new command.</param>
        private void RemoveCommand(ICommand oldCommand, ICommand newCommand)
        {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="oldCommand">The old command.</param>
        /// <param name="newCommand">The new command.</param>
        private void AddCommand(ICommand oldCommand, ICommand newCommand)
        {
            var handler = new EventHandler(CanExecuteChanged);
            _canExecuteChangedHandler = handler;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += _canExecuteChangedHandler;
            }
        }

        private EventHandler _canExecuteChangedHandler;

        /// <summary>
        /// Determines whether this instance [can execute changed] the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command == null) return;
            var command = Command as RoutedCommand;

            // If a RoutedCommand.
            IsEnabled = command != null ? command.CanExecute(CommandParameter, CommandTarget) : Command.CanExecute(CommandParameter);
        }
        #endregion

        #region CommantParameter Property


        public static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(Object), typeof(CommandComboBox),
            new PropertyMetadata(null));

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        #endregion

        #region CommandTarget Property

        public static DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(CommandComboBox),
            new PropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }
            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }

        #endregion

    }
}
