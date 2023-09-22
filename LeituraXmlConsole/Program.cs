using LeituraXmlConsole.utils;
using Microsoft.VisualBasic;

namespace LeituraXmlConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ScheduleDay scheduleDay = new ScheduleDay(); ;
            string path = string.Empty;
            string dateS = string.Empty;
            int condicion = 3;

            DateTime date;
            do
            {
                if (condicion == 3)
                {
                    Console.WriteLine("Digite o Caminho da Pasta pgm");
                    path = Console.ReadLine();
                    Console.WriteLine("Digite a data do arquivo montagem (dd-MM-yyyy)");
                    dateS = Console.ReadLine();
                    scheduleDay = new ScheduleDay();
                    condicion = -1;
                }
                Console.Clear();
                Console.WriteLine("Imprimir xml(1): ");
                Console.WriteLine("Xml para Json(2): ");
                Console.WriteLine("Carregar outro montagem.xml(3)");
                Console.WriteLine("Sair(0)");
                condicion = int.Parse(Console.ReadLine());

                if (condicion == 1)
                {
                    if (Parse(dateS))
                    {
                        date = DateTime.Parse(dateS);
                        scheduleDay.ReadScheduleDay(path, date);
                        Console.WriteLine(scheduleDay.ToString());
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Data Invalida!");
                    }
                }
                else if (condicion == 2)
                {
                    string pathjson;
                    Console.WriteLine("Caminho onde o Json sera salvo:");
                    pathjson = Console.ReadLine();
                    if (Parse(dateS))
                    {
                        date = DateTime.Parse(dateS);
                        scheduleDay.ReadScheduleDay(path, pathjson, date);
                        string jsonString = File.ReadAllText(pathjson +@"\"+dateS+".json");
                        Console.WriteLine("Json Salvo com Sucesso!\n\n\n");
                        Console.WriteLine(jsonString);
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Data invalida!");
                    }



                }
                
            } while (condicion!=0);
            
        }
        //Adicionei essa verificação, pois o pedido era passar um datetime como parametro, mas caso fosse string,
        //já conseguiria ser tratada dentro da propria classe ScheduleDay.
        private static bool Parse(string dateS)
        {
            string[] dateParts = dateS.Split("-");

            if (dateParts.Length != 3)
            {
                return false;
            }
            return int.TryParse(dateParts[0], out _) &&
                   int.TryParse(dateParts[1], out _) &&
                   int.TryParse(dateParts[2], out _);
        }
    }
}