using Billing.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Billing.Repositories
{
    public class tokenRepository
    {
        BillingContext context;
        public tokenRepository(BillingContext context)
        {
            this.context = context;

        }
        public ApplicationUser CheckByName(string name)
        {

            var user = context.Users.Where(u=>u.NormalizedUserName == name.ToUpper()).FirstOrDefault();
            if (user == null)
            {
                return user;
            }
            return null;
        }
        public List<ApplicationUser> GetAll()
        { 
          return context.Users.ToList();
        }
        public ApplicationUser GetById(string id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
        public void Delete(string id)
        { 
            var user=GetById(id);
            if (user != null)
            {
                context.Users.Remove(user);
                Save();
            }
            
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
