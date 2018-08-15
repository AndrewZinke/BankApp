using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApp.Models
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int AccountOneId { get; set; }
        public virtual IAccount AccountOne { get; set; }
        public int AccountTwoId { get; set; }
        public virtual IAccount AccountTwo { get; set; }

    }
}