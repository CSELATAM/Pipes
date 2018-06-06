using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pipes.Core
{
    public class PipeOutput
    {
        public static PipeOutput Empty = new PipeOutput(new string[] { });

        IEnumerator<string> _enumerator;

        public PipeOutput(IEnumerable<string> lines)
        {
            if( lines == null)
                throw new ArgumentNullException(nameof(lines));

            _enumerator = lines.GetEnumerator();
        }

        public static PipeOutput ConvertBase64(string rawOutput)
        {
            var outputBytes = Encoding.UTF8.GetBytes(rawOutput);
            string outputBase64 = System.Convert.ToBase64String(outputBytes);
            return new PipeOutput(new string[] { outputBase64 });
        }

        public static PipeOutput FromString(string output)
        {
            return new PipeOutput(new string[] { output });
        }

        public static PipeOutput From(IEnumerable<string> lines)
        {
            return new PipeOutput(lines);
        }

        public string Read()
        {
            if (_enumerator == null)
                return null;

            bool hasData = _enumerator.MoveNext();

            if( !hasData )
            {
                _enumerator.Dispose();
                _enumerator = null;
                return null;
            }

            return _enumerator.Current;
        }
    }
}
