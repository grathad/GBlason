using System.Collections.Generic;
using FluentAssertions;
using Grammar.English.Helpers;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Token;
using Xunit;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class DivisionBy3 : IntegrationTest
    {
        [Fact]
        internal void DivisionBy3_tierced()
        {
            var pilot = GetPilot(BlazonRepository.DivisionTierced);
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
<td colspan=""8"" rowspan=""1"">
Shield
</td>
</tr>
<tr>
<td>
1
</td>
<td colspan=""8"" rowspan=""1"">
Field
</td>
</tr>
<tr>
<td>
2
</td>
<td colspan=""8"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""8"" rowspan=""1"">
DivisionBy3
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""3"" rowspan=""1"">
DivisionBy3Name
</td>
<td colspan=""5"" rowspan=""1"">
SimpleDivisionBy2Field
</td>
</tr>
<tr>
<td>
5
</td>
<td colspan=""0"" rowspan=""4"">
Tierced
</td>
<td colspan=""0"" rowspan=""4"">
per
</td>
<td colspan=""0"" rowspan=""4"">
fess
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
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
,
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
<td colspan=""0"" rowspan=""1"">
azure
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
        internal void DivisionBy3_pall()
        {
            var pilot = GetPilot(BlazonRepository.DivisionPall);
            var position = TokenParsingPosition.DefaultStartingPosition;
            var result = pilot.Parse(position).ResultToken as ContainerToken;


            var debugTable = result.ToHtmlTable();

            result.As<ContainerToken>().Should().NotBeNull()
                .And.Subject.As<ContainerToken>().Children.Should().NotBeEmpty()
                .And.Subject.As<IEnumerable<Token>>().Should().HaveCount(1);
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
<td colspan=""7"" rowspan=""1"">
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
</tr>
<tr>
<td>
2
</td>
<td colspan=""7"" rowspan=""1"">
Division
</td>
</tr>
<tr>
<td>
3
</td>
<td colspan=""7"" rowspan=""1"">
DivisionBy3
</td>
</tr>
<tr>
<td>
4
</td>
<td colspan=""2"" rowspan=""1"">
DivisionBy3Name
</td>
<td colspan=""5"" rowspan=""1"">
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
Pall
</td>
<td colspan=""1"" rowspan=""1"">
Tincture
</td>
<td colspan=""1"" rowspan=""1"">
LightSeparator
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
,
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
TinctureColour
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
<td colspan=""0"" rowspan=""1"">
Gules
</td>
<td colspan=""0"" rowspan=""1"">
azure
</td>
<td colspan=""0"" rowspan=""1"">
Argent
</td>
</tr>
</table>
</body>
</html>");
        }
    }
}
