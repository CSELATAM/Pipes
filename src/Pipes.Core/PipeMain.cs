using System;
using System.Collections.Generic;
using System.Reflection;
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

            bool shouldRunLoop = IsMethodOverriden();

            if (shouldRunLoop)
            {
                while ((line = input.ReadLine()) != null)
                {
                    if (line == "")
                        break;

                    var output = Run(new PipeInput(line));

                    yield return output;
                }
            }

            yield return RunAfter();
        }
        
        void SetupArgs(string[] args)
        {
            var arguments = new T();
            arguments.Init(args);

            this.Args = arguments;
        }

        void Process(PipeOutput output)
        {
            string text = output.ToString();
            string[] lines = text.Split('\n');

            foreach(var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        bool IsMethodOverriden()
        {
            var method = this.GetType().GetMethod("Run");
            return method.DeclaringType != typeof(PipeMain<T>);
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
