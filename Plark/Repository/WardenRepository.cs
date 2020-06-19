using Microsoft.EntityFrameworkCore;
using Plark.Context;
using Plark.Models;
using Plark.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plark.Repository
{
    public class WardenRepository : Repository<Warden>, IWardenRepository
    {
        public WardenRepository(PlarkContext context) : base(context)
        {

        }
    }
}
