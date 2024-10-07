
namespace Billing.DTO
{
    public class InvoiceDTO
    {
        public int? Id { get; set; } // For update
        public string Number { get; set; }

        public decimal BillTotal { get; set; }

        public DateTime BillDate { get; set; }

        public int? DiscountPercentage { get; set; }

        public decimal? Net { get; set; }

        public decimal? PaidUp { get; set; }

        public decimal? Rest { get; set; }

        public int ClientId { get; set; }

        public List<InvoiceItemDTO> InvoiceItems { get; set; }



    }
}

