//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Text
{
    /// <summary>
    /// Define the TextFormatter base class that enabling Trace.
    /// </summary>
    public abstract class TraceableFormatter:ITextFormatter
    {
        public TraceableFormatter() { Trace = new TraceContext(); }

        /// <summary>
        /// Gets/Sets the Trace context.
        /// </summary>
        public TraceContext Trace { get; set; }

        public abstract string Format(string text);

        string ITextFormatter.Format(string text)
        {
            return this.Format(text);
        }
    }
}
