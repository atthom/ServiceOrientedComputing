using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml;

namespace ProjetTigli
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class TigliService : ITigliService
    {

        public string GetItinerary(string origin, string destination)
        {

            string orig_latitude = origin.Split('>')[1].Split('<')[0];
            string orig_long = origin.Split('>')[3].Split('<')[0];

            string dest_latitude = destination.Split('>')[1].Split('<')[0];
            string dest_long = destination.Split('>')[3].Split('<')[0];
            
            // !!! Debug !!!
            Console.WriteLine("Origin coordinates: lat = " + orig_latitude + " long = " + orig_long + ".\nDestination coordinates: lat = " + dest_latitude + ", long = " + dest_long);
            

            // Get closest stations to origin and destination (using latitude and longitude coordinates)
            XmlNode closest_orig_station = GetClosestStation(orig_latitude, orig_long);
            XmlNode closest_dest_station = GetClosestStation(dest_latitude, dest_long);

            string result = "";

            // If origin and destination have the same closest station then you are close so you can walk
            // Otherwise we compute the itinerary !!!
            if (closest_dest_station.Attributes["name"].Value.Equals(closest_orig_station.Attributes["name"].Value))
            {
                result = "Pas besoin de prendre le vélo, tu es suffisament près pour y aller à pied mec...";
            }
            else
            {
                //TODO: Reduce this if possible
                result = "Walk to the station : " + closest_orig_station.Attributes["name"].Value;

                String walkingTotheStop = GetPathBetweenCoords(
                    orig_latitude, orig_long,
                    closest_orig_station.Attributes["lat"].Value,
                    closest_orig_station.Attributes["lng"].Value,
                    "walking");

                result += FormatXMLAnwser(walkingTotheStop);
                result += "Bike from the station: " + closest_orig_station.Attributes["name"].Value + " to the station: " + closest_dest_station.Attributes["name"].Value;

                String bikingtothestop = GetPathBetweenCoords(
                    closest_orig_station.Attributes["lat"].Value, closest_orig_station.Attributes["lng"].Value,
                    closest_dest_station.Attributes["lat"].Value, closest_dest_station.Attributes["lng"].Value,
                    "bicycling");

                result += FormatXMLAnwser(bikingtothestop);
                result += "Walk from the station: " + closest_dest_station.Attributes["name"].Value + " to your final destination: ";

                String walkingtotheend = GetPathBetweenCoords(
                     closest_dest_station.Attributes["lat"].Value, closest_dest_station.Attributes["lng"].Value,
                     dest_latitude, dest_long,
                    "walking");

                result += FormatXMLAnwser(bikingtothestop);
            }

            //TODO: return itinerary instead !
            //return ("Origin coordinates: lat = " + orig_latitude + " long = " + orig_long + ".\nDestination coordinates: lat = " + dest_latitude + ", long = " + dest_long);
            return result;
        }

        public string GetCoords(string address)
        {
            string api_url = "https://maps.googleapis.com/maps/api/geocode/xml?";
            string address_url_format = address.Replace(" ", "+");
            string api_key = "&key=AIzaSyBtSz9ZE9l8bKrmRg7juvP_DpomyU7epO8";

            string responseFromServer = new StreamReader(
                WebRequest.Create(api_url + "address=" + address_url_format + api_key)
                .GetResponse()
                .GetResponseStream()
                ).ReadToEnd();
            
            // Static response used to test of line
            /**string responseFromServer = "<GeocodeResponse><status>OK</status><result><type>street_address</type>"
                    + "<formatted_address>1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA</formatted_address>"
                    + "<address_component><long_name>1600</long_name><short_name>1600</short_name><type>street_number</type></address_component>"
                    + "<address_component><long_name>Amphitheatre Pkwy</long_name><short_name>Amphitheatre Pkwy</short_name><type>route</type></address_component>"
                    + "<geometry><location><lat>37.4217550</lat><lng>-122.0846330</lng></location><location_type>ROOFTOP</location_type>"
                    + "<viewport><southwest><lat>37.4188514</lat><lng>-122.0874526</lng></southwest><northeast><lat>37.4251466</lat><lng>-122.0811574</lng></northeast></viewport>"
                    + "</geometry></result></GeocodeResponse>";
            **/
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseFromServer);

            float position_lat;
            float position_lng;

            if (doc.GetElementsByTagName("status")[0].InnerXml.Equals("OK"))
             {
                XmlNodeList elemList = doc.GetElementsByTagName("location")[0].ChildNodes;

                if (!(elemList[0].Name.Equals("lat") && elemList[1].Name.Equals("lng")))
                {
                    return "Uhh, Uhh! Something is wrong with your location coordinates! ";
                }
                
                 position_lat = float.Parse(
                     elemList[0].InnerXml,
                     CultureInfo.InvariantCulture.NumberFormat);

                 position_lng = float.Parse(
                     elemList[1].InnerXml,
                     CultureInfo.InvariantCulture.NumberFormat);
                
                // !!! Debug !!!
                Console.WriteLine("Address: " + address + "\nCoordinates: lat = " + position_lat + " long = " + position_lng);

                return doc.GetElementsByTagName("location")[0].InnerXml;
            }

            return "Status: " + doc.GetElementsByTagName("status")[0].InnerXml;
        }


        private float Distance(float x1, float y1, float x2, float y2)
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

            float minDist = float.MaxValue;
            int minIndex = 42;

            for (int i = 0; i < elemList.Count; i++)
            {
                float station_lat = float.Parse(
                    elemList[i].Attributes["lat"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);

                float station_lng = float.Parse(
                    elemList[i].Attributes["lng"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);

                // Factor 10000 to be sure that round up doesn't mess with the distance computation
                if (Distance(f_lat * 10000, f_longi * 10000, station_lat * 10000, station_lng * 10000) < minDist)
                {
                    minIndex = i;
                    minDist = Distance(f_lat * 10000, f_longi * 10000, station_lat * 10000, station_lng * 10000);
                }
            }

            return elemList[minIndex];
        }

        // Give the path to go from (lat1, long1) to (lat2, long2)
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

        // TODO: Explain what this is reduce the function if possible.
        private string FormatXMLAnwser(string response)
        {
            response = HttpUtility.HtmlDecode(response);            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            string start = doc.GetElementsByTagName("start_address")[0].InnerXml;
            string end = doc.GetElementsByTagName("end_address")[0].InnerXml;

            string path_to_follow = "You are at : " + start + "\n";


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

                path_to_follow += "\tStep " + i + ": " + str_mode + distance + " during " + duration + "\n\t\t Hint : " + html_instructions +"\n";
            }

            path_to_follow += "\nYou will then be at : " + start + "\n";

            return path_to_follow;
        }

    }
}
