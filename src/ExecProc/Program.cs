using Pipes.Core;
using System;
using System.Diagnostics;
using System.IO;
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
            
            Console.WriteLine(_process +  _arguments);
        }

        public override PipeOutput Run(PipeInput input)
        {
            string args = input;

            var procStartInfo = new ProcessStartInfo
            {
                FileName = _process,
                Arguments = String.Format(_arguments, args),
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                RedirectStandardOutput = true
            };

            var process = Process.Start(procStartInfo);
            
            var reader = process.StandardOutput;
            string result = reader.ReadToEnd().Trim('\n');
            
            return PipeOutput.FromString(result);
        }
    }

    class Program
    {
        static int Main(string[] args) => (new ExecProcMain()).RunWith(args);
    }
}
