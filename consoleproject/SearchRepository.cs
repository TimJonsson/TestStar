using System;

namespace consoleproject
{
    public class SearchRepository
    {
        private JsonHandler _jsonHandler;

        public SearchRepository(JsonHandler jsonHandler)
        {
            _jsonHandler = jsonHandler;
        }

        // Searching for an item name and returning item values if name found
        public Item searchItem(string itemName)
        {
            var parsedJson = _jsonHandler.loadJsonFile();
            foreach (var item in parsedJson["items"])
            {
                if (item["Name"] == itemName)
                {
                    var itemMaterial = Convert.ToString(item["Material"]);
                    var itemQuantity = Convert.ToInt32(item["Quantity"]);
                    var itemPrice = Convert.ToInt32(item["Price"]);
                    return new Item(itemName, itemMaterial, itemQuantity, itemPrice);
                }
            }
            return null;
        }
    }
}