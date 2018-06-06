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
        static Regex ArticleTypeRegex = new Regex(@"(.*) N[o°º]\s+(\d+)");
        
        public ExecSqlCmd CreateInput(string data)
        {
            try
            {
                string command = CreateInputCommand();
                var inputModel = CreateInputModel(data);

                return new ExecSqlCmd { Command = command, Parameters = inputModel };
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return null;
        }
        
        string CreateInputCommand()
        {
            return "INSERT tbArtigos(Filename, PubName, PubDate, ArtigoType, ArtigoTypeSeq, ArtigoId, Identifica, Orgao, Hierarquia, Assinaturas) VALUES( @Filename, @PubName, @PubDate, @ArtigoType, @ArtigoTypeSeq, @ArtigoId, @Identifica, @Orgao, @Hierarquia, @Assinaturas)";
        }

        object CreateInputModel(string input)
        {
            var doc = new XmlDocument();

            // Quick fix
            input = input
                    .Replace("<Identifica>", "<Identifica><![CDATA[")
                    .Replace("</Identifica>", "]]></Identifica>")
                    .Replace("<Ementa>", "<Ementa><![CDATA[")
                    .Replace("</Ementa>", "]]></Ementa>");

            doc.LoadXml(input);

            string name = doc.SelectSingleNode("xml/@filename")?.Value;
            string pubName = doc.SelectSingleNode("xml/article/@pubName").Value;
            string pubDate = doc.SelectSingleNode("xml/article/@pubDate").Value;
            string artCategory = doc.SelectSingleNode("xml/article/@artCategory").Value;
            string identifica = doc.SelectSingleNode("xml/article/body/Identifica").InnerText;

            string filename = name.Split('/').Last();
            string[] hierarquia = artCategory
                                    .Split('/')
                                    .Select(h => h.Trim())
                                    .ToArray();
            string[] assinaturas = doc.SelectNodes("xml/article/body/Autores/assina")
                                    .Cast<XmlNode>()
                                    .Select(x => x.InnerText)
                                    .ToArray();
            string orgao = hierarquia[0];

            var artigoMatch = ArticleIdRegex.Match(name);
            var artigoTypeMatch = ArticleTypeRegex.Match(identifica);

            string artType = null;
            int? artTypeSequence = null;

            if ( artigoTypeMatch.Success )
            {

                artType = artigoTypeMatch.Groups[1].Value;
                artTypeSequence = Int32.Parse(artigoTypeMatch.Groups[2].Value);
            }

            int artigoId = Int32.Parse(artigoMatch.Groups[1].Value );

            return new { 
                Filename = filename,
                PubName = pubName,
                PubDate = pubDate,
                ArtigoType = artType,
                ArtigoTypeSeq = artTypeSequence,
                ArtigoId = artigoId,
                Identifica = identifica,
                Orgao = orgao,
                Hierarquia = String.Join('/', hierarquia).Replace(",/", ", "), // fix some issues with hierarchy
                Assinaturas = String.Join('/', assinaturas)
            };
        }
    }
}
