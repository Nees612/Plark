using Plark.Models;
using Plark.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Repository.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<Car> GetCarBynumberPlate(string numberPlate);
    }
}
