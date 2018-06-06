using System;
using System.Collections.Generic;
using System.Text;

namespace ReadBlob
{
    class CustomDou
    {
        public static string AddName(string content, string filename)
        {
            return content.Replace("<xml>", $"<xml filename=\"{filename}\">");
        }
    }
}
