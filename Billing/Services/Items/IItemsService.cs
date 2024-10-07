using Billing.DTO;
using Billing.Models;

namespace Billing.Services.Items
{
    public interface IItemsService : ICRUDService<ItemsDTO>
    {
        List<ItemsDTO> GetAll(string search, string sortBy, bool sortDesc, int page, int pageSize);
        List<ItemsDTO> GetItemsByTypeID(int id);
        public void save();
    }
}
