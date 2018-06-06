using System;
using System.Collections.Generic;
using System.Text;

namespace InsertDB
{
    class InsertDouData
    {
        public ExecSqlCmd CreateInput(string data)
        {
            string command = CreateInputCommand();
            var inputModel = CreateInputModel(data);

            return new ExecSqlCmd { Command = command, Parameters = inputModel };
        }
        
        string CreateInputCommand()
        {
            return "INSERT tbLog(id) VALUES(@Param1)";
        }

        object CreateInputModel(string input)
        {
            return new { Param1 = "111" };
        }
    }
}
