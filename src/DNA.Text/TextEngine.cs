//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Web.Mvc;
using System.Text;
using Manoli.Utils.CSharpFormat;

namespace DNA.Text
{
    public static class TextEngine
    {
        public static string Text(string text)
        {
            return new TextBuilder(text).ClearHtml().ToString();
        }

        public static MvcHtmlString SafeHtml(string text)
        {
            return new TextBuilder(text).NoScripts().ToHtmlString();
        }

        public static MvcHtmlString CodeFile(string filename, string language = "html", bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false)
        {
            if (!string.IsNullOrEmpty(filename) && System.IO.File.Exists(filename))
            {
                var srcFormat = _GetSourceCodeFormat(language, alternate, embedStyleSheet, lineNumbers);
                var txt = System.IO.File.ReadAllText(filename);
                if (!string.IsNullOrEmpty(txt)) 
                    return MvcHtmlString.Create(srcFormat.FormatCode(txt));
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Code(string source, string language = "html", bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false)
        {
            var srcFormat = _GetSourceCodeFormat(language, alternate, embedStyleSheet, lineNumbers);
            return MvcHtmlString.Create(srcFormat.FormatCode(source));
        }

        private static SourceFormat _GetSourceCodeFormat(string language, bool alternate, bool embedStyleSheet, bool lineNumbers)
        {
            SourceFormat srcFormat = new HtmlFormat();

            if (language.Equals("c#", StringComparison.OrdinalIgnoreCase) || language.Equals("cs", StringComparison.OrdinalIgnoreCase) ||
                language.Equals("csharp", StringComparison.OrdinalIgnoreCase))
                srcFormat = new CSharpFormat();

            if (language.Equals("vb", StringComparison.OrdinalIgnoreCase) || language.Equals("visualbasic", StringComparison.OrdinalIgnoreCase) ||
                language.Equals("vbscript", StringComparison.OrdinalIgnoreCase))
                srcFormat = new VisualBasicFormat();

            if (language.Equals("javascript", StringComparison.OrdinalIgnoreCase) || language.Equals("js", StringComparison.OrdinalIgnoreCase) ||
language.Equals("jscript", StringComparison.OrdinalIgnoreCase))
                srcFormat = new JavaScriptFormat();

            if (language.Equals("sql", StringComparison.OrdinalIgnoreCase))
                srcFormat = new TsqlFormat();

            if (language.Equals("msh", StringComparison.OrdinalIgnoreCase))
                srcFormat = new MshFormat();

            srcFormat.Alternate = alternate;
            srcFormat.EmbedStyleSheet = embedStyleSheet;
            srcFormat.LineNumbers = lineNumbers;
            return srcFormat;
        }

        //public static MvcHtmlString Html(string text)
        //{
        //    return new TextBuilder(text).ToHtmlString();
        //}

        public static TextBuilder New(string text)
        {
            return new TextBuilder(text);
        }

        public static MvcHtmlString Wiki(string text,
            bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false,
            string topicCssClass = "",
            string topicCssClassPrefix = "")
        {
            var formatter = new WikiFormatterContainer(alternate, embedStyleSheet, lineNumbers,  topicCssClass, topicCssClassPrefix);

            var builder = New(text)
                .AddFormat(formatter)
                .Format();

            return builder.ToHtmlString();
        }

        public static MvcHtmlString BBCode(string text,
            bool alternate = false,
            bool embedStyleSheet = false,
            bool lineNumbers = false)
        {
            var formatter = new BBFormatterContainer(alternate, embedStyleSheet, lineNumbers);
            var builder = New(text)
                .AddFormat(formatter)
                .Format();
            return builder.ToHtmlString();
        }

        public static MvcHtmlString Markdown(string md) 
        {
            if (!string.IsNullOrEmpty(md))
            {
                var _markdown = new anrControls.Markdown();
                return MvcHtmlString.Create(_markdown.Transform(md));
            }
            else
                return MvcHtmlString.Empty;
        }
    }
}