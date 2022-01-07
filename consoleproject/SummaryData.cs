using System.Collections.Generic;
namespace consoleproject
{
    // Class for handling Summar data when reading/writing to Json files
    public class SummaryData
    {
        public int SummaryId { get; set; }
        public string Created { get; set; }

        public string Updated { get; set; }

        public string SavedAs { get; set; }

        public string CustomerName { get; set; }

        public string DueDate { get; set; }

        public string ConfirmedDate { get; set; }

        public List<Item> items { get; set; }

        public double TotalCost { get; set; }

        public double TotalCostWithVAT { get; set; }
    }
}