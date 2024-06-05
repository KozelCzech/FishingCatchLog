using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FishingCatchLog
{
    public class jsonReader
    {
        static readonly string _jsonPath = "../../../Data/catchData.json";

        public static JArray GetAllFishSpecies()
        {
            string json = File.ReadAllText(_jsonPath);
            return JArray.Parse(json);
        }


        public static JObject GetFishSpeciesByName(string name)
        {
            string json = File.ReadAllText(_jsonPath);
            JObject fish = (JObject)JArray.Parse(json).FirstOrDefault(x => x["species"].ToString() == name);
            return fish;
        }


        public static List<string> GetAllLocations()
        {
            List<string> locations = new List<string>();
            JArray fishSpecies = jsonReader.GetAllFishSpecies();
            foreach (JObject species in fishSpecies)
            {
                foreach (JObject catches in species["catches"])
                {
                    locations.Add(catches["location"].ToString());
                }
            }

            locations = locations.Distinct().ToList();

            return locations;
        }


        public static void SaveJson(JArray json)
        {
            using (StreamWriter sw = new StreamWriter(_jsonPath, false))
                sw.Write(json.ToString());
        }
    }
}
