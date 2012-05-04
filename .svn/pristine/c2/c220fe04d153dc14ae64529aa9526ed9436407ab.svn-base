using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBL.Repository.CoatOfArms;
using GBL.Repository.Resources;

namespace GBlasonLogic.Rules
{
    public static class DivisionRules
    {
        public static bool IsAllowed(DivisionType division, Object parent)
        {
            //for now, only plain coa are available
            return parent is CoatOfArms && division == DivisionType.PartyPlain;
        }
    }
}
