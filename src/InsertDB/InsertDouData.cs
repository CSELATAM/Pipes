using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

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
            var doc = new XmlDocument();
            doc.LoadXml(input);

            string name = doc.SelectSingleNode("xml/@name")?.Value;
            string pubName = doc.SelectSingleNode("xml/article/@pubName").Value;
            string pubDate = doc.SelectSingleNode("xml/article/@pubDate").Value;
            string artCategory = doc.SelectSingleNode("xml/article/@artCategory").Value;
            string identifica = doc.SelectSingleNode("xml/article/body/Identifica").InnerText;

            string[] hierarquia = artCategory.Split('/');
            string[] assinaturas = doc.SelectNodes("xml/article/body/Autores/assina")
                                    .Cast<XmlNode>()
                                    .Select(x => x.InnerText)
                                    .ToArray();

            return new { 
                Filename = name,
                PubName = pubName,
                PubDate = pubDate,
                Hierarquia = hierarquia,
                Assinaturas = assinaturas
            };
        }
    }
}
