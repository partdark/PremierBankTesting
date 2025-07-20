using PremierBankTesting.Bank.Models;
using PremierZalTesting.Core.Models;

namespace PremierBankTesting.Application.Services
{
    public interface IPremierBankTestingServices
    {
        Task<Client> CheckOrCreateClient(string email);
        Task<Guid> CreateNewTransaction(BankTransaction bankTransaction);
        Task<List<BankTransaction>> GetAllUnprocessedTransaction();
        Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsGroupedByComment();
        Task<Dictionary<string, decimal>> GetMonthProcessedTransactionsGroupedByUser();
        Task<Guid> MarkTransactionAsProcessed(Guid id);
        Task<int> ImportData();
    }
}