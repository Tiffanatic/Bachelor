using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface IAssignmentTypeService
    {
        IEnumerable<AssignmentTypeEntity> GetAll();
        void Delete(int assignmentTypeId);
        AssignmentTypeEntity[] GetByNameOrNumber(string input);
        int Insert(AssignmentTypeEntity assignmentTypeEntity);
        AssignmentTypeEntity GetById(int i);

        void Update(AssignmentTypeEntity assignmentTypeEntity);
    }
}