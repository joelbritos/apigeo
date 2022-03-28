using System;
using APIGEO.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGEO.Repository
{
    public class APIGEOContext: DbContext
    {
        public APIGEOContext(DbContextOptions<APIGEOContext> opt): base(opt)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Operacion>().HasKey(x => new { x.Id });
            
            builder.Entity<Pedido>().HasKey(x => new { x.Id });
        }

        public DbSet<Operacion> Operacion {get; set;}
        public DbSet<Pedido> Pedidos {get; set;}
    }
}