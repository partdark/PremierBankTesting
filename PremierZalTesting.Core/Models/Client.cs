using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierZalTesting.Core.Models
{
    public class Client
    {

        public Client(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; set; }

        public string Email { get; set; }
    }
}
