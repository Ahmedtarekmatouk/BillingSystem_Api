using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public decimal BillTotal { get; set; }

        public DateTime BillDate { get; set; }

        public int? DiscountPercentage { get; set; }

        public decimal? Net { get; set; }

        public decimal? PaidUp { get; set; }

        public decimal? Rest { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual Client? Client { get; set; }

        public List<InvoiceItem>? InvoiceItems { get; set; } = new List<InvoiceItem>();

        public bool IsDeleted { get; set; } = false;
    }
}
