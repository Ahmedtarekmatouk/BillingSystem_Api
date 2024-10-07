namespace Billing.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int Number  { get; set; }

        public string Address { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
