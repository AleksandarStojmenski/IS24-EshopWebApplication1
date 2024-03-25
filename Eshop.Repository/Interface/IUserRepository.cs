using Eshop.DomainEntities;

namespace Eshop.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<EshopApplicationUser> GetAll();
        EshopApplicationUser Get(string id);
        void Insert(EshopApplicationUser entity);
        void Update(EshopApplicationUser entity);
        void Delete(EshopApplicationUser entity);
    }
}