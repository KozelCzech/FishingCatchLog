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
        public static int CoughtFish() => jsonReader.GetAllFishSpecies().Sum(species => species["catches"].Count());


        public static int KeptFish()
        {
            JArray fishSpecies = jsonReader.GetAllFishSpecies();

            return fishSpecies.Sum(species => species["catches"].Where(catches => Convert.ToBoolean(catches["kept"])).Count());
        }


        public static string FavoriteLocation()
        {
            JArray fishSpecies = jsonReader.GetAllFishSpecies();

            return fishSpecies.SelectMany(species => species["catches"]).GroupBy(catches => catches["location"].ToString()).OrderByDescending(g => g.Count()).First().Key;
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
                SlowWriter.WriteLine($"{i + 1}. {Enum.GetName(typeof(Weather), i + 1)} | {i + 2}. {Enum.GetName(typeof(Weather), i + 2)}");
        }


        public static void WriteAll(string jsonToken)
        {
            Console.WriteLine();

            JArray allSpecies = jsonReader.GetAllFishSpecies();

            List<string> methods = new List<string>();

            foreach (JObject fishes in allSpecies)
            {
                foreach (JObject fish in fishes["catches"])
                    methods.Add(fish[jsonToken].ToString());
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
