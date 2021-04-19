using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface IUnitofWork
    {
        IRepository<Contact> ContactRepository { get; }
        
        
        void Commit();
        void Rollback();
    }
}