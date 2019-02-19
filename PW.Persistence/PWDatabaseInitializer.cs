using PW.Domain.Entities;
using System;
using System.Linq;

namespace PW.Persistence
{
    public class PWDatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            var initializer = new PWDatabaseInitializer();
            initializer.Seed(context);
        }

        public void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Accounts.Any())
            {
                return; // Db has been seeded
            }

            var fakeSystemAccountUserId = Guid.NewGuid();
            if (!context.Accounts.Any(x => x.Name == "System Account"))
            {
                context.Accounts.Add(new Account
                {
                    Id = AccountConst.SystemAccountGuid,
                    Name = "System Account",
                    UserId = fakeSystemAccountUserId
                });
            }

            context.SaveChanges();
        }
    }
}
