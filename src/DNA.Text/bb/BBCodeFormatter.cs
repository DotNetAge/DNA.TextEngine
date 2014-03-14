//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class BBCodeFormatter:CodeFormatter
    {
        private readonly Regex regex = new Regex("\\[code=(.+?)\\]((.|\\n|\\r)+?)\\[\\/code\\]", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public override Regex Regex
        {
            get { return regex; }
        }
    }
}
