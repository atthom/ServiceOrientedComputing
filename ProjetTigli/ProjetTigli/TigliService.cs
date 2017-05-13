using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace ProjetTigli
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class TigliService : ITigliService
    {

        public string echoget(string origin, string destination)
        {

            //    Console.Write("Envoyez les coordonnées sergent !\n");
            // Console.Write("Latitude : ");
            //  string latitude = Console.ReadLine();
            string orig_latitude = "48.859150287255645";

            //   Console.Write("Longitude : ");
            //  string longitude = Console.ReadLine();
            string orig_long = "2.347620087684511";

            // Console.Write("Latitude : ");
            //  string latitude = Console.ReadLine();
            string dest_latitude = "48.89465024997023";

            //   Console.Write("Longitude : ");
            //  string longitude = Console.ReadLine();
            string dest_long = "2.381868729508476";

            // GetClosestStation(latitude, longitude);
            XmlNode closest_orig_station = GetClosestStation(orig_latitude, orig_long);
            XmlNode closest_dest_station = GetClosestStation(dest_latitude, dest_long);

            // si trop près aller directement à pied !!!



            if (closest_dest_station["name"] == closest_orig_station["name"])
            {
                Console.WriteLine("Pas besoin de prendre le vélo, tu es suffisament près pour y aller à pied mec...");
            }
            else
            {
                Console.WriteLine("Walking to the station : " + closest_orig_station["name"]);

                String walkingTotheStop = GetPathBetweenCoords(
                    orig_latitude, orig_long,
                    closest_orig_station.Attributes["lat"].Value,
                    closest_orig_station.Attributes["lng"].Value,
                    "walking");

                formatXMLAnwser(walkingTotheStop);

                Console.WriteLine("Biking from the station" + closest_orig_station["name"] + " to the station : " + closest_dest_station["name"]);

                String bikingtothestop = GetPathBetweenCoords(
                    closest_orig_station.Attributes["lat"].Value, closest_orig_station.Attributes["lng"].Value,
                    closest_dest_station.Attributes["lat"].Value, closest_dest_station.Attributes["lng"].Value,
                    "bicycling");

                formatXMLAnwser(bikingtothestop);

                Console.WriteLine("Walking from the station " + closest_dest_station["name"] + " to your final destination :");

                String walkingtotheend = GetPathBetweenCoords(
                     closest_dest_station.Attributes["lat"].Value, closest_dest_station.Attributes["lng"].Value,
                     dest_latitude, dest_long,
                    "walking");

                formatXMLAnwser(bikingtothestop);

            }

            Console.ReadKey();


            return origin + "whatever" + destination;
        }

        private float distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        private XmlNode GetClosestStation(string lat, string longi)
        {
            float f_lat = float.Parse(lat, CultureInfo.InvariantCulture.NumberFormat);
            float f_longi = float.Parse(longi, CultureInfo.InvariantCulture.NumberFormat);

            // Get Response from the Service 
            // Get the stream containing content returned by the server.
            // Open the stream using a StreamReader for easy access and put it into a string
            // Read the content.
            // Put it in a string 
            string responseFromServer = new StreamReader(
                WebRequest.Create("http://www.velib.paris/service/carto")
                .GetResponse()
                .GetResponseStream()
                ).ReadToEnd();

            // Parse the response and put the entries in XmlNodeList 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseFromServer);
            XmlNodeList elemList = doc.GetElementsByTagName("marker");

            float minDist = 100.0f;
            int minIndex = 42;

            for (int i = 0; i < elemList.Count; i++)
            {
                //lat="48.857940092963034" lng="2.347010058114489"
                float station_lat = float.Parse(
                    elemList[i].Attributes["lat"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);

                float station_lng = float.Parse(
                    elemList[i].Attributes["lng"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);

                float dist = distance(f_lat, f_longi, station_lat, station_lng);

                if (dist < minDist)
                {
                    minDist = dist;
                    minIndex = i;
                }
            }

            return elemList[minIndex];
        }


        private string GetPathBetweenCoords(string lat1, string long1, string lat2, string long2, string mod)
        {
            string basereq = "https://maps.googleapis.com/maps/api/directions/xml?";

            string origin = "origin=" + lat1 + "," + long1;
            string dest = "&destination=" + lat2 + "," + long2;
            string mode = "&mode=" + mod;

            string key = "&key=AIzaSyAZc64V_gBwlyFUr065QIczOqzpU6kYU9k";

            return new StreamReader(
                WebRequest.Create(basereq + origin + dest + mode + key)
                .GetResponse()
                .GetResponseStream()
                ).ReadToEnd();
        }

        private void formatXMLAnwser(string response)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            string start = doc.GetElementsByTagName("start_address")[0].InnerXml;
            string end = doc.GetElementsByTagName("end_address")[0].InnerXml;

            Console.WriteLine("You are at : " + start);


            XmlNodeList steps = doc.GetElementsByTagName("step");

            for (int i = 0; i < steps.Count; i++)
            {
                XmlNodeList childs = steps[i].ChildNodes;

                string travel_mode = "";
                string duration = "";
                string distance = "";
                string html_instructions = "";


                for (int j = 0; j < childs.Count; j++)
                {
                    if (childs[j].Name == "travel_mode")
                    {
                        travel_mode = childs[j].InnerXml;
                    }

                    if (childs[j].Name == "duration")
                    {
                        duration = childs[j].LastChild.InnerXml;
                    }

                    if (childs[j].Name == "distance")
                    {
                        distance = childs[j].LastChild.InnerXml;
                    }

                    if (childs[j].Name == "html_instructions")
                    {
                        html_instructions = childs[j].InnerXml;
                    }

                }

                string str_mode = "";
                if (travel_mode == "WALKING")
                {
                    str_mode = "Walk ";
                }
                else
                {
                    str_mode = "Bike ";
                }

                Console.WriteLine("Step " + i + ": " + str_mode + distance + " during " + duration + "\n\t Hint : " + html_instructions);

            }

            Console.WriteLine("You are now at : " + start);
        }

    }
}
