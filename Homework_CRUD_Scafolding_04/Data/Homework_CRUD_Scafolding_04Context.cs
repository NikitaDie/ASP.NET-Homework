using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Homework_CRUD_Scafolding_04.Models;

namespace Homework_CRUD_Scafolding_04.Data
{
    public class Homework_CRUD_Scafolding_04Context : DbContext
    {
        public Homework_CRUD_Scafolding_04Context (DbContextOptions<Homework_CRUD_Scafolding_04Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Homework_CRUD_Scafolding_04.Models.Employee> Employee { get; set; } = default!;
    }
}
