using System.Collections.Generic;
using System.Threading.Tasks;
using PremierBankTesting.Bank.Models;

namespace PremierBankTesting.Bank;

public interface IBankApiClient
{
    public Task<List<BankTransaction>> GetRecentTransactionsAsync();
}