using System;

namespace PW.Models
{
    public class UserVm
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
