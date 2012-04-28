using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GBlason.Common.Attribute;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    public class CoatOfArmViewModel : CoatOfArmComponent
    {
        public CoatOfArmViewModel()
        {
            if (GbrFileViewModel.GetResources.ScaledForMenuShapeResources.Any())
                _currentShape = GbrFileViewModel.GetResources.ScaledForMenuShapeResources[0];
        }

        #region COA properties

        /// <summary>
        /// Gets or sets the current shape of the coat of armss.
        /// </summary>
        /// <value>
        /// The current shape.
        /// </value>
        [CoaProperty("PropertyNameShape")]
        public ShapeViewModel CurrentShape
        {
            get { return _currentShape; }
            set
            {
                if (_currentShape == value)
                    return;
                _currentShape = value;
                OnPropertyChanged("CurrentShape");
            }
        }
        private ShapeViewModel _currentShape;

        #endregion
    }
}
