//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class BBFormatterContainer : FormatterContainer
    {
        #region Patterns
        private readonly Regex BoldRegex = new Regex(@"\[b\](.+?)\[\/b\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Bold_Replacement = "<span style=\"font-weight:bold;\">${1}</span>";
        private readonly Regex ItalicRegex = new Regex(@"\[i\](.+?)\[\/i\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Italic_Replacement = "<span style=\"font-style:italic;\">${1}</span>";
        private readonly Regex UnderlineRegex = new Regex(@"\[u\](.+?)\[\/u\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Underline_Replacement = "<span style=\"text-decoration:underline;\">${1}</span>";
        private readonly Regex StrikeThoughRegex = new Regex(@"\[s\](.+?)\[\/s\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string StrikeThough_Replacement = "<span style=\"text-decoration:line-through;\">$1</span>";
        private readonly Regex ColorRegex = new Regex(@"\[color=(.+?)\](.+?)\[\/color\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Color_Replacement = "<span style=\"color:$1\">$2</span>";
        private readonly Regex CenterRegex = new Regex(@"\[center\](.+?)\[\/center\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Center_Replacement = "<span style=\"text-align:center;\">$1</span>";
        private readonly Regex SizeRegex = new Regex(@"\[size=(.+?)\](.+?)\[\/size\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Size_Replacement = "<span style=\"font-size:$1\">$2</span>";
        private readonly Regex QuoteRegex = new Regex(@"\[quote\](.+?)\[\/quote\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Quote_Replacement = "<blockquote><p>$1</p></blockquote>";
        private readonly Regex QuoteRegex2 = new Regex(@"\[quote=(.+?)\](.+?)\[\/quote\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Quote_Replacement2 = "<blockquote><p><b>$1</b></p><p>$2</p></blockquote>";
        private readonly Regex MailRegex = new Regex(@"\[email\](.+?)\[\/email\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Mail_Replacement = "<a href=\"mailto:$1\" rel=\"nofollow\">mailto:$1</a>";
        private readonly Regex LinkRegex = new Regex(@"\[url\](.+?)\[\/url\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Link_Replacement = "<a href=\"$1\" rel=\"nofollow\">$1</a>";
        private readonly Regex Link2Regex = new Regex(@"\[url=(.+?)\](.+?)\[\/url\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Link2_Replacement = "<a href=\"$1\" rel=\"nofollow\">$2</a>";
        private readonly Regex ImgRegex = new Regex(@"\[img\](.+?)\[\/img\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Img_Replacement = "<img src=\"$1\" alt=\"\" />";
        private readonly Regex ImgRegex1 = new Regex(@"\[img width=(.+?) height=(.+?)\](.+?)\[\/img\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Img_Replacement1 = "<img src=\"$3\" alt=\"\" width=\"$1\" height=\"$2\" />";
        private readonly Regex ImgRegex2 = new Regex(@"\[img (.+?)x(.+?)\](.+?)\[\/img\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Img_Replacement2 = "<img src=\"$3\" alt=\"\" width=\"$1\" height=\"$2\" />";
        private readonly Regex OrderListRegex = new Regex(@"\[ol\](.+?)\[\/ol\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string OrderList_Replacement = "<ol>${1}</ol>";
        private readonly Regex UnOrderListRegex = new Regex(@"\[ul\](.+?)\[\/ul\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string UnOrderList_Replacement = "<ul>${1}</ul>";
        private readonly Regex ListItemRegex = new Regex(@"\[li\](.+?)\[\/li\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string ListItem_Replacement = "<li>${1}</li>";
        private readonly Regex TableRegex = new Regex(@"\[table\](.+?)\[\/table\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Table_Replacement = "<table>${1}</table>";
        private readonly Regex TableRowRegex = new Regex(@"\[tr\](.+?)\[\/tr\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string TableRow_Replacement = "<tr>${1}</tr>";
        private readonly Regex TableCellRegex = new Regex(@"\[td\](.+?)\[\/td\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string TableCell_Replacement = "<td>${1}</td>";
        private readonly Regex TableHeaderCellRegex = new Regex(@"\[th\](.+?)\[\/th\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string TableHeaderCell_Replacement = "<th>${1}</th>";
        private readonly Regex H1Regex = new Regex(@"\[h1\](.+?)\[\/h1\]", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H1_Replacement = "<h1>$1</h1>";
        private readonly Regex H2Regex = new Regex(@"\[h2\](.+?)\[\/h2\]", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H2_Replacement = "<h2>$1</h2>";
        private readonly Regex H3Regex = new Regex(@"\[h3\](.+?)\[\/h3\]", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H3_Replacement = "<h3>$1</h3>";
        private readonly Regex H4Regex = new Regex(@"\[h4\](.+?)\[\/h4\]", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H4_Replacement = "<h4>$1</h4>";
        private readonly Regex H5Regex = new Regex(@"\[h5\](.+?)\[\/h5\]", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H5_Replacement = "<h5>$1</h5>";
        private readonly Regex REGEX_BREAK = new Regex(@"(\r\n|\r|\n|\n\r)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //"([\r\n])[\s]+"
        #endregion

        public BBFormatterContainer(bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false)
        {
            Formatters.Add(new BBCodeFormatter() 
            {
                Alternate = alternate,
                EmbedStyleSheet = embedStyleSheet,
                LineNumbers = lineNumbers 
            });

            AddFormat(BoldRegex, Bold_Replacement, true);
            AddFormat(ItalicRegex, Italic_Replacement, true);
            AddFormat(UnderlineRegex, Underline_Replacement, true);
            AddFormat(StrikeThoughRegex, StrikeThough_Replacement, true);
            AddFormat(ColorRegex, Color_Replacement, true);
            AddFormat(CenterRegex, Center_Replacement, true);  
            AddFormat(SizeRegex, Size_Replacement, true);
            AddFormat(MailRegex, Mail_Replacement, true);
            AddFormat(LinkRegex, Link_Replacement, true);
            AddFormat(Link2Regex, Link2_Replacement, true);
            AddFormat(QuoteRegex, Quote_Replacement, true);
            AddFormat(QuoteRegex2, Quote_Replacement2, true);
            AddFormat(ImgRegex, Img_Replacement, true);
            AddFormat(ImgRegex1, Img_Replacement1, true);
            AddFormat(ImgRegex2, Img_Replacement2, true);
            AddFormat(OrderListRegex, OrderList_Replacement, true);
            AddFormat(UnOrderListRegex, UnOrderList_Replacement, true);
            AddFormat(ListItemRegex, ListItem_Replacement, true);
            AddFormat(TableRegex, Table_Replacement, true);
            AddFormat(TableRowRegex, TableRow_Replacement, true);
            AddFormat(TableCellRegex, TableCell_Replacement, true);
            AddFormat(TableHeaderCellRegex, TableHeaderCell_Replacement, true); 
            AddFormat(H5Regex, H5_Replacement, true);
            AddFormat(H4Regex, H4_Replacement, true);
            AddFormat(H3Regex, H3_Replacement, true);
            AddFormat(H2Regex, H2_Replacement, true);
            AddFormat(H1Regex, H1_Replacement, true);
            AddFormat(REGEX_BREAK, "<br/>",true);

            Alternate = alternate;
            EmbedStyleSheet = embedStyleSheet;
            LineNumbers = lineNumbers;
        }

        //public override string Format(string text)
        //{
        //    return base.Format(text).Replace("\r\n/g","<br/>");
        //}
        public bool Alternate { get; set; }

        public bool EmbedStyleSheet { get; set; }

        public bool LineNumbers { get; set; }
    }
}
