using Newtonsoft.Json;
using System;

namespace consoleproject
{
    class Program
    {
        static void Main(string[] args)
        {

            // Down below I will test all the functional requirements
            // Will later be converted to Unit tests 

            // Creating instanses of each used class 
            JsonSerializer jsonSerializer = new JsonSerializer();
            JsonHandler jsonHandler = new JsonHandler(jsonSerializer);
            SearchRepository searchRepository = new SearchRepository(jsonHandler);
            SummaryData summaryData = new SummaryData();
            CostSummary costSummary = new CostSummary(searchRepository, jsonHandler);
            SummaryManager summaryManager = new SummaryManager(searchRepository, jsonHandler, summaryData);

            //Functional requirements below

            // Create and work with a Cost summary 
            costSummary.addItem("Mdf", 99);
            costSummary.addItem("Teak", 50);
            costSummary.addCustomerName("Tim Jonsson");
            // Fix Dudate
            var dueDate1 = new DateTime(2022, 03, 01);
            costSummary.addDueDate(dueDate1);
            costSummary.updateVatAmountOnItem("Mdf", 0.3);

            //Show the current summary on screen
            summaryManager.showSummaryDetails(costSummary);

            //Save the summary to a file
            summaryManager.saveSummaryToFile(costSummary);

            //load a previously saved summary  and continue working on it
            summaryManager.loadSummaryFromFile("TimJonsson_03-01-2022_1.json");

            //Confirm summary as order in progress
            summaryManager.confirmSummary(costSummary);


            // Non functional requirements below

            // Add an item that doesn't exist
            // CostSummary costSummary1 = new CostSummary(searchRepository,jsonHandler);
            // costSummary1.addItem("MDG", 50);

            // Add item which quantity is not in stock. 
            // CostSummary costSummary2 = new CostSummary(searchRepository,jsonHandler);
            // costSummary2.addItem("Mdf", 100000);
            // summaryManager.confirmSummary(costSummary2);

            // Confirm CostSummary without any Items. 
            // CostSummary costSummary3 = new CostSummary(searchRepository, jsonHandler);
            // summaryManager.confirmSummary(costSummary3);

            //Omit any missing data 
            // CostSummary costSummary4 = new CostSummary(searchRepository, jsonHandler);
            // costSummary4.addItem("Mdf", 5);
            // summaryManager.showSummaryDetails(costSummary4);

            //Choose VAT amount not in span
            // CostSummary costSummary5 = new CostSummary(searchRepository,jsonHandler);
            // costSummary5.addItem("Mdf", 8);
            // costSummary5.updateVatAmountOnItem("Mdf", 1.2);
            // costSummary5.updateVatAmountOnItem("Mdf", -0.2);

            //Confirm summary without adding items
            // CostSummary costSummary6 = new CostSummary(searchRepository,jsonHandler);
            // summaryManager.confirmSummary(costSummary6);
        }
    }
}
