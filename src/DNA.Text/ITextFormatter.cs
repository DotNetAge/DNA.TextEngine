//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Text
{
    /// <summary>
    /// Defines the methods that use in a text formater.
    /// </summary>
    public interface ITextFormatter
    {
        /// <summary>
        /// Format the text and returns the formatted result.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Format(string text);
    }
}
