using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class Symbol : IntegrationTest
    {
        /// <summary>
        /// Testing that we recognize a symbol
        /// </summary>
        [Fact]
        internal void SimpleSymbol()
        {
            var pilot = GetPilot(BlazonRepository.GordonofTacachie_Patrick);
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
<td colspan=""12"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""10"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""10"">
,
</td>
<td colspan=""10"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""10"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""4"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""7"">
Azure
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
between
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""5"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""3"" rowspan=""1"">
Symbol
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
8
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""3"" rowspan=""1"">
SymbolName
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
</tr>
<tr>
<td>
9
</td>
<td colspan=""0"" rowspan=""3"">
sheaf
</td>
<td colspan=""0"" rowspan=""3"">
of
</td>
<td colspan=""0"" rowspan=""3"">
arrows
</td>
<td colspan=""0"" rowspan=""3"">
boars’
</td>
<td colspan=""0"" rowspan=""3"">
heads
</td>
<td colspan=""0"" rowspan=""3"">
couped
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""1"">
Or
</td>
</tr>
</table>
</body>
</html>");
        }

        /// <summary>
        /// Testing that we recognize the alteration in the blazon
        /// </summary>
        [Fact]
        internal void Symbol_Alteration()
        {
            //var origin = BlazonRepository.Kinnoull_Earlof_Hay_;

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

        /// <summary>
        /// Testing that we consider a name of a symbol even if extended (fleur DE lis)
        /// Including testing variation of line
        /// </summary>
        [Fact]
        internal void Symbol_NamePlusExtension()
        {
            var pilot = GetPilot(BlazonRepository.VariationOfLineTouraine);
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
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
<td colspan=""7"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""7"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""5"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""4"" rowspan=""1"">
FieldVariation
</td>
</tr>
<tr>
<td>
4
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
<td colspan=""2"" rowspan=""1"">
Ordinary
</td>
<td colspan=""4"" rowspan=""1"">
FieldVariation2Tinctures
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""4"">
Azure
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
a
</td>
<td colspan=""2"" rowspan=""1"">
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
6
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
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""1"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""0"" rowspan=""3"">
compony
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
<td colspan=""0"" rowspan=""2"">
bordure
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
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
or
</td>
<td colspan=""0"" rowspan=""1"">
engrailed
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

        /// <summary>
        /// Testing reading the symbol's attitudes
        /// </summary>
        [Fact]
        internal void Symbol_Attitude()
        {
            //need advanced position support (see #28)
            //var origin = BlazonRepository.CottisingGloucester;

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

        /// <summary>
        /// Testing attitude + attribute of the attitude
        /// </summary>
        [Fact]
        internal void Symbol_AttitudeAttribute()
        {
            var pilot = GetPilot(BlazonRepository.DevonAmerideth);
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
<td colspan=""7"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""7"">
,
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
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
4
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
5
</td>
<td colspan=""0"" rowspan=""4"">
Gules
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
<td colspan=""1"" rowspan=""1"">
SymbolAttitudeAttribute
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""3"">
lion
</td>
<td colspan=""0"" rowspan=""3"">
rampant
</td>
<td colspan=""0"" rowspan=""3"">
regardant
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
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""1"">
or
</td>
</tr>
</table>
</body>
</html>");


        }

        /// <summary>
        /// Testing the reading of the tincture when there is no subcharge
        /// </summary>
        [Fact]
        internal void Symbol_Tincture()
        {

            var pilot = GetPilot(BlazonRepository.ChargeFleursDeLys);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(2);

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
<td colspan=""6"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
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
2
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
3
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
4
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
5
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
</tr>
<tr>
<td>
6
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
7
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""1"">
or
</td>
</tr>
</table>
</body>
</html>");


        }

        /// <summary>
        /// Testing the reading of sub parts groups, listed or not
        /// </summary>
        [Fact]
        internal void Symbol_SubPartGroups()
        {
            var pilot = GetPilot(BlazonRepository.ChargeLeón);
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
<td colspan=""1"" rowspan=""1"">
Field
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""11"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""9"">
,
</td>
<td colspan=""11"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""11"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""10"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""6"">
Argent
</td>
<td colspan=""0"" rowspan=""6"">
a
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
<td colspan=""7"" rowspan=""1"">
SymbolSubPartGroup
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""5"">
lion
</td>
<td colspan=""0"" rowspan=""5"">
rampant
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
SymbolSubPart
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
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
TinctureColour
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""4"">
,
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
purpure
</td>
<td colspan=""0"" rowspan=""3"">
crowned
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""3"">
langued
</td>
<td colspan=""0"" rowspan=""3"">
armed
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
TinctureMetal
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
Or
</td>
<td colspan=""0"" rowspan=""1"">
gules
</td>
</tr>
</table>
</body>
</html>");


        }
    }
}
