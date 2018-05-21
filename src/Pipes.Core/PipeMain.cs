using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    class PipeMain<T>
        where T: PipeArgs
    {
        public T Args { get; private set; }

        public void With(string[] args)
        {
        }

        public virtual void Initialize(T args)
        {
        }

        public virtual PipeOutput RunBefore()
        {
            return PipeOutput.Empty;
        }

        public virtual PipeOutput Run(PipeInput input)
        {
            return PipeOutput.Empty;
        }

        public virtual PipeOutput RunAfter()
        {
            return PipeOutput.Empty;
        }
    }
}
