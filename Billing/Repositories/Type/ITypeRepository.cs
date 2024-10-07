namespace Billing.Repositories
{
    public interface ITypeRepository : ICRUDRepository<Billing.Models.Type>
    {
        bool IsTypeNameUnique(string typeName);
    }
}
