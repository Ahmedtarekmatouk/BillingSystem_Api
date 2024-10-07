using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }

        public virtual Invoice? Invoice { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public virtual Item? Item { get; set; }

     

        public int Quantity { get; set; }

        public double Price { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
