
namespace consoleproject
{
    public class Item
    {
        public string Name { get; set; }
        public string Material { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        private double _vatPercentage = 1.25;
        public double VatPercentage
        {
            get => _vatPercentage;
            set => _vatPercentage = value;
        }

        public double PriceWithVat
        {
            get => Price * _vatPercentage;
        }

        public Item(string name, string material, int quantity, int price)
        {
            this.Name = name;
            this.Material = material;
            this.Quantity = quantity;
            this.Price = price;
        }
    }
}