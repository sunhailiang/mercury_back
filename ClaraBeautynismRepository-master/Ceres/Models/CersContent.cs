using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ceres.Models
{
    public class CersContent:DbContext
    {
       

        public CersContent(DbContextOptions<CersContent> options) : base(options)
        {

        }
        public virtual DbSet<RequisiteAmount> RequisiteAmount { get; set; }
        public virtual DbSet<FoodComposition> FoodComposition { get; set; }
    }
}
