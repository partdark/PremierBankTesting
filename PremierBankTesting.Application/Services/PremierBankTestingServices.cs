using PremierBankTesting.Bank.Models;
using PremierZalTesting.Core.Models;
using PremierZalTesting.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierBankTesting.Application.Services
{
    public class PremierBankTestingServices : IPremierBankTestingServices
    {
        private readonly IPremierBankTestingRepository _premierBankTestingRepository;

        public PremierBankTestingServices(IPremierBankTestingRepository premierBankTestingRepository)
        {
            _premierBankTestingRepository = premierBankTestingRepository;
        }
        public async Task<Client> CheckOrCreateClient(string email)
        {
            return await _premierBankTestingRepository.CheckOrCreateClientByEmail(email);
        }

        public async Task<Guid> CreateNewTransaction(BankTransaction bankTransaction)
        {
            return await _premierBankTestingRepository.CreateTransaction(bankTransaction);
        }
        public async Task<List<BankTransaction>> GetAllUnprocessedTransaction()
        {
            return await _premierBankTestingRepository.GetAllUnProcessed();
        }
        public async Task<Guid> MarkTransactionAsProcessed(Guid id)
        {
            return await _premierBankTestingRepository.MarkAsProcessed(id);
        }
        public async Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsGroupedByUser()
        {
            return await _premierBankTestingRepository.GetMonthProcessedTransactionsByUser();
        }

        public async Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsGroupedByComment()
        {
            return await _premierBankTestingRepository.GetMonthProcessedTransactionsByComment();
        }

        public async Task<int> ImportData()
        {
           return await _premierBankTestingRepository.ImportTransactionsAsync();
        }
    }
}
