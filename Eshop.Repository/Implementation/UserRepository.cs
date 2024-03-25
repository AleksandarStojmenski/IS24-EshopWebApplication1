using Eshop.DomainEntities;
using Eshop.Repository.Interface;
using EShop.Repository.Interface;
using EshopWebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Repository.Implementation
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<EshopApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<EshopApplicationUser>();
        }
        public IEnumerable<EshopApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public EshopApplicationUser Get(string id)
        {
            var strGuid = id.ToString();
            return entities.Include(z=>z.UserCart).Include(z => z.UserCart.ProductsInShoppingCarts).Include("UserCart.ProductsInShoppingCarts.Product").SingleOrDefault(s => s.Id == strGuid);
        }
        public void Insert(EshopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(EshopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(EshopApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
