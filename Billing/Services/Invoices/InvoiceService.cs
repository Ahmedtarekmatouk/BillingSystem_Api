using Billing.DTO;
using Billing.Models;
using Billing.Repositories.Invoices;
using Billing.Repositories.Stock;

namespace Billing.Services.Invoices
{
    public class InvoiceService : IInvoiceService
    {
          IInvoiceRepository repository;
        IStocksRepository stocksRepository;
        public InvoiceService(IInvoiceRepository repository, IStocksRepository stocks)
        {
            this.repository = repository;
            this.stocksRepository = stocks;
        }
        public Invoice Create(InvoiceDTO invoiceDTO)
        {
            DateTime currentDate = DateTime.Now;

            
            string invoiceNumber = Guid.NewGuid().ToString("N").Substring(0, 10);

            decimal billTotal =(decimal) invoiceDTO.InvoiceItems.Sum(i => i.Price * i.Quantity);


            decimal discountPercentage = invoiceDTO.DiscountPercentage ?? 0; 
            decimal discountAmount = invoiceDTO.BillTotal * (discountPercentage / 100);
            decimal net = invoiceDTO.BillTotal - discountAmount; 
            decimal rest =  (decimal)invoiceDTO.PaidUp- (decimal)net; 

            Invoice invoice = new Invoice()
            {
                Number = invoiceNumber, 
                BillDate = currentDate,
                BillTotal = invoiceDTO.BillTotal,
                DiscountPercentage = (int)discountPercentage, 
                Net = net, 
                PaidUp = invoiceDTO.PaidUp,
                Rest = rest,
                ClientId = invoiceDTO.ClientId,
                InvoiceItems = invoiceDTO.InvoiceItems.Select(i => new InvoiceItem()
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Price = i.Price
                  ,
                }).ToList()
            };

           
            foreach (var item in invoiceDTO.InvoiceItems)
            {
                var existingStock = stocksRepository.GetAll()
                                        .FirstOrDefault(s => s.ItemsId == item.ItemId);
                if (existingStock == null || existingStock.Quntity < item.Quantity)
                {
                    throw new InvalidOperationException($"Stock for item with ID {item.ItemId} is insufficient or does not exist.");
                }

                existingStock.Quntity -= item.Quantity;
                stocksRepository.Update(existingStock);
            }

            var dbObj = repository.Add(invoice);
            repository.Save();

            return dbObj;
        }

        public InvoiceDTO GetByIdAsDTO(int id)
        {
            Invoice invoice = repository.GetById(id);

            if (invoice == null)
                return null;

            var invoiceDTO = new InvoiceDTO
            {
                Id = invoice.Id,
                Number = invoice.Number,
                BillTotal = invoice.BillTotal,
                BillDate = invoice.BillDate,
                DiscountPercentage = invoice.DiscountPercentage,
                Net = invoice.Net,
                PaidUp = invoice.PaidUp,
                Rest = invoice.Rest,
                ClientId = invoice.ClientId,
                
                InvoiceItems = invoice.InvoiceItems.Select(item => new InvoiceItemDTO
                {
                    Id = item.Id,
                    ItemId = item.ItemId,
                    
                    Quantity = item.Quantity,
                    Price = item.Price,
                  
                }).ToList(),
                
            };
            return invoiceDTO;
        }

        public Invoice GetById(int id)
        {
            Invoice invoice = repository.GetById(id);

            if (invoice == null)
                return null;

            return invoice;
        }

        public void Save()
        {
            repository.Save();
        }

        public bool Update(InvoiceDTO invoiceFromReq)
        {
            var id = invoiceFromReq.Id ?? 0;
            Invoice invoiceFromDb = GetById(id);

            if (invoiceFromDb == null)
            {
                return false;
            }

            invoiceFromDb.Number = invoiceFromReq.Number;
            invoiceFromDb.BillDate = invoiceFromReq.BillDate;
            invoiceFromDb.BillTotal = invoiceFromReq.BillTotal;
            invoiceFromDb.DiscountPercentage = invoiceFromReq.DiscountPercentage;
            invoiceFromDb.Net = invoiceFromReq.Net;
            invoiceFromDb.PaidUp = invoiceFromReq.PaidUp;
            invoiceFromDb.Rest = invoiceFromReq.Rest;
            invoiceFromDb.ClientId = invoiceFromReq.ClientId;




            var deletedItems = invoiceFromDb.InvoiceItems
           .Where(i => !invoiceFromReq.InvoiceItems.Any(reqItem => reqItem.Id == i.Id))
           .ToList();

            foreach (var item in deletedItems)
            {
                item.IsDeleted = true;
            }

            foreach (var reqItem in invoiceFromReq.InvoiceItems)
            {
                if (reqItem.Id == null)
                {
                    
                    var newItem = new InvoiceItem
                    {
                        ItemId = reqItem.ItemId,
                       
                        Quantity = reqItem.Quantity,
                        Price = reqItem.Price
                    };
                
                    var stock = stocksRepository.GetAll()
                                        .FirstOrDefault(s => s.ItemsId == reqItem.ItemId);
                    if (stock == null || stock.Quntity < reqItem.Quantity)
                    {
                        return false;
                    }

                    stock.Quntity-= reqItem.Quantity;
                    stocksRepository.Update(stock);

                    invoiceFromDb.InvoiceItems.Add(newItem);
                }
                else
                {
                    
                    var existingItem = invoiceFromDb.InvoiceItems.FirstOrDefault(i => i.Id == reqItem.Id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = reqItem.Quantity;
                        existingItem.Price = reqItem.Price;
                     
                       
                    }
                }
            }

            repository.Update(invoiceFromDb);
            repository.Save();
            return true;
        }

        public List<InvoiceDTO> GetAll(string search = null, string sortBy = "asc", bool sortDesc = false, int page = 1, int pageSize = 10)
        {
            var query = repository.GetAll().AsQueryable();

            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(invoice => invoice.Number.Contains(search) ||
                                                invoice.Client.Name.Contains(search));
            }

           
            switch (sortBy?.ToLower())
            {
                case "number":
                    query = sortDesc ? query.OrderByDescending(i => i.Number) : query.OrderBy(i => i.Number);
                    break;
                case "billtotal":
                    query = sortDesc ? query.OrderByDescending(i => i.BillTotal) : query.OrderBy(i => i.BillTotal);
                    break;
                case "billdate":
                    query = sortDesc ? query.OrderByDescending(i => i.BillDate) : query.OrderBy(i => i.BillDate);
                    break;
                default:
                    query = sortDesc ? query.OrderByDescending(i => i.Id) : query.OrderBy(i => i.Id);
                    break;
            }

            
            var totalItems = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            // Convert to DTOs
            var invoiceDTOs = query.Select(invoice => new InvoiceDTO
            {
                Id = invoice.Id,
                Number = invoice.Number,
                BillTotal = invoice.BillTotal,
                BillDate = invoice.BillDate,
                DiscountPercentage = invoice.DiscountPercentage,
                Net = invoice.Net,
                PaidUp = invoice.PaidUp,
                Rest = invoice.Rest,
                ClientId = invoice.ClientId,
                InvoiceItems = invoice.InvoiceItems.Select(item => new InvoiceItemDTO
                {
                    Id = item.Id,
                    ItemId = item.ItemId,
                    
                    Quantity = item.Quantity,
                    Price = item.Price,
                }).ToList(),
            }).ToList();

            return invoiceDTOs;
        }
        public void Delete(int id)
        {   
            repository.Delete(id);
            repository.Save();
        }

        
    }
}
