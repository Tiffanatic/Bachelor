using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Data
{
    public class UnitofWork : IUnitofWork
    {
        private readonly RapidTimeDbContext _context;
        private IRepository<Contact> _Contactrepository;

        public UnitofWork(RapidTimeDbContext context)
        {
            _context = context;
        }

        public IRepository<Contact> ContactRepository
        {
            get { return _Contactrepository ??= new Repository<Contact>(_context); }
        }

        public void Commit()
        {
            _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        
    }
}