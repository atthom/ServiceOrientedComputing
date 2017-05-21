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
            VelibService.VelibServiceClient intranetClient = new VelibService.VelibServiceClient("intranet");
            string shouldContinue = "yes";

            Console.WriteLine("Would you like to have a demo first ? (Yes,y|No,n)");
            string wantDemo = Console.ReadLine();
            if(wantDemo.ToLower().Equals("yes") || wantDemo.ToLower().Equals("y"))
            {
                InteractionDemo(intranetClient); 
            }

            Console.WriteLine("Would you like to try our service yourself now ? (Yes,y|No,n)");
            shouldContinue = Console.ReadLine();


            while (shouldContinue.ToLower().Equals("yes") || shouldContinue.ToLower().Equals("y"))
            {
                InteractWithUser(intranetClient);
                Console.WriteLine("Do you want to lookup a new itinerary ? (Yes,y|No,n)");
                shouldContinue = Console.ReadLine();
            }

        }

        // Equivalent of a form, ask to the user his travel information 
        private static void InteractWithUser(VelibService.VelibServiceClient intranetClient)
        {
            Console.WriteLine("\nEnter your departure address: ");
            //Instance of departure/arrival address format : 39 Avenue de la Redoute, 92600 Asnières sur Seine, France
            string departure_address = Console.ReadLine();
            string departure_coords = intranetClient.GetCoords(departure_address);
            if (departure_coords == null || !(departure_coords.Contains("lat") && departure_coords.Contains("lng")))
            {
                Console.WriteLine("Uhh Uhh ! Something went wrong are you sure that's were you want to depart from ?\n"
                                + "If yes check that server correctly talk with the api !");
                return;
            }

            Console.WriteLine("\nEnter your destination address: ");
            string arrival_address = Console.ReadLine();
            string arrival_coords = intranetClient.GetCoords(arrival_address);
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

            Console.WriteLine(intranetClient.GetItinerary(departure_coords, arrival_coords));


        }

        private static void InteractionDemo(VelibService.VelibServiceClient intranetClient)
        {
            string departure_address = "Place Charles de Gaulle, 75008 Paris, France";
            string departure_coords = intranetClient.GetCoords(departure_address);

            string arrival_address = "Rue de Rivoli, 75001 Paris, France";
            string arrival_coords = intranetClient.GetCoords(arrival_address);

            Console.WriteLine("Itinerary from: " + departure_address + " to " + arrival_address + "\n");

            Console.WriteLine(intranetClient.GetItinerary(departure_coords, arrival_coords));

            departure_address = "Asnières, France";
            departure_coords = intranetClient.GetCoords(departure_address);

            arrival_address = "Versailles, France";
            arrival_coords = intranetClient.GetCoords(arrival_address);

            Console.WriteLine("Itinerary from: " + departure_address + " to " + arrival_address + "\n");

            Console.WriteLine(intranetClient.GetItinerary(departure_coords, arrival_coords));
        }
    }
}
