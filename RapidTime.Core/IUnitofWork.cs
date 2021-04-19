namespace RapidTime.Core
{
    public interface IUnitofWork
    {
        
        
        
        void Commit();
        void Rollback();
    }
}