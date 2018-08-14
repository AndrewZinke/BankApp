using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public interface ITransaction
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; }
        DateTime DateOfTransaction { get; set; }
        double Amount { get; set; }
        int AccountId { get; set; }
    }
}
