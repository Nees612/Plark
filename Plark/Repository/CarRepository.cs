using Microsoft.EntityFrameworkCore;
using Plark.Context;
using Plark.Models;
using Plark.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Plark.Repository
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(PlarkContext context) : base(context)
        {

        }

        public async Task<Car> GetCarBynumberPlate(string numberPlate)
        {
            var car = await _dbSet.FirstOrDefaultAsync(c => c.NumberPlate.Equals(numberPlate));

            return car;
        }
    }
}
