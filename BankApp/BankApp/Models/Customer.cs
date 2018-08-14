using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public List<IAccount> CheckingAccounts;
        public List<IAccount> BusinessAccounts;
    }
}