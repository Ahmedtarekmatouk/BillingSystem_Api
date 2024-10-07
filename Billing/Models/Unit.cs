namespace Billing.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; }=new List<Item>();
        public string? Notes { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
