using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    class PipeOutput
    {
        public static PipeOutput Empty => new PipeOutput();

        public static PipeOutput FromString(string output)
        {
            return new PipeOutput();
        }

        public static implicit operator PipeOutput(string output)
        {
            return new PipeOutput();
        }

    }
}
