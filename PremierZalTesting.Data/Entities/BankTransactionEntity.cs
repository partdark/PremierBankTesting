using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierZalTesting.Data.Entities
{
    public class BankTransactionEntity
    {



        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserEmail { get; set; }

        public bool IsProcessed { get; set; } = false;

        public ClientEntity Client { get; set; }




    }
}
