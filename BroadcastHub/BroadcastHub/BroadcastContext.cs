using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BroadcastHub.Models;

namespace BroadcastHub
{
    public class BroadcastContext : DbContext 
    {
        public BroadcastContext(DbContextOptions<BroadcastContext> options) : base(options)
        {
        }
        
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSet>().ToTable("Datum");
        }
        
    
        public DbSet<BroadcastHub.Models.DataSet> DataSet { get; set; }
    }
}
