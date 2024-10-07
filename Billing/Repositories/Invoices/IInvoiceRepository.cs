using Billing.Models;

namespace Billing.Repositories.Invoices
{
    public interface IInvoiceRepository : ICRUDRepository<Invoice>
    {
       void Save();
    }
}
