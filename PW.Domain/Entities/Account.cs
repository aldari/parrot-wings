using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
