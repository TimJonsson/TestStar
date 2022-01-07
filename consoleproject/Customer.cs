namespace consoleproject
{
    public class Customer
    {

        // Making name an empty string in case customer name is not given when showing cost summary details
        private string _name = "";
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Customer(string name)
        {
            this._name = name;
        }
    }
}
