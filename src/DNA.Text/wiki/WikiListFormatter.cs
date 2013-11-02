//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System.Text;
using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class WikiListFormatter : ExplicitlyFormatter
    {
        private readonly Regex regex = new Regex("(?<=(\\n|^))((\\*|\\#)+(\\ )?.+?\\n)+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public override Regex Regex
        {
            get { return regex; }
        }

        public WikiListFormatter() : base() { }

        public override string GetReplaceResult(System.Text.RegularExpressions.Match match)
        {
            var lines = match.Value.Split('\n');
            var d = 0;
            return GenerateList(lines, 0, 0, ref d);
        }

        /// <summary>
        /// Generates list HTML markup.
        /// </summary>
        /// <param name="lines">The lines in the list WikiMarkup.</param>
        /// <param name="line">The current line.</param>
        /// <param name="level">The current level.</param>
        /// <param name="currLine">The current line.</param>
        /// <returns>The correct HTML markup.</returns>
        private static string GenerateList(string[] lines, int line, int level, ref int currLine)
        {
            StringBuilder sb = new StringBuilder(200);
            if (lines[currLine][level] == '*') sb.Append("<ul>");
            else if (lines[currLine][level] == '#') sb.Append("<ol>");
            while (currLine <= lines.Length - 1 && CountBullets(lines[currLine]) >= level + 1)
            {
                if (CountBullets(lines[currLine]) == level + 1)
                {
                    sb.Append("<li>");
                    sb.Append(lines[currLine].Substring(CountBullets(lines[currLine])).Trim());
                    sb.Append("</li>");
                    currLine++;
                }
                else
                {
                    sb.Remove(sb.Length - 5, 5);
                    sb.Append(GenerateList(lines, currLine, level + 1, ref currLine));
                    sb.Append("</li>");
                }
            }
            if (lines[line][level] == '*') sb.Append("</ul>");
            else if (lines[line][level] == '#') sb.Append("</ol>");
            return sb.ToString();
        }

        private static int CountBullets(string line)
        {
            if (string.IsNullOrEmpty(line)) return 0;

            int res = 0, count = 0;
            while (line[count] == '*' || line[count] == '#')
            {
                res++;
                count++;
            }
            return res;
        }
    }


    
}
