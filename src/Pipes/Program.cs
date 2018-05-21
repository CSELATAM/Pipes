using System;

namespace Pipes
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("PIPES_CONFIG", "pipes.json", EnvironmentVariableTarget.Process);

            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd");
            
            var process = System.Diagnostics.Process.Start(processInfo);
        }
    }
}
