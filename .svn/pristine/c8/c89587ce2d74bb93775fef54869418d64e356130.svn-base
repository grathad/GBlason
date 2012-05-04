using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Used as a wrapper to manage the blason layered components in a treeview
    /// </summary>
    public class ComponentTreeViewViewModel
    {
        readonly ICommand _searchCommand;

        IEnumerator<CoatOfArmComponent> _matchingComponentEnumerator;
        string _searchText = String.Empty;

        public ComponentTreeViewViewModel(CoatOfArmComponent root)
        {
            RootForTreeView = new ObservableCollection<CoatOfArmComponent>
                                   {
                                       root
                                   };
            //_searchCommand = new SearchFamilyTreeCommand(this);
        }

        /// <summary>
        /// Returns a read-only collection containing the first person 
        /// in the family tree, to which the TreeView can bind.
        /// </summary>
        public ObservableCollection<CoatOfArmComponent> RootForTreeView { get; private set; }


        //#region SearchCommand

        ///// <summary>
        ///// Returns the command used to execute a search in the family tree.
        ///// </summary>
        //public ICommand SearchCommand
        //{
        //    get { return _searchCommand; }
        //}

        //private class SearchFamilyTreeCommand : ICommand
        //{
        //    readonly FamilyTreeViewModel _familyTree;

        //    public SearchFamilyTreeCommand(FamilyTreeViewModel familyTree)
        //    {
        //        _familyTree = familyTree;
        //    }

        //    public bool CanExecute(object parameter)
        //    {
        //        return true;
        //    }

        //    event EventHandler ICommand.CanExecuteChanged
        //    {
        //        // I intentionally left these empty because
        //        // this command never raises the event, and
        //        // not using the WeakEvent pattern here can
        //        // cause memory leaks.  WeakEvent pattern is
        //        // not simple to implement, so why bother.
        //        add { }
        //        remove { }
        //    }

        //    public void Execute(object parameter)
        //    {
        //        _familyTree.PerformSearch();
        //    }
        //}

        //#endregion // SearchCommand

        //#region SearchText

        ///// <summary>
        ///// Gets/sets a fragment of the name to search for.
        ///// </summary>
        //public string SearchText
        //{
        //    get { return _searchText; }
        //    set
        //    {
        //        if (value == _searchText)
        //            return;

        //        _searchText = value;

        //        _matchingPeopleEnumerator = null;
        //    }
        //}

        //#endregion // SearchText

        //#region Search Logic

        //void PerformSearch()
        //{
        //    if (_matchingPeopleEnumerator == null || !_matchingPeopleEnumerator.MoveNext())
        //        this.VerifyMatchingPeopleEnumerator();

        //    var person = _matchingPeopleEnumerator.Current;

        //    if (person == null)
        //        return;

        //    // Ensure that this person is in view.
        //    if (person.Parent != null)
        //        person.Parent.IsExpanded = true;

        //    person.IsSelected = true;
        //}

        //void VerifyMatchingPeopleEnumerator()
        //{
        //    var matches = this.FindMatches(_searchText, _rootPerson);
        //    _matchingPeopleEnumerator = matches.GetEnumerator();

        //    if (!_matchingPeopleEnumerator.MoveNext())
        //    {
        //        MessageBox.Show(
        //            "No matching names were found.",
        //            "Try Again",
        //            MessageBoxButton.OK,
        //            MessageBoxImage.Information
        //            );
        //    }
        //}

        //IEnumerable<PersonViewModel> FindMatches(string searchText, PersonViewModel person)
        //{
        //    if (person.NameContainsText(searchText))
        //        yield return person;

        //    foreach (PersonViewModel child in person.Children)
        //        foreach (PersonViewModel match in this.FindMatches(searchText, child))
        //            yield return match;
        //}

        //#endregion // Search Logic
    }
}
