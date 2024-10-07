using Billing.Models;
using Billing.Repositories.Stock;

namespace Billing.Services.Stock
{
    public class StockService : IStockService
    {
        IStocksRepository StocksRepository;
        public StockService(IStocksRepository stocksRepository) 
        {
            this.StocksRepository = stocksRepository;
        }
        public Stocks Create(Stocks entity)
        {
            return StocksRepository.Add(entity);
            
        }

        public void Delete(int id)
        {
            StocksRepository.Delete(id);
        }

        public List<Stocks> GetAll()
        {
         return StocksRepository.GetAll();
        }

        public Stocks GetById(int id)
        {
           return StocksRepository.GetById(id);
        }

        public List<Stocks> GetByName(string name)
        {
            return StocksRepository.GetByName(name);
        }

        public Stocks Update(Stocks entity)
        {
            return StocksRepository.Update(entity);
        }
    }
}
