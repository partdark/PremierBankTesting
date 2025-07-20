using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PremierBankTesting.Bank.Models;

namespace PremierBankTesting.Bank;

public  class BankApiClient : IBankApiClient
{

    public  Task<List<BankTransaction>> GetRecentTransactionsAsync ()
    {
        return Task.FromResult(new List<BankTransaction>
         {
             new(

                 Guid.NewGuid(), 1000,  "Пополнение",  DateTime.UtcNow,
                  "ivanov@test.com"
             ),
             new(

                  Guid.NewGuid(),  2000,  "Оплата", DateTime.UtcNow,
                 "unknown@test.com"
             ),
             new(

                 Guid.NewGuid(), -500,  "Списание",  DateTime.UtcNow,
                 "unknown@test.com"
             ),
             new(

                  Guid.NewGuid(),  1000,  "Оплата",  DateTime.UtcNow.AddDays(-3),
                  "ivanov@test.com"
             ),
             new(

                  Guid.NewGuid(),  1500,  "Пополнение",
                  DateTime.UtcNow.AddDays(-10),  "petrov@test.com"
             ),
             new(

                 Guid.NewGuid(),  500,  "Подарок",  DateTime.UtcNow.AddDays(-15),
                  "ivanov@test.com"
             )
         });
    }
}