using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Core
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
