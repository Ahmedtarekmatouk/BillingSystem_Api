using Billing.DTO;
using Billing.Models;
using Billing.Repositories.item;


namespace Billing.Services.Items
{
    public class ItemsService : IItemsService
    {
        IItemsRepository itemsRepository;
        public ItemsService(IItemsRepository _itemsRepository)
        {
            itemsRepository = _itemsRepository;
        }
        public ItemsDTO Create(ItemsDTO entity)
        {
             Item item = new Item()
            {
                Name = entity.Name,
                Notes = entity.Notes,
                BuyingPrice = entity.BuyingPrice,
                SellingPrice = entity.SellingPrice,
                CompanyId = entity.CompanyId,
                TypeId = entity.TypeId,
                Unit = new Unit { Name=entity.UnitName},
                UnitId = entity.UnitId,
                Stock = new Stocks { Quntity = entity.OpeningBalance },
                IsDeleted = false

            };
            itemsRepository.Add(item);
            itemsRepository.save();
            return entity;
        }
        public void Delete(int id)
        {
            Item item = itemsRepository.GetById(id);
            if (item != null)
            {
                item.IsDeleted = true;
                itemsRepository.Delete(id);
            }
        }
        public List<ItemsDTO> GetAll(string search = null, string sortBy = null, bool sortDesc = false, int page = 1, int pageSize = 10)
        {
            var query = itemsRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(item => item.Name.Contains(search) ||
                                            item.Notes.Contains(search));
            }


            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortDesc ? query.OrderByDescending(i => i.Name) : query.OrderBy(i => i.Name);
                    break;
                case "buyingprice":
                    query = sortDesc ? query.OrderByDescending(i => i.BuyingPrice) : query.OrderBy(i => i.BuyingPrice);
                    break;
                case "sellingprice":
                    query = sortDesc ? query.OrderByDescending(i => i.SellingPrice) : query.OrderBy(i => i.SellingPrice);
                    break;
                default:
                    query = sortDesc ? query.OrderByDescending(i => i.Id) : query.OrderBy(i => i.Id);
                    break;
            }


            var totalItems = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);


            var itemsDTOs = query.Select(item => new ItemsDTO
            {
                Name = item.Name,
                Notes = item.Notes,
                BuyingPrice = item.BuyingPrice,
                SellingPrice = item.SellingPrice,
                CompanyId = item.CompanyId,
                TypeId = item.TypeId,
                OpeningBalance = item.Stock.Quntity,
                UnitName = item.Unit.Name,
                UnitId = item.Unit.Id,
            }).ToList();

            return itemsDTOs;
        }

        public List<ItemsDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public ItemsDTO GetById(int id)
        {
            Item item = itemsRepository.GetById(id);
            if (item != null)
            {
                ItemsDTO itemsDTO = new ItemsDTO()
                {
                    Name = item.Name,
                    Notes = item.Notes,
                    BuyingPrice = item.BuyingPrice,
                    SellingPrice = item.SellingPrice,
                    CompanyId = item.CompanyId,
                    TypeId = item.TypeId,
                    UnitName= item.Unit.Name,
                    OpeningBalance = item.Stock.Quntity,
                    UnitId= item.Unit.Id,

                };
                return itemsDTO;
            }
            return null;
        }

        public List<ItemsDTO> GetByName(string name)
        {
            var item = itemsRepository.GetByName(name);
            if (item != null)
            {
                var itemsDTOs = item.Select(i => new ItemsDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    BuyingPrice = i.BuyingPrice,
                    SellingPrice = i.SellingPrice,
                    CompanyId = i.CompanyId,
                    TypeId = i.TypeId,
                    Notes = i.Notes,
                    UnitName = i.Unit.Name,
                    OpeningBalance = i.Stock.Quntity,
                    UnitId = i.Unit.Id,

                }).ToList();
                return itemsDTOs;
            }
            return null;



        }

        public List<ItemsDTO> GetItemsByTypeID(int id)
        {
            List<Item> item = itemsRepository.GetItemsByTypeID(id);
            List<ItemsDTO> itemsDTO = new List<ItemsDTO>();
            foreach (Item items in item)
            {
                ItemsDTO itemDTO = new ItemsDTO()
                {
                    Name = items.Name,
                    Notes = items.Notes,
                    BuyingPrice = items.BuyingPrice,
                    SellingPrice = items.SellingPrice,
                    CompanyId = items.CompanyId,
                    TypeId = items.TypeId,
                    UnitName = items.Unit.Name,
                    OpeningBalance = items.Stock.Quntity,
                    UnitId = items.Unit.Id,

                };
                itemsDTO.Add(itemDTO);
            }
            return itemsDTO;
        }

        public void save()
        {
            itemsRepository.save();
        }
        public ItemsDTO Update(ItemsDTO entity)
        {
            Item item = itemsRepository.GetById(entity.Id);
            item.Name = entity.Name; item.Notes = entity.Notes;
            item.BuyingPrice = entity.BuyingPrice;
            item.SellingPrice = entity.SellingPrice;
            item.CompanyId = entity.CompanyId;
            item.TypeId = entity.TypeId;
            item.Unit = new Unit {Name=entity.UnitName };
            item.Stock.Quntity = entity.OpeningBalance;
            item.UnitId = entity.UnitId;
            if (item != null)
            {
                itemsRepository.Update(item);
                itemsRepository.save();
                return entity;
            }
            return null;
        }
    }
}
