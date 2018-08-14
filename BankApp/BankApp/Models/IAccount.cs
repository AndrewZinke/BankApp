using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public interface IAccount
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        Customer Customer { get; set; }
        double Balance { get; set; }
        Boolean Active { get; set; }
        List<ITransaction> Transactions { get; set; }
    }
}
