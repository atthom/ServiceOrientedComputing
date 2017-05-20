using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTigli
{

    // 77 Boulevard de Courcelles 48.878319,2.2981875
    // 57 Avenue du Dr Arnold Netter 48.8449208,2.403417
    class Program
    {
        static void Main(string[] args)
        {
            ServiceTigli.TigliServiceClient tigliClient = new ServiceTigli.TigliServiceClient();
            string shouldContinue = "yes";

            while(shouldContinue.ToLower().Equals("yes") || shouldContinue.ToLower().Equals("y"))
            {
                InteractWithUser(tigliClient);
                Console.WriteLine("Do you want to lookup a new itinerary ? (Yes,y|No,n)");
                shouldContinue = Console.ReadLine();
            }

        }

        private static void InteractWithUser(ServiceTigli.TigliServiceClient tigliClient)
        {
            Console.WriteLine("Enter your departure address: ");
            //Instance of departure address format : 39 Avenue de la Redoute, 92600 Asnières sur Seine, France
            string departure_address = Console.ReadLine();
            string departure_coords = tigliClient.GetCoords(departure_address);
            if(departure_coords == null || !(departure_coords.Contains("lat") && departure_coords.Contains("lng")))
            {
                Console.WriteLine("Uhh Uhh ! Something went wrong are you sure that's were you want to depart from ?\n"
                                + "If yes check that server correctly talk with the api !");
                return;
            }
            Console.WriteLine("Address: " + departure_address + "\nCoordinates: " + departure_coords);

            Console.WriteLine("Enter your destination address: ");
            string arrival_address = Console.ReadLine();
            string arrival_coords = tigliClient.GetCoords(arrival_address);
            if (arrival_coords == null || !(arrival_coords.Contains("lat") && arrival_coords.Contains("lng")))
            {
                Console.WriteLine("Uhh Uhh ! Something went wrong are you sure that's were you want to go ?\n"
                                + "If yes check that server correctly talk with the api !");
                return;
            }


            // TODO: Use regex instead
            string orig_latitude = departure_coords.Split('>')[1].Split('<')[0];
            string orig_long = departure_coords.Split('>')[3].Split('<')[0];

            string dest_latitude = arrival_coords.Split('>')[1].Split('<')[0];
            string dest_long = arrival_coords.Split('>')[3].Split('<')[0];


            // !!! Debug !!!
            Console.WriteLine("Origin coordinates: lat = " + orig_latitude + " long = " + orig_long + ".\nDestination coordinates: lat = " + dest_latitude + ", long = " + dest_long);
            Console.WriteLine("Here is the way to go: ");
            
            Console.WriteLine(
                tigliClient.GetItinerary(
                    departure_coords, 
                    arrival_coords));
        }
    }
}
