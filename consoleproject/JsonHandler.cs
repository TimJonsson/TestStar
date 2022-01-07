using Newtonsoft.Json;
using System.IO;

namespace consoleproject
{
    public class JsonHandler
    {
        private JsonSerializer _jsonSerializer;

        public JsonHandler(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        private string defaultFileName = "warehouse.json";

        // Deserializing a Json file specified with help of Newtonsoft.Json
        public dynamic loadJsonFile(string fileName = null)
        {
            if (fileName == null)
                fileName = defaultFileName;
            string json = File.ReadAllText(fileName);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return jsonObj;
        }

        // Updating an exisitng Json file with help of Netwonsoft.Json
        public void updateJsonFile(dynamic loadedJson, string fileName = null)
        {
            if (fileName == null)
                fileName = defaultFileName;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(loadedJson, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, output);

        }

        // Saving summary data by serializing json to a file
        public void saveSummaryToFile(SummaryData summaryData, string fileName)
        {
            using (StreamWriter file = File.CreateText(fileName))
            {
                _jsonSerializer.Serialize(file, summaryData);
            }
        }
    }
}