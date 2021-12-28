using System;

namespace consoleproject
{
    public class SearchRepository
    {

        public Item searchItem(string itemName)
        {
            JsonHandler jsonHandler = new JsonHandler();
            var parsedJson = jsonHandler.loadJsonFile();
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