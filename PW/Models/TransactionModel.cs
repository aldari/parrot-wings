using System;

namespace PW.Models
{
    public class TransactionModel
    {
        public Guid Recipient { get; set; }
        public int Amount { get; set; }
    }
}
