//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System.Text.RegularExpressions;

namespace DNA.Text
{
    public class WikiCodeFormatter : CodeFormatter
    {
        private readonly Regex regex = new Regex("\\<code\\slanguage=(.+?)\\>((.|\\n|\\r)+?)\\<\\/code\\>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public override Regex Regex
        {
            get { return regex; }
        }
    }

    //public class WikiCodeFormatterEx : CodeFormatter
    //{
    //    private readonly Regex regex = new Regex("\\{code:(.+?)\\}((.|\\n|\\r)+?)\\{\\code\\}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

    //    public override Regex Regex
    //    {
    //        get { return regex; }
    //    }
    //}

}
