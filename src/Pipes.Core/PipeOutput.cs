using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    public class PipeOutput
    {
        string _line;

        public PipeOutput(string line)
        {
            this._line = line;
        }

        public static PipeOutput Empty = new PipeOutput(null);

        public static PipeOutput FromString(string output)
        {
            return new PipeOutput(EscapeString(output));
        }

        static string EscapeString(string text)
        {
            return text.Replace("\\","\\\\").Replace("\n", "\\n").Replace("\r", "");
        }

        static string UnescapeString(string text)
        {
            return text.Replace("\\n", "\n");
        }

        //public static PipeOutput From(Func<IEnumerable<string>> producer)
        //{
        //    return new PipeOutput();
        //}

        public static implicit operator PipeOutput(string output)
        {
            return new PipeOutput(output);
        }

        public override string ToString()
        {
            return _line;
        }
    }
}
