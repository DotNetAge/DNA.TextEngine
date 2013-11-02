//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class WikiFormatterContainer : FormatterContainer
    {
        #region Patterns
        private readonly Regex BoldRegex = new Regex("'''(.+?)'''", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Bold_Replacement = "<strong>$1</strong>";
        private readonly Regex ItalicRegex = new Regex("''(.+?)''", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Italic_Replacement = "<i>$1</i>";
        private readonly Regex UnderlineRegex = new Regex("__(.+?)__", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Underline_Replacement = "<span style=\"text-decoration:underline;\">$1</span>";
        private readonly Regex StrikeThoughRegex = new Regex("--(.+?)--", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string StrikeThough_Replacement = "<span style=\"text-decoration:line-through;\">$1</span>";
        private readonly Regex HorizontallineRegex = new Regex("^----$", RegexOptions.Compiled);
        private readonly string Horizontalline_Replacement = "<hr/>";
        private readonly Regex LineBreakRegex = new Regex("\\{br\\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly string LineBreak_Replacement = "<br/>";
        private readonly Regex SuperscriptRegex = new Regex("\\^\\^(.+?)\\^\\^", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Superscript_Replacement = "<sup>$1</sup>";
        private readonly Regex SubscriptRegex = new Regex(",,(.+?),,", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Subscript_Replacement = "<sub>$1</sub>";
        private readonly Regex LTRRegex = new Regex(">\\{(.+?)\\}>", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string LTR_Replacement = "<div style=\"direction:ltr;\">$1</div>";
        private readonly Regex RTLRegex = new Regex("<\\{(.+?)\\}<", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string RTL_Replacement = "<div style=\"direction:rtl;\">$1</div>";
        private readonly Regex LinkRegex = new Regex("\\[([a-zA-z]+:\\/\\/.+?)\\|(.+?)\\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Link_Replacement = "<a href=\"$1\" rel=\"nofollow\">$2</a>";
        private readonly Regex ImgRegex = new Regex("\\[image\\|(.+?)\\|(.+?)\\]", RegexOptions.Compiled | RegexOptions.Singleline);
        private readonly string Img_Replacement = "<img src=\"$2\" alt=\"$1\" />";

        private readonly Regex H1Regex = new Regex(@"^==(.+?)==\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H1_Replacement = "<h2>$1</h2>";
        private readonly Regex H2Regex = new Regex(@"^===(.+?)===\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H2_Replacement = "<h3>$1</h3>";
        private readonly Regex H3Regex = new Regex(@"^====(.+?)====\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H3_Replacement = "<h4>$1</h4>";
        private readonly Regex H4Regex = new Regex(@"^=====(.+?)=====\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H4_Replacement = "<h5>$1</h5>";
        private readonly Regex H5Regex = new Regex(@"^======(.+?)======\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly string H5_Replacement = "<h6>$1</h6>";
        private readonly Regex REGEX_BREAK = new Regex(@"\r\n", RegexOptions.IgnoreCase);
        //private readonly Regex REGEX_BREAK = new Regex(@"(\r\n|\r|\n|\n\r)", RegexOptions.IgnoreCase);

        #endregion

        public WikiFormatterContainer( bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false,
            //bool enableTracing = false,
            string topicCssClass = "",
            string topicCssClassPrefix = "")
            : base()
        {
            Alternate = alternate;
            EmbedStyleSheet = embedStyleSheet;
            LineNumbers = lineNumbers;
            CssClass = topicCssClass;
            TopicCssClassPrefix = topicCssClassPrefix;

            Formatters.Add(new WikiTocFormatter()
            {
                CssClass = topicCssClass,
                TopicCssClassPrefix = topicCssClassPrefix
            });

            Formatters.Add(new WikiListFormatter());

            //Formatters.Add(new WikiCodeFormatterEx()
            //{
            //    Alternate = alternate,
            //    EmbedStyleSheet = embedStyleSheet,
            //    LineNumbers = lineNumbers
            //});

            Formatters.Add(new WikiCodeFormatter()
            {
                Alternate = alternate,
                EmbedStyleSheet = embedStyleSheet,
                LineNumbers = lineNumbers
            });

            Formatters.Add(new WikiTableFormatter());
            AddFormat(BoldRegex, Bold_Replacement, true);
            AddFormat(ItalicRegex, Italic_Replacement, true);
            AddFormat(UnderlineRegex, Underline_Replacement, true);
            AddFormat(StrikeThoughRegex, StrikeThough_Replacement, true);
            AddFormat(HorizontallineRegex, Horizontalline_Replacement, true);
            AddFormat(LineBreakRegex, LineBreak_Replacement, true);
            AddFormat(SuperscriptRegex, Superscript_Replacement, true);
            AddFormat(SubscriptRegex, Subscript_Replacement, true);
            AddFormat(LTRRegex, LTR_Replacement, true);
            AddFormat(RTLRegex, RTL_Replacement, true);
            AddFormat(LinkRegex, Link_Replacement, true);
            AddFormat(ImgRegex, Img_Replacement, true);
            AddFormat(H5Regex, H5_Replacement, true);
            AddFormat(H4Regex, H4_Replacement, true);
            AddFormat(H3Regex, H3_Replacement, true);
            AddFormat(H2Regex, H2_Replacement, true);
            AddFormat(H1Regex, H1_Replacement, true);
            AddFormat(REGEX_BREAK, "<br/>");
        }

        public bool Alternate { get; set; }

        public bool EmbedStyleSheet { get; set; }

        public bool LineNumbers { get; set; }

        public string CssClass { get; set; }

        public string TopicCssClassPrefix { get; set; }
    }
}
