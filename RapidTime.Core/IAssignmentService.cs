using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface IAssignmentService
    {
        IEnumerable<AssignmentEntity> GetAll();
        AssignmentEntity[] FindAssignmentByNameOrNumber(string nameOrNumber);
        void Delete(int id);
        int Insert(AssignmentEntity assignmentEntity);
        AssignmentEntity GetById(int id);
        void Update(AssignmentEntity assignmentEntity);
    }
}