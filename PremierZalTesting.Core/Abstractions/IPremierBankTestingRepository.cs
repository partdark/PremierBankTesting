using PremierBankTesting.Bank.Models;
using PremierZalTesting.Core.Models;

namespace PremierZalTesting.Data.Repository
{
    public interface IPremierBankTestingRepository
    {
        Task<Client> CheckOrCreateClientByEmail(string email);
        Task<Guid> CreateTransaction(BankTransaction bankTransaction);
        Task<List<BankTransaction>> GetAllUnProcessed();
        Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsByComment();
        Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsByUser();
        Task<Guid> MarkAsProcessed(Guid id);
        Task<int> ImportTransactionsAsync();
    }
}