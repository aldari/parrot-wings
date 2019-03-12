using System;
using System.ComponentModel.DataAnnotations;

namespace PW.Models
{
    public class TransactionModel
    {
        [Required]
        public Guid Recipient { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
