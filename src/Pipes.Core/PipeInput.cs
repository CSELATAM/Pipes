using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    public class PipeInput
    {
        public static implicit operator string(PipeInput output)
        {
            return "";
        }
    }
}
