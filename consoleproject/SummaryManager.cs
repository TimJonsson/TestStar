using System; 
using System.Collections.Generic;

namespace consoleproject
{
    public class SummaryManager {

        private List<CostSummary> costSummaries = new List<CostSummary>();

        private void addCostSummary(CostSummary costSummary) {
            costSummaries.Add(costSummary);
        }

        public void showSummaryDetails(CostSummary summary) {
                System.Console.WriteLine($"Summary ID: {summary.SummaryID}");
                System.Console.WriteLine($"Summary Created: {summary.Created}");
                System.Console.WriteLine($"Summar Updated: {summary.Updated}");
                System.Console.WriteLine($"Summary Saved: {summary.SummarySavedName}");
                System.Console.WriteLine($"Summary Due Date: {summary.DueDate}");
                if (summary.customer == null || summary.customer.Name =="")
                    System.Console.WriteLine($"Customer Name: ");
                else
                    System.Console.WriteLine($"Customer Name: {summary.customer.Name}");
                System.Console.WriteLine($"Summary confirmed: {summary.ConfirmedDate}");

                var itemsList = summary.getItemDetails();
                foreach (var item in itemsList) {
                    System.Console.WriteLine($"Item Name: {item.Name} Item Material: {item.Material} Item Quantiy: {item.Quantity} Item Price: {item.Price} Item price with VAT {item.PriceWithVat}");
                }
        }

        public void saveSummaryToFile(CostSummary costSummary, string fileName = null) {
            addCostSummary(costSummary);
            SummaryData summaryData = new SummaryData();

            foreach (var summary in costSummaries) {
                if (summary == costSummary) {
                    var itemsList = summary.getItemDetails();
                    var customerName = checkCustomerName(summary);
                    // Second foreach not needed? 
                    foreach (var item in itemsList) {
                        summaryData = new SummaryData {
                        SummaryId = summary.SummaryID,
                        Created = summary.Created,
                        Updated = summary.Updated,
                        SavedAs = summary.SummarySavedName,
                        CustomerName = customerName,
                        DueDate = summary.DueDate,
                        ConfirmedDate = summary.ConfirmedDate,
                        items = itemsList
                        };
                        // System.Console.WriteLine($"Item Name: {item.Name} Item Material: {item.Material} Item Quantiy: {item.Quantity} Item Price: {item.Price} Item price with VAT {item.PriceWithVat}");
                        string defaultFileName = $"{customerName}_{summary.DueDate.Replace("/", "-")}_{summary.SummaryID}.json";

                        if (fileName == null)
                            fileName = defaultFileName;
                        // serialize JSON directly to a file
                        JsonHandler jsonHandler = new JsonHandler();
                        jsonHandler.saveSummaryToFile(summaryData, fileName);
                        costSummary.SummarySavedName = fileName;
                    }
                }
                else
                    System.Console.WriteLine("Summary was not found");
            }
        }

        public CostSummary loadSummaryFromFile(string fileName) {
            JsonHandler jsonHandler = new JsonHandler();
            var loadedJson = jsonHandler.loadJsonFile(fileName);
            var summaryID = Convert.ToInt32(loadedJson["SummaryId"]);
            var summaryCreated = Convert.ToString(loadedJson["Created"]);
            CostSummary costSummary = new CostSummary(summaryID, summaryCreated);
            costSummary.Updated = loadedJson["Updated"];
            costSummary.SummarySavedName = loadedJson["SavedAs"];
            // See if we can do this string conversion somewhere else
            costSummary.addCustomerName(Convert.ToString(loadedJson["CustomerName"]));
            costSummary.DueDate = loadedJson["DueDate"];
            costSummary.ConfirmedDate = loadedJson["ConfirmedDate"];
            addCostSummary(costSummary);

            // Figure out why to add new Item into CostSummary
            foreach (var item in loadedJson["items"]) {
                // Create a method that's converts from JSONlinq to right types. Also use this one in searchreposititory
                costSummary.addItems(Convert.ToString(item["Name"]), Convert.ToString(item["Material"]), Convert.ToInt32(item["Quantity"]), Convert.ToInt32(item["Price"]));
            }

            showSummaryDetails(costSummary);
            return costSummary;
        }

        public void confirmSummary(CostSummary costSummary) {
            var itemDetails = costSummary.getItemDetails();
            if (itemDetails.Count == 0) {
                System.Console.WriteLine("Summary can't be confirmed since no items are added to the costSummary");
                return; 
            }
            if (costSummary.isSummaryOrderConfirmed == true) {
                System.Console.WriteLine("Summary is already confirmed");
                return;
            }
            foreach (var item in itemDetails) {
                string itemName = item.Name;
                int requestedItemQuantity = item.Quantity;
                SearchRepository searchRepository = new SearchRepository();
                var currentItem = searchRepository.searchItem(itemName);
                if (currentItem.Quantity < requestedItemQuantity) {
                    System.Console.WriteLine($"Requested item quantity \"{requestedItemQuantity}\" doesn't exist. Item quantity for item {currentItem.Name} is {currentItem.Quantity}");
                    return;
                }
                var newQuantity = currentItem.Quantity - requestedItemQuantity;
                updateItemQuantity(itemName, newQuantity);
                costSummary.isSummaryOrderConfirmed = true;
                costSummary.ConfirmedDate = Convert.ToString(DateTime.Now);
            }
        }

        public void updateItemQuantity(string name, int quantity) {
            JsonHandler jsonHandler = new JsonHandler();
            var loadedJson = jsonHandler.loadJsonFile(); 
            foreach(var items in loadedJson["items"]) {
                var item = Convert.ToString(items["Name"]);
                if (item == name)
                    items["Quantity"] = quantity; 
            }
            jsonHandler.updateJsonFile(loadedJson);
        }

        private string checkCustomerName(CostSummary costSummary) {
            if (costSummary.customer == null)
                return "";
            else
                return costSummary.customer.Name;
        }
    }
}