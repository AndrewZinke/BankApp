using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class CheckingTransaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        public virtual CheckingAccount Account { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public DateTime DateOfTransaction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Type { get; set; }
    }
}