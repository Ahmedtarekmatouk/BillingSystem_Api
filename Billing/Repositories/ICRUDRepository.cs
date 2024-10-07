namespace Billing.Repositories
{
    public interface ICRUDRepository<T> : IRepository
    {
        List<T> GetAll();
        T GetById(int id);
        T Add(T entity);
        T Update(T entity);
        void Delete(int id);
        List<T> GetByName(string name);
        
    }
}
