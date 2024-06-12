using Newtonsoft.Json.Linq;

namespace FishingCatchLog
{
    public class BucketList
    {
        #region Major
        public static void AddFish()
        {
            Console.WriteLine();
            SlowWriter.WriteLine("What fish would you like to add to your bucket list?");
            string fish = Console.ReadLine() ?? "";

            JArray bucketList = jsonReader.GetBucketList();
            bucketList.Add(new JObject
            {
                ["species"] = fish,
                ["cought"] = false
            });

            jsonReader.SaveBucketListJson(bucketList);
            SlowWriter.WriteLine($"{fish} added to bucket list");
        }


        public static void RemoveFish()
        {
            Console.WriteLine();
            JArray bucketList = jsonReader.GetBucketList();
            WriteOutFishes(bucketList);
            SlowWriter.WriteLine("Which fish would you like to remove from your bucket list?");
            string fish = Console.ReadLine() ?? "";
            bucketList.Remove(bucketList.First(x => x["species"].ToString() == fish));

            jsonReader.SaveBucketListJson(bucketList);
            SlowWriter.WriteLine($"{fish} removed from bucket list");
        }


        public static void DisplayBucketList(string display = "all")
        {
            Console.Clear();
            SlowWriter.WriteLine("All bucket list fish:");
           
            switch (display)
            {
                case "cought":
                    JArray coughtFish = jsonReader.GetCoughtList();
                    WriteOutFishes(coughtFish);
                    break;
                case "uncought":
                    JArray uncoughtFish = jsonReader.GetUncoughtList();
                    WriteOutFishes(uncoughtFish);
                    break;
                default:
                    JArray bucketList = jsonReader.GetBucketList();
                    WriteOutFishes(bucketList);
                    break;
            }
        }


        public static void MarkFishCought()
        {
            Console.Clear();
            JArray uncoughtList = jsonReader.GetUncoughtList();
            WriteOutFishes(uncoughtList);
            SlowWriter.WriteLine("Which fish would you like to mark as cought?");
            string fish = Console.ReadLine() ?? "";

            JArray bucketList = jsonReader.GetBucketList();
            bucketList.First(x => x["species"].ToString() == fish)["cought"] = true;
            
            if (fish != "")
            jsonReader.SaveBucketListJson(bucketList);
        }
        #endregion

    
        #region Minor
        private static void WriteOutFishes(JArray bucketList)
        {
            foreach (JObject fish in bucketList)
            {
                SlowWriter.Write($"{fish["species"]}");

                if ((bool)fish["cought"])
                    SlowWriter.Write(" [X]");
                else
                    SlowWriter.Write(" [ ]");
                Console.WriteLine();
            }
        }
        #endregion
    }
}
