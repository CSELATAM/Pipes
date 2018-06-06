using Pipes.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace InsertDB
{
    class InsertDBArgs : PipeArgs
    {
        public string ConnectionString => Argument(0);
    }

    class InsertDBMain : PipeMain<InsertDBArgs>
    {
        private string _connectionString;
        private int _lineNumber = 0;

        public override void Initialize(InsertDBArgs args)
        {
            _connectionString = args.ConnectionString;
        }

        public override PipeOutput Run(PipeInput input)
        {
            using (var conn = GetConnectionSqlServer())
            {
                string command = CreateInputCommand();
                var inputModel = CreateInputModel(input.GetString());
                conn.Execute(command, inputModel);
            }

            return PipeOutput.FromString((_lineNumber++).ToString());
        }

        IDbConnection GetConnectionSqlServer()
        {
            return new SqlConnection(_connectionString);
        }

        string CreateInputCommand()
        {
            return "SELECT @Param1";
        }

        object CreateInputModel(string input)
        {
            return new { Param1 = "111"};
        }
    }

    class Program
    {
        static int Main(string[] args) => (new InsertDBMain()).RunWith(args);
    }
}
