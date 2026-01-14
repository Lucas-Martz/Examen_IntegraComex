using System.Collections.Generic;
using System.Data.Entity;
using Examen_IntegraComex.Models;

namespace Examen_IntegraComex.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
