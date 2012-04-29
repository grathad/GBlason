﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace GBlason.Common.CustomCommand
{
    /// <summary>
    /// Override RoutedUiCommand to allow the application to handle the undo / redo behaviour for the command performed on the CoatOfArmsComponent's properties or children
    /// </summary>
    public abstract class CommandGeneric : RoutedCommand, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGeneric"/> class.
        /// </summary>
        protected CommandGeneric()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGeneric"/> class.
        /// </summary>
        /// <param name="text">The text used in this case as a Description.</param>
        /// <param name="ownerType">Type of the owner.</param>
        protected CommandGeneric(String text, Type ownerType)
            : base(text, ownerType)
        {
            Description = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGeneric"/> class.
        /// </summary>
        /// <param name="text">The text. Used in this case as a description</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <param name="input">The input.</param>
        protected CommandGeneric(String text, Type ownerType, InputGestureCollection input)
            : base(text, ownerType, input)
        {
            Description = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGeneric"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <param name="input">The input.</param>
        /// <param name="description">The description.</param>
        protected CommandGeneric(String text, Type ownerType, InputGestureCollection input, String description)
            : base(text, ownerType, input)
        {
            Description = description;
        }

        /// <summary>
        /// Determines whether this <see cref="T:System.Windows.Input.RoutedCommand"/> can execute in its current state.
        /// </summary>
        /// <param name="parameter">A user defined data type.</param>
        /// <param name="target">The command target.</param>
        /// <returns>
        /// true if the command can execute on the current command target; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="target"/> is not a <see cref="T:System.Windows.UIElement"/> or <see cref="T:System.Windows.ContentElement"/>.</exception>
        public new abstract bool CanExecute(Object parameter, IInputElement target);

        /// <summary>
        /// Executes the <see cref="T:System.Windows.Input.RoutedCommand"/> on the current command target.
        /// Save the parameters (parameter and target) in a CommandGeneric instance and then save the instance in the command history that handle this file
        /// </summary>
        /// <param name="parameter">User defined parameter to be passed to the handler.</param>
        /// <param name="target">Element at which to being looking for command handlers.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="target"/> is not a <see cref="T:System.Windows.UIElement"/> or <see cref="T:System.Windows.ContentElement"/>.</exception>
        public new virtual void Execute(Object parameter, IInputElement target)
        {
            var iCommandTarget = target as ICommandTarget;
            if (iCommandTarget == null) return;
            CommandTarget = iCommandTarget;
            Parameter = parameter;
            iCommandTarget.FindRepository().CommandHistory.Add(this);
            Done = true;
        }

        /// <summary>
        /// Undoes this instance.
        /// </summary>
        public virtual void Undo()
        {
            Done = false;
        }

        private bool _done;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandGeneric"/> is done.
        /// </summary>
        /// <value>
        ///   <c>true</c> if done; otherwise, <c>false</c>.
        /// </value>
        public bool Done
        {
            get { return _done; }
            set
            {
                if (value == _done)
                    return;
                _done = value;
                OnPropertyChanged("Done");
            }
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public virtual Object Parameter { get; set; }

        private ICommandTarget _commandTarget;

        /// <summary>
        /// Gets or sets the command target.
        /// </summary>
        /// <value>
        /// The command target.
        /// </value>
        public ICommandTarget CommandTarget
        {
            get { return _commandTarget; }
            set
            {
                if (value == _commandTarget)
                    return;
                _commandTarget = value;
                OnPropertyChanged("CommandTarget");
            }
        }

        private String _description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
