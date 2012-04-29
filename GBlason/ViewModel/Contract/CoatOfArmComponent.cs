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
    /// </summary>
    public abstract class CoatOfArmComponent : INotifyPropertyChanged
    {
        public String ComponentName
        {
            get
            {
                var rman = new ResourceManager(typeof(BlasonVocabulary));
                return rman.GetString(GetType().Name) ?? String.Empty;
            }
        }

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

        public CoatOfArmComponent Parent { get; set; }

        public CoatOfArmComponent RootParent
        {
            get
            {
                return Parent == null ? this : Parent.RootParent;
            }
        }

        //private ObservableCollection<PropertyTableViewModel> ReflectionAttributesForPropertiesReading()
        //{
        //    var type = GetType();
        //    var retour = new ObservableCollection<PropertyTableViewModel>();
        //    foreach (var property in type.GetProperties())
        //    {
        //        foreach (var attr in property.GetCustomAttributes(typeof(DisplayInformation), true))
        //        {
        //            var attrCheck = attr as DisplayInformation;
        //            if (attrCheck == null) continue;
        //            retour.Add(new PropertyTableViewModel()
        //                {PropertyName = attrCheck.DisplayName,
        //                CellTemplateEditionStyleKey = attrCheck.CellEditingTemplateKey,
        //                CellTemplateStyleKey = attrCheck.CellTemplateKey,
        //                PropertyValue = property.});
        //        }
        //    }
        //    return retour;
        //}

        public void UpdateBindingOnSelected()
        {
            OnPropertyChanged("ComponentName");
            OnPropertyChanged("PropertiesData");
        }

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
