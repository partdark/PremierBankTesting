using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierZalTesting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierZalTesting.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
           builder.HasKey( c => c.Id );

            builder.Property(c => c.Id).IsRequired();

            builder.Property(c => c.Email).IsRequired() ;
            builder.HasIndex(c => c.Email).IsUnique();

            builder.HasMany(t => t.BankTransactions)
                .WithOne(c => c.Client)
                .HasPrincipalKey(c => c.Email);
                
        }
    }
}
