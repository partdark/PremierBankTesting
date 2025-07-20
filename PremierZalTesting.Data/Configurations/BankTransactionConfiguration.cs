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
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransactionEntity>

    {
        public void Configure(EntityTypeBuilder<BankTransactionEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Amount).IsRequired();

            builder.Property(p => p.Comment).IsRequired();

            builder.Property(p => p.Timestamp).IsRequired();

            builder.Property(p => p.UserEmail).IsRequired();

            builder.HasOne(c => c.Client)
                .WithMany(b => b.BankTransactions)
                .HasForeignKey(b => b.UserEmail);

        }

    }
    }

