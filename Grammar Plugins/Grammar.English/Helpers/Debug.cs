using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Token;

namespace Grammar.English.Helpers
{
    /// <summary>
    /// Helper containing static and extensions to trace and debug purpose
    /// </summary>    
    public static class Debug
    {
        /// <summary>
        /// Turn a <see cref="Token"/> tree in a string representing an HTML table
        /// </summary>
        /// <param name="root">The root element to turn into table</param>
        /// <returns>A string representing the token and its children</returns>
        public static string ToHtmlTable(this ContainerToken root)
        {
            if (root == null)
            {
                return string.Empty;
            }
            //building the lines
            var lines = new List<HtmlTableLine>();
            var firstCell = new HtmlTableCell { Content = root.Type.ToString(), Id = Guid.NewGuid() };
            var firstLine = new HtmlTableLine
            {
                Level = 0,
                Cells = new List<HtmlTableCell>
                {
                    firstCell
                }
            };
            lines.Add(firstLine);
            CreateLine(root.Children, lines, firstCell, 0);

            var htmlResult = new StringBuilder();

            htmlResult.Append($"<html>{Environment.NewLine}");
            htmlResult.Append($"<header>{Environment.NewLine}");
            htmlResult.Append($"<style>{Environment.NewLine}");
            htmlResult.Append($"table,th,td{{{ Environment.NewLine}");
            htmlResult.Append($"border: 1px solid black;{Environment.NewLine}");
            htmlResult.Append($"border-collapse: collapse;{Environment.NewLine}");
            htmlResult.Append($"}}{ Environment.NewLine}");
            htmlResult.Append($"th,td{{{ Environment.NewLine}");
            htmlResult.Append($"padding: 5px;{Environment.NewLine}");
            htmlResult.Append($"text-align: center;{Environment.NewLine}");
            htmlResult.Append($"}}{ Environment.NewLine}");
            htmlResult.Append($"</style>{Environment.NewLine}");
            htmlResult.Append($"</header>{Environment.NewLine}");
            htmlResult.Append($"<body>{Environment.NewLine}");
            htmlResult.Append($"<table>{Environment.NewLine}");
            //lines.Reverse();
            foreach (var line in lines)
            {
                htmlResult.Append($"<tr>{Environment.NewLine}");
                htmlResult.Append($"<td>{Environment.NewLine}");
                htmlResult.Append($"{line.Level}{Environment.NewLine}");
                htmlResult.Append($"</td>{Environment.NewLine}");
                foreach (var cell in line.Cells)
                {
                    var deep = 1;
                    var beforeL = lines.FirstOrDefault(l => l.Level == line.Level + 1);
                    //if we are at a leaf then we have a row span
                    if (beforeL != null && beforeL.Cells.All(c => c.ParentId != cell.Id))
                    {
                        //we are a leaf
                        deep = lines.Count - line.Level;
                    }

                    htmlResult.Append($"<td colspan=\"{cell.ChildCount}\" rowspan=\"{deep}\">{Environment.NewLine}");
                    htmlResult.Append($"{cell.Content}{Environment.NewLine}");
                    htmlResult.Append($"</td>{Environment.NewLine}");
                }
                htmlResult.Append($"</tr>{Environment.NewLine}");

            }

            htmlResult.Append($"</table>{Environment.NewLine}");
            htmlResult.Append($"</body>{Environment.NewLine}");
            htmlResult.Append("</html>");
            return htmlResult.ToString();
        }

        private static void CreateLine(IEnumerable<object> roots, List<HtmlTableLine> lines, HtmlTableCell parent, int level)
        {
            if (roots == null)
            {
                return;
            }

            var currentLevel = level + 1;

            var currentLine = lines.FirstOrDefault(l => l.Level == currentLevel);
            if (currentLine == null)
            {
                currentLine = new HtmlTableLine { Level = currentLevel };
                lines.Add(currentLine);
            }

            foreach (var root in roots)
            {
                if (root is ContainerToken container)
                {
                    var nc = new HtmlTableCell { Content = container.Type.ToString(), Id = Guid.NewGuid(), ParentId = parent.Id, ChildCount = 0 };
                    currentLine.Cells.Add(nc);
                    CreateLine(container.Children, lines, nc, currentLevel);
                    parent.ChildCount += nc.ChildCount;
                }
                else if (root is LeafToken leaf)
                {
                    var lc = new HtmlTableCell { Content = leaf.Type.ToString(), Id = Guid.NewGuid(), ParentId = parent.Id, ChildCount = 0 };
                    currentLine.Cells.Add(lc);
                    CreateLine(leaf.OriginalKw, lines, lc, currentLevel);
                    parent.ChildCount += lc.ChildCount;
                }
                else if (root is ParsedKeyword pkw)
                {
                    parent.ChildCount += 1;
                    var fc = new HtmlTableCell { Content = pkw.Value, Id = new Guid(), ParentId = parent.Id };
                    currentLine.Cells.Add(fc);
                }
            }
        }
    }

    internal class HtmlTableLine
    {
        internal int Level { get; set; }


        internal List<HtmlTableCell> Cells { get; set; } = new List<HtmlTableCell>();
    }

    internal class HtmlTableCell
    {
        internal string Content { get; set; }

        internal int ChildCount { get; set; }

        internal Guid Id { get; set; }

        internal Guid ParentId { get; set; }
    }
}
