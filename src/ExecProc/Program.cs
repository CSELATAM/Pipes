using Pipes.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace ExecProc
{
    class ExecProcArgs : PipeArgs
    {
        public string CommandLine => Argument(0);
    }

    class ExecProcMain : PipeMain<ExecProcArgs>
    {
        string _process;
        string _arguments;

        public override void Initialize(ExecProcArgs args)
        {
            string commandLine = args.CommandLine;
            var paramList = commandLine.Split(' ', 2);

            this._process = paramList[0];
            this._arguments = (paramList.Length > 1) ? paramList[1] : null;
            
            Console.WriteLine(_process + " + " + _arguments[0]);
        }

        public override PipeOutput Run(PipeInput input)
        {
            var procStartInfo = new ProcessStartInfo
            {
                FileName = _process,
                Arguments = _arguments,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            var process = Process.Start(procStartInfo);
            
            string str = input;
            return str;
        }
    }

    class Program
    {
        static int Main(string[] args) => (new ExecProcMain()).RunWith(args);
    }
}
