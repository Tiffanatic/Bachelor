using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly RapidTimeDbContext _context;
        private DbSet<T> entities;
        
        public Repository(RapidTimeDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> getAll()
        {
            return entities.AsEnumerable();
        }

        public T GetbyId(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Add(entity);
            
        }

        public void Update(T entity)
        {
            if ( entity == null) throw new ArgumentNullException("entity");
            entities.Update(entity);
        }

        public void Delete(int id)
        {
            if ( id == null) throw new ArgumentNullException(nameof(id));

            T entity = entities.SingleOrDefault(s => s.Id == id);

            if (entity == null)
            {
                throw new ArgumentException("Item was not found");
            }
            entities.Remove(entity);
        }
    }
}