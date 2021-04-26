using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IAssignment
    {
        IEnumerable<Assignment> GetAllAssignments();
        Assignment[] FindAssignmentByNameOrNumber(string nameOrNumber);
        void DeleteAssignment(int id);
        void Insert(Assignment assignment);
        Assignment FindById(int id);
        void Update(Assignment assignment);
    }
}