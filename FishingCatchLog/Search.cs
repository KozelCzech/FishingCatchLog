using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FishingCatchLog
{
    public class Search
    {
        public static void BySpecies()
        {
            Console.Clear();
            Info.WriteAllSpecies();
            SlowWriter.WriteLine("What species would you like to search for?");

            string speciesString = Console.ReadLine();


            JObject species = jsonReader.GetFishSpeciesByName(speciesString);

            foreach (JObject fish in species["catches"])
            {
                DisplayMenu.DisplayFishInfo(fish);

                SlowWriter.WriteLine("\t--------------------");
            }
        }


        public static void ByLocation()
        {
            Console.Clear();
            Info.WriteAllLocations();

            SlowWriter.WriteLine("Which location would you like to search for?");
            string searchLocation = Console.ReadLine();

            //find all fish cought on that location
            JArray allSpecies = jsonReader.GetAllFishSpecies();
            foreach (JObject species in allSpecies)
            {

                foreach (JObject fish in species["catches"])
                {
                    if (fish["location"].ToString() == searchLocation)
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                }
            }
        }


        public static void ByDate(string searchDateString = "")
        {
            Console.Clear();
            DateTime searchDate = DateTime.Now;
            Console.WriteLine();
            SlowWriter.WriteLine("Which date would you like to search for? (format: dd/mm/yyyy)");
            if (searchDateString == string.Empty)
            {
                bool isDateValid = DateTime.TryParse(Console.ReadLine(), out searchDate);
                if (!isDateValid)
                    ByDate();
                else
                    searchDateString = searchDate.ToString("dd/MM/yyyy");
            }
            else
                SlowWriter.WriteLine(searchDateString);

            Console.WriteLine();
            SlowWriter.WriteLine("1. Before");
            SlowWriter.WriteLine("2. After");
            SlowWriter.WriteLine("Would you like to search for entires before or after this date?");

            bool isDirectionValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int direction);
            if (!isDirectionValid)
                ByDate(searchDateString);

            //execute the search here
            //make sure to implement the before and after date searching
            Console.WriteLine();
            JArray allSpecies = jsonReader.GetAllFishSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    DateTime catchDate = DateTime.Parse(fish["date"].ToString());

                    if (direction == 1 && catchDate < searchDate)
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                    else if (direction == 2 && catchDate > searchDate)
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                }
            }


        }


        public static void ByWeather()
        {
            Console.Clear();
            Weather weather = Weather.Undefined;

            Info.WriteAllWeather();

            SlowWriter.WriteLine("Which weather would you like to search for?");
            bool isWeatherValid = int.TryParse(Console.ReadLine(), out int weatherChoice);
            if (!isWeatherValid)
                ByWeather();
            else
                weather = (Weather)weatherChoice;

            JArray allSpecies = jsonReader.GetAllFishSpecies();

            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["weather"].ToString() == weather.ToString())
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                }
            }
        }


        public static void ByBait()
        {
            Console.Clear();

            Info.WriteAllBaits();

            SlowWriter.WriteLine("What bait would you like to search for?");

            string baitString = Console.ReadLine();
            JArray allSpecies = jsonReader.GetAllFishSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["bait"].ToString() == baitString)
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                }
            }
        }


        public static void ByMehtod()
        {
            Console.Clear();

            Info.WriteAllMethods();

            SlowWriter.WriteLine("What method would you like to search for?");

            string methodString = Console.ReadLine();
            JArray allSpecies = jsonReader.GetAllFishSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["method"].ToString() == methodString)
                    {
                        SlowWriter.WriteLine(species["species"].ToString());
                        DisplayMenu.DisplayFishInfo(fish);
                        SlowWriter.WriteLine("\t--------------------");
                    }
                }
            }

        }
    }
}
