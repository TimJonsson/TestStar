using Newtonsoft.Json;
using System.IO;

namespace consoleproject
{
    public class JsonHandler
    {

        private string defaultFileName = "warehouse.json";

        public dynamic loadJsonFile(string fileName = null)
        {
            if (fileName == null)
                fileName = defaultFileName;
            string json = File.ReadAllText(fileName);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return jsonObj;
        }

        public void updateJsonFile(dynamic loadedJson, string fileName = null)
        {
            if (fileName == null)
                fileName = defaultFileName;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(loadedJson, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, output);

        }

        public void saveSummaryToFile(SummaryData summaryData, string fileName)
        {
            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, summaryData);
            }
        }
    }
}