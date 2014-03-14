//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.dotnetage.com/home/en-US/the-mit-license-mit.html

using System.Collections.Generic;
using System.Linq;

namespace DNA.Text
{
    internal class HeaderEnumerator
    {
        private List<WikiHeader> headers;
        private string text;
        private int cursor = 0;
        private int count = 0;
        private int textEnd = 0;

        internal HeaderEnumerator(List<WikiHeader> _headers, string input)
        {
            headers = _headers.OrderBy(h => h.Index).ToList();
            count = headers.Count;
            cursor = 0;
            text = input;
            textEnd = text.Length - 1;
        }

        internal bool IsLast { get { return cursor == count; } }

        internal WikiHeader Current { get { return headers[cursor]; } }

        internal string OnNext()
        {
            //var currentHeader = headers[cursor];
            //var nextHeader = IsLast ? null : headers[cursor + 1];
            var nextHeader = (cursor==count-1) ? null : headers[cursor + 1];
            int start = Current.End;
            int end = nextHeader != null ? nextHeader.StartAt : textEnd;
            int len = end - start;
            cursor++;

            var sbtext = text.Substring(start, len);
            return sbtext;
        }
    }
}
