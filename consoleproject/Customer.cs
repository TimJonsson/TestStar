namespace consoleproject
{
    public class Customer
    {

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
