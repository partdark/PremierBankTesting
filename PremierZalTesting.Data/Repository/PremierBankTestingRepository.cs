using Microsoft.EntityFrameworkCore;
using PremierBankTesting.Bank;
using PremierBankTesting.Bank.Models;
using PremierZalTesting.Core.Models;
using PremierZalTesting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PremierZalTesting.Data.Repository
{
    public class PremierBankTestingRepository : IPremierBankTestingRepository
    {
        private readonly PremierZalTestingBDContext _context;
        private readonly IBankApiClient _bankApiClient;

        public PremierBankTestingRepository(PremierZalTestingBDContext context, IBankApiClient bankApiClient)
        {
            _context = context;
            _bankApiClient = bankApiClient;
        }

        public async Task<Client> CheckOrCreateClientByEmail(string email)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);
            if (client == null)
            {

                client = new ClientEntity
                {
                    Id = Guid.NewGuid(),
                    Email = email
                };
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();

            }

            return new Client(client.Id, client.Email);

        }
        public async Task<Guid> CreateTransaction(BankTransaction bankTransaction)
        {
            var client = await CheckOrCreateClientByEmail(bankTransaction.UserEmail);


            var bankTransactionEntity = new BankTransactionEntity()
            {
                Id = bankTransaction.Id,
                Amount = bankTransaction.Amount,
                Comment = bankTransaction.Comment,
                Timestamp = bankTransaction.Timestamp,
                UserEmail = bankTransaction.UserEmail,
                IsProcessed = bankTransaction.IsProcessed,



            };
            await _context.BankTransactions.AddAsync(bankTransactionEntity);
            await _context.SaveChangesAsync();

            return bankTransactionEntity.Id;
        }

        public async Task<List<BankTransaction>> GetAllUnProcessed()
        {
            var bankTransactionsEntity = await _context.BankTransactions.AsNoTracking()
                .Where(b => !b.IsProcessed)
                .ToListAsync();

            var bankTransactions = bankTransactionsEntity
                .Select(b => new BankTransaction(
                    b.Id,
                    b.Amount,
                    b.Comment,
                    b.Timestamp,
                    b.UserEmail
                    )
                ).ToList();

            return bankTransactions;

        }
        public async Task<Guid> MarkAsProcessed(Guid id)
        {

            var count = await _context.BankTransactions
                    .Where(b => b.Id == id)
                     .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsProcessed, true));

            if (count == 0)
            {
                throw new KeyNotFoundException($"Транзакция не найдена, Id транзакции:{id}");
            }



            return id;

        }

        public async Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsByUser()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var data = await _context.BankTransactions
                .Where(b => b.IsProcessed && b.Timestamp >= startDate)
                .GroupBy(b => b.UserEmail)
                .Select(d => new
                {
                    UserEmail = d.Key,
                    MothSum = d.Sum(b => b.Amount)
                }).ToDictionaryAsync(b => b.UserEmail, b => b.MothSum);

            return data;
        }

        public async Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsByComment()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var data = await _context.BankTransactions
                .Where(b => b.IsProcessed && b.Timestamp >= startDate)
                .GroupBy(b => b.Comment)
                .Select(d => new
                {
                    Comment = d.Key,
                    MothSum = d.Sum(b => b.Amount)
                }).ToDictionaryAsync(b => b.Comment, b => b.MothSum);

            return data;
        }

        public async Task<int> ImportTransactionsAsync()
        {


            var transactions = await _bankApiClient.GetRecentTransactionsAsync();
            var importedCount = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var t in transactions)
                {
                    var findTransaction = await _context.BankTransactions
                        .FirstOrDefaultAsync(tr => tr.Id == t.Id);

                    if (findTransaction != null)
                        continue;

                    var client = await CheckOrCreateClientByEmail(t.UserEmail);

                    var newTransaction = new BankTransactionEntity
                    {
                        Id = t.Id,
                        Amount = t.Amount,
                        Comment = t.Comment,
                        Timestamp = t.Timestamp,
                        UserEmail = t.UserEmail,
                        IsProcessed = false
                    };

                    await _context.BankTransactions.AddAsync(newTransaction);
                    importedCount++;
                }

                await _context.SaveChangesAsync();


                await transaction.CommitAsync();
                return importedCount;
            }
            catch
            {

                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

