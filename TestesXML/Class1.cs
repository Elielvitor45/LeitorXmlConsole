using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeituraXmlConsole;
using LeituraXmlConsole.utils;
namespace TestesXML
{
    public class Class1
    {
        [Fact]
        public void teste()
        {
            ScheduleDay s = new ScheduleDay();
            int resultado = 5 + 5;
            Assert.Equal(10, resultado);
        }
    }
}
