using Newtonsoft.Json.Linq;


namespace FishingCatchLog
{
    public class DisplayMenu
    {
        #region Major
        public static int MainMenu()
        {
            Console.Clear();

            SlowWriter.WriteLine("Catch Logging Software");

            SlowWriter.WriteLine($"Different species cought: {jsonReader.GetAllSpecies().Count}");
            SlowWriter.WriteLine($"Total fish cought: {Info.CoughtFish()} | Total fish kept: {Info.KeptFish()}");
            SlowWriter.WriteLine($"Favorite location: {Info.FavoriteLocation()}");

            SlowWriter.WriteLine("\nWhat do you wish to do:");
            SlowWriter.WriteLine("\t1. Log a new catch");
            SlowWriter.WriteLine("\t2. Search catch log");
            SlowWriter.WriteLine("\t3. Display catch log");
            SlowWriter.WriteLine("\t4. Open bucket list menu");
            SlowWriter.WriteLine("\t5. Targets menu");
            SlowWriter.WriteLine("\t0. Exit");

            bool isValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int choice);
            if (!isValid || choice < 0 || choice > 5)
            {
                SlowWriter.WriteLine("Invalid input. Please enter a valid number!");
                Console.ReadKey();
                return MainMenu();
            }
            Console.WriteLine();
            return choice;
        }

        #region Catch menus
        public static void LogCatchMenu(string? species = "", double weight = 0, double length = 0, string location = "", string date = "",Weather weather = Weather.Undefined, string bait = "", string method = "", int keepChoice = 0)
        {
            Console.Clear();

            SlowWriter.WriteLine("Log a new catch");

            #region Species
            Info.WriteAllSpecies();

            SlowWriter.WriteLine("\nWhat is the species of the fish?");
            if (species == string.Empty)
                species = Console.ReadLine();
            else
                SlowWriter.WriteLine(species);
            #endregion

            #region Weight
            SlowWriter.WriteLine("\nWhat is the weight of the fish?");
            if (weight == 0)
            {
                bool isWeightValid = double.TryParse(Console.ReadLine(), out weight);
                if (!isWeightValid)
                    LogCatchMenu(species: species);
            }
            else
                SlowWriter.WriteLine(weight.ToString());
            #endregion

            #region Length
            SlowWriter.WriteLine("\nWhat is the length of the fish?");
            if (length == 0)
            {
                bool isLengthValid = double.TryParse(Console.ReadLine(), out length);
                if (!isLengthValid)
                    LogCatchMenu(species: species, weight: weight);
            }
            else
                SlowWriter.WriteLine(length.ToString());
            #endregion

            #region Location
            Info.WriteAllLocations();

            SlowWriter.WriteLine($"Where did you catch the fish?");

            if (location == string.Empty)
                location = Console.ReadLine();
            else
                SlowWriter.WriteLine(location);
            #endregion

            #region Date
            SlowWriter.WriteLine("\nWhen did you catch the fish? (dd/mm/yyyy)");

            if (date == string.Empty)
                date = Console.ReadLine();
            else
                SlowWriter.WriteLine(date);
            //just day month year
            #endregion

            #region Weather
            Info.WriteAllWeather();
            SlowWriter.WriteLine("What was the weather like at the time of the catch? (please select by number)");

            if (weather == Weather.Undefined)
            {
                bool isWeatherValid = int.TryParse(Console.ReadLine(), out int weatherChoice);
                if (!isWeatherValid)
                    LogCatchMenu(species: species, weight: weight, length: length);
                else
                    weather = (Weather)weatherChoice;
            }
            else
            {
                int value = (int)weather;
                SlowWriter.WriteLine(value.ToString());
            }

            #endregion

            #region Bait & Method
            SlowWriter.WriteLine("\nWhat bait was used?");
            if (bait == string.Empty)
                bait = Console.ReadLine();
            else
                SlowWriter.WriteLine(bait);

            SlowWriter.WriteLine("\nWhat method was used?");
            if (method == string.Empty)
                method = Console.ReadLine();
            else
                SlowWriter.WriteLine(method);
            #endregion

            #region Keep
            SlowWriter.WriteLine("1. Yes");
            SlowWriter.WriteLine("2. No");
            SlowWriter.WriteLine("Was the fish kept?");
            bool isKeptValid = int.TryParse(Console.ReadLine(), out keepChoice);
            if (!isKeptValid || keepChoice < 1 || keepChoice > 2)
                LogCatchMenu(species: species, weight: weight, length: length, weather: weather, bait: bait, method: method);

            string keptString = "";
            bool isKept = false;
            if (keepChoice == 1)
            {
                keptString = "Yes";
                isKept = true;
            }
            else
            {
                keptString = "No";
                isKept = false;
            }
            #endregion

            #region Confirmation & Save
            SlowWriter.WriteLine("\nConfirm catch information:");
            SlowWriter.WriteLine($"Species: {species} - Weight: {weight}, Length: {length} - Location: {location}, Date: {date}, Weather: {weather} - Kept: {keptString} - Bait: {bait}, Method: {method}");
            SlowWriter.WriteLine("Press any key to save...");
            Console.ReadKey();

            //save the catch to catchData here
            JArray allCatches = jsonReader.GetAllSpecies();
            JObject newCatch = new JObject
            {
                ["weather"] = weather.ToString(),
                ["date"] = date,
                ["location"] = location,
                ["weight"] = weight,
                ["length"] = length,
                ["bait"] = bait,
                ["method"] = method,
                ["kept"] = isKept
            };

            JArray speciesObject = (JArray)allCatches.FirstOrDefault(x => x["species"].ToString().ToLower() == species.ToLower())["catches"];
            if (speciesObject != null)
                speciesObject.Add(newCatch);

            SlowWriter.WriteLine("\nLog has been saved...");
            jsonReader.SaveCatchJson(allCatches);
            Console.ReadKey();
            #endregion
        }


        public static void DisplayAllCatchesMenu()
        {
            Console.Clear();
            JArray allSpecies = jsonReader.GetAllSpecies();
            foreach (JObject species in allSpecies)
            {
                SlowWriter.WriteLine($"{species["species"]}:");

                foreach (JObject fish in species["catches"])
                {
                    DisplayFishInfo(fish);
                    SlowWriter.WriteLine("\t--------------------");
                }
            }
            Console.ReadKey();
        }


        public static void SearchCatchesMenu()
        {
            #region Options
            Console.Clear();
            SlowWriter.WriteLine("Search by:");
            SlowWriter.WriteLine("1. Species");
            SlowWriter.WriteLine("2. Location");
            SlowWriter.WriteLine("3. Date");
            SlowWriter.WriteLine("4. Weather");
            SlowWriter.WriteLine("5. Bait");
            SlowWriter.WriteLine("6. Method");
            SlowWriter.WriteLine("0. Back");
            #endregion

            bool isChoiceValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int choice);
            if (!isChoiceValid || choice < 0 || choice > 6)
                SearchCatchesMenu();

            switch (choice)
            {
                case 1:
                    Search.BySpecies();
                    break;
                case 2:
                    Search.ByLocation();
                    break;
                case 3:
                    Search.ByDate();
                    break;
                case 4:
                    Search.ByWeather();
                    break;
                case 5:
                    Search.ByBait();
                    break;
                case 6:
                    Search.ByMethod();
                    break;
                case 0:
                    MainMenu();
                    break;
                default:
                    SearchCatchesMenu();
                    break;
            }
            Console.ReadKey();
        }
        #endregion

        #region bucketList
        public static void DisplayBucketListMenu()
        {
            Console.Clear();
            SlowWriter.WriteLine("Bucket list:");

            SlowWriter.WriteLine("\t1. Add a fish");
            SlowWriter.WriteLine("\t2. Remove a fish");
            SlowWriter.WriteLine("\t3. Display bucket list");
            SlowWriter.WriteLine("\t4. Mark fish cought");
            SlowWriter.WriteLine("\t0. Back");

            bool isChoiceValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int choice);
            if (isChoiceValid || choice < 0 || choice > 4)
            {
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        BucketList.AddFish();
                        break;
                    case 2:
                        BucketList.RemoveFish();
                        break;
                    case 3:
                        Console.Clear();
                        #region GetInput
                        SlowWriter.WriteLine("1. Cought");
                        SlowWriter.WriteLine("2. Uncought");
                        SlowWriter.WriteLine("3. All");
                        SlowWriter.WriteLine("0. Back");
                        SlowWriter.WriteLine("Do you wish to display cought, uncought or all fish?");
                        bool isDisplayChoiceValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int displayChoice);
                        string chosenMethod = "all";
                        #endregion
                        #region UseInput
                        if (isDisplayChoiceValid || choice < 0 || choice > 3)
                        {
                            switch (displayChoice)
                            {
                                case 1:
                                    chosenMethod = "cought";
                                    break;
                                case 2:
                                    chosenMethod = "uncought";
                                    break;
                                case 3:
                                    chosenMethod = "all";
                                    break;
                                case 0:
                                    DisplayBucketListMenu();
                                    break;
                            }
                        }
                        BucketList.DisplayBucketList(chosenMethod);
                        #endregion
                        break;
                    case 4:
                        BucketList.MarkFishCought();
                        break;
                    default:
                        DisplayBucketListMenu();
                        break;
                }
            }
            else
                DisplayBucketListMenu();

            Console.ReadKey();
            if (choice != 0)
                DisplayBucketListMenu();
        }
        #endregion

        #region Targeting
        public static void TargetingMenu()
        {
            Console.Clear();
            SlowWriter.WriteLine("Target\n");

            SlowWriter.WriteLine("1. Add target");
            SlowWriter.WriteLine("2. Remove target");
            SlowWriter.WriteLine("3. Display targets");
            SlowWriter.WriteLine("4. Reset target");
            SlowWriter.WriteLine("0. Back");

            bool isChoiceValid = int.TryParse(Console.ReadKey().KeyChar.ToString(), out int choice);
            if (isChoiceValid || choice >= 0 && choice < 5)
            {
                switch (choice)
                {
                    case 0:
                        MainMenu();
                        break;
                    case 1:
                        Targeting.AddTarget();
                        break;
                    case 2:
                        Targeting.RemoveTarget();
                        break;
                    case 3:
                        Targeting.DisplayTargets();
                        break;
                    case 4:
                        Targeting.ResetTarget();
                        break;
                    default:
                        TargetingMenu();
                        break;
                }
            }
            else
                TargetingMenu();


        }
        #endregion
        #endregion

        #region Minor
        public static void DisplayFishInfo(JObject fish)
        {
            SlowWriter.WriteLine($"\tWeight: {fish["weight"]} | Length: {fish["length"]}");
            SlowWriter.WriteLine($"\tLocation: {fish["location"]} | Date: {fish["date"]} | Weather: {fish["weather"]}");
            if ((bool)fish["kept"])
                SlowWriter.WriteLine("\tKept: Yes");
            else
                SlowWriter.WriteLine("\tKept: No");
        }
        #endregion
    }
}
