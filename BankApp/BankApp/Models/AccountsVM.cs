using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class AccountsVM
    {
        public int CustomerId { get; set; }
        public string CustomerFN { get; set; }
        public string CustomerLN { get; set; }

        IList<IAccount> Accounts { get; set; }
    }
}