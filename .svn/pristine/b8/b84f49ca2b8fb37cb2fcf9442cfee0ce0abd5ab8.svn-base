using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GBL.Repository.CoatOfArms;
using GBL.Repository.Resources;
using GBlason.Culture;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    public class DivisionViewModel : CoatOfArmComponent, IDivisible, ILineCustomizable
    {
        public override String ComponentName
        {
            get
            {
                var rm = new System.Resources.ResourceManager(typeof(BlasonVocabulary));
                return String.Format(CultureInfo.CurrentCulture,
                    base.ComponentName,
                    rm.GetString(DivisionType.ToString(), CultureInfo.CurrentCulture));
            }
        }

        #region properties

        /// <summary>
        /// Pareil, en prevision, il s'agit de la propriété qui définie le comportement des sous parties de la partition (affichage coupé ou scalé)
        /// </summary>
        private bool _marshalling;

        public DivisionType DivisionType { get; set; }

        private Division _originObject;

        public override object OriginObject
        {
            get { return _originObject ?? (_originObject = new Division()); }
            set
            {
                if (value != _originObject)
                    return;
                var div = value as Division;
                _originObject = div;
                OnPropertyChanged("OriginObject");
            }
        }

        #endregion

        public void AddDivision(DivisionType division)
        {
            throw new NotImplementedException();
        }

        public object VariationOfLine
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
