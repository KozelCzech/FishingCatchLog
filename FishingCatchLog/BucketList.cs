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
            string fish = Console.ReadLine();

            JArray bucketList = jsonReader.GetBucketList();
            bucketList.Add(new JObject
            {
                ["species"] = fish,
                ["cought"] = false
            });

            jsonReader.SaveBucketListJson(bucketList);
        }


        public static void RemoveFish()
        {
            Console.WriteLine();
            JArray bucketList = jsonReader.GetBucketList();
            WriteOutFish(bucketList);
            SlowWriter.WriteLine("Which fish would you like to remove from your bucket list?");
            string fish = Console.ReadLine();
            bucketList.Remove(bucketList.First(x => (string)x["species"] == fish));

            jsonReader.SaveBucketListJson(bucketList);
        }


        public static void DisplayBucketList(string display = "all")
        {
            Console.Clear();
            SlowWriter.WriteLine("All bucket list fish:");
           
            switch (display)
            {
                case "cought":
                    JArray coughtFish = jsonReader.GetCoughtList();
                    WriteOutFish(coughtFish);
                    break;
                case "uncought":
                    JArray uncoughtFish = jsonReader.GetUncoughtList();
                    WriteOutFish(uncoughtFish);
                    break;
                default:
                    JArray bucketList = jsonReader.GetBucketList();
                    WriteOutFish(bucketList);
                    break;
            }
        }


        public static void MarkFishCought()
        {
            Console.Clear();
            JArray uncoughtList = jsonReader.GetUncoughtList();
            WriteOutFish(uncoughtList);
            SlowWriter.WriteLine("Which fish would you like to mark as cought?");
            string fish = Console.ReadLine();

            JArray bucketList = jsonReader.GetBucketList();
            bucketList.FirstOrDefault(x => x["species"].ToString() == fish)["cought"] = true;

            jsonReader.SaveBucketListJson(bucketList);
        }
        #endregion


        #region Minor
        private static void WriteOutFish(JArray bucketList)
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
