using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBL.Repository.Resources;

namespace GBlason.ViewModel.Contract
{
    interface IDivisible
    {
        void AddDivision(DivisionType division);
    }
}
