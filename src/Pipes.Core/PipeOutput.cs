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

        public static PipeOutput From(IEnumerable<string> producer)
        {
            string lines = String.Join('\n', (IEnumerable<string>)producer);

            return new PipeOutput(lines);
        }

        static string EscapeString(string text)
        {
            return text.Replace("\\","\\\\").Replace("\n", "\\n").Replace("\r", "");
        }

        static string UnescapeString(string text)
        {
            return text.Replace("\\n", "\n");
        }
        
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
