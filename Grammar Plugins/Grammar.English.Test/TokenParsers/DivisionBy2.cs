using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class DivisionBy2 : IntegrationTest
    {
        [Fact]
        internal void DivisionBy2_SimpleQuarter()
        {
            var pilot = GetPilot(BlazonRepository.CarnegieofPittarrow_SirDavid);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(3);
            //current status:
            //The symbol at the end, should not be a multi charge
            //the priority need to be reviewed, it have to be a symbol And a symbol part
            //if "an" is replaced by "and overall" then the part after the coma will still be considered the end of the field of the secondary field
            //rather than the end of the field of the FIRST field (which it is)

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
<td colspan=""6"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""8"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""6"" rowspan=""1"">
Division
</td>
<td colspan=""0"" rowspan=""9"">
,
</td>
<td colspan=""8"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""6"" rowspan=""1"">
DivisionBy2
</td>
<td colspan=""8"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""3"" rowspan=""1"">
DivisionBy2Name
</td>
<td colspan=""3"" rowspan=""1"">
SimpleDivisionBy2Field
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""7"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""6"">
Parted
</td>
<td colspan=""0"" rowspan=""6"">
per
</td>
<td colspan=""0"" rowspan=""6"">
pale
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
<td colspan=""0"" rowspan=""6"">
an
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""4"" rowspan=""1"">
SymbolSubPartGroup
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""5"">
and
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""5"">
eagle
</td>
<td colspan=""0"" rowspan=""5"">
displayed
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
</tr>
<tr>
<td>
7
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""0"" rowspan=""4"">
and
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
8
</td>
<td colspan=""0"" rowspan=""3"">
Or
</td>
<td colspan=""0"" rowspan=""3"">
Argent
</td>
<td colspan=""0"" rowspan=""3"">
Azure
</td>
<td colspan=""0"" rowspan=""3"">
armed
</td>
<td colspan=""0"" rowspan=""3"">
beaked
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
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""1"">
Gules
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void DivisionBy2_MultiChargesCounterChanged()
        {
            //this one is broken because the position never really worked
            //there is a problem with the logic in the grammar, meaning that the location of an element is not defined
            //for example the location can be defined before or after, but we should only consider ONE location per charge or symbol sub part
            //so I need to change the way we write the grammar, and split the shared properties into different potential assigned values
            //shared properties should only exist as refactoring from the children for the potential items (like tincture, location)
            //this will be supported in #29 see https://gitlab.com/gblason/webclient/issues/29
            //also "within" and "all within" should get a better definition.
            //meaning, within a bordure is implied to mean all the children within the bordure, but it is still written as "within"
            //so I need a way to detect this and consider the within and the bordure to be part of the parent, potentially by altering the position grammar
            //this will be supported in #28 see https://gitlab.com/gblason/webclient/issues/28
            //            var origin = BlazonRepository.DevonFord;

            //            var errors = new List<ParserError>();

            //            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(origin)))
            //            {
            //                var keywords = new Detector(errors).DetectKeywords(ms);

            //                var result = new Spec.ShieldParser().TryConsume(TokenParsingPosition.DefaultStartingPosition) as ContainerToken;

            //                var debugTable = result.ToHtmlTable();


            //                result.As<ContainerToken>().Should().NotBeNull()
            //                    .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
            //                    .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(5);

            //                debugTable.Should().Be(@"<html>
            //<header>
            //<style>
            //table,th,td{
            //border: 1px solid black;
            //border-collapse: collapse;
            //}
            //th,td{
            //padding: 5px;
            //text-align: center;
            //}
            //</style>
            //</header>
            //<body>
            //<table>
            //<tr>
            //<td>
            //0
            //</td>
            //<td colspan=""23"" rowspan=""1"">
            //Shield
            //</td>
            //</tr>
            //<tr>
            //<td>
            //1
            //</td>
            //<td colspan=""6"" rowspan=""1"">
            //Field
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LightSeparator
            //</td>
            //<td colspan=""5"" rowspan=""1"">
            //Charge
            //</td>
            //<td colspan=""8"" rowspan=""1"">
            //Charge
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //AllCounterChanged
            //</td>
            //</tr>
            //<tr>
            //<td>
            //2
            //</td>
            //<td colspan=""6"" rowspan=""1"">
            //Division
            //</td>
            //<td colspan=""0"" rowspan=""8"">
            //,
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //Location
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //SimpleCharge
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //Location
            //</td>
            //<td colspan=""6"" rowspan=""1"">
            //PositionnedCharges
            //</td>
            //<td colspan=""0"" rowspan=""8"">
            //all
            //</td>
            //<td colspan=""0"" rowspan=""8"">
            //counter
            //</td>
            //<td colspan=""0"" rowspan=""8"">
            //changed
            //</td>
            //</tr>
            //<tr>
            //<td>
            //3
            //</td>
            //<td colspan=""6"" rowspan=""1"">
            //DivisionBy2
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //SimpleLocation
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Determiner
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //Symbol
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //SimpleLocation
            //</td>
            //<td colspan=""6"" rowspan=""1"">
            //SimplePositionnedCharges
            //</td>
            //</tr>
            //<tr>
            //<td>
            //4
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //DivisionBy2Name
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //SimpleDivisionBy2Field
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LocationBefore
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LocationPoint
            //</td>
            //<td colspan=""0"" rowspan=""6"">
            //a
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //SymbolName
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //SymbolAttitude
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LocationBefore
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LocationPoint
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //Charge
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //PositionBetween
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //Charge
            //</td>
            //</tr>
            //<tr>
            //<td>
            //5
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //Party
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //per
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //fesse
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Tincture
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //And
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Tincture
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //in
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //chief
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //greyhound
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //courant
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //in
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //base
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //SimpleCharge
            //</td>
            //<td colspan=""0"" rowspan=""5"">
            //within
            //</td>
            //<td colspan=""3"" rowspan=""1"">
            //SimpleCharge
            //</td>
            //</tr>
            //<tr>
            //<td>
            //6
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //SimpleTincture
            //</td>
            //<td colspan=""0"" rowspan=""4"">
            //and
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //SimpleTincture
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Determiner
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Symbol
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //Determiner
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //Ordinary
            //</td>
            //</tr>
            //<tr>
            //<td>
            //7
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //TinctureMetal
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //TinctureColour
            //</td>
            //<td colspan=""0"" rowspan=""3"">
            //an
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //SymbolName
            //</td>
            //<td colspan=""0"" rowspan=""3"">
            //a
            //</td>
            //<td colspan=""2"" rowspan=""1"">
            //SimpleOrdinary
            //</td>
            //</tr>
            //<tr>
            //<td>
            //8
            //</td>
            //<td colspan=""0"" rowspan=""2"">
            //or
            //</td>
            //<td colspan=""0"" rowspan=""2"">
            //sable
            //</td>
            //<td colspan=""0"" rowspan=""2"">
            //owl
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //OrdinaryHonourable
            //</td>
            //<td colspan=""1"" rowspan=""1"">
            //LineVariation
            //</td>
            //</tr>
            //<tr>
            //<td>
            //9
            //</td>
            //<td colspan=""0"" rowspan=""1"">
            //bordure
            //</td>
            //<td colspan=""0"" rowspan=""1"">
            //engrailed
            //</td>
            //</tr>
            //</table>
            //</body>
            //</html>");

            //            }
        }

        [Fact]
        internal void DivisionBy2_LineVariation()
        {
            var pilot = GetPilot(BlazonRepository.HendersonofStLawrence_Henry);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(3);

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
<td colspan=""20"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""7"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""12"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""7"" rowspan=""1"">
Division
</td>
<td colspan=""0"" rowspan=""12"">
,
</td>
<td colspan=""12"" rowspan=""1"">
MultiCharges
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""7"" rowspan=""1"">
DivisionBy2
</td>
<td colspan=""4"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""7"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""3"" rowspan=""1"">
DivisionBy2Name
</td>
<td colspan=""1"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""3"" rowspan=""1"">
SimpleDivisionBy2Field
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""10"">
and
</td>
<td colspan=""7"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""9"">
Parted
</td>
<td colspan=""0"" rowspan=""9"">
per
</td>
<td colspan=""0"" rowspan=""9"">
pale
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
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
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""7"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""8"">
indented
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""8"">
and
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""3"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
PositionBefore
</td>
<td colspan=""3"" rowspan=""1"">
Charge
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
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""7"">
two
</td>
<td colspan=""2"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""7"">
on
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""6"">
Sable
</td>
<td colspan=""0"" rowspan=""6"">
Argent
</td>
<td colspan=""0"" rowspan=""6"">
harts’
</td>
<td colspan=""0"" rowspan=""6"">
attires
</td>
<td colspan=""1"" rowspan=""1"">
CounterChanged
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
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
<td colspan=""0"" rowspan=""5"">
counterchanged
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
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
Gules
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""3"">
Or
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""2"">
chief
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""1"">
crescent
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void DivisionBy2_TinctureRefactoring()
        {
            var pilot = GetPilot(BlazonRepository.DevonEveleigh);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(3);

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
<td colspan=""13"" rowspan=""1"">
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
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""7"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""5"" rowspan=""1"">
Division
</td>
<td colspan=""0"" rowspan=""9"">
,
</td>
<td colspan=""7"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""5"" rowspan=""1"">
DivisionBy2
</td>
<td colspan=""7"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""2"" rowspan=""1"">
DivisionBy2Name
</td>
<td colspan=""3"" rowspan=""1"">
SimpleDivisionBy2Field
</td>
<td colspan=""2"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""4"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""6"">
Per
</td>
<td colspan=""0"" rowspan=""6"">
pale
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
<td colspan=""2"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""6"">
between
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""5"">
and
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
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
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""3"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""3"">
or
</td>
<td colspan=""0"" rowspan=""3"">
sable
</td>
<td colspan=""0"" rowspan=""3"">
two
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""0"" rowspan=""3"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
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
OrdinaryDiminutive
</td>
<td colspan=""0"" rowspan=""2"">
griffins
</td>
<td colspan=""0"" rowspan=""2"">
passant
</td>
<td colspan=""1"" rowspan=""1"">
CounterChanged
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""1"">
chevronels
</td>
<td colspan=""0"" rowspan=""1"">
counterchanged
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void DivisionBy2_PositionnedHalves()
        {
            var pilot = GetPilot(BlazonRepository.DivisionNevers);
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
<td colspan=""35"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""35"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""35"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""35"" rowspan=""1"">
DivisionBy2
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""2"" rowspan=""1"">
DivisionBy2Name
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""32"" rowspan=""1"">
PositionnedHalves
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""14"">
Per
</td>
<td colspan=""0"" rowspan=""14"">
fess
</td>
<td colspan=""0"" rowspan=""14"">
,
</td>
<td colspan=""1"" rowspan=""1"">
FirstDivisionNumber
</td>
<td colspan=""18"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
SecondDivisionNumber
</td>
<td colspan=""12"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""13"">
1
</td>
<td colspan=""17"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""0"" rowspan=""13"">
2
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
<td colspan=""6"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""17"" rowspan=""1"">
Division
</td>
<td colspan=""0"" rowspan=""12"">
,
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""6"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""17"" rowspan=""1"">
DivisionBy2
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""5"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""4"" rowspan=""1"">
FieldVariation
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""2"" rowspan=""1"">
DivisionBy2Name
</td>
<td colspan=""15"" rowspan=""1"">
SimpleDivisionShield
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
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""4"" rowspan=""1"">
FieldVariation2Tinctures
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""9"">
per
</td>
<td colspan=""0"" rowspan=""9"">
pale
</td>
<td colspan=""7"" rowspan=""1"">
Shield
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""7"" rowspan=""1"">
Shield
</td>
<td colspan=""0"" rowspan=""9"">
azure
</td>
<td colspan=""0"" rowspan=""9"">
three
</td>
<td colspan=""3"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""9"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
FieldVariationName
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
11
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""8"">
and
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""6"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""8"">
fleurs
</td>
<td colspan=""0"" rowspan=""8"">
de
</td>
<td colspan=""0"" rowspan=""8"">
lys
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""8"">
compony
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""8"">
and
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
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""6"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""7"">
bordure
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
13
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
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""4"" rowspan=""1"">
FieldVariation
</td>
<td colspan=""0"" rowspan=""6"">
or
</td>
<td colspan=""0"" rowspan=""6"">
argent
</td>
<td colspan=""0"" rowspan=""6"">
gules
</td>
</tr>
<tr>
<td>
14
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
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
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
<td colspan=""4"" rowspan=""1"">
FieldVariation2Tinctures
</td>
</tr>
<tr>
<td>
15
</td>
<td colspan=""0"" rowspan=""4"">
gules
</td>
<td colspan=""0"" rowspan=""4"">
an
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""4"">
an
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""4"">
Or
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
FieldVariationName
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
16
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""3"">
argent
</td>
<td colspan=""0"" rowspan=""3"">
escarbuncle
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
chequy
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
17
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""2"">
fess
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
18
</td>
<td colspan=""0"" rowspan=""1"">
escutcheon
</td>
<td colspan=""0"" rowspan=""1"">
Or
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
        internal void DivisionBy2_PositionnedHalvesPale()
        {
            //todo semy support see #27: https://gitlab.com/gblason/webclient/issues/27
            //var origin = BlazonRepository.SemeBrunswickLunebourg;

            ////first step, getting the keywords
            //var errors = new List<ParserError>();

            //using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(origin)))
            //{
            //    var keywords = new Detector(errors).DetectKeywords(ms);

            //    var result = new Spec.ShieldParser().TryConsume(TokenParsingPosition.DefaultStartingPosition) as ContainerToken;

            //    var debugTable = result.ToHtmlTable();

            //    result.As<ContainerToken>().Should().NotBeNull()
            //        .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
            //        .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(3);

            //    debugTable.Should().Be(@"");

            //}
        }
    }
}
