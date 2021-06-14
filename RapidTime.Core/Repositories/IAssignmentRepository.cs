using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Repositories
{
    public interface IAssignmentRepository
    {
        IEnumerable<AssignmentEntity> GetAll();
        AssignmentEntity Insert(AssignmentEntity assignmentEntity);
        AssignmentEntity GetbyId(int id);
        void Update(AssignmentEntity assignmentEntity);
        void Delete(int id);
    }
}