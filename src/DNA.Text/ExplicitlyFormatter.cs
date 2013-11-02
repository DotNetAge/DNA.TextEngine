//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DNA.Text
{
    /// <summary>
    /// Presents the Formatter base class uses to format the text explicitly
    /// </summary>
    public abstract class ExplicitlyFormatter : ITextFormatter
    {
        public ExplicitlyFormatter() : base() { }

        /// <summary>
        /// Gets the Regex definition.
        /// </summary>
        public abstract Regex Regex { get; }

        /// <summary>
        /// Use to custom the replacment match result.
        /// </summary>
        /// <param name="match">The match object.</param>
        /// <returns>Returns the custom replacment match result.</returns>
        public abstract string GetReplaceResult(Match match);
        
        /// <summary>
        /// Explicitly match the text and replace the match result.
        /// </summary>
        /// <param name="text">The format target text.</param>
        /// <returns>Returns the formatted text.</returns>
        public virtual string Format(string text)
        {
            var match = Regex.Match(text);
            int startIndex = 0;
            int len = text.Length;
            string formatted = "";

            while (match.Success)
            {
                if (match.Index > 0)
                {
                    int length = match.Index - startIndex;
                    formatted += text.Substring(startIndex, length);
                }

                var target = text.Substring(match.Index, match.Length);

                formatted += Regex.Replace(target, GetReplaceResult);
                startIndex = (match.Index + match.Length);
                match = match.NextMatch();
            }

            if (startIndex != (len - 1))
                formatted += text.Substring(startIndex);

            if (!string.IsNullOrEmpty(formatted))
                return formatted;
            return text;
        }
    }
}
