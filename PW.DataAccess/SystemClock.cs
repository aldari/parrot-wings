using System;
using PW.Core;

namespace PW.DataAccess
{
    public class Clock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
