using System;
using System.Collections.Generic;
using System.Text;

namespace Pipes.Core
{
    public abstract class PipeMain<T>
        where T: PipeArgs, new()
    {
        public T Args { get; private set; }
        
        public int RunWith(string[] args)
        {
            SetupArgs(args);

            Initialize(this.Args);
            
            foreach(var output in LoopRun())
            {
                if( output != null && output != PipeOutput.Empty)
                    Process(output);
            }

            return 0;
        }

        IEnumerable<PipeOutput> LoopRun()
        {
            var input = Console.In;
            string line;

            yield return RunBefore();

            while ((line = input.ReadLine()) != null)
            {
                var output = Run(new PipeInput(line));

                yield return output;
            }

            yield return RunAfter();
        }

        void Process(PipeOutput output)
        {
            Console.WriteLine(output);
        }

        void SetupArgs(string[] args)
        {
            var arguments = new T();
            arguments.Init(args);

            this.Args = arguments;
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
