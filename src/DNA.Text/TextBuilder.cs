//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DNA.Text
{
    /// <summary>
    /// Present the text bulder. Provide the text Fluent APIs
    /// </summary>
    public class TextBuilder
    {
        private readonly Regex REGEX_SCRIPT = new Regex(@"<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
        private readonly Regex REGEX_TAGS = new Regex(@"<(.[^>]*)>", RegexOptions.IgnoreCase);
        private readonly Regex REGEX_BREAK = new Regex(@"([\r\n])[\s]+", RegexOptions.IgnoreCase);
        private int timeoutInterval = 2000;

        public int TimeoutInterval
        {
            get { return timeoutInterval; }
            set { timeoutInterval = value; }
        }

        protected string Text { get; private set; }

        public List<ITextFormatter> Formatters { get; private set; }

        public TextBuilder(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Text = reader.ReadToEnd();
                Formatters = new List<ITextFormatter>();
            }
        }

        public TextBuilder(string text)
        {
            Text = text;
            Formatters = new List<ITextFormatter>();
        }

        public TextBuilder AddFormat(ITextFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");

            if (Formatters.Contains(formatter))
                throw new Exception("The formatter has been added.");

            Formatters.Add(formatter);
            return this;
        }

        public TextBuilder Highlight(string matchText)
        {
            string formattedText = Text;
            Regex regex = new Regex(matchText, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = regex.Match(formattedText);
            if (match.Success)
                formattedText = regex.Replace(formattedText, "<span class='ui-state-highlight'>$0</span>");
            return this;
        }

        public TextBuilder ClearHtml(bool keepBreak = false)
        {
            string output = HttpUtility.HtmlDecode(Text);
            output = Regex.Replace(output, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"-->", "", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"<!--.*", "", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            output.Replace("<", "");
            output.Replace(">", "");
            if (!keepBreak)
                output.Replace("\r\n", "");
            Text = output;
            return this;
        }

        /// <summary>
        /// Encode all script tags.
        /// </summary>
        /// <returns></returns>
        public TextBuilder NoScripts()
        {
            Text = REGEX_SCRIPT.Replace(Text, "&lt;script $1&gt; \r\n $2 \r\n &lt;/script&gt;");
            return this;
        }

        public TextBuilder Format()
        {
            foreach (var formatter in Formatters)
            {
                try
                {
                    var formattedText = formatter.Format(Text);
                    Text = formattedText;
                }
                catch
                {
                    continue;
                }
            }
            return this;
        }

        public TextBuilder WordFilter(Dictionary<string, string> filters)
        {
            //Executors.Add(new Action(() =>
            //{
            foreach (var key in filters.Keys)
            {
                var regex = new Regex("\\" + key + "\\", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (regex.IsMatch(Text))
                    Text = regex.Replace(Text, filters[key]);
            }
            // }));
            return this;
        }

        public new string ToString()
        {
            //foreach (var action in Executors)
            //    action.Invoke();
            return Text;
        }

        public MvcHtmlString ToHtmlString()
        {
            if (!string.IsNullOrEmpty(Text))
                Text = Text.Replace("\r\n/g", "<br/>");
            return MvcHtmlString.Create(Text);
        }

    }
}
