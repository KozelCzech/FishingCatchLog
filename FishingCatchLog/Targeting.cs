using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FishingCatchLog
{
    public class Targeting
    {
        public static void AddTarget(string target = "", string name = "", string date = "")
        {
            Console.Clear();
            
            #region WriteOutTargets
            List<string> targets = jsonReader.GetAllTargets();
            List<string> speciesList = new List<string>();
            JArray allSpecies = jsonReader.GetAllSpecies();
            JArray bucketList = jsonReader.GetBucketList();

            foreach (JObject species in allSpecies)
            {
                targets.Add(species["species"].ToString());
                speciesList.Add(species["species"].ToString());
            }
            foreach (JObject species in bucketList)
            {
                targets.Add(species["species"].ToString());
                speciesList.Add(species["species"].ToString());
            }

            speciesList = speciesList.Distinct().ToList();
            targets = targets.Distinct().ToList();
            for (int i = 0; i < targets.Count; i++)
            {
                SlowWriter.Write($"{targets[i]}");
                if (i != targets.Count - 1)
                    SlowWriter.Write(", ");

                if(i % 10 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();
            #endregion

            #region Input
            SlowWriter.WriteLine("What would you like to target?");
            if (target == "")
            {
                target = Console.ReadLine();

                if (!targets.Any(x => x == target)) //check if chosen target is valid
                    AddTarget();
            }
            else
                SlowWriter.WriteLine(target);

            SlowWriter.WriteLine("\nWhat would you like to name this target?");
            if (name == "")
                name = Console.ReadLine();
            else
                SlowWriter.WriteLine(name);

            SlowWriter.WriteLine("\nUntil when would you like this target to be active? (dd/mm/yyyy)");
            if (date == "")
                date = Console.ReadLine();
            else
                SlowWriter.WriteLine(date);

            SlowWriter.WriteLine("\nWhat amount would you like to aim for");
            bool isAmountValid = int.TryParse(Console.ReadLine(), out int amount);
            if (!isAmountValid)
                AddTarget(target, name, date);
            #endregion

            #region Save
            string targetType = "";
            if (speciesList.Contains(target))
                targetType = "species";
            else
                targetType = "feature";

            JObject newTarget = new JObject
            {
                ["targetType"] = targetType,
                ["target"] = target,
                ["name"] = name,
                ["amount"] = amount,
                ["startDate"] = DateTime.Now.ToString("dd/MM/yyyy"),
                ["endDate"] = date,
                ["complete"] = false
            };

            JArray allTargets = jsonReader.GetTargetList();
            allTargets.Add(newTarget);
            jsonReader.SaveTargetList(allTargets);
            #endregion

            SlowWriter.WriteLine("Target added successfully");
            Console.ReadKey();
        }


        public static void RemoveTarget()
        {
            Console.Clear();
            List<string> targetNames = jsonReader.GetAllTargetNames();
            for (int i = 0; i < targetNames.Count; i++)
            {
                SlowWriter.Write($"{targetNames[i]}");
                if (i != targetNames.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();

            SlowWriter.WriteLine("Which target would you like to remove?");
            string target = Console.ReadLine();
            if (!targetNames.Any(x => x == target))
                RemoveTarget();

            JArray allTargets = jsonReader.GetTargetList();
            allTargets.Remove(allTargets.First(x => x["name"].ToString() == target));
            jsonReader.SaveTargetList(allTargets);
            SlowWriter.WriteLine("Target removed successfully");
            Console.ReadKey();
        }


        public static void DisplayTargets()
        {
            Console.Clear();
            JArray allTargets = jsonReader.GetTargetList();
            foreach (JObject target in allTargets)
            {
                int currentAmount = 0;
                JArray allSpecies = jsonReader.GetAllSpecies();

                if (target["targetType"].ToString() == "species")
                {
                    try
                    {
                        JObject species = (JObject)allSpecies.First(x => x["species"].ToString() == target["target"].ToString());
                        foreach (JObject fish in species["catches"])
                        {
                            DateTime catchDate = DateTime.Parse(fish["date"].ToString());
                            DateTime startDate = DateTime.Parse(target["startDate"].ToString());
                            DateTime endDate = DateTime.Parse(target["endDate"].ToString());
                            if (catchDate >= startDate && catchDate <= endDate)
                                currentAmount++;
                        }
                    }
                    catch(Exception) { }
                }
                else if (target["targetType"].ToString() == "feature")
                {
                    foreach (JObject species in allSpecies)
                    {
                        foreach (JObject fish in species["catches"])
                        {
                            DateTime catchDate = DateTime.Parse(fish["date"].ToString());
                            DateTime startDate = DateTime.Parse(target["startDate"].ToString());
                            DateTime endDate = DateTime.Parse(target["endDate"].ToString());
                            if (catchDate >= startDate && catchDate <= endDate)
                                currentAmount += (int)Math.Round(Convert.ToDouble(fish[target["target"].ToString()]));
                        }
                    }
                }

                #region WriteOutTarget
                SlowWriter.WriteLine($"{target["name"]}");
                SlowWriter.WriteLine($"  Target: {target["target"]}");
                SlowWriter.WriteLine($"  Start date: {target["startDate"]} | End date: {target["endDate"]}");
                SlowWriter.Write($"  Progress: {currentAmount}/{target["amount"]}");
                if ((bool)target["complete"] == true || currentAmount >= (int)target["amount"])
                {
                    target["complete"] = true;
                    SlowWriter.Write($" - Target complete!");
                }
                Console.WriteLine();
                SlowWriter.WriteLine($"--------------------");
                #endregion
            }
            jsonReader.SaveTargetList(allTargets);
            Console.ReadKey();
        }


        public static void ResetTarget()
        {
            Console.Clear();
            #region WriteOutTargets
            List<string> targetNames = jsonReader.GetAllTargetNames();
            for (int i = 0; i < targetNames.Count; i++)
            {
                SlowWriter.Write($"{targetNames[i]}");
                if (i != targetNames.Count - 1)
                    SlowWriter.Write(", ");
            }
            Console.WriteLine();
            #endregion

            #region Inputs
            SlowWriter.WriteLine("Which target would you like to reset?");
            string name = Console.ReadLine();
            if (!targetNames.Any(x => x == name))
                ResetTarget();

            JArray allTargets = jsonReader.GetTargetList();
            JObject target = (JObject)allTargets.First(x => x["name"].ToString() == name);

            SlowWriter.WriteLine($"{target["name"]}");
            SlowWriter.WriteLine($"  Target: {target["target"]}");
            SlowWriter.WriteLine($"  Start date: {target["startDate"]} | End date: {target["endDate"]}");
            SlowWriter.WriteLine($"  Target amount: {target["amount"]}"); 

            SlowWriter.WriteLine("\nWhat will the new end date be? (dd/MM/yyyy)");
            string date = Console.ReadLine();
            #endregion

            #region Save
            allTargets.First(x => x["name"].ToString() == name)["endDate"] = date;
            allTargets.First(x => x["name"].ToString() == name)["startDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            jsonReader.SaveTargetList(allTargets);
            SlowWriter.WriteLine("Target reset successfully");
            Console.ReadKey();
            #endregion
        }
    }
}
