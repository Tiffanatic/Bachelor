using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IAssignmentService
    {
        IEnumerable<AssignmentEntity> GetAll();
        AssignmentEntity[] FindAssignmentByNameOrNumber(string nameOrNumber);
        void Delete(int id);
        void Insert(AssignmentEntity assignmentEntity);
        AssignmentEntity GetById(int id);
        void Update(AssignmentEntity assignmentEntity);
    }
}