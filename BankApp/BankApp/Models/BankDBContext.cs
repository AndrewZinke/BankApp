using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class BankDBContext : DbContext
    {
        public BankDBContext()
            : base("name = BankDBContext")
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
        public DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public DbSet<BusinessTransaction> BusinessTransactions { get; set; }
        public DbSet<CheckingTransaction> CheckingTransaction { get; set; }

        public System.Data.Entity.DbSet<BankApp.Models.Loan> Loans { get; set; }

        public System.Data.Entity.DbSet<BankApp.Models.TermDeposit> TermDeposits { get; set; }

        public System.Data.Entity.DbSet<BankApp.Models.Transfer> Transfers { get; set; }
    }
}