using System;
using System.Collections.Generic;

namespace consoleproject
{
    public class CostSummary
    {

        private JsonHandler _jsonHandler;
        private SearchRepository _searchRepository;
        private string _updated = "";
        private string _dueDate = "";

        private string _confirmedDate = "";
        private List<Item> items = new List<Item>();

        private readonly int _summaryID;
        public int SummaryID
        {
            get => _summaryID;
        }

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

        private string _summarySavedName = "";

        public string SummarySavedName
        {
            get => _summarySavedName;
            set => _summarySavedName = value;
        }

        public Customer customer { get; set; }

        public bool isSummaryOrderConfirmed { get; set; }

        public string ConfirmedDate
        {
            get => _confirmedDate;
            set => _confirmedDate = value;
        }

        public CostSummary(SearchRepository searchRepository, JsonHandler jsonHandler)
        {
            _searchRepository = searchRepository;
            _jsonHandler = jsonHandler;
            _created = Convert.ToString(DateTime.Now);
            _summaryID = getSummaryID();
        }

        public CostSummary(int summaryID, string created)
        {
            _summaryID = summaryID;
            _created = created;
        }
        public void addItem(string item, int quantity)
        {
            var currentItem = _searchRepository.searchItem(item);
            if (currentItem == null)
            {
                System.Console.WriteLine($"Item \"{item}\" was not found");
                return;
            }
            currentItem.Quantity = quantity;
            items.Add(currentItem);
            System.Console.WriteLine();
        }

        public void addItems(string name, string material, int quantity, int price)
        {
            items.Add(new Item(name, material, quantity, price));
        }

        public void addCustomerName(string name)
        {
            customer = new Customer(name);
        }

        public void updateSummary()
        {
            Updated = Convert.ToString(DateTime.Now);
        }

        public void addDueDate(DateTime dueDate)
        {
            this.DueDate = removeHoursFromDueDate(dueDate);
        }

        private string removeHoursFromDueDate(DateTime dueDate)
        {
            return dueDate.ToShortDateString();
        }

        public List<Item> getItemDetails()
        {
            return items;

        }
        public void updateVatAmountOnItem(String item, double vatPercentage)
        {
            foreach (var currentItem in items)
            {
                if (item == currentItem.Name)
                {
                    currentItem.VatPercentage = vatPercentage;
                    updateSummary();
                }
            }
        }

        public int getSummaryID()
        {
            var loadedJson = _jsonHandler.loadJsonFile("SummaryID.json");
            System.Console.WriteLine(loadedJson);
            var currentSummaryID = loadedJson["SummaryID"] += 1;
            updateIDCounter(loadedJson);
            return currentSummaryID;
        }

        private void updateIDCounter(dynamic loadedJson)
        {
            _jsonHandler.updateJsonFile(loadedJson, "SummaryID.json");
        }
    }
}