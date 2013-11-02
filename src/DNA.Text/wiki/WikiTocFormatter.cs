//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace DNA.Text
{
    public class WikiTocFormatter : ITextFormatter
    {
        private static readonly Regex TocRegex = new Regex("\\{toc\\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex H1Regex = new Regex(@"^==(.+?)==\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H2Regex = new Regex(@"^===(.+?)===\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H3Regex = new Regex(@"^====(.+?)====\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H4Regex = new Regex(@"^=====(.+?)=====\n?", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex H5Regex = new Regex(@"^======(.+?)======\n?", RegexOptions.Compiled | RegexOptions.Multiline);

        public string CssClass { get; set; }

        public string TopicCssClassPrefix { get; set; }

        public string Format(string input)
        {
            var match = TocRegex.Match(input);

            if (match.Success)
            {
                var text = TocRegex.Replace(input, "");
                var headers = new List<WikiHeader>();

                ///Notes:Detect the start level. The document my not start with h1 so we need to ensure which level is the starting.
                var level = 1;
                while (level < 5)
                {
                    headers = DetectedSiblings(text, level);
                    if (headers.Count > 0)
                        break;
                    level++;
                }


                if (headers.Count > 0)
                {
                    ///Notes:Recursive add all children to start level headers.
                    Recursive(headers, text);

                    #region Generate Toc Html

                    var builder = new StringBuilder(text);
                    int offset = 0;
                    WriteHeaders(ref builder, ref offset, headers);

                    var tocUL = GenerateToc(headers, true);
                    builder.Insert(0, tocUL.ToString());

                    #endregion

                    return builder.ToString();
                }

            }
            return input;
        }

        private void Recursive(List<WikiHeader> parents, string inputText)
        {
            if (parents.Count > 0)
            {
                var headers = new HeaderEnumerator(parents, inputText);

                while (!headers.IsLast)
                {
                    var _curHeader = headers.Current;
                    int _offset =  _curHeader.End;

                    string txtBlock = headers.OnNext();

                    var children = DetectedSiblings(txtBlock, _curHeader.Level + 1);

                    if (children.Count > 0)
                    {
                        _curHeader.Children.AddRange(children);
                        int nextLevel = _curHeader.Level + 2;
                        if (nextLevel < 5)
                            Recursive(children, txtBlock);
                    }
                }
            }
        }

        /// <summary>
        /// Generate the Topic catalog
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="includeCss"></param>
        /// <returns></returns>
        private string GenerateToc(List<WikiHeader> headers, bool includeCss = false)
        {
            var tocUL = new TagBuilder("ul");

            if (includeCss)
            {
                if (!string.IsNullOrEmpty(CssClass))
                    tocUL.AddCssClass(CssClass);
            }

            foreach (var header in headers)
            {
                var hLI = new TagBuilder("li");
                if (!string.IsNullOrEmpty(TopicCssClassPrefix))
                    hLI.AddCssClass(TopicCssClassPrefix + header.Level);
                var link = new TagBuilder("a");
                link.Attributes.Add("href", "#" + header.Id);
                link.SetInnerText(header.Title);
                hLI.InnerHtml = link.ToString();
                if (header.Children.Count > 0)
                    hLI.InnerHtml += GenerateToc(header.Children);
                tocUL.InnerHtml += hLI.ToString();
            }
            return tocUL.ToString();
        }

        private void WriteHeaders(ref StringBuilder builder, ref int offset, List<WikiHeader> headers)
        {
            foreach (var header in headers)
            {
                header.Render(ref builder, ref offset);
                if (header.Children.Count > 0)
                {
                    foreach (var child in header.Children)
                        child.StartAt += header.End;

                    WriteHeaders(ref builder, ref offset, header.Children);
                }
            }
        }

        private List<WikiHeader> DetectedSiblings(string text, int level)
        {
            if (level <= 0)
                throw new ArgumentOutOfRangeException("Level must be in 1-5");
            var headers = new List<WikiHeader>();

            var regex = H1Regex;

            switch (level)
            {
                case 1:
                    regex = H1Regex;
                    break;
                case 2:
                    regex = H2Regex;
                    break;
                case 3:
                    regex = H3Regex;
                    break;
                case 4:
                    regex = H4Regex;
                    break;
                case 5:
                    regex = H5Regex;
                    break;
            }

            var match = regex.Match(text);
            int count = 0;
            //int end = 0;
            string levelPrefix = "=";

            for (int i = 0; i < level; i++)
                levelPrefix += "=";

            while (match.Success)
            {
                //end = match.Index + match.Length;
                string title = match.Result("$1");

                if (!title.StartsWith("="))
                {
                    var _header = new WikiHeader()
                    {
                        Index = count++,
                        Length = match.Length,
                        Level = level,
                        Title = title,
                        StartAt = match.Index
                    };
                    headers.Add(_header);
                }
                match = match.NextMatch();
            }

            return headers;
        }

    }
    
}
