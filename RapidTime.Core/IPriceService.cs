using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IPriceService
    {
        Price GetById(int i);
        IEnumerable<Price> GetAll();
        void Insert(Price price);
        void Update(Price price);
        void Delete(int priceId);
    }
}