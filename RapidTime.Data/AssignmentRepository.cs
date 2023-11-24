using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RapidTime.Core.Models;
using RapidTime.Core.Repositories;

namespace RapidTime.Data
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly RapidTimeDbContext _context;

        public AssignmentRepository(RapidTimeDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AssignmentEntity> GetAll()
        {
            return _context.Assignments.Include(entity => entity.AssignmentTypeEntity);
        }

        public AssignmentEntity Insert(AssignmentEntity assignmentEntity)
        {
            if (assignmentEntity is null) throw new ArgumentNullException("assignmentEntity");
            assignmentEntity.Created = DateTime.UtcNow;
            assignmentEntity.Updated = DateTime.UtcNow;
            var entityEntry = _context.Assignments.Add(assignmentEntity);
            return entityEntry.Entity;
        }

        public AssignmentEntity GetbyId(int id)
        {
            return _context.Assignments
                .Include(entity => entity.AssignmentTypeEntity)
                .Include(e => e.TimeRecords)
                .SingleOrDefault(s => s.Id == id);
        }

        public void Update(AssignmentEntity assignmentEntity)
        {
            if ( assignmentEntity == null) throw new ArgumentNullException("assignmentEntity");
            assignmentEntity.Updated = DateTime.UtcNow;
            _context.Assignments.Update(assignmentEntity);
        }

        public void Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));
            AssignmentEntity entity = _context.Assignments.SingleOrDefault(s => s.Id == id);
            if (entity == null)
            {
                throw new ArgumentException("Item was not found");
            }
            _context.Assignments.Remove(entity);
        }
    }
}