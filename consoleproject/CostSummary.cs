using System;
using System.Collections.Generic;

namespace consoleproject
{
    public class CostSummary
    {

        // Creating variables that are instansiated in constructor 
        private JsonHandler _jsonHandler;
        private SearchRepository _searchRepository;

        // Giving class variables value of an empty string to print out no value in case value was not given when showing summary details
        private string _updated = "";
        private string _dueDate = "";

        private string _summarySavedName = "";

        private string _confirmedDate = "";
        private List<Item> items = new List<Item>();

        // Making readonly to only to be assigned once. 
        private readonly int _summaryID;
        public int SummaryID
        {
            get => _summaryID;
        }

        // Making readonly to only to be assigned once. 
        private readonly string _created;
        public string Created
        {
            get => _created;
        }
        public string Updated
        {
            get => _updated;
            set => _updated = value;
        }
        public string DueDate
        {
            get => _dueDate;
            set => _dueDate = value;
        }

        public string SummarySavedName
        {
            get => _summarySavedName;
            set => _summarySavedName = value;
        }

        public string ConfirmedDate
        {
            get => _confirmedDate;
            set => _confirmedDate = value;
        }

        public Customer customer { get; set; }

        // Constructor when Cost summary is instansiated in program
        public CostSummary(SearchRepository searchRepository, JsonHandler jsonHandler)
        {
            _searchRepository = searchRepository;
            _jsonHandler = jsonHandler;
            _created = Convert.ToString(DateTime.Now);
            _summaryID = getSummaryID();
        }

        // Constructor when Cost summary is instansiated by loading summary from file
        public CostSummary(int summaryID, string created)
        {
            _summaryID = summaryID;
            _created = created;
        }

        // Adding item to cost summary
        public void addItem(string item, int quantity)
        {
            var currentItem = _searchRepository.searchItem(item);
            if (currentItem == null)
            {
                System.Console.WriteLine($"Item \"{item}\" was not found");
                return;
            }
            // Setting item  quantity to specified searched quantity since currentItem is having stock amount as quantiy
            currentItem.Quantity = quantity;
            items.Add(currentItem);
        }

        // Adding items from a loaded cost summary
        public void addItems(string name, string material, int quantity, int price)
        {
            items.Add(new Item(name, material, quantity, price));
        }

        public void addCustomerName(string name)
        {
            customer = new Customer(name);
        }

        // Giving the update variable a value of current time when cost summary is updated
        public void updateSummary()
        {
            Updated = Convert.ToString(DateTime.Now);
        }

        public void addDueDate(DateTime dueDate)
        {
            this.DueDate = removeHoursFromDate(dueDate);
        }

        // Removing time from a DateTime object and making it a string
        private string removeHoursFromDate(DateTime date)
        {
            return date.ToShortDateString();
        }

        // Returning items added to a cost summary
        public List<Item> getItemDetails()
        {
            return items;
        }
        public void updateVatAmountOnItem(String itemnName, double vatPercentage)
        {
            if (vatPercentage < 0 || vatPercentage > 1)
            {
                System.Console.WriteLine("Please enter VAT percentage as a decimal number between 0 and 1");
                return;
            }
            foreach (var currentItem in items)
            {
                if (itemnName == currentItem.Name)
                {
                    currentItem.VatPercentage = vatPercentage;
                    updateSummary();
                }
            }
        }

        // Getting summary ID from a Json file
        private int getSummaryID()
        {
            var loadedJson = _jsonHandler.loadJsonFile("SummaryID.json");
            var currentSummaryID = loadedJson["SummaryID"] += 1;
            updateIDCounter(loadedJson);
            return currentSummaryID;
        }

        // Updating Summmar ID to the Json file
        private void updateIDCounter(dynamic loadedJson)
        {
            _jsonHandler.updateJsonFile(loadedJson, "SummaryID.json");
        }

        public double getTotalSummaryCostWithoutVAT()
        {
            double totalCost = 0;

            foreach (var item in items)
            {
                totalCost += item.Price * item.Quantity;
            }

            return totalCost;
        }

        public double getTotalSummaryCostWithVAT()
        {
            double totalCost = 0;

            foreach (var item in items)
            {
                totalCost += item.PriceWithVat * item.Quantity;
            }

            return totalCost;
        }
    }
}