using System;

namespace Pipes.Core
{
    public class PipeArgs
    {
        string[] _args;

        public PipeArgs()
        {
        }

        public void Init(string[] args)
        {
            _args = args;
        }
        
        public string Argument(int n) => _args[n];
        public string Optional(int n) => (n < _args.Length) ? _args[n] : null;
        public string Flag(params string[] flags) => "";
    }
}
