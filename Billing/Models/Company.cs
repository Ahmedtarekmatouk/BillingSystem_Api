namespace Billing.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Notes { get; set; }

        public virtual List<Type>? Types { get; set; } = new List<Type>();

        public virtual List<Item>? Items { get; set; } = new List<Item>();

        public bool IsDeleted { get; set; } = false;
    }
}
