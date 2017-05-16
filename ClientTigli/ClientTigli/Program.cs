using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTigli
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceTigli.TigliServiceClient tigliClient = new ServiceTigli.TigliServiceClient();
            //Console.WriteLine(tigliClient.echoget("Paris", "Grenoble"));
            Console.WriteLine(tigliClient.getCoords("39 Avenue de la Redoute, 92600 Asnières sur Seine, France"));
            Console.ReadKey();
        }
    }
}
