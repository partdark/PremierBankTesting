using Microsoft.EntityFrameworkCore;
using PremierZalTesting.Data.Configurations;
using PremierZalTesting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PremierZalTesting.Data
{
    public class PremierZalTestingBDContext : DbContext
    {
        public PremierZalTestingBDContext(DbContextOptions<PremierZalTestingBDContext> options) : base (options)
        {
                       
        }
        public DbSet<BankTransactionEntity> BankTransactions { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BankTransactionConfiguration());
            modelBuilder.ApplyConfiguration( new ClientConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
