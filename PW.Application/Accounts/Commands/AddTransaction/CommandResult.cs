using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Application.Accounts.Commands.AddTransaction
{
    public class CommandResult
    {
        public Object Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        // for message transaltion
        public long Code { get; set; }
    }
}
