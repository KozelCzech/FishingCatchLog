using Newtonsoft.Json.Linq;

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
            JArray allSpecies = jsonReader.GetAllSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["location"].ToString() == searchLocation)
                        WriteFishInfo(species, fish);
                }
            }
        }


        public static void ByDate(string searchDateString = "")
        {
            #region DateInput
            Console.Clear();
            DateTime searchDate = DateTime.Now;
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
            #endregion

            #region SpecifySearchInput
            Console.WriteLine();
            SlowWriter.WriteLine("1. Before");
            SlowWriter.WriteLine("2. After");
            SlowWriter.WriteLine("Would you like to search for entires before or after the chosen date?");

            bool isDirectionValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int direction);
            if (!isDirectionValid)
                ByDate(searchDateString);
            #endregion

            #region WriteOutput
            Console.WriteLine();
            JArray allSpecies = jsonReader.GetAllSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    DateTime catchDate = DateTime.Parse(fish["date"].ToString());

                    if ((direction == 1 && catchDate < searchDate) || (direction == 2 && catchDate > searchDate))
                        WriteFishInfo(species, fish);
                }
            }
            #endregion
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

            JArray allSpecies = jsonReader.GetAllSpecies();

            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["weather"].ToString() == weather.ToString())
                        WriteFishInfo(species, fish);
                }
            }
        }


        public static void RehtaewYb()
        {
            Console.Clear();
            Weather weather = Weather.Undefined;
            Info.WriteAllWeather();

            SlowWriter.WriteLine("Which weather would you like to search for?");

        }


        public static void ByBait()
        {
            Console.Clear();
            Info.WriteAll("bait");
            SlowWriter.WriteLine("What bait would you like to search for?");

            string baitString = Console.ReadLine();
            JArray allSpecies = jsonReader.GetAllSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["bait"].ToString() == baitString)
                        WriteFishInfo(species, fish);
                }
            }
        }


        public static void ByMethod()
        {
            Console.Clear();

            Info.WriteAll("method");
            SlowWriter.WriteLine("What method would you like to search for?");

            string methodString = Console.ReadLine();
            JArray allSpecies = jsonReader.GetAllSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject fish in species["catches"])
                {
                    if (fish["method"].ToString() == methodString)
                        WriteFishInfo(species, fish);
                }
            }
        }


        private static void WriteFishInfo(JObject species, JObject fish)
        {
            SlowWriter.WriteLine(species["species"].ToString());
            DisplayMenu.DisplayFishInfo(fish);
            SlowWriter.WriteLine("\t--------------------");
        }
    }
}
