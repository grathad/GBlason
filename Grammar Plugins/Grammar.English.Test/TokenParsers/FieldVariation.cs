using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class FieldVariation : IntegrationTest
    {
        [Fact]
        internal void FieldVariation_SemyOnOrdinary()
        {
            var pilot = GetPilot(BlazonRepository.HayofMuntan_Alexander);
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
<td colspan=""14"" rowspan=""1"">
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
<td colspan=""12"" rowspan=""1"">
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
<td colspan=""12"" rowspan=""1"">
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
<td colspan=""8"" rowspan=""1"">
Charge
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""9"">
Argent
</td>
<td colspan=""3"" rowspan=""1"">
SimplestCharge
</td>
<td colspan=""0"" rowspan=""9"">
within
</td>
<td colspan=""8"" rowspan=""1"">
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
<td colspan=""3"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""5"" rowspan=""1"">
FieldVariation
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
<td colspan=""2"" rowspan=""1"">
Ordinary
</td>
<td colspan=""5"" rowspan=""1"">
FieldVariationSemy
</td>
</tr>
<tr>
<td>
8
</td>
<td colspan=""0"" rowspan=""6"">
three
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""6"">
a
</td>
<td colspan=""2"" rowspan=""1"">
SimpleOrdinary
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
</tr>
<tr>
<td>
9
</td>
<td colspan=""1"" rowspan=""1"">
OrdinarySubordinary
</td>
<td colspan=""0"" rowspan=""5"">
Gules
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""1"" rowspan=""1"">
LineVariationDefinition
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""5"">
semy
</td>
<td colspan=""0"" rowspan=""5"">
of
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
10
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryMobile
</td>
<td colspan=""0"" rowspan=""4"">
bordure
</td>
<td colspan=""1"" rowspan=""1"">
LineVariation
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
</tr>
<tr>
<td>
11
</td>
<td colspan=""0"" rowspan=""3"">
escutcheons
</td>
<td colspan=""0"" rowspan=""3"">
engrailed
</td>
<td colspan=""0"" rowspan=""3"">
cinquefoils
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
Argent
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void FieldVariation_SemyAsField()
        {
            var pilot = GetPilot(BlazonRepository.SemeFrance);
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
FieldVariation
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""5"" rowspan=""1"">
FieldVariationSemy
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
</tr>
<tr>
<td>
5
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""5"">
semy
</td>
<td colspan=""0"" rowspan=""5"">
de
</td>
<td colspan=""2"" rowspan=""1"">
Symbol
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
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""3"">
lis
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
TinctureMetal
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
        internal void FieldVariation_SemyFromName()
        {
            var pilot = GetPilot(BlazonRepository.PiersDeCoudray);
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
FieldVariation
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""3"" rowspan=""1"">
FieldVariationSemy
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""1"" rowspan=""1"">
SemyName
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
</td>
<td colspan=""0"" rowspan=""2"">
Billety
</td>
<td colspan=""1"" rowspan=""1"">
TinctureMetal
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""0"" rowspan=""1"">
Gules
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
        internal void FieldVariation_2Tincture()
        {
            var pilot = GetPilot(BlazonRepository.FieldVariationLaMarche);
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
SimpleTincture
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
bordure
</td>
<td colspan=""1"" rowspan=""1"">
TinctureColour
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
TinctureMetal
</td>
<td colspan=""0"" rowspan=""2"">
gules
</td>
<td colspan=""0"" rowspan=""2"">
argent
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
        internal void FieldVariation_2TinctureNumbered()
        {
            var pilot = GetPilot(BlazonRepository.Jackson);
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
<td colspan=""11"" rowspan=""1"">
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
<td colspan=""4"" rowspan=""1"">
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
<td colspan=""0"" rowspan=""7"">
,
</td>
<td colspan=""4"" rowspan=""1"">
SimplestCharge
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""6"" rowspan=""1"">
FieldVariation2Tinctures
</td>
<td colspan=""4"" rowspan=""1"">
SimpleCharge
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
<td colspan=""1"" rowspan=""1"">
Determiner
</td>
<td colspan=""3"" rowspan=""1"">
Symbol
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""4"">
Barry
</td>
<td colspan=""0"" rowspan=""4"">
of
</td>
<td colspan=""0"" rowspan=""4"">
ten
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""4"">
and
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
<td colspan=""1"" rowspan=""1"">
Tincture
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
<td colspan=""0"" rowspan=""3"">
lion
</td>
<td colspan=""0"" rowspan=""3"">
rampant
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
</tr>
<tr>
<td>
7
</td>
<td colspan=""0"" rowspan=""2"">
Argent
</td>
<td colspan=""0"" rowspan=""2"">
Azure
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
Gules
</td>
</tr>
</table>
</body>
</html>");
        }

        [Fact]
        internal void FieldVariation_2TinctureNameBetween()
        {
            var pilot = GetPilot(BlazonRepository.FieldVariationLozengy);
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
FieldVariation
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""3"" rowspan=""1"">
FieldVariation2Tinctures
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
FieldVariationName
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""3"">
lozengy
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
TinctureMetal
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
gules
</td>
</tr>
</table>
</body>
</html>");

        }

        [Fact]
        internal void FieldVariation_Predefinedsemy()
        {
            var pilot = GetPilot(BlazonRepository.SemyBezanty);
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
<td colspan=""1"" rowspan=""1"">
LightSeparator
</td>
<td colspan=""4"" rowspan=""1"">
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
<td colspan=""4"" rowspan=""1"">
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
<td colspan=""2"" rowspan=""1"">
SimpleCharge
</td>
<td colspan=""2"" rowspan=""1"">
FieldVariation
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
<td colspan=""1"" rowspan=""1"">
Ordinary
</td>
<td colspan=""2"" rowspan=""1"">
FieldVariationKnownSemy
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""4"">
or
</td>
<td colspan=""0"" rowspan=""4"">
a
</td>
<td colspan=""1"" rowspan=""1"">
SimpleOrdinary
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
PredefinedSemy
</td>
</tr>
<tr>
<td>
6
</td>
<td colspan=""1"" rowspan=""1"">
OrdinaryHonourable
</td>
<td colspan=""1"" rowspan=""1"">
SimpleTincture
</td>
<td colspan=""0"" rowspan=""3"">
bezanty
</td>
</tr>
<tr>
<td>
7
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
8
</td>
<td colspan=""0"" rowspan=""1"">
argent
</td>
</tr>
</table>
</body>
</html>");

        }
    }

}
