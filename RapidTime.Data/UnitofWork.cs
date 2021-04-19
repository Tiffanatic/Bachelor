using RapidTime.Core;

namespace RapidTime.Data
{
    public class UnitofWork : IUnitofWork
    {
        private readonly RapidTimeDbContext _context;
        private IRepository _repository;

        public UnitofWork(RapidTimeDbContext context)
        {
            _context = context;
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