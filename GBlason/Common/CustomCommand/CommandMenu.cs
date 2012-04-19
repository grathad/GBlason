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
    }
}
