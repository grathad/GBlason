using Grammar.PluginBase.Attributes;

namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// The list of all the type of tokens supported in the english grammar parser plugin
    /// </summary>
    [Redundancy] //enforce that all types get checked for redundancy and need to be overriden for removing the check
    public enum TokenNames
    {
        /// <summary>
        /// No token name defined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The shield is always the root element
        /// </summary>
        Shield,
        /// <summary>
        /// The field is the description of how to fill an area
        /// </summary>
        [ContainsOr(Tincture, Division, FieldVariation)]
        Field,
        /// <summary>
        /// The charges are objects placed on a field or on other objects
        /// </summary>
        [Redundancy(true)]
        Charge,
        /// <summary>
        /// A tincture is a coloured or pattern way of filling an area
        /// </summary>
        [ContainsOr(SimpleTincture, TinctureFur, CounterChanged)]
        Tincture,
        /// <summary>
        /// A simple tincture is a filling with a simple colour or with the reference to another simple tincture (so still filling with a simple color)
        /// </summary>
        [ContainsOr(TinctureColour, TinctureMetal, TinctureReference, TinctureProper)]
        SimpleTincture,
        /// <summary>
        /// A tincture representing a metal
        /// </summary>
        TinctureMetal,
        /// <summary>
        /// A tincture representing a colour
        /// </summary>
        TinctureColour,
        /// <summary>
        /// A tincture referencing another tincture
        /// </summary>
        TinctureReference,
        /// <summary>
        /// A division of the field is a way of filling the field by splitting it in different areas
        /// </summary>
        [ContainsOr(DivisionBy2, DivisionBy3, DivisionBy4)]
        Division,
        /// <summary>
        /// A division of the field that split the parent area in 4 areas
        /// </summary>
        DivisionBy4,
        /// <summary>
        /// A division of the field that split the parent area in 2 areas
        /// </summary>
        DivisionBy2,
        /// <summary>
        /// A division of the field that split the parent area in 3 areas
        /// </summary>
        DivisionBy3,
        /// <summary>
        /// An element is a single object definition that can be used either as multiple instance or as a unique one
        /// </summary>
        [ContainsOr(SingleChargeElement, PluralChargeElement)]
        ChargeElement,
        /// <summary>
        /// A single charge element are objects that are alone, either because their determiner is singular, or because they can't be multiple (like all honourable)
        /// </summary>
        //[ContainsOr(Ordinary, Symbol, SymbolCross)]
        SingleChargeElement,
        /// <summary>
        /// A plural charge element are objects that are multiple, either because of their determiner is plural, or because they can only be a plural charge (like bars)
        /// </summary>
        //[ContainsOr(Ordinary, Symbol, SymbolCross)]
        PluralChargeElement,
        /// <summary>
        /// A geometric shape used as a charge
        /// </summary>
        [ContainsOr(SingleOrdinary, PluralOrdinary)]
        Ordinary,
        /// <summary>
        /// An ordinary that is defined alone (like a bordure)
        /// </summary>
        SingleOrdinary,
        /// <summary>
        /// Leaf representing the name of an ordinary that can only be defined alone
        /// </summary>
        SingleOrdinaryName,
        /// <summary>
        /// An ordinary that can and is defined as multiple instances of itself (like 3 chevrons)
        /// </summary>
        PluralOrdinary,
        /// <summary>
        /// Leaf representing the name of an ordinary that can be defined as multiple instances
        /// </summary>
        PluralOrdinaryName,
        /// <summary>
        /// A customized symbol used as a charge
        /// </summary>
        Symbol,
        /// <summary>
        /// A token representing the name of a symbol or a sub part of a symbol
        /// </summary>
        SymbolName,
        /// <summary>
        /// A part of a symbol that can receive different parameter and define the charge more explicitly
        /// </summary>
        SymbolSubPart,
        /// <summary>
        /// The key word that represent the possession between a symbol and its sub parts
        /// </summary>
        SymbolSubPartPossession,
        /// <summary>
        /// Represent a coma, used to split list, and usually valid for refactoring (so we share the same property with all the items separated)
        /// </summary>
        Separator,
        /// <summary>
        /// Represent a key word that mark a set of predecessor or successor element as the subject of the following (or predecessing) element (each, the last, every ...)
        /// </summary>
        SharedKeyWord,
        /// <summary>
        /// Represent a number of items (can be one or more) using any type of numeric determiner
        /// </summary>
        Determiner,
        /// <summary>
        /// Represent a number of items (can only be one) using any type of numeric or specific determiner
        /// </summary>
        SingleDeterminer,
        /// <summary>
        /// Represent a number of items (more than one) using any type of numeric determiner
        /// </summary>
        PluralDeterminer,
        /// <summary>
        /// The list of attitude that a symbol can support (not all of them are supported only the most common)
        /// </summary>
        SymbolAttitude,
        /// <summary>
        /// The attitude of an attribute for the known attributes only
        /// </summary>
        SymbolAttitudeAttribute,
        /// <summary>
        /// Represent the state of a sub part of a symbol (a detail on its representation).
        /// </summary>
        SymbolState,
        /// <summary>
        /// Those are the keywords that preceed a symbol sub part state like "in"
        /// </summary>
        SymbolStateDeterminer,
        /// <summary>
        /// This token represent a relative position definer, that is present between the 2 objects affected by the position definition (A between B, A Within B ...) 
        /// </summary>
        PositionBetween,
        /// <summary>
        /// This is the definition of an honourable ordinary (usually based on the name of the ordinary)
        /// </summary>
        OrdinaryHonourable,
        /// <summary>
        /// This is the definition of a "sub ordinary" based on its name
        /// </summary>
        OrdinarySubordinary,
        /// <summary>
        /// This is the definition of a ordinary diminutive
        /// </summary>
        OrdinaryDiminutive,
        /// <summary>
        /// A sub ordinary of mobile type
        /// </summary>
        OrdinaryMobile,
        /// <summary>
        /// A sub Ordinary of fixed type
        /// </summary>
        OrdinaryFixed,
        /// <summary>
        /// Represent a word that specify that the previous element is charged with the following element (a charge)
        /// </summary>
        Charged,
        /// <summary>
        /// This represent an alteration of an otherwise normally defined symbol (like half a symbol)
        /// </summary>
        SymbolAlteration,
        /// <summary>
        /// This represent a charge that is composed by multiple ordinaries (usually sharing properties) it can be a list of names of ordinaries
        /// </summary>
        MultiOrdinary,
        /// <summary>
        /// This represent the name of a division by 4
        /// </summary>
        DivisionBy4Name,
        /// <summary>
        /// this represent a simple shield in a division (Not numbered, just a plain shield after the division definition) in shield and shield format
        /// </summary>
        SimpleDivisionShield,
        /// <summary>
        /// Represent a variation of the field
        /// </summary>
        [ContainsOr(FieldVariation2Tinctures, FieldVariationSemy, FieldVariationKnownSemy)]
        FieldVariation,
        /// <summary>
        /// The representation of a fur
        /// </summary>
        [ContainsOr(SimpleFur, Vair, Vaire)]
        TinctureFur,
        /// <summary>
        /// Represent an ordinary that is just one item (by opposition of multiple ordinaries in one definition)
        /// </summary>
        SimpleOrdinary,
        /// <summary>
        /// A simple fur is a fur that is based on a predefined name (like ermine)
        /// </summary>
        SimpleFur,
        /// <summary>
        /// A vair is a fur that is defined with a symbol (predefined) and properties (to define the pattern) and pre defined tinctures
        /// </summary>
        Vair,
        /// <summary>
        /// A vaire, is a vair where the tincture are not predefined (thus specified)
        /// </summary>
        Vaire,
        /// <summary>
        /// Represent the AND (or similar) keywords that are used to group multiple object together
        /// </summary>
        And,
        /// <summary>
        /// Those token represent the part of the blazon that contains properties (like tincture, orientation, position ...) that is shared with multiple items
        /// </summary>
        SharedProperties,
        /// <summary>
        /// Represent the reference to an object that is either the owner of a shared property or a target of the shared property
        /// </summary>
        SharedObjectReference,
        /// <summary>
        /// Represent a position or behavior that is applied to the objects either as the shared property or on top of the shared properties
        /// </summary>
        SharedPropertyAdverb,
        /// <summary>
        /// Represent only one property that can be shared
        /// </summary>
        SharedProperty,
        /// <summary>
        /// Represent a property defining a direction for an object, a line, a charge etc ...
        /// </summary>
        Direction,
        /// <summary>
        /// This represent 2 objects that are positionned with the position being defined first, like within a border a lion, or, between a pale 2 bezant ...
        /// </summary>
        PositionBefore,
        /// <summary>
        /// Represent a charge that is the minimum object definition of a complete charge. Without edge cases like multiple or cross positionned charges with shared properties
        /// </summary>
        SimpleCharge,
        /// <summary>
        /// Represent a charge that is the minimum object definition of a complete charge. With a unique element (differentiated because it is used as a limitation for semy for example)
        /// </summary>
        SingleSimpleCharge,
        /// <summary>
        /// Represent a charge that is the minimum object definition of a complete charge. With a plural determiner implying multiple instance of the same element
        /// </summary>
        PluralSimpleCharge,
        /// <summary>
        /// Represent one of the name that can be used for a vair
        /// </summary>
        VairName,
        /// <summary>
        /// The counter token to "counter" some element (mostly furs)
        /// </summary>
        Counter,
        /// <summary>
        /// The orientation name for fur (vair and vaire ...)
        /// </summary>
        FurOrientationName,
        /// <summary>
        /// Represent a variation of the line token
        /// </summary>
        LineVariation,
        /// <summary>
        /// Token representing a light separator, usually optional since this is not respected. Used to accept grammar that might not be perfect but are not invalid
        /// The light separator is not a charge separator
        /// </summary>
        LightSeparator,
        /// <summary>
        /// Token representing a light separator, usually optional since this is not respected. Used to accept grammar that might not be perfect but are not invalid
        /// The light separator is not a charge separator
        /// </summary>
        FieldSeparator,
        /// <summary>
        /// Token representing a separator for charges using the ;
        /// </summary>
        ChargeSeparator,
        /// <summary>
        /// token representing all of the complex multiple charges construct available on a single field
        /// </summary>
        //[ReAttemptParsingForParent(PositionnedCharges)]
        [ContainsOr(ChargesList, ChargeBetweenPosition, ChargeOnPosition, ChargeWithin, ChargeSurmounted, ChargeOverall, ChargeCharged)]
        MultiCharges,
        /// <summary>
        /// token that contains multiple charges one after the other, in a list (usually separated by coma and "and") as well as containing a specific location for the charges in the list that are not the principal one (or all of them even)
        /// </summary>
        ChargesList,
        /// <summary>
        /// token that represent multiple charge relative position based on surmounted
        /// </summary>
        [ContainsOr(SurmountedSingle, SurmountedPlural)]
        ChargeSurmounted,
        /// <summary>
        /// token that contains all the charges that can be used in a complex "between" relatively positionned list of charges
        /// </summary>
        [ContainsOr(BetweenStart, BetweenMiddle)]
        ChargeBetweenPosition,
        /// <summary>
        /// token that represent what kind of charges can be included in a charge list
        /// </summary>
        [ContainsOr(SimpleCharge, ChargeOnPosition, ChargeBetweenPosition, ChargeCharged)]
        AndPossibleGroup,
        /// <summary>
        /// token that contains all the charges that can be used in a complex "on" relatively positionned list of charges
        /// </summary>
        [ContainsOr(OnStart, OnMiddle)]
        ChargeOnPosition,
        /// <summary>
        /// token that contains all the charges that can be used in a complex "within" relatively positionned list of charges
        /// </summary>
        ChargeWithin,
        /// <summary>
        /// token that contains all the charges that can be used in a complex "charged" relatively positionned list of charges
        /// </summary>
        ChargeCharged,
        /// <summary>
        /// token that contains all the charges that can be used in a complex "overall" relatively positionned list of charges
        /// </summary>
        ChargeOverall,
        /// <summary>
        /// Grammar for all the charges that are between for which the position key word is in the middle of the charge
        /// </summary>
        BetweenMiddle,
        /// <summary>
        /// Grammar for all the charges that are between for which the position key word is at the start of the charge
        /// </summary>
        BetweenStart,
        /// <summary>
        /// Grammar for all the charges that are On for which the position key word is in the middle of the charge
        /// </summary>
        OnMiddle,
        /// <summary>
        /// Grammar for all the charges that are On for which the position key word is at the start of the charge
        /// </summary>
        OnStart,
        /// <summary>
        /// Token for the On keywords
        /// </summary>
        On,
        /// <summary>
        /// Token for possible group of charges in relatively positionned on charge list
        /// </summary>
        [ContainsOr(SimpleCharge, ChargeBetweenPosition, ChargeSurmounted, ChargeCharged)]
        OnPossibleGroup,
        /// <summary>
        /// Grammar for all the charges that can be grouped first in a between list
        /// </summary>
        [ContainsOr(SimpleCharge, ChargesList, ChargeSurmounted, ChargeCharged)]
        BetweenInsideGroup,
        /// <summary>
        /// Grammar for all the charges that can be grouped second in a between list
        /// </summary>
        [ContainsOr(PluralSimpleCharge, ChargesList, SurmountedPlural, ChargeCharged)]
        BetweenSurroundingGroup,
        /// <summary>
        /// Token for the between keyword
        /// </summary>
        Between,
        /// <summary>
        /// token that represent a gorup of a single charge surmounted
        /// </summary>
        SurmountedSingle,
        /// <summary>
        /// token that represent a gorup of multiple charges surmounted
        /// </summary>
        SurmountedPlural,
        /// <summary>
        /// token that represent a gorup of multiple charges surmounted
        /// </summary>
        [ContainsOr(PluralSimpleCharge, PluralChargeCharged)]
        SurmountedPossibleFirstPluralGroup,
        /// <summary>
        /// token that represent a gorup of multiple charges surmounted
        /// </summary>
        [ContainsOr(SingleSimpleCharge, SingleChargeCharged)]
        SurmountedPossibleFirstSingleGroup,
        /// <summary>
        /// token that represent a gorup of multiple charges surmounted
        /// </summary>
        [ContainsOr(SimpleCharge, BetweenMiddle, ChargeCharged)]
        SurmountedPossibleSecondGroup,
        /// <summary>
        /// token that represent the surmounted keyword
        /// </summary>
        Surmounted,
        /// <summary>
        /// token that represent the each keyword
        /// </summary>
        Each,
        /// <summary>
        /// token that contains all the charges at the start of a complex "overall" relatively positionned list of charges
        /// </summary>
        OverallPossibleFirstGroup,
        /// <summary>
        /// token that contains all the charges at the end of a complex "overall" relatively positionned list of charges
        /// </summary>
        OverallPossibleSecondGroup,
        /// <summary>
        /// token that represent the overall keywords
        /// </summary>
        Overall,
        /// <summary>
        /// token that represent the possible charge in a charged group
        /// </summary>
        [ContainsOr(SimpleCharge, ChargesList, BetweenMiddle, ChargeSurmounted)]
        ChargedPossibleGroup,
        /// <summary>
        /// token that represent the possible charge in a charged group
        /// </summary>
        PluralChargeCharged,
        /// <summary>
        /// token that represent the possible charge in a charged group
        /// </summary>
        SingleChargeCharged,
        /// <summary>
        /// the possible first group for within charges
        /// </summary>
        WithinPossibleFirstGroup,
        /// <summary>
        /// the possible first group for within charges
        /// </summary>
        WithinPossibleSecondGroup,
        /// <summary>
        /// Token for whole keyword
        /// </summary>
        Whole,
        /// <summary>
        /// Token for within keyword
        /// </summary>
        Within,
        /// <summary>
        /// Token for within keyword
        /// </summary>
        WithinWhole,
        /// <summary>
        /// Token for within keyword
        /// </summary>
        WithinAll,
        /// <summary>
        /// Token for within keyword
        /// </summary>
        WithinAllWhole,
        /// <summary>
        /// Token for All keyword
        /// </summary>
        All,
        /// <summary>
        /// Token representing multiple charges that are grouped together using a position link
        /// </summary>
        [ReAttemptParsingForParent(MultiCharges)]
        PositionnedCharges,
        /// <summary>
        /// Token representing multiple charges positionned in relation to each other with a charge level separator to separate complex constructs
        /// </summary>
        [ReAttemptParsingForParent(MultiCharges)]
        ComplexPositionnedCharges,
        /// <summary>
        /// Token representing a simple position based list of charges
        /// </summary>
        [ReAttemptParsingForParent(MultiCharges)]
        SimplePositionnedCharges,
        /// <summary>
        /// Token representing a definition of a location. This is different than a position in the way that a location is "absolute" wihtin the parent as a position is relative to another element
        /// </summary>
        Location,
        /// <summary>
        /// Represent an orientation token defining the orientation of an element
        /// </summary>
        Orientation,
        /// <summary>
        /// Represent a key word used to separate the target from the source when 
        /// </summary>
        ComplexPositionnedChargeSeparator,
        /// <summary>
        /// Represent a tincture using the "proper" definition (default)
        /// </summary>
        TinctureProper,
        /// <summary>
        /// Represent a cross that is not an honourable but a symbol (with variation that prevent if to split the fields if need be)
        /// </summary>
        SymbolCross,
        /// <summary>
        /// Represent the key words that identify a cross by opposition of the honourable (just the name in itself)
        /// </summary>
        Cross,
        /// <summary>
        /// Represent a known cross variation token
        /// </summary>
        CrossVariation,
        /// <summary>
        /// Represent a field variation that need 2 tinctures to be specified
        /// </summary>
        FieldVariation2Tinctures,
        /// <summary>
        /// Represent a field variation that is filling the field using a semy definition
        /// </summary>
        FieldVariationSemy,
        /// <summary>
        /// Represent a field variation that is filling the field using a known semy definition
        /// </summary>
        FieldVariationKnownSemy,
        /// <summary>
        /// Represent one of the valid names of a field variation
        /// </summary>
        FieldVariationName,
        /// <summary>
        /// Represent the number of pieces in the variation of the field
        /// </summary>
        FieldVariationNumber,
        /// <summary>
        /// Represent the keyword "of"
        /// </summary>
        Of,
        /// <summary>
        /// Represent the over all definition when used after the field
        /// </summary>
        PositionOverall,
        /// <summary>
        /// Represent a location that use multiple points or multiple reference to define the following object
        /// </summary>
        MultiLocations,
        /// <summary>
        /// Represent a simple one definition location
        /// </summary>
        SimpleLocation,
        /// <summary>
        /// Represent the possible names of a point definition
        /// </summary>
        LocationPoint,
        /// <summary>
        /// Represent the keyword that initially describe a location (in, on ...)
        /// </summary>
        LocationBefore,
        /// <summary>
        /// Represent the keyword "point"
        /// </summary>
        Point,
        /// <summary>
        /// A specifier defining how to define a location
        /// </summary>
        LocationSpecifier,
        /// <summary>
        /// Represent a flank of the shield
        /// </summary>
        LocationFlank,
        /// <summary>
        /// Represent one of the possible side of the shield
        /// </summary>
        ShieldSide,
        /// <summary>
        /// Represent the determiner that is introducing a semy
        /// </summary>
        SemyDeterminer,
        /// <summary>
        /// Represent pre determined, known semy names that are used with custom tinctures
        /// </summary>
        SemyName,
        /// <summary>
        /// Represent the actual "semy" keyword
        /// </summary>
        Semy,
        /// <summary>
        /// Represent the charges that are used to "semy" a field, when a semy actually use a custom charge
        /// </summary>
        SemyCharge,
        /// <summary>
        /// Represent a name of a semy that is known and imply the tincture
        /// </summary>
        PredefinedSemy,
        /// <summary>
        /// Represent one of the name that can be used for a vaire
        /// </summary>
        VaireName,
        /// <summary>
        /// Represent the name of a vaire when surrounded by tinctures
        /// </summary>
        VaireBetweenName,
        /// <summary>
        /// Represent a name that start a division by 2
        /// </summary>
        DivisionBy2Name,
        /// <summary>
        /// Represent a shield in a division by 2 that is clearly positionned in one half (2 of them are expected)
        /// </summary>
        PositionnedHalves,
        /// <summary>
        /// Represent the key word that is used to validate a counter changed
        /// </summary>
        CounterChanged,
        /// <summary>
        /// Represent a number identifying a first division
        /// </summary>
        FirstDivisionNumber,
        /// <summary>
        /// Represent a number identifying a second division
        /// </summary>
        SecondDivisionNumber,
        /// <summary>
        /// represent a valid name for a symbol sub part definition
        /// </summary>
        SubPartName,
        /// <summary>
        /// represent a list of sub part names following each others
        /// </summary>
        SubPartNameList,
        /// <summary>
        /// represent a list of independent sub parts constructs for symbol generation
        /// </summary>
        SymbolSubPartList,
        /// <summary>
        /// represent a group of dependant sub parts for symbol generation
        /// </summary>
        SymbolSubPartGroup,
        /// <summary>
        /// Represent a construct that is a field, and contain 2 separated field fillers for a simple field division by 2
        /// </summary>
        SimpleDivisionBy2Field,
        /// <summary>
        /// Represent the key word and the different token and grammatical rule when all the charges in a shield are counterchanged, not the same as counterchanged
        /// </summary>
        AllCounterChanged,
        /// <summary>
        /// The extension of a name of a symbol when there is key words that are still considered as part of the name
        /// </summary>
        SymbolNameExtension,
        /// <summary>
        /// Represent names that are used to divide the field in 3
        /// </summary>
        DivisionBy3Name,
        /// <summary>
        /// Represent 3 different tinctures or field variation that are filling the divided field
        /// </summary>
        SimpleDivisionBy3Field,
        /// <summary>
        /// Represent a punctuation used to separate the word used to define a division by 4 and the rest of the division by 4 definition
        /// </summary>
        DivisionBy4Separator,
        /// <summary>
        /// Represent a list of shields that are numbered for each quarters
        /// </summary>
        PositionnedQuarters,
        /// <summary>
        /// Represent a number identifying the third division
        /// </summary>
        ThirdDivisionNumber,
        /// <summary>
        /// Represent a number identifying the fourth division
        /// </summary>
        FourthDivisionNumber,
        /// <summary>
        /// Represent a number identifying the first and the fourth division
        /// </summary>
        FirstAndFourthDivisionNumber,
        /// <summary>
        /// Represent a number identifying the second and the third division
        /// </summary>
        SecondAndThirdDivisionNumber,
        /// <summary>
        /// Represent a key word used to reference a quarter
        /// </summary>
        Quarter,
        /// <summary>
        /// Represent a complex construct defining a variation of a line
        /// </summary>
        LineVariationDefinition,
        /// <summary>
        /// Represent a property assigned to an ordinary that alters it
        /// </summary>
        OrdinaryAlteration,
        /// <summary>
        /// Represent the key word at the end of a charge when this charge is used as a cadency
        /// </summary>
        Cadency
    }
}