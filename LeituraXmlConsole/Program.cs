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
            int condicion = 4;
            DateTime date;
            Console.WriteLine("Digite o Caminho da Pasta pgm");
            path = Console.ReadLine();
            do
            {
                if (condicion == 4)
                {
                    Console.WriteLine("Digite a data do arquivo montagem (dd-MM-yyyy)");
                    dateS = Console.ReadLine();
                    scheduleDay = new ScheduleDay();
                    condicion = -1;
                }
                Console.Clear();
                Console.WriteLine("Imprimir xml(1): ");
                Console.WriteLine("Imprimir montagem escolhendo o horario e a quantidade de horas(2)");
                Console.WriteLine("Xml para Json(3): ");
                Console.WriteLine("Carregar outro montagem.xml(4)");
                Console.WriteLine("Imprimir apenas um tipo de bloco(5): ");
                Console.WriteLine("Imprimir em relacao as pastas(6):");
                Console.WriteLine("Sair(0)");
                condicion = int.Parse(Console.ReadLine());

                if (condicion == 1)
                {
                    if (DataVerify(dateS))
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
                    if (DataVerify(dateS))
                    {
                        string hora; 
                        int quantidadeDeHoras;
                        Console.WriteLine("Digite o horario inicial da programacao:(Digite nesse formato 00:00)");
                        hora = Console.ReadLine();
                        Console.WriteLine("Digite a quantidade de horas que deseja ser impressa:(Exemplo, se for 8 horas, apenas digite 8)");
                        quantidadeDeHoras = int.Parse(Console.ReadLine());
                        date = DateTime.Parse(dateS);
                        scheduleDay.ReadScheduleDay(path, date);
                        Console.WriteLine(scheduleDay.PrintByTime(hora,quantidadeDeHoras));
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Data Invalida!");
                    }
                }
                else if (condicion == 3)
                {
                    string pathjson;
                    //Em relacao ao usuario escolher onde sera salvo o json, ele ja pode escolher por aqui.
                    Console.WriteLine("Caminho onde o Json sera salvo:");
                    pathjson = Console.ReadLine();
                    if (DataVerify(dateS))
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
                else if (condicion == 5)
                {
                    if (DataVerify(dateS))
                    {
                        int type;
                        Console.WriteLine("Digite o qual bloco vai ser Comercial(1);Musical(2):");
                        type = int.Parse(Console.ReadLine());
                        date = DateTime.Parse(dateS);
                        scheduleDay.ReadScheduleDay(path, date);
                        Console.WriteLine(scheduleDay.PrintByType(type));
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Data Invalida!");
                    }
                }
                else if (condicion == 6)
                {
                    if (DataVerify(dateS))
                    {
                        int num;
                        //Fiz atualmente dessa forma, mas depois irei tentar uma forma mais dinamica.
                        Console.WriteLine("Digite se quer Insercoes da pasta Comercial(1) e Musical(2)");
                        num = int.Parse(Console.ReadLine());
                        date = DateTime.Parse(dateS);
                        scheduleDay.ReadScheduleDay(path, date);
                        Console.WriteLine(scheduleDay.PrintByFolder(num));
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Data Invalida!");
                    }
                }
                
            } while (condicion!=0);
        }
        private static bool DataVerify(string dateS)
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