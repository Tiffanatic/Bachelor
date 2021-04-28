using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IAssignmentService
    {
        IEnumerable<Assignment> GetAll();
        Assignment[] FindAssignmentByNameOrNumber(string nameOrNumber);
        void Delete(int id);
        void Insert(Assignment assignment);
        Assignment GetById(int id);
        void Update(Assignment assignment);
    }
}