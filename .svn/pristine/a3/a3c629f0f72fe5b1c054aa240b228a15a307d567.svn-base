using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using GBlason.Common.Attribute;
using GBlason.Culture;
using System.Reflection;

namespace GBlason.ViewModel.Contract
{
    /// <summary>
    /// Contract for all the different layers and component of the tree logic coat of arms composition
    /// Help defining all the data for the properties display
    /// Help defining all the links for the treeview display
    /// All this capacities are also useful when drawing he shape.
    /// </summary>
    public abstract class CoatOfArmComponent : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the name of the component. Have to exist in the resource (the type is used as the resource key)
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public virtual String ComponentName
        {
            get
            {
                var rman = new ResourceManager(typeof(BlasonVocabulary));
                return rman.GetString(GetType().Name) ?? String.Empty;
            }
        }

        protected CoatOfArmComponent()
        {
            _children = new ObservableCollection<CoatOfArmComponent>();
        }

        public abstract Object OriginObject { get; set; }

        #region properties

        /// <summary>
        /// Gets the properties data.
        /// They are loaded each time by reflection (each time because we might need to update the value)
        /// </summary>
        public virtual ObservableCollection<PropertyDisplayer> PropertiesData
        {
            get
            {
                var type = GetType();
                var retour = new ObservableCollection<PropertyDisplayer>();
                var rm = new ResourceManager(typeof(BlasonVocabulary));

                foreach (var propertyInfo in type.GetProperties())
                {
                    var attr = propertyInfo.GetCustomAttributes(true);
                    if (!attr.Any(at => at is CoaPropertyAttribute)) continue;
                    var propAttr = attr.First(at => at is CoaPropertyAttribute) as CoaPropertyAttribute;
                    //we add this property to the list of properties
                    if (propAttr != null)
                        retour.Add(new PropertyDisplayer
                                       {
                                           PropertyName = rm.GetString(propAttr.NameResourceKey, CultureInfo.CurrentCulture),
                                           PropertyValue = propertyInfo.GetValue(this, null)
                                       });
                }
                return retour;
            }
        }
        #endregion

        #region tree logic
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public CoatOfArmComponent Parent { get; set; }

        /// <summary>
        /// Gets the root parent.
        /// </summary>
        public CoatOfArmComponent RootParent
        {
            get
            {
                return Parent == null ? this : Parent.RootParent;
            }
        }

        /// <summary>
        /// Adds the child to the list of children (the child is added only if it does not already exist).
        /// </summary>
        /// <param name="child">The child.</param>
        public virtual void AddChild(CoatOfArmComponent child)
        {
            if (Children != null && !Children.Any(ch => ch == child))
                Children.Add(child);
        }

        /// <summary>
        /// Removes the child. If it exists in the collection
        /// </summary>
        /// <param name="child">The child.</param>
        public virtual void RemoveChild(CoatOfArmComponent child)
        {
            if (Children == null) return;
            Children.Remove(child);
        }

        private ObservableCollection<CoatOfArmComponent> _children;
        public ObservableCollection<CoatOfArmComponent> Children
        {
            get { return _children; }
            protected set
            {
                if (value == _children)
                    return;
                _children = value;
                OnPropertyChanged("Children");
            }
        }

        #region IsExpanded

        private bool _isExpanded;
        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        private bool _isSelected;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    OnPropertyChanged("ComponentName");
                    OnPropertyChanged("PropertiesData");
                    OnPropertyChanged("Children");
                }
            }
        }

        #endregion // IsSelected

        #region NameContainsText

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(ComponentName))
                return false;
            return ComponentName.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        #endregion // NameContainsText

        #endregion

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
