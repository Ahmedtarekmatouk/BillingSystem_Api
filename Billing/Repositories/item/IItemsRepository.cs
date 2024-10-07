using Billing.Models;

namespace Billing.Repositories.item
{
    public interface IItemsRepository : ICRUDRepository<Item>
    {
        List<Item> GetItemsByTypeID(int id);
        void save();
    }
}
