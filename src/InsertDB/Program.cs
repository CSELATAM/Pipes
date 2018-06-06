using Pipes.Core;
using System;
using System.Data;
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
            using (var conn = GetConnection())
            {
                string command = CreateInputCommand();
                var inputModel = CreateInputModel(input.GetString());
                conn.Execute(command, inputModel);
            }

            return PipeOutput.FromString((_lineNumber++).ToString());
        }

        IDbConnection GetConnection()
        {
            if (_connectionString.Contains(".mysql."))
                return GetConnectionMySql();

            return GetConnectionSqlServer();
        }

        IDbConnection GetConnectionSqlServer()
        {
            return new System.Data.SqlClient.SqlConnection(_connectionString);
        }

        IDbConnection GetConnectionMySql()
        {
            return new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        }

        string CreateInputCommand()
        {
            return "INSERT tbLog(id) VALUES(@Param1)";
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
