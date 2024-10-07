using Billing.Models;

namespace Billing.Repositories.InvoiceItems
{
    public interface IInvoiceItemRepository : ICRUDRepository<InvoiceItem>
    {
        public List<InvoiceItem> GetAllByInvoiceId(int invoiceId);
        public void Save();
    }
}
