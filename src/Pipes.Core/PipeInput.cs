using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    public class PipeInput
    {
        private readonly string _line;

        public PipeInput(string line)
        {
            this._line = line;
        }

        public string FromBase64()
        {
            var buferBytes = System.Convert.FromBase64String(_line);
            return Encoding.UTF8.GetString(buferBytes);
        }

        public string GetString()
        {
            return _line;
        }

        public static implicit operator string(PipeInput output)
        {
            return output._line;
        }
    }
}
