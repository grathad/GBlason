using System;
using System.Linq;
using GBL.Repository.CoatOfArms;
using GBlason.Common.Attribute;
using GBlason.Common.Converter;
using GBlason.Common.CustomCommand;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Handle the presentation logic for the root of a coat of arm
    /// </summary>
    public class CoatOfArmViewModel : CoatOfArmComponent, ICommandTarget, IDivisible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoatOfArmViewModel"/> class.
        /// Initialize the shape with the first found resource shape
        /// Initialize the tree by setting this object as the root
        /// </summary>
        public CoatOfArmViewModel()
        {
            if (GbrFileViewModel.GetResources.ScaledForMenuShapeResources.Any())
                CurrentShape = GbrFileViewModel.GetResources.ScaledForMenuShapeResources[0];
            Parent = null; //this is the root of the coat of arms
        }

        private CoatOfArms _originObject;

        /// <summary>
        /// Return the CoatOfArms from the repository linked with this object
        /// </summary>
        public override Object OriginObject
        {
            get { return _originObject ?? (_originObject = new CoatOfArms {Shape = CurrentShape.ConvertToShape()}); }
            set
            {
                if (value == _originObject) return;
                var coa = value as CoatOfArms;
                if (coa != null)
                    CurrentShape = coa.Shape.ConvertToViewModel();
                _originObject = coa;
                OnPropertyChanged("OriginObject");
            }
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

        #region ICommandTarget
        /// <summary>
        /// Finds the commands repository.
        /// Used to handle all the commands applied on the current coat of arms (and GbsFile)s
        /// </summary>
        /// <returns></returns>
        public CommandRepository FindRepository()
        {
            var gbsFileViewModel = GlobalApplicationViewModel.GetApplicationViewModel.OpenedFiles.FirstOrDefault(
                opf => opf.RootCoatOfArm == this);
            return gbsFileViewModel != null ? gbsFileViewModel.CommandsApplied : null;
        }
        #endregion


        public void AddDivision(GBL.Repository.Resources.DivisionType division)
        {
            AddChild(new DivisionViewModel{DivisionType = division, Parent = this});
        }
    }
}
