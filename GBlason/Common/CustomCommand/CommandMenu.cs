using System.Windows;
using System.Windows.Input;

namespace GBlason.Common.CustomCommand
{
    public static class CommandMenu
    {
        /// <summary>
        /// Used to define when a user click on a button on one of the multi object list in the recent files menu tab
        /// </summary>
        public static readonly RoutedUICommand RecentObjectSelect = new RoutedUICommand("Select Recent Object", "RecentObjectSelect", typeof(UIElement));

        /// <summary>
        /// Used to flag / unflag a recent element in the list of recent element
        /// </summary>
        public static readonly RoutedUICommand RecentObjectFlag = new RoutedUICommand("Flag Recent Object", "RecentObjectFlag", typeof(UIElement));

        /// <summary>
        /// CommandGeneric, that means can perform an undo / redo action, and handle the shape changing action for the overall coat of arm
        /// Parameter : The new shape, Target : the coat of arm. Both may be set
        /// </summary>
        public static readonly ChangeShape ChangingShape = new ChangeShape(typeof(UIElement));
    }
}
