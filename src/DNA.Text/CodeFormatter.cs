//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Manoli.Utils.CSharpFormat;

namespace DNA.Text
{
    public abstract class CodeFormatter : ExplicitlyFormatter
    {
        public bool Alternate { get; set; }

        public bool EmbedStyleSheet { get; set; }

        public bool LineNumbers { get; set; }

        public override string GetReplaceResult(Match match)
        {
            string lang = match.Result("$1");
            string body = match.Result("$2");
            SourceFormat srcFormat = new HtmlFormat();

            if (lang.Equals("c#", StringComparison.OrdinalIgnoreCase) || lang.Equals("cs", StringComparison.OrdinalIgnoreCase) ||
                lang.Equals("csharp", StringComparison.OrdinalIgnoreCase))
                srcFormat = new CSharpFormat();

            if (lang.Equals("vb", StringComparison.OrdinalIgnoreCase) || lang.Equals("visualbasic", StringComparison.OrdinalIgnoreCase) ||
                lang.Equals("vbscript", StringComparison.OrdinalIgnoreCase))
                srcFormat = new VisualBasicFormat();

            if (lang.Equals("javascript", StringComparison.OrdinalIgnoreCase) || lang.Equals("js", StringComparison.OrdinalIgnoreCase) ||
lang.Equals("jscript", StringComparison.OrdinalIgnoreCase))
                srcFormat = new JavaScriptFormat();

            if (lang.Equals("sql", StringComparison.OrdinalIgnoreCase))
                srcFormat = new TsqlFormat();

            if (lang.Equals("msh", StringComparison.OrdinalIgnoreCase))
                srcFormat = new MshFormat();

            srcFormat.Alternate = Alternate;
            srcFormat.EmbedStyleSheet = EmbedStyleSheet;
            srcFormat.LineNumbers = LineNumbers;
            return srcFormat.FormatCode(body);
        }
    }
}
