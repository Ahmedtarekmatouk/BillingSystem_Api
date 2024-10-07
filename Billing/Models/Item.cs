using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Notes { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [ForeignKey("Type")]
        public int? TypeId { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Type? Type { get; set; }
        public virtual Stocks? Stock { get; set; }
        [ForeignKey("Unit")]
        public int? UnitId {  get; set; } 
        public virtual Unit? Unit { get; set; }
        public virtual List<InvoiceItem>? InvoiceItems { get; set; } = new List<InvoiceItem>();

        public bool IsDeleted { get; set; } = false;
    }
}
