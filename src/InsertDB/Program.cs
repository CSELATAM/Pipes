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
        private InsertDouData _insertDouData = new InsertDouData();
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
                var inputModel = _insertDouData.CreateInput(input.GetString());
 
                conn.Execute(inputModel.Command, inputModel.Parameters);
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
    }

    class Program
    {
        static int Main(string[] args) => (new InsertDBMain()).RunWith(args);
    }
}
