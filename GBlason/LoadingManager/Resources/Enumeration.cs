using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GBL.Repository.Resources
{
    public enum ResourceType
    {
        Original = 1,
        Custom = 2
    }

    public enum Splittings
    {
        Division, // partition (FR)
        Ordinary, // pièce (FR)
        Charges   // meuble (FR)
    }

    public enum DivisionType
    {
        PartyPlain,
        PartyFess,
        PartyPale,
        PartyBend,
        PartyBendSinister,
        PartySaltire,
        PartyCross,
        PartyPall,
        TiercedBend,
        TiercedBendSinister,
        TiercedFess,
        TiercedPale
    }
}
