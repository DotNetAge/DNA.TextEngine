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
    /// Present the container class that contains multi-formatter
    /// </summary>
    public class FormatterContainer : ITextFormatter
    {
        public bool IsEnableTracing { get; set; }

        public FormatterContainer() : base() { Formatters = new List<ITextFormatter>(); }

        /// <summary>
        /// Gets formatter collection.
        /// </summary>
        public virtual List<ITextFormatter> Formatters { get; private set; }

        public virtual void AddFormat(string pattern, string replacement, bool isExplicitly = false)
        {
            Formatters.Add(new ReplacementTextFormatter(pattern, replacement, isExplicitly));
        }

        public virtual void AddFormat(Regex regex, string replacement, bool isExplicitly = false)
        {
            Formatters.Add(new ReplacementTextFormatter(regex, replacement, isExplicitly));
        }

        public string Format(string text)
        {
            var formattedText = text;
            foreach (var formatter in Formatters)
            {
                try
                {
                    formattedText = formatter.Format(formattedText);
                }
                catch
                {
                    if (IsEnableTracing)
                       //Trace.Warn(e.Message, e);
                    continue;
                }
            }
            return formattedText;
        }
    }
}
