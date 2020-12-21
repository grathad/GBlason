using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class Tincture : IntegrationTest
    {
        [Fact]
        internal void Tincture_SimpleFur()
        {
            //first step, getting the keywords
            var pilot = GetPilot(BlazonRepository.FieldVariationEtampes);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(2);

            //I recently changed (I25) the logic of a semy to have at first a simple tincture and not a tincture

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
<td colspan=""5"" rowspan=""1"">
Field
</td>
<td colspan=""6"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""5"" rowspan=""1"">
FieldVariation
</td>
<td colspan=""6"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""5"" rowspan=""1"">
FieldVariationSemy
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
4
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""2"" rowspan=""1"">
Semy
</td>
<td colspan=""2"" rowspan=""1"">
SimpleCharge
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
5
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""5"">
seme
</td>
<td colspan=""0"" rowspan=""5"">
de
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
</td>
<td colspan=""0"" rowspan=""5"">
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
6
</td>
<td colspan=""0"" rowspan=""4"">
Azure
</td>
<td colspan=""1"" rowspan=""1"">
SymbolName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""4"">
compony
</td>
<td colspan=""1"" rowspan=""1"">
TinctureFur
</td>
<td colspan=""0"" rowspan=""4"">
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
<td colspan=""0"" rowspan=""3"">
lys
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""3"">
bend
</td>
<td colspan=""1"" rowspan=""1"">
SimpleFur
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""2"">
ermine
</td>
<td colspan=""0"" rowspan=""2"">
gules
</td>
</tr>
<tr>
<td>
9
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
        internal void Tincture_Vair()
        {
            var pilot = GetPilot(BlazonRepository.BelchesofTofts);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""0"" rowspan=""8"">
,
</td>
<td colspan=""7"" rowspan=""1"">
MultiCharges
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
<td colspan=""3"" rowspan=""1"">
Charge
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
SimplestCharge
</td>
<td colspan=""0"" rowspan=""6"">
and
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""5"">
Or
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
Determiner
</td>
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureFur
</td>
</tr>
<tr>
<td>
7
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
Vair
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryDiminutive
</td>
<td colspan=""0"" rowspan=""2"">
Gules
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""1"" rowspan=""1"">
VairName
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""0"" rowspan=""1"">
pallets
</td>
<td colspan=""0"" rowspan=""1"">
chief
</td>
<td colspan=""0"" rowspan=""1"">
Vair
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void Tincture_Vaire()
        {
            var pilot = GetPilot(BlazonRepository.FurPlumeté);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""4"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""4"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""4"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""4"" rowspan=""1"">
TinctureFur
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""4"" rowspan=""1"">
Vaire
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""1"" rowspan=""1"">
VaireName
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
And
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""2"">
Plumeté
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
<td colspan=""0"" rowspan=""2"">
and
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""1"">
or
</td>
<td colspan=""0"" rowspan=""1"">
sable
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void Tincture_VaireBetween()
        {
            var pilot = GetPilot(BlazonRepository.FurPapellony);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""3"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""3"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""3"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""3"" rowspan=""1"">
TinctureFur
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""3"" rowspan=""1"">
Vaire
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
VaireBetweenName
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""2"">
papellony
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""1"">
Gules
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
        internal void Tincture_Colour()
        {
            var pilot = GetPilot(BlazonRepository.TinctureAzure);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""1"" rowspan=""1"">
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
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""1"">
Azure
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void Tincture_Metal()
        {
            var pilot = GetPilot(BlazonRepository.TinctureArgent);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""1"" rowspan=""1"">
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
</tr>
<tr>
<td>
2
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
5
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
        internal void Tincture_Reference()
        {
            var pilot = GetPilot(BlazonRepository.DevonBellot);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position)?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""11"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
Charge
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
Argent
</td>
<td colspan=""0"" rowspan=""7"">
on
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""5"" rowspan=""1"">
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
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
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
<td colspan=""3"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
sable
</td>
<td colspan=""0"" rowspan=""3"">
cinquefoils
</td>
<td colspan=""3"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""2"">
chief
</td>
<td colspan=""3"" rowspan=""1"">
TinctureReference
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""1"">
of
</td>
<td colspan=""0"" rowspan=""1"">
the
</td>
<td colspan=""0"" rowspan=""1"">
field
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void Tincture_Proper()
        {
            var pilot = GetPilot(BlazonRepository.DevonAmadas);

            //var result = pilot.Parse(position, token: TokenNames.SimplePositionnedCharges);
            var result = pilot.Parse()?.ResultToken as ContainerToken;

            var debugTable = result.ToHtmlTable();
            var calls = pilot.CallTree;

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
<td colspan=""11"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""9"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
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
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
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
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureFur
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
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
SimpleFur
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
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""0"" rowspan=""3"">
ermine
</td>
<td colspan=""0"" rowspan=""3"">
oaken
</td>
<td colspan=""0"" rowspan=""3"">
slips
</td>
<td colspan=""0"" rowspan=""3"">
acorned
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""0"" rowspan=""2"">
chevron
</td>
<td colspan=""1"" rowspan=""1"">
TinctureProper
</td>
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""1"">
proper
</td>
</tr>
</table>
</body>
</html>");

        }
    }
}
