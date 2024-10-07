using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Models
{
    public class Type
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Notes{ get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public virtual List<Item>? Items { get; set; } = new List<Item>();

        public bool IsDeleted { get; set; } = false;

    }
}
