using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    public class PipeMain<T>
        where T: PipeArgs
    {
        public T Args { get; private set; }

        public void With(string[] args)
        {
        }

        public int RunWith(string[] args)
        {
            return 0;
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
