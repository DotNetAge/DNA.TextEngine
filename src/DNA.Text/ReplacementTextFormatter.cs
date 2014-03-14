//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace DNA.Text
{
    /// <summary>
    /// Represents a text formatter use to replace the match pattern.
    /// </summary>
    public class ReplacementTextFormatter :ExplicitlyFormatter //  ITextFormatter
    {
        private Regex _regex;

        public bool IsExplicitly { get; set; }

        /// <summary>
        /// Gets the Regex instance.
        /// </summary>
        public override Regex Regex { get { return _regex; } }

        /// <summary>
        /// Gets/Sets the replacement pattern
        /// </summary>
        public string Replacement { get; set; }

        public ReplacementTextFormatter(Regex regex, string replacement, bool isExplicitly = false)
        {
            _regex = regex;
            Replacement = replacement;
            IsExplicitly = isExplicitly;

        }

        public ReplacementTextFormatter(string match, string replacement, bool isExplicitly = false)
            : this(new Regex(match, RegexOptions.Compiled | RegexOptions.IgnoreCase), replacement, isExplicitly)
        {
        }

        public override string Format(string text)
        {
            if (IsExplicitly)
                return base.Format(text);  //return FormatExplicitly(text);
            else
                return FormatBlock(text);
        }

        protected string FormatBlock(string text)
        {
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(Replacement))
            {
                if (Regex.Match(text).Success)
                    return Regex.Replace(text, this.Replacement);
            }
            return text;
        }
        
        public override string GetReplaceResult(Match match)
        {
            return match.Result(this.Replacement);
        }

        //protected string FormatExplicitly(string text)
        //{
        //    var match = this.Regex.Match(text);
        //    int startIndex = 0;
        //    int len = text.Length;
        //    string formatted = "";

        //    while (match.Success)
        //    {
        //        if (match.Index > 0)
        //        {
        //            int length = match.Index - startIndex;
        //            formatted += text.Substring(startIndex, length);
        //        }

        //        var target = text.Substring(match.Index, match.Length);

        //        formatted += Regex.Replace(target, this.Replacement);
        //        startIndex = (match.Index + match.Length);
        //        match = match.NextMatch();
        //    }

        //    if (startIndex != (len - 1))
        //        formatted += text.Substring(startIndex);

        //    if (!string.IsNullOrEmpty(formatted))
        //        return formatted;

        //    return text;
        //}
    }
}
