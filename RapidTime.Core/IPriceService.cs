using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IPriceService
    {
        PriceEntity GetById(int i);
        IEnumerable<PriceEntity> GetAll();
        void Insert(PriceEntity priceEntity);
        void Update(PriceEntity priceEntity);
        void Delete(int priceId);
    }
}