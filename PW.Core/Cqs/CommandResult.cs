using System;

namespace PW.Core.Cqs
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
