using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LeituraXmlConsole.utils
{
    public class Ins
    {
        public Ins(XmlNode listIns)
        {
            string[] attributeNames = { "Id", "CTA", "Source", "MovedTo", "Type", "Title", "File", "Folder", "Text", "Composer", "Comment", "Checked", "Err", "sErr", "HoraAudio", "HoraPK", "IsAudioFile", "DurOrig", "Dur", "Refr", "DurRefr", "PtVh", "PtMx", "MxIni", "Intro", "PtLoc", "Vol", "Bitrate", "Reg", "MD5" };
            foreach (string attributeName in attributeNames)
            {
                string attributeValue = listIns.Attributes[attributeName]?.Value;
                if (string.IsNullOrEmpty(attributeValue))
                {
                    attributeValue = "";
                }
                typeof(Ins).GetProperty(attributeName).SetValue(this, attributeValue);
            }
            this.Name = listIns.Name;
        }
        public override string ToString()
        {
            return $"Name: {Name}, " +
                   $"Id: {Id}, " +
                   $"CTA: {CTA}, " +
                   $"Source: {Source}, " +
                   $"MovedTo: {MovedTo}, " +
                   $"Type: {Type}, " +
                   $"Title: {Title}, " +
                   $"File: {File}, " +
                   $"Folder: {Folder}, " +
                   $"Text: {Text}, " +
                   $"Composer: {Composer}, " +
                   $"Comment: {Comment}, " +
                   $"Checked: {Checked}, " +
                   $"Err: {Err}, " +
                   $"sErr: {sErr}, " +
                   $"HoraAudio: {HoraAudio}, " +
                   $"HoraPK: {HoraPK}, " +
                   $"IsAudioFile: {IsAudioFile}, " +
                   $"DurOrig: {DurOrig}, " +
                   $"Dur: {Dur}, " +
                   $"Refr: {Refr}, " +
                   $"DurRefr: {DurRefr}, " +
                   $"PtVh: {PtVh}, " +
                   $"PtMx: {PtMx}, " +
                   $"MxIni: {MxIni}, " +
                   $"Intro: {Intro}, " +
                   $"PtLoc: {PtLoc}, " +
                   $"Vol: {Vol}, " +
                   $"Bitrate: {Bitrate}, " +
                   $"Reg: {Reg}, " +
                   $"MD5: {MD5}\n\n";
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string CTA { get; set; }
        public string Source { get; set; }
        public string MovedTo { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public string Folder { get; set; }
        public string Text { get; set; }
        public string Composer { get; set; }
        public string Comment { get; set; }
        public string Checked { get; set; }
        public string Err { get; set; }
        public string sErr { get; set; }
        public string HoraAudio { get; set; }
        public string HoraPK { get; set; }
        public string IsAudioFile { get; set; }
        public string DurOrig { get; set; }
        public string Dur { get; set; }
        public string Refr { get; set; }
        public string DurRefr { get; set; }
        public string PtVh { get; set; }
        public string PtMx { get; set; }
        public string MxIni { get; set; }
        public string Intro { get; set; }
        public string PtLoc { get; set; }
        public string Vol { get; set; }
        public string Bitrate { get; set; }
        public string Reg { get; set; }
        public string MD5 { get; set; }
    }
}
