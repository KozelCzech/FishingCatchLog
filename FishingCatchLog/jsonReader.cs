using Newtonsoft.Json.Linq;

namespace FishingCatchLog
{
    public class jsonReader
    {
        #region catchData
        static readonly string _jsonPath = "../../../Data/catchData.json";


        public static JArray GetAllFishSpecies() => JArray.Parse(File.ReadAllText(_jsonPath));


        public static JObject GetFishSpeciesByName(string name)
        {
            string json = File.ReadAllText(_jsonPath);
            return (JObject)JArray.Parse(json).FirstOrDefault(x => x["species"].ToString() == name);
        }


        public static List<string> GetAllLocations()
        {
            List<string> locations = new List<string>();
            JArray allSpecies = GetAllFishSpecies();
            foreach (JObject species in allSpecies)
            {
                foreach (JObject catches in species["catches"])
                    locations.Add(catches["location"].ToString());
            }
            return locations.Distinct().ToList();
        }


        public static void SaveCatchJson(JArray json) => File.WriteAllText(_jsonPath, json.ToString());
        #endregion

        #region butcketList
        static readonly string _bucketListPath = "../../../Data/BucketList.json";


        public static JArray GetBucketList() => JArray.Parse(File.ReadAllText(_bucketListPath));


        public static JArray GetCoughtList()
        {
            JArray bucketList = GetBucketList();
            JArray coughtList = new JArray();
            foreach (JObject fish in bucketList)
            {
                if ((bool)fish["cought"])
                    coughtList.Add(fish);
            }
            return coughtList;
        }


        public static JArray GetUncoughtList()
        {
            JArray bucketList = GetBucketList();
            JArray uncoughtList = new JArray();
            foreach (JObject fish in bucketList)
            {
                if (!(bool)fish["cought"])
                    uncoughtList.Add(fish);
            }
            return uncoughtList;
        }


        public static void SaveBucketListJson(JArray json) => File.WriteAllText(_bucketListPath, json.ToString());
        #endregion
    }
}
