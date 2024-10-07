using Billing.DTO.TypeDTO;

namespace Billing.Services
{
    public interface ITypeService : ICRUDService<TypeDTO>
    {
        bool IsTypeNameUnique(string typeName);
    }
}