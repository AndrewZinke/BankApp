using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class TermDeposit
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateOpened { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public double InterestRate { get; set; }
        public bool HasMatured { get; set; }
    }
}