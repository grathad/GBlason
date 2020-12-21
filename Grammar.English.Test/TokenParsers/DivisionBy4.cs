using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class DivisionBy4 : IntegrationTest
    {
        [Fact]
        internal void DivisionBy4_QuarterlyNoNumberComplex()
        {
            var pilot = GetPilot(BlazonRepository.DivisionQuarterlyNoNumber3);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            debugTable.Should().Be(@"<html>
<header>
<style>
table,th,td{
border: 1px solid black;
border-collapse: collapse;
}
th,td{
padding: 5px;
text-align: center;
}
</style>
</header>
<body>
<table>
<tr>
<td>
0
</td>
<td colspan=""15"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""15"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""15"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""15"" rowspan=""1"">
DivisionBy4
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
DivisionBy4Name
</td>
<td colspan=""1"" rowspan=""1"">
DivisionBy4Separator
</td>
<td colspan=""13"" rowspan=""1"">
SimpleDivisionShield
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""9"">
quarterly
</td>
<td colspan=""0"" rowspan=""9"">
,
</td>
<td colspan=""7"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""4"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""6"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""8"">
,
</td>
<td colspan=""0"" rowspan=""8"">
and
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""6"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""2"" rowspan=""1"">
SharedProperties
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""5"">
,
</td>
<td colspan=""2"" rowspan=""1"">
SharedProperty
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""4"">
azure
</td>
<td colspan=""0"" rowspan=""4"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""2"" rowspan=""1"">
Direction
</td>
<td colspan=""0"" rowspan=""4"">
or
</td>
<td colspan=""0"" rowspan=""4"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""3"">
or
</td>
<td colspan=""0"" rowspan=""3"">
in
</td>
<td colspan=""0"" rowspan=""3"">
pale
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
gules
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
<td colspan=""0"" rowspan=""2"">
bends
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""1"">
mullets
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void DivisionBy4_CrossNoNumberSimple()
        {
            var pilot = GetPilot(BlazonRepository.DivisionPerCrossNoNumber);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            debugTable.Should().Be(@"<html>
<header>
<style>
table,th,td{
border: 1px solid black;
border-collapse: collapse;
}
th,td{
padding: 5px;
text-align: center;
}
</style>
</header>
<body>
<table>
<tr>
<td>
0
</td>
<td colspan=""5"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""5"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""5"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""5"" rowspan=""1"">
DivisionBy4
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""2"" rowspan=""1"">
DivisionBy4Name
</td>
<td colspan=""3"" rowspan=""1"">
SimpleDivisionBy2Field
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""4"">
per
</td>
<td colspan=""0"" rowspan=""4"">
cross
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""3"">
and
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""1"">
argent
</td>
<td colspan=""0"" rowspan=""1"">
gules
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void DivisionBy4_SaltireNoNumberComplex()
        {
            var pilot = GetPilot(BlazonRepository.DivisionPerSaltireNoNumber2);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            debugTable.Should().Be(@"<html>
<header>
<style>
table,th,td{
border: 1px solid black;
border-collapse: collapse;
}
th,td{
padding: 5px;
text-align: center;
}
</style>
</header>
<body>
<table>
<tr>
<td>
0
</td>
<td colspan=""11"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""11"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""11"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""11"" rowspan=""1"">
DivisionBy4
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""3"" rowspan=""1"">
DivisionBy4Name
</td>
<td colspan=""8"" rowspan=""1"">
SimpleDivisionShield
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""9"">
Party
</td>
<td colspan=""0"" rowspan=""9"">
per
</td>
<td colspan=""0"" rowspan=""9"">
saltire
</td>
<td colspan=""6"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""1"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""8"">
and
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""5"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""4"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""4"">
azure
</td>
<td colspan=""0"" rowspan=""4"">
three
</td>
<td colspan=""3"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""4"">
gules
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""3"">
fleurs
</td>
<td colspan=""0"" rowspan=""3"">
de
</td>
<td colspan=""0"" rowspan=""3"">
lys
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""1"">
or
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void DivisionBy4_QuarterlyNumbered()
        {
            var pilot = GetPilot(BlazonRepository.DivisionUnitedKingdom);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            debugTable.Should().Be(@"<html>
<header>
<style>
table,th,td{
border: 1px solid black;
border-collapse: collapse;
}
th,td{
padding: 5px;
text-align: center;
}
</style>
</header>
<body>
<table>
<tr>
<td>
0
</td>
<td colspan=""41"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""41"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""41"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""41"" rowspan=""1"">
DivisionBy4
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
DivisionBy4Name
</td>
<td colspan=""1"" rowspan=""1"">
DivisionBy4Separator
</td>
<td colspan=""39"" rowspan=""1"">
PositionnedQuarters
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""12"">
Quarterly
</td>
<td colspan=""0"" rowspan=""12"">
,
</td>
<td colspan=""3"" rowspan=""1"">
FirstAndFourthDivisionNumber
</td>
<td colspan=""12"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
ChargeSeparator
</td>
<td colspan=""1"" rowspan=""1"">
SecondDivisionNumber
</td>
<td colspan=""1"" rowspan=""1"">
Quarter
</td>
<td colspan=""12"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
ChargeSeparator
</td>
<td colspan=""1"" rowspan=""1"">
ThirdDivisionNumber
</td>
<td colspan=""1"" rowspan=""1"">
Quarter
</td>
<td colspan=""6"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
FirstDivisionNumber
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""1"" rowspan=""1"">
FourthDivisionNumber
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""11"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""11"">
;
</td>
<td colspan=""0"" rowspan=""11"">
second
</td>
<td colspan=""0"" rowspan=""11"">
quarter
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""11"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""11"">
;
</td>
<td colspan=""0"" rowspan=""11"">
third
</td>
<td colspan=""0"" rowspan=""11"">
quarter
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""10"">
first
</td>
<td colspan=""0"" rowspan=""10"">
and
</td>
<td colspan=""0"" rowspan=""10"">
fourth
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""11"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""11"" rowspan=""1"">
PositionnedCharges
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""11"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""11"" rowspan=""1"">
SimplePositionnedCharges
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""5"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""10"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""7"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""4"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""7"">
Gules
</td>
<td colspan=""0"" rowspan=""7"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
</td>
<td colspan=""2"" rowspan=""1"">
SharedProperty
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""4"" rowspan=""1"">
SymbolSubPartGroup
</td>
<td colspan=""0"" rowspan=""7"">
Or
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
within
</td>
<td colspan=""7"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
Azure
</td>
<td colspan=""0"" rowspan=""7"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""2"" rowspan=""1"">
SymbolSubPartGroup
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""6"">
Lions
</td>
<td colspan=""0"" rowspan=""6"">
passant
</td>
<td colspan=""0"" rowspan=""6"">
gardant
</td>
<td colspan=""2"" rowspan=""1"">
Direction
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
SymbolSubPart
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""2"" rowspan=""1"">
SymbolSubPart
</td>
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""6"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""6"">
Harp
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SymbolSubPart
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""5"">
in
</td>
<td colspan=""0"" rowspan=""5"">
pale
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""0"" rowspan=""5"">
and
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""5"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""4"">
Or
</td>
<td colspan=""0"" rowspan=""4"">
armed
</td>
<td colspan=""0"" rowspan=""4"">
langued
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryAlteration
</td>
<td colspan=""4"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""4"">
Or
</td>
<td colspan=""0"" rowspan=""4"">
stringed
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
14
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""3"">
Lion
</td>
<td colspan=""0"" rowspan=""3"">
rampant
</td>
<td colspan=""0"" rowspan=""3"">
double
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""3"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""0"" rowspan=""3"">
Gules
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
15
</td>
<td colspan=""0"" rowspan=""2"">
Azure
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryFixed
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
</td>
<td colspan=""1"" rowspan=""1"">
Counter
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
</td>
<td colspan=""0"" rowspan=""2"">
Argent
</td>
</tr>
<tr>
<td>
16
</td>
<td colspan=""0"" rowspan=""1"">
tressure
</td>
<td colspan=""0"" rowspan=""1"">
flory
</td>
<td colspan=""0"" rowspan=""1"">
counter
</td>
<td colspan=""0"" rowspan=""1"">
flory
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void DivisionBy4_QuarterlyInQuarterly()
        {
            //var origin = BlazonRepository.DivisionDukeAumont;
            //todo add support for numbered position and the issue that add support for the requirements
            //"4 in chief and 3 in base"
            //comments
            //variation of the charge (mullet of 16 ray)
            //fleurdelysee as a cross variation

            //var pilot = GetPilot(BlazonRepository.DivisionUnitedKingdom);
            //var position = TokenParsingPosition.DefaultStartingPosition;
            //var result = pilot.Parse(position).ResultToken as ContainerToken;

            //    var debugTable = result.ToHtmlTable();

            //    result.As<ContainerToken>().Should().NotBeNull()
            //        .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
            //        .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            //    debugTable.Should().Be(@"");
        }

        [Fact]
        internal void DivisionBy4_QuarterlyAllNumberDifferent()
        {
            //var origin = BlazonRepository.MarshallingSaintCloud;
            //todo support for advanced shared properties and other, they will need specific issue to handle
            //like 
            //"of three tiers"
            //"bordered"
            //position by number "in orle, 3 2 and 3"
            //cross advanced variations "crosslet in foot fitchy"
            //"brochant"
            //comments: "(du palatinat)"
            //var pilot = GetPilot(BlazonRepository.DivisionUnitedKingdom);
            //var position = TokenParsingPosition.DefaultStartingPosition;
            //var result = pilot.Parse(position).ResultToken as ContainerToken;

            //    var result =
            //        new Spec.ShieldParser().TryConsume(TokenParsingPosition.DefaultStartingPosition) as ContainerToken;

            //    var debugTable = result.ToHtmlTable();

            //    result.As<ContainerToken>().Should().NotBeNull()
            //        .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
            //        .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);

            //    debugTable.Should().Be(@"");
        }
    }
}
