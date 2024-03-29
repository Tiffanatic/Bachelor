﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RapidTime.Core.Models;
using RapidTime.Core.Repositories;

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

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public T GetbyId(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public T Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.Created = DateTime.UtcNow;
            entity.Updated = DateTime.UtcNow;
            var entityEntry = entities.Add(entity);
            return entityEntry.Entity;
        }

        public void Update(T entity)
        {
            if ( entity == null) throw new ArgumentNullException(nameof(entity));
            entity.Updated = DateTime.UtcNow;
            entities.Update(entity);
        }

        public void Delete(int id)
        {
            T entity = entities.SingleOrDefault(s => s.Id == id);

            if (entity == null)
            {
                throw new ArgumentException("Item was not found");
            }
            entities.Remove(entity);
        }
    }
}