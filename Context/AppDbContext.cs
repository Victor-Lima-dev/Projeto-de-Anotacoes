using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Anotacao> Anotacoes { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}