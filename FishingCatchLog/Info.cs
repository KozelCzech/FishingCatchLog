using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatchLog
{
    public class Info
    {
        public static int CoughtFish()
        {
            //cought the catches JObject inside each species
            JArray fishSpecies = jsonReader.GetAllFishSpecies();
            int coughtFish = 0;

            foreach (JObject species in fishSpecies)
            {
                coughtFish += species["catches"].Count();
            }


            return coughtFish;
        }


        public static int KeptFish()
        {
            JArray fishSpecies = jsonReader.GetAllFishSpecies();
            int keptFish = 0;

            foreach (JObject species in fishSpecies)
            {
                foreach (JObject catches in species["catches"])
                {
                    if (Convert.ToBoolean(catches["kept"]) == true)
                    {
                        keptFish++;
                    }
                }
            }

            return keptFish;
        }


        public static string FavoriteLocation()
        {
            JArray fishSpecies = jsonReader.GetAllFishSpecies();
            List<string> locations = new List<string>();

            foreach (JObject species in fishSpecies)
            {
                foreach (JObject catches in species["catches"])
                {
                    locations.Add(catches["location"].ToString());
                }
            }
            string favoriteLocation = locations.GroupBy(x => x)
                                      .OrderByDescending(g => g.Count())
                                      .First()
                                      .Key;

            return favoriteLocation;
        }


        public static void WriteAllSpecies()
        {
            Console.WriteLine();
            JArray allSpecies = jsonReader.GetAllFishSpecies();
            for (int i = 0; i < allSpecies.Count; i++)
            {
                SlowWriter.Write($"{allSpecies[i]["species"]}");
                if (i != allSpecies.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();
        }


        public static void WriteAllLocations()
        {
            Console.WriteLine();
            List<string> allLocations = jsonReader.GetAllLocations();

            for (int i = 0; i < allLocations.Count; i++)
            {
                SlowWriter.Write($"{allLocations[i]}");
                if (i != allLocations.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();
        }


        public static void WriteAllWeather()
        {
            Console.WriteLine();
            for (int i = 0; i < Enum.GetNames(typeof(Weather)).Length; i = i + 2)
            {
                SlowWriter.WriteLine($"{i + 1}. {Enum.GetName(typeof(Weather), i + 1)} | {i + 2}. {Enum.GetName(typeof(Weather), i + 2)}");
            }
        }


        public static void WriteAllBaits()
        {
            Console.WriteLine();

            JArray allSpecies = jsonReader.GetAllFishSpecies();

            List<string> baits = new List<string>();

            foreach ( JObject fishes in allSpecies)
            {
                foreach (JObject fish in fishes["catches"])
                {
                    baits.Add(fish["bait"].ToString());
                }
            }

            baits = baits.Distinct().ToList();
            
            for (int i = 0; i < baits.Count; i++)
            {
                SlowWriter.Write($"{baits[i]}");

                if (i != baits.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();
        }


        public static void WriteAllMethods()
        {
            Console.WriteLine();

            JArray allSpecies = jsonReader.GetAllFishSpecies();

            List<string> methods = new List<string>();

            foreach (JObject fishes in allSpecies)
            {
                foreach (JObject fish in fishes["catches"])
                {
                    methods.Add(fish["method"].ToString());
                }
            }

            methods = methods.Distinct().ToList();

            for (int i = 0; i < methods.Count; i++)
            {
                SlowWriter.Write($"{methods[i]}");

                if (i != methods.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();
        }
    }
}
