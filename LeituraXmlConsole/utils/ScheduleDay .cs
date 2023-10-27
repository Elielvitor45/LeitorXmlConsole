using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LeituraXmlConsole.utils
{
    public class ScheduleDay
    {
        public string path_Montagem { get; set; }
        public DateTime date { get; set; }
        public List<Break> Breaks { get; set; } = new List<Break>();
        public override string ToString()
        {
            string parseString = string.Join("\n", Breaks.Select(x => x.ToString()));
            return parseString;
        }
        public void ReadScheduleDay(string path, DateTime date)
        {
            this.path_Montagem = path;
            this.date = date;
            Init();
        }
        public void ReadScheduleDay(string pathMontagem, string pathJson, DateTime date)
        {
            this.path_Montagem = pathMontagem;
            this.date = date;
            if (Init())
            {
                ParseJson(pathJson);
            }
        }
        public string ParseDictionaryatString(Dictionary<string,int> Insertions) {
            return Insertions.Select(kvp =>
            $"Tipo da Inserção: {kvp.Key}, Quantidade: {kvp.Value}")
            .Aggregate((x, y) => x + "\n" + y);
        }
        public Dictionary<string,int> MointInsercionsSummary() {
            Dictionary<string,int> InsertionsType = new Dictionary<string,int>();
            foreach (Break item in Breaks)
            {
                if (InsertionsType.Count == 0)
                {
                    InsertionsType = item.InsercionsType();
                }
                else
                {
                    var temp = item.InsercionsType();
                    foreach (var kvp in temp) {
                        if (InsertionsType.ContainsKey(kvp.Key))
                        {
                                InsertionsType[kvp.Key] += kvp.Value;
                        }
                        else
                        {
                            InsertionsType[kvp.Key] = kvp.Value;
                        }
                    }
                }
            }
            return InsertionsType;
        }
        public string ProgrammingSummary() {
            string program = $"Resumo da Programacao do dia {date.ToString("dd-MM-yyyy")}\n";
            bool erros = false;
            if (Breaks != null)
            {
                program += "Insercoes com Erro-------------------------------------------------------------\n";
                foreach (Break item in Breaks)
                {
                    if (!string.IsNullOrEmpty(item.InsercionsErr()))
                    {
                        program += item.InsercionsErr()+"\n";
                    }
                }
                if (erros == false)
                {
                    program += $"Não existe nenhuma inserção com erro na Programação do Dia {date.ToString("dd-MM-yyyy")}\n ";
                }
                program += "------------------------------------------------------------------------------\n\n\n";
            }
            program += "Tipo de Insercoes--------------------------------------------------------------\n";
            program += ParseDictionaryatString(MointInsercionsSummary())+ "\n";
            program += "-------------------------------------------------------------------------------\n";

            return program;
        }    
        public string ListSource(int num)
        {
            if (num == 1)
            {
                return "C";
            }
            else if (num == 2)
            {
                return "M";
            }
            else
            {
                return string.Empty;
            }
        }
        public string PrintByFolder(int num)
        {
            string Source = ListSource(num);
            string parseString = "";
            foreach (var item in Breaks) { 
                parseString += item.PrintByFolder(Source)+"\n";
            }
            return parseString;
        }
        public string PrintByType(int num) {
            string parseString = "";
            foreach (var item in Breaks)
            {
                int type = int.Parse(item.Type);
                if (type == num)
                {
                    parseString += item.ToString() + "\n";
                }
            }
            return parseString;
        }
        public string PrintByTime(string hora, int horastotais) {
            int horario = int.Parse(hora.Split(':')[0]) * 60;
            horario += int.Parse(hora.Split(':')[1]);
            string parseString = "";
            foreach (var Break in Breaks)
            {
                int Orig = TransformOrigInMinutes(Break.Orig);
                if (Orig >= horario && Orig< horario + horastotais*60)
                {
                    parseString += Break.ToString() + "\n";
                }
            }
            return parseString;
        }
        public int TransformOrigInMinutes(string Orig) {
            return int.Parse(Orig.Split(':')[0]) * 60 + int.Parse(Orig.Split(':')[1]);
        }
        private void ParseJson(string path)
        {
            string nameJson = $@"{date.ToString("dd-MM-yyyy")}.json";
            if (string.IsNullOrEmpty(path))
            {
            }
            else
            {
                string filePath = @$"{path}\{nameJson}";
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(Breaks, options);
                File.WriteAllText(filePath, jsonString);
            }
        }
        private string Getpath(string path)
        {
            string datePath = date.ToString("dd-MM-yyyy");
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            string fullPath = @path;
            fullPath += @"\Montagem";
            string filename = @"\" + $"{datePath}" + ".zip";
            fullPath += filename + "%" + @$"{datePath}.xml";  
            return fullPath;
        }
        XmlDocument xmlDocument = new XmlDocument();
        private bool ReadXmlDocument()
        {
            string[] fullPath;
            if (string.IsNullOrEmpty(Getpath(path_Montagem)))
            {
                return false;
            }
            else
            {
                fullPath = Getpath(path_Montagem).Split("%");
            }
            try
            {
                if (!File.Exists(fullPath[0]))
                {
                    return false;
                }
                using (FileStream zipToOpen = new FileStream(fullPath[0], FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        ZipArchiveEntry xmlEntry = archive.GetEntry(fullPath[1]);
                        if (xmlEntry != null)
                        {
                            using (Stream xmlStream = xmlEntry.Open())
                            {
                                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                                xmlDocument.Load(xmlStream);
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        private XmlNodeList GetXmlNodeList()
        {
            XmlNodeList NodeList = xmlDocument.SelectNodes("//Playlist/*");
            return NodeList;
        }
        public void ReadBreaks()
        {
            XmlNodeList listXML = GetXmlNodeList();
            foreach (XmlNode item in listXML)
            {
                Break Break0 = new Break(item);
                Breaks.Add(Break0);
            }
        }
        private bool Init()
        {
            if (ReadXmlDocument())
            {
                ReadBreaks();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
