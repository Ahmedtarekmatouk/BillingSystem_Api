using Billing.DTO;
using Billing.Models;

namespace Billing.Services.Invoices
{
    public interface IInvoiceService:IService
    {

        Invoice Create(InvoiceDTO invoiceDTO);

        InvoiceDTO GetByIdAsDTO(int id);

        Invoice GetById(int id);

        bool Update(InvoiceDTO invoiceDTO);

        List<InvoiceDTO> GetAll(string search , string sortBy , bool sortDesc , int page, int pageSize);

        void Delete(int id);

        void Save();
    }
}
