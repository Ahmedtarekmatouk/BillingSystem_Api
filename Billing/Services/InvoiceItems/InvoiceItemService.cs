using Billing.DTO;
using Billing.Models;
using Billing.Repositories.InvoiceItems;
using Billing.Repositories.Stock;

namespace Billing.Services.InvoiceItems
{
    public class InvoiceItemService : IInvoiceItemService
    {
        IInvoiceItemRepository repository;
        IStocksRepository stocksRepository;
        public InvoiceItemService(IInvoiceItemRepository repository, IStocksRepository stocksRepository)
        {
            this.repository = repository;
            this.stocksRepository = stocksRepository;
        }

        public InvoiceItemDTO GetById(int id)
        {
            var dbObj = repository.GetById(id);
            InvoiceItemDTO invoiceItemDTO = new InvoiceItemDTO()
            {
                Id = id,
                ItemId = dbObj.ItemId,
 
                Price = dbObj.Price,
                Quantity = dbObj.Quantity,
                
            };

            return invoiceItemDTO;
        }
        public void Delete(int id)
        {
            var invoiceItem = repository.GetById(id);
            var stock = stocksRepository.GetAll().FirstOrDefault(s => s.ItemsId == invoiceItem.ItemId);

            if (stock != null)
            {
                stock.Quntity += invoiceItem.Quantity; // Return the quantity to stock
                stocksRepository.Update(stock);
            }

            repository.Delete(id);
            repository.Save();
        }


        public void Save()
        {
            repository.Save();
        }



        public InvoiceItemDTO Create(InvoiceItemDTO entity)
        {
            var stock = stocksRepository.GetAll().FirstOrDefault(s => s.ItemsId == entity.ItemId);

            if (stock == null || stock.Quntity < entity.Quantity)
            {
                throw new Exception($"Insufficient stock for this  {entity.ItemId}");
            }

            // Deduct the stock quantity
            stock.Quntity -= entity.Quantity;
            stocksRepository.Update(stock);

            // Create new invoice item
            var newInvoiceItem = new InvoiceItem()
            {
                ItemId = entity.ItemId,
                
                Price = entity.Price,
                Quantity = entity.Quantity,
               
            };

            repository.Add(newInvoiceItem);
            repository.Save();


            entity.Id = newInvoiceItem.Id;
            return entity;
        }


        public List<InvoiceItemDTO> GetAll()
        {
            var invoiceItems = repository.GetAll();
            return invoiceItems.Select(ii => new InvoiceItemDTO
            {
                Id = ii.Id,
                ItemId = ii.ItemId,
                
                Price = ii.Price,
                Quantity = ii.Quantity,
               
            }).ToList();
        }

        public List<InvoiceItemDTO> GetAllByInvoiceId(int invoiceId)
        {
            var invoiceItems = repository.GetAllByInvoiceId(invoiceId);
            return invoiceItems.Select(ii => new InvoiceItemDTO
            {
                Id = ii.Id,
                ItemId = ii.ItemId,
                
                Price = ii.Price,
                Quantity = ii.Quantity,
                
            }).ToList();
        }



        public InvoiceItemDTO Update(InvoiceItemDTO entity)
        {
            var existingInvoiceItem = repository.GetById((int)entity.Id);
            var stock = stocksRepository.GetAll().FirstOrDefault(s => s.ItemsId == entity.ItemId);

            if (existingInvoiceItem == null)
            {
                throw new Exception("Invoice item not found.");
            }

            if (stock == null)
            {
                throw new Exception($"Stock not found for ItemId {entity.ItemId}");
            }

            
            int quantityChange = entity.Quantity - existingInvoiceItem.Quantity;
            if (quantityChange > 0 && stock.Quntity < quantityChange)
            {
                throw new Exception($"Insufficient stock to update InvoiceItem. Available: {stock.Quntity}, Requested: {quantityChange}");
            }

            stock.Quntity -= quantityChange;
            stocksRepository.Update(stock);

            
            existingInvoiceItem.Quantity = entity.Quantity;
            existingInvoiceItem.Price = entity.Price;
            
           

            repository.Update(existingInvoiceItem);
            repository.Save();

            return entity;
        }




    }
}
