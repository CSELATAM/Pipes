using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;

namespace InsertDB
{
    class InsertDouData
    {
        static Regex ArticleIdRegex = new Regex(@"-artigo(\d+)");

        public ExecSqlCmd CreateInput(string data)
        {
            string command = CreateInputCommand();
            var inputModel = CreateInputModel(data);

            return new ExecSqlCmd { Command = command, Parameters = inputModel };
        }
        
        string CreateInputCommand()
        {
            return "INSERT tbArtigos(Filename, PubName, PubDate, ArtigoId, Hierarquia, Assinaturas) VALUES( @Filename, @PubName, @PubDate, @ArtigoId, @Hierarquia, @Assinaturas)";
        }

        object CreateInputModel(string input)
        {
            var doc = new XmlDocument();
            doc.LoadXml(input);

            string name = doc.SelectSingleNode("xml/@filename")?.Value;
            string pubName = doc.SelectSingleNode("xml/article/@pubName").Value;
            string pubDate = doc.SelectSingleNode("xml/article/@pubDate").Value;
            string artCategory = doc.SelectSingleNode("xml/article/@artCategory").Value;
            string identifica = doc.SelectSingleNode("xml/article/body/Identifica").InnerText;

            string filename = name.Split('/').Last();
            string[] hierarquia = artCategory.Split('/');
            string[] assinaturas = doc.SelectNodes("xml/article/body/Autores/assina")
                                    .Cast<XmlNode>()
                                    .Select(x => x.InnerText)
                                    .ToArray();

            var artigoMatch = ArticleIdRegex.Match(name);
            int artigoId = Int32.Parse(artigoMatch.Groups[1].Value );

            return new { 
                Filename = filename,
                PubName = pubName,
                PubDate = pubDate,
                ArtigoId = artigoId,
                Hierarquia = String.Join('/', hierarquia),
                Assinaturas = String.Join('/', assinaturas)
            };
        }
    }
}
