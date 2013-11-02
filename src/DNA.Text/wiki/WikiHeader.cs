//  Copyright (c) 2011 Ray Liang (http://www.dotnetage.com)
//  Licensed MIT: http://www.opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using System.Text;
//using System.Web.Mvc;

namespace DNA.Text
{
    internal class WikiHeader
    {
        internal WikiHeader()
        {
            Children = new List<WikiHeader>();
        }

        internal int Level { get; set; }

        internal int Index { get; set; }

        internal int StartAt { get; set; }

        internal int End { get { return StartAt + Length; } }

        internal string Title { get; set; }

        internal int Length { get; set; }

        private string id = "";

        internal string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                    id = "h" + Level.ToString() + "_" + System.IO.Path.GetRandomFileName().Replace(".", "");
                return id;
            }
        }

        internal List<WikiHeader> Children { get; private set; }

        internal void Render(ref StringBuilder sb, ref int offset)
        {
            string htmlText = "<span id=\""+Id+"\">"+Title+"</span>";
            int currentStart = StartAt + offset;
            if (currentStart + Length > sb.Length)
                return;
            sb.Replace(Title, htmlText, currentStart, Length);
            offset += htmlText.Length - Title.Length;
        }
    }
}
