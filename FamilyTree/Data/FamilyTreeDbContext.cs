using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FamilyTree;

namespace FamilyTree.Models
{
    public class FamilyTreeDbContext : DbContext
    {
        public FamilyTreeDbContext (DbContextOptions<FamilyTreeDbContext> options)
            : base(options)
        {
        }

        public DbSet<FamilyTree.Person> Person { get; set; }
    }
}
