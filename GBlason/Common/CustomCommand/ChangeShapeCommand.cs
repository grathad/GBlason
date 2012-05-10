using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;
using GBlason.Culture;
using GBlason.Properties;
using GBlason.ViewModel;

namespace GBlason.Common.CustomCommand
{
    /// <summary>
    /// The command handling the changing of the coat of arms shape
    /// </summary>
    public class ChangeShapeCommand : CommandGeneric
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeShapeCommand"/> class.
        /// </summary>
        /// <param name="ownertype">The ownertype.</param>
        public ChangeShapeCommand(Type ownertype)
            : base("Change shape", ownertype)
        {
        }

        private ShapeViewModel _oldShape;

        private ShapeViewModel _newShape;

        /// <summary>
        /// Determines whether this instance can execute the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="element">The element.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanExecute(Object parameter, IInputElement element)
        {
            var elemCoa = element as CoatOfArmViewModel;
            var shapeParam = parameter as ShapeViewModel;
            return elemCoa != null && shapeParam != null;
        }

        /// <summary>
        /// Executes the <see cref="T:System.Windows.Input.RoutedCommand"/> on the current command target.
        /// Save the parameters (parameter and target) in a CommandGeneric instance and then save the instance in the command history that handle this file
        /// </summary>
        /// <param name="parameter">User defined parameter to be passed to the handler have to be a <see cref="CommandParameter"/>.</param>
        /// <param name="target">Element at which to being looking for command handlers.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="target"/> is not a <see cref="T:System.Windows.UIElement"/> or <see cref="T:System.Windows.ContentElement"/>.</exception>
        ///   
        /// <exception cref="ArgumentNullException">if parameter is null</exception>
        public override void Execute(CommandParameter parameter, IInputElement target)
        {
            _newShape = parameter.Parameter as ShapeViewModel;

            if (_newShape == null)
                throw new ArgumentNullException("parameter");


            _oldShape = parameter.FileTargeted.RootCoatOfArm.CurrentShape;

            parameter.FileTargeted.RootCoatOfArm.CurrentShape = _newShape;

            Description = String.Format(CultureInfo.CurrentCulture,
                                        Resources.CommandDescChangeShape,
                                        _oldShape == null ? BlasonVocabulary.NoShape : _oldShape.LocalizedName,
                                        _newShape.LocalizedName
                );

            //sauvegarde, c'est affreux, je peux pas le faire dans la classe abstraite parceque c'est cette instance que je dois cloner :(
            //copie de code pour tous les execute !!
            Parameter = parameter;
            Done = true;
            parameter.FileTargeted.CommandsApplied.CommandHistory.Add((ChangeShapeCommand)Clone());
        }

        public override void Redo()
        {
            Undo();
            Done = true;
        }

        /// <summary>
        /// Undoes this instance.
        /// Setting the old shape to the coat of arms
        /// </summary>
        public override void Undo()
        {
            var coaTarget = Parameter.FileTargeted.RootCoatOfArm;
            if (coaTarget == null) return;
            coaTarget.CurrentShape = _oldShape;
            _oldShape = _newShape;
            _newShape = coaTarget.CurrentShape;
            Done = false;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
