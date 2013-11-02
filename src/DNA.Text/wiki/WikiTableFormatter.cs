//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class WikiTableFormatter : ExplicitlyFormatter
    {
        private static readonly Regex regex = new Regex(@"\{\|((.|\n|\r)+?)\|\}", RegexOptions.Compiled | RegexOptions.Singleline);

        public WikiTableFormatter() : base() { }

        public override System.Text.RegularExpressions.Regex Regex
        {
            get { return regex; }
        }

        public override string GetReplaceResult(System.Text.RegularExpressions.Match match)
        {
            return BuildTable(match.Value);
        }

        /// <summary>
        /// Builds a HTML table from WikiMarkup.
        /// </summary>
        /// <param name="table">The WikiMarkup.</param>
        /// <returns>The HTML.</returns>
        private string BuildTable(string table)
        {
            // Proceed line-by-line, ignoring the first and last one
            string[] lines = table.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
            {
                return "<b>FORMATTER ERROR (Malformed Table)</b>";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<table");
            if (lines[0].Length > 2)
            {
                sb.Append(" ");
                sb.Append(lines[0].Substring(3));
            }
            sb.Append(">");
            int count = 1;
            if (lines[1].Length >= 3 && lines[1].Trim().StartsWith("|+"))
            {
                // Table caption
                sb.Append("<caption>");
                sb.Append(lines[1].Substring(3));
                sb.Append("</caption>");
                count++;
            }

            if (!lines[count].StartsWith("|-")) sb.Append("<tr>");

            bool thAdded = false;

            string item;
            for (int i = count; i < lines.Length - 1; i++)
            {
                if (lines[i].Trim().StartsWith("|-"))
                {
                    // New line
                    if (i != count) sb.Append("</tr>");

                    sb.Append("<tr");
                    if (lines[i].Length > 2)
                    {
                        string style = lines[i].Substring(3);
                        if (style.Length > 0)
                        {
                            sb.Append(" ");
                            sb.Append(style);
                        }
                    }
                    sb.Append(">");
                }
                else if (lines[i].Trim().StartsWith("|"))
                {
                    // Cell
                    if (lines[i].Length < 3) continue;
                    item = lines[i].Substring(2);
                    if (item.IndexOf(" || ") != -1)
                    {
                        sb.Append("<td>");
                        sb.Append(item.Replace(" || ", "</td><td>"));
                        sb.Append("</td>");
                    }
                    else if (item.IndexOf(" | ") != -1)
                    {
                        sb.Append("<td ");
                        sb.Append(item.Substring(0, item.IndexOf(" | ")));
                        sb.Append(">");
                        sb.Append(item.Substring(item.IndexOf(" | ") + 3));
                        sb.Append("</td>");
                    }
                    else
                    {
                        sb.Append("<td>");
                        sb.Append(item);
                        sb.Append("</td>");
                    }
                }
                else if (lines[i].Trim().StartsWith("!"))
                {
                    // Header
                    if (lines[i].Length < 3) continue;

                    // only if ! is found in the first row of the table, it is an header
                    if (lines[i + 1] == "|-") thAdded = true;

                    item = lines[i].Substring(2);
                    if (item.IndexOf(" !! ") != -1)
                    {
                        sb.Append("<th>");
                        sb.Append(item.Replace(" !! ", "</th><th>"));
                        sb.Append("</th>");
                    }
                    else if (item.IndexOf(" ! ") != -1)
                    {
                        sb.Append("<th ");
                        sb.Append(item.Substring(0, item.IndexOf(" ! ")));
                        sb.Append(">");
                        sb.Append(item.Substring(item.IndexOf(" ! ") + 3));
                        sb.Append("</th>");
                    }
                    else
                    {
                        sb.Append("<th>");
                        sb.Append(item);
                        sb.Append("</th>");
                    }
                }
            }
            if (sb.ToString().EndsWith("<tr>"))
            {
                sb.Remove(sb.Length - 4 - 1, 4);
                sb.Append("</table>");
            }
            else
            {
                sb.Append("</tr></table>");
            }
            sb.Replace("<tr></tr>", "");

            // Add <thead>, <tbody> tags, if table contains header
            if (thAdded)
            {
                int thIndex = sb.ToString().IndexOf("<th");
                //if(thIndex >= 4) sb.Insert(thIndex - 4, "<thead>");
                sb.Insert(thIndex - 4, "<thead>");

                // search for the last </th> tag in the first row of the table
                int thCloseIndex = -1;
                int thCloseIndex_temp = -1;
                do
                {
                    thCloseIndex = thCloseIndex_temp;
                    thCloseIndex_temp = sb.ToString().IndexOf("</th>", thCloseIndex + 1);
                }
                while (thCloseIndex_temp != -1/* && thCloseIndex_temp < sb.ToString().IndexOf("</tr>") #443, but disables row-header support */);

                sb.Insert(thCloseIndex + 10, "</thead><tbody>");
                sb.Insert(sb.Length - 8, "</tbody>");
            }

            return sb.ToString();
        }
    }
}
