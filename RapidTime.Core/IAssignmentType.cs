using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IAssignmentType
    {
        IEnumerable<AssignmentType> GetAll();
        void Delete(int assignmentTypeId);
        AssignmentType[] GetByNameOrNumber(string input);
        void Insert(AssignmentType assignmentType);
        AssignmentType GetById(int i);

        void Update(AssignmentType assignmentType);
    }
}