using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;

namespace consoleproject
{
   class Program
    {
        static void Main(string[] args)
        {
            // CostSummary costSummary = new CostSummary(); 
            // costSummary.addItem("Teak", 200);
            // // costSummary.addCustomerName("TImmie");
            // SummaryManager summaryManager = new SummaryManager(); 
            // summaryManager.showSummaryDetails(costSummary); 
            // // costSummary.addItem("MDF", 200);
            // // costSummary.addItem("Fujalusa", 200);
            // costSummary.addCustomerName("Timmie");
            // var dueDate = new DateTime(2008, 3, 1, 7, 0, 0);
            // costSummary.addDueDate(dueDate);

            // CostSummary costSummary1 = new CostSummary(); 
            // costSummary1.addItem("Fujalusa", 200);
            // costSummary1.addItem("MDF", 200);
            // costSummary1.addItem("Teak", 300);
            // costSummary1.addCustomerName("Alex");
            // var dueDate1 = new DateTime(2008, 3, 1, 8, 0, 0);
            // costSummary1.addDueDate(dueDate1);
            // costSummary1.updateVatAmountOnItem("MDF", 1.90);

            // SummaryManager.addCostSummary(costSummary1);
            // // summaryManager.showSummaryDetails();
            // SummaryManager.saveSummaryToFile(costSummary1);
            // SummaryManager.showSummaryDetails(costSummary1);
            SearchRepository searchRepository = new SearchRepository();
            JsonHandler jsonHandler = new JsonHandler();
            CostSummary costSummary = new CostSummary(searchRepository, jsonHandler); 
            costSummary.addItem("MDF", 99);
            SummaryManager summaryManager = new SummaryManager(); 
            summaryManager.confirmSummary(costSummary);
            // // summaryManager.addCostSummary(costSummary);
            // summaryManager.showSummaryDetails(costSummary);
            // summaryManager.saveSummaryToFile(costSummary);

            // CostSummary costSummary1 = summaryManager.loadSummaryFromFile("Timmie_03-01-2008_1.json");

            // CostSummary costSummary1 = summaryManager.loadSummaryFromFile("Alex__2.json");
            // SummaryManager.confirmSummary(costSummary1);
            // SummaryManager.showSummaryDetails();
        }   
    }
}
