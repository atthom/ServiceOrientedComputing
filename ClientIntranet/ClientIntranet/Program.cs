using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientIntranet
{
    class Program
    {
        static void Main(string[] args)
        {
            VelibService.VelibServiceClient tigliClient = new VelibService.VelibServiceClient("intranet");
            string shouldContinue = "yes";

            while (shouldContinue.ToLower().Equals("yes") || shouldContinue.ToLower().Equals("y"))
            {
                InteractWithUser(tigliClient);
                Console.WriteLine("Do you want to lookup a new itinerary ? (Yes,y|No,n)");
                shouldContinue = Console.ReadLine();
            }

        }

        // Equivalent of a form, ask to the user his travel information 
        private static void InteractWithUser(VelibService.VelibServiceClient tigliClient)
        {
            Console.WriteLine("Enter your departure address: ");
            //Instance of departure/arrival address format : 39 Avenue de la Redoute, 92600 Asnières sur Seine, France
            string departure_address = Console.ReadLine();
            string departure_coords = tigliClient.GetCoords(departure_address);
            if (departure_coords == null || !(departure_coords.Contains("lat") && departure_coords.Contains("lng")))
            {
                Console.WriteLine("Uhh Uhh ! Something went wrong are you sure that's were you want to depart from ?\n"
                                + "If yes check that server correctly talk with the api !");
                return;
            }

            Console.WriteLine("\nEnter your destination address: ");
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


            Console.WriteLine("\nOrigin coordinates: lat = " + orig_latitude + " long = " + orig_long + ".\nDestination coordinates: lat = " + dest_latitude + ", long = " + dest_long + "\n");
            Console.WriteLine("Here is the way to go: ");

            Console.WriteLine(tigliClient.GetItinerary(departure_coords, arrival_coords));


        }
    }
}
