using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }
        public double Amount { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public bool Active { get; set; }
    }
}