using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
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
            string parseString = string.Join("\n",Breaks.Select(x => x.ToString()));
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
            int day, month, year;
            if ((int.TryParse(datePath.Split('-')[0], out day) && int.TryParse(datePath.Split('-')[1], out month) && int.TryParse(datePath.Split('-')[2], out year)))
            {
                string filename = @"\" + $"{datePath}" + ".zip";
                fullPath += filename + "%" + @$"{datePath}.xml";
            }
            else
            {
                return null;
            }
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
