using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class CheckingAccount :IAccount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public static double Interest = .04;
        [Required]
        public Boolean Active { get; set; }
        public List<ITransaction> Transactions { get; set; }
    }
}