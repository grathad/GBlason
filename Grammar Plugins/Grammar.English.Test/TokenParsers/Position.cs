using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class Position : IntegrationTest

    {
        [Fact]
        internal void PositionnedCharge_WithinAll()
        {
            var pilot = GetPilot(BlazonRepository.AitchisonofSydserf);
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
<td colspan=""23"" rowspan=""1"">
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
<td colspan=""21"" rowspan=""1"">
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
<td colspan=""0"" rowspan=""14"">
,
</td>
<td colspan=""21"" rowspan=""1"">
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
<td colspan=""21"" rowspan=""1"">
ComplexPositionnedCharges
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""15"" rowspan=""1"">
MultiCharges
</td>
<td colspan=""1"" rowspan=""1"">
ComplexPositionnedChargeSeparator
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
<td colspan=""0"" rowspan=""11"">
Argent
</td>
<td colspan=""6"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""8"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""11"">
all
</td>
<td colspan=""0"" rowspan=""11"">
within
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""6"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""10"">
and
</td>
<td colspan=""8"" rowspan=""1"">
PositionnedCharges
</td>
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""6"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""8"" rowspan=""1"">
SimplePositionnedCharges
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""2"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""5"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
PositionBefore
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""4"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""8"">
a
</td>
<td colspan=""2"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""0"" rowspan=""7"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAlteration
</td>
<td colspan=""2"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
SymbolAttitude
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
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""1"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""0"" rowspan=""7"">
Sable
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""6"">
double
</td>
<td colspan=""0"" rowspan=""6"">
headed
</td>
<td colspan=""0"" rowspan=""6"">
eagle
</td>
<td colspan=""0"" rowspan=""6"">
displayed
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
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""0"" rowspan=""6"">
bordure
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
</td>
</tr>
<tr>
<td>
11
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
<td colspan=""3"" rowspan=""1"">
Symbol
</td>
<td colspan=""0"" rowspan=""5"">
invected
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""4"">
Sable
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
two
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
Vert
</td>
<td colspan=""0"" rowspan=""3"">
spur
</td>
<td colspan=""0"" rowspan=""3"">
rowells
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
14
</td>
<td colspan=""0"" rowspan=""2"">
chief
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
15
</td>
<td colspan=""0"" rowspan=""1"">
Or
</td>
</tr>
</table>
</body>
</html>");

        }
        
        [Fact]
        internal void PositionnedCharge_Within()
        {
            var pilot = GetPilot(BlazonRepository.OrdinaryAnstrutherOfAirdrie);
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""7"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
4
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
<td colspan=""3"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""6"">
Argent
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""6"">
within
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
6
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
7
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
8
</td>
<td colspan=""0"" rowspan=""3"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""3"">
a
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
9
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""2"">
Sable
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""2"">
Gules
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""1"">
piles
</td>
<td colspan=""0"" rowspan=""1"">
bordure
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void PositionnedCharge_Between()
        {
            var pilot = GetPilot(BlazonRepository.VariationOfLineFlintShire);
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
<td colspan=""20"" rowspan=""1"">
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
<td colspan=""19"" rowspan=""1"">
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
<td colspan=""19"" rowspan=""1"">
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
<td colspan=""19"" rowspan=""1"">
SimplePositionnedCharges
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
PositionBefore
</td>
<td colspan=""10"" rowspan=""1"">
Charge
</td>
<td colspan=""8"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""10"">
Argent
</td>
<td colspan=""0"" rowspan=""10"">
on
</td>
<td colspan=""10"" rowspan=""1"">
PositionnedCharges
</td>
<td colspan=""8"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""10"" rowspan=""1"">
SimplePositionnedCharges
</td>
<td colspan=""8"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""4"" rowspan=""1"">
Charge
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""2"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
between
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
between
</td>
<td colspan=""2"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""3"" rowspan=""1"">
Tincture
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""3"" rowspan=""1"">
SymbolCross
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
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""3"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
Cross
</td>
<td colspan=""1"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""1"" rowspan=""1"">
CrossVariation
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""4"">
four
</td>
<td colspan=""2"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""3"" rowspan=""1"">
TinctureReference
</td>
<td colspan=""0"" rowspan=""4"">
four
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""3"">
Cross
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
</td>
<td colspan=""0"" rowspan=""3"">
fleury
</td>
<td colspan=""0"" rowspan=""3"">
Sable
</td>
<td colspan=""0"" rowspan=""3"">
Cornish
</td>
<td colspan=""0"" rowspan=""3"">
Choughs
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""3"">
of
</td>
<td colspan=""0"" rowspan=""3"">
the
</td>
<td colspan=""0"" rowspan=""3"">
field
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""2"">
engrailed
</td>
<td colspan=""1"" rowspan=""1"">
TinctureProper
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
</tr>
<tr>
<td>
14
</td>
<td colspan=""0"" rowspan=""1"">
proper
</td>
<td colspan=""0"" rowspan=""1"">
Mascle
</td>
<td colspan=""0"" rowspan=""1"">
Plates
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void PositionnedCharge_OverAll()
        {
            var pilot = GetPilot(BlazonRepository.LundieofthatIlk);
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
<td colspan=""16"" rowspan=""1"">
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
<td colspan=""10"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""6"" rowspan=""1"">
FieldVariation
</td>
<td colspan=""10"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""6"" rowspan=""1"">
FieldVariation2Tinctures
</td>
<td colspan=""10"" rowspan=""1"">
ComplexPositionnedCharges
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
FieldVariationName
</td>
<td colspan=""1"" rowspan=""1"">
Of
</td>
<td colspan=""1"" rowspan=""1"">
FieldVariationNumber
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
PositionOverall
</td>
<td colspan=""8"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""8"">
Paly
</td>
<td colspan=""0"" rowspan=""8"">
of
</td>
<td colspan=""0"" rowspan=""8"">
six
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
<td colspan=""0"" rowspan=""8"">
and
</td>
<td colspan=""0"" rowspan=""8"">
overall
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
Charged
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
charged
</td>
<td colspan=""0"" rowspan=""7"">
with
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""6"">
Argent
</td>
<td colspan=""0"" rowspan=""6"">
Gules
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
</tr>
<tr>
<td>
8
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
</tr>
<tr>
<td>
9
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
three
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
10
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
Azure
</td>
<td colspan=""0"" rowspan=""3"">
cushions
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""2"">
bend
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""1"">
Argent
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void PositionnedCharge_In()
        {
            var pilot = GetPilot(BlazonRepository.MurrayofStruan_John);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(4);

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
<td colspan=""16"" rowspan=""1"">
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
<td colspan=""12"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""12"" rowspan=""1"">
MultiCharges
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""8"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
and
</td>
<td colspan=""5"" rowspan=""1"">
Location
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""6"">
Azure
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""5"" rowspan=""1"">
SimpleLocation
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""2"" rowspan=""1"">
SymbolName
</td>
</tr>
<tr>
<td>
6
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
LocationBefore
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""2"" rowspan=""1"">
LocationPoint
</td>
<td colspan=""1"" rowspan=""1"">
Point
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
for
</td>
<td colspan=""0"" rowspan=""5"">
difference
</td>
</tr>
<tr>
<td>
7
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
<td colspan=""0"" rowspan=""4"">
in
</td>
<td colspan=""0"" rowspan=""4"">
the
</td>
<td colspan=""0"" rowspan=""4"">
middle
</td>
<td colspan=""0"" rowspan=""4"">
chief
</td>
<td colspan=""0"" rowspan=""4"">
point
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
8
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""3"">
Argent
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
9
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""1"">
mullets
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
        internal void PositionnedCharge_SurmountedBy()
        {
            var pilot = GetPilot(BlazonRepository.Murray_David);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(4);

            //for difference is not supported yet and considered a charge in the output, will be likely to break when implemented
            //since in the current version we declare the parse as without logic, and will handle the refactoring in the second pass.
            //the result is not the final "correct" one it's just the intermediary consumption of all the tokens possible

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
<td colspan=""29"" rowspan=""1"">
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
<td colspan=""25"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""0"" rowspan=""17"">
,
</td>
<td colspan=""25"" rowspan=""1"">
MultiCharges
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""21"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""15"">
and
</td>
<td colspan=""3"" rowspan=""1"">
Location
</td>
<td colspan=""18"" rowspan=""1"">
PositionnedCharges
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""14"">
Azure
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
SimpleLocation
</td>
<td colspan=""18"" rowspan=""1"">
SimplePositionnedCharges
</td>
<td colspan=""2"" rowspan=""1"">
SymbolName
</td>
</tr>
<tr>
<td>
6
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
LocationBefore
</td>
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
LocationPoint
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""13"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""13"">
for
</td>
<td colspan=""0"" rowspan=""13"">
difference
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""12"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""12"">
in
</td>
<td colspan=""0"" rowspan=""12"">
the
</td>
<td colspan=""0"" rowspan=""12"">
centre
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""12"">
surmounted
</td>
<td colspan=""0"" rowspan=""12"">
by
</td>
<td colspan=""13"" rowspan=""1"">
MultiCharges
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""11"">
Argent
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""9"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
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
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""10"">
and
</td>
<td colspan=""3"" rowspan=""1"">
Location
</td>
<td colspan=""6"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""9"">
stars
</td>
<td colspan=""0"" rowspan=""9"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
SimpleLocation
</td>
<td colspan=""6"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""8"">
Argent
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
LocationBefore
</td>
<td colspan=""2"" rowspan=""1"">
LocationPoint
</td>
<td colspan=""2"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""2"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""7"">
cross
</td>
<td colspan=""0"" rowspan=""7"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""7"">
in
</td>
<td colspan=""0"" rowspan=""7"">
dexter
</td>
<td colspan=""0"" rowspan=""7"">
chief
</td>
<td colspan=""2"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""7"">
surmounted
</td>
<td colspan=""0"" rowspan=""7"">
by
</td>
<td colspan=""2"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""6"">
Gules
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
</tr>
<tr>
<td>
14
</td>
<td colspan=""0"" rowspan=""5"">
saltire
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
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
</tr>
<tr>
<td>
15
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
</tr>
<tr>
<td>
16
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
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
OrdinaryMobile
</td>
</tr>
<tr>
<td>
18
</td>
<td colspan=""0"" rowspan=""1"">
crescent
</td>
<td colspan=""0"" rowspan=""1"">
mullet
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void PositionnedCharge_SurmountedOf()
        {
            var pilot = GetPilot(BlazonRepository.OrdinaryRibbon);
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
<td colspan=""22"" rowspan=""1"">
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
<td colspan=""20"" rowspan=""1"">
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
<td colspan=""0"" rowspan=""12"">
,
</td>
<td colspan=""20"" rowspan=""1"">
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
<td colspan=""20"" rowspan=""1"">
ComplexPositionnedCharges
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""8"" rowspan=""1"">
SimplePositionnedCharges
</td>
<td colspan=""1"" rowspan=""1"">
ComplexPositionnedChargeSeparator
</td>
<td colspan=""1"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""10"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""9"">
Or
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
PositionBetween
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""0"" rowspan=""9"">
;
</td>
<td colspan=""0"" rowspan=""9"">
within
</td>
<td colspan=""10"" rowspan=""1"">
PositionnedCharges
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""8"">
surmounted
</td>
<td colspan=""0"" rowspan=""8"">
of
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""10"" rowspan=""1"">
SimplePositionnedCharges
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""3"" rowspan=""1"">
Charge
</td>
<td colspan=""2"" rowspan=""1"">
Charged
</td>
<td colspan=""5"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""3"" rowspan=""1"">
Symbol
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
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""6"">
charged
</td>
<td colspan=""0"" rowspan=""6"">
with
</td>
<td colspan=""5"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
9
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
<td colspan=""0"" rowspan=""5"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""4"">
lion
</td>
<td colspan=""0"" rowspan=""4"">
rampant
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryDiminutive
</td>
<td colspan=""0"" rowspan=""4"">
sable
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
<td colspan=""3"" rowspan=""1"">
Symbol
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""3"">
ribbon
</td>
<td colspan=""0"" rowspan=""3"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""3"">
three
</td>
<td colspan=""3"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
12
</td>
<td colspan=""0"" rowspan=""2"">
gules
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""2"">
azure
</td>
<td colspan=""0"" rowspan=""2"">
boars
</td>
<td colspan=""0"" rowspan=""2"">
heads
</td>
<td colspan=""0"" rowspan=""2"">
erased
</td>
<td colspan=""0"" rowspan=""2"">
or
</td>
</tr>
<tr>
<td>
13
</td>
<td colspan=""0"" rowspan=""1"">
bordure
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void PositionnedCharge_Between_CustomCharges()
        {
            var pilot = GetPilot(BlazonRepository.Liddell_RobertinEdinburgh);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
        }
    }
}
