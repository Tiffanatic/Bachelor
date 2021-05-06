using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IAssignmentType
    {
        IEnumerable<AssignmentTypeEntity> GetAll();
        void Delete(int assignmentTypeId);
        AssignmentTypeEntity[] GetByNameOrNumber(string input);
        void Insert(AssignmentTypeEntity assignmentTypeEntity);
        AssignmentTypeEntity GetById(int i);

        void Update(AssignmentTypeEntity assignmentTypeEntity);
    }
}