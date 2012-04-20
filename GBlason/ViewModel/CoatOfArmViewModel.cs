using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GBlason.Common.Attributes;
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
        [DisplayInformation("PropertyNameShape")]
        public ShapeViewModel CurrentShape
        {
            get { return _currentShape; }
            set
            {
                if (_currentShape == value)
                    return;
                _currentShape = value;
                OnPropertyChanged("currentShape");
            }
        }
        private ShapeViewModel _currentShape;

        #endregion

        

    }
}
