using System;
using System.Collections.Generic;

namespace consoleproject
{
    public class SummaryManager
    {

        private SearchRepository _searchRepository;

        private JsonHandler _jsonHandler;

        private SummaryData _summaryData;
        private List<CostSummary> costSummaries = new List<CostSummary>();

        public SummaryManager(SearchRepository searchRepository, JsonHandler jsonHandler, SummaryData summaryData)
        {
            _searchRepository = searchRepository;
            _jsonHandler = jsonHandler;
            _summaryData = summaryData;
        }

        // Adding a new cost summary to the list of cost summaries
        private void addCostSummary(CostSummary costSummary)
        {
            costSummaries.Add(costSummary);
        }

        // Printing out a cost summary
        public void showSummaryDetails(CostSummary summary)
        {
            var customerName = checkCustomerName(summary);
            System.Console.WriteLine($"ID: {summary.SummaryID}");
            System.Console.WriteLine($"Created: {summary.Created}");
            System.Console.WriteLine($"Updated: {summary.Updated}");
            System.Console.WriteLine($"Saved: {summary.SummarySavedName}");
            System.Console.WriteLine($"Customer Name: {customerName}");
            System.Console.WriteLine($"Due Date: {summary.DueDate}");
            System.Console.WriteLine($"Summary confirmed: {summary.ConfirmedDate}");

            var itemsList = summary.getItemDetails();
            var itemNumber = 1;

            foreach (var item in itemsList)
            {
                System.Console.WriteLine($" Item Number: {itemNumber} \n Item Name: {item.Name} \n Item Material: {item.Material} \n Item Quantiy: {item.Quantity} \n Item Price: {item.Price} \n Item price with VAT {item.PriceWithVat} \n");
                itemNumber++;
            }

            System.Console.WriteLine("Total cost");
            System.Console.WriteLine($"Total: {summary.getTotalSummaryCostWithoutVAT()}");
            System.Console.WriteLine($"Total with VAT: {summary.getTotalSummaryCostWithVAT()}");
        }

        public void saveSummaryToFile(CostSummary costSummary, string fileName = null)
        {
            addCostSummary(costSummary);

            // Looping through added cost summaries
            foreach (var summary in costSummaries)
            {
                // If summary exist, getting items and customer name
                if (summary == costSummary)
                {
                    var itemsList = summary.getItemDetails();
                    var customerName = checkCustomerName(summary);
                    // Looping through items and creating a new SummaryData object 
                    foreach (var item in itemsList)
                    {
                        _summaryData = new SummaryData
                        {
                            SummaryId = summary.SummaryID,
                            Created = summary.Created,
                            Updated = summary.Updated,
                            SavedAs = summary.SummarySavedName,
                            CustomerName = customerName,
                            DueDate = summary.DueDate,
                            ConfirmedDate = summary.ConfirmedDate,
                            items = itemsList,
                            TotalCost = summary.getTotalSummaryCostWithoutVAT(),
                            TotalCostWithVAT = summary.getTotalSummaryCostWithVAT()
                        };
                        // Trimming any spaces given in customerName since it's not a good practise to have a file name with spaces
                        var customerNameWithoutSpaces = customerName.Replace(" ", "");
                        string defaultFileName = $"{customerNameWithoutSpaces}_{summary.DueDate.Replace("/", "-")}_{summary.SummaryID}.json";

                        if (fileName == null)
                            fileName = defaultFileName;
                        // Saving SummaryData object to a Json file
                        _jsonHandler.saveSummaryToFile(_summaryData, fileName);
                        costSummary.SummarySavedName = fileName;
                    }
                }
                else
                    System.Console.WriteLine("Summary was not found");
            }
        }

        public CostSummary loadSummaryFromFile(string fileName)
        {

            var loadedJson = _jsonHandler.loadJsonFile(fileName);
            var summaryID = Convert.ToInt32(loadedJson["SummaryId"]);
            var summaryCreated = Convert.ToString(loadedJson["Created"]);
            CostSummary costSummary = new CostSummary(summaryID, summaryCreated);
            costSummary.Updated = loadedJson["Updated"];
            costSummary.SummarySavedName = loadedJson["SavedAs"];
            costSummary.addCustomerName(Convert.ToString(loadedJson["CustomerName"]));
            costSummary.DueDate = loadedJson["DueDate"];
            costSummary.ConfirmedDate = loadedJson["ConfirmedDate"];
            addCostSummary(costSummary);

            foreach (var item in loadedJson["items"])
            {
                costSummary.addItems(Convert.ToString(item["Name"]), Convert.ToString(item["Material"]), Convert.ToInt32(item["Quantity"]), Convert.ToInt32(item["Price"]));
            }

            showSummaryDetails(costSummary);
            return costSummary;
        }

        public void confirmSummary(CostSummary costSummary)
        {
            var itemDetails = costSummary.getItemDetails();
            if (itemDetails.Count == 0)
            {
                System.Console.WriteLine("Summary can't be confirmed since no items are added to the costSummary");
                return;
            }
            if (costSummary.ConfirmedDate != "")
            {
                System.Console.WriteLine("Summary is already confirmed");
                return;
            }
            foreach (var item in itemDetails)
            {
                string itemName = item.Name;
                int requestedItemQuantity = item.Quantity;
                var currentItem = _searchRepository.searchItem(itemName);
                // Checking if quantity requested actually exists in warehouse.json
                if (currentItem.Quantity < requestedItemQuantity)
                {
                    System.Console.WriteLine($"Requested item quantity \"{requestedItemQuantity}\" doesn't exist. Item quantity for item {currentItem.Name} is {currentItem.Quantity}");
                    return;
                }
                var newQuantity = currentItem.Quantity - requestedItemQuantity;
                updateItemQuantity(itemName, newQuantity);
                costSummary.ConfirmedDate = Convert.ToString(DateTime.Now);
            }
        }

        // Updating item quantity after cost summary is confirmed. 
        public void updateItemQuantity(string name, int quantity)
        {
            var loadedJson = _jsonHandler.loadJsonFile();
            foreach (var items in loadedJson["items"])
            {
                var item = Convert.ToString(items["Name"]);
                if (item == name)
                    items["Quantity"] = quantity;
            }
            _jsonHandler.updateJsonFile(loadedJson);
        }

        // Checking if customerName is assigned, if not returning an empty string
        private string checkCustomerName(CostSummary costSummary)
        {
            if (costSummary.customer == null)
                return "";
            else
                return costSummary.customer.Name;
        }
    }
}