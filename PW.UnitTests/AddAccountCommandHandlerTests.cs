using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace PW.UnitTests
{
    //[TestFixture]
    //public class AddAccountCommandHandlerTests
    //{
    //    public async Task ExecuteReduceCreditAccountBy120IncreaseDebitAccountBy120()
    //    {
    //        // Arrange
    //        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    //            .UseInMemoryDatabase("ExecuteReduceCreditAccountBy120IncreaseDebitAccountBy120")
    //            .Options;

    //        var context = new ApplicationDbContext(options);

    //        var creditAccount = Guid.NewGuid();
    //        var debitAccount = Guid.NewGuid();
    //        IClock clock = new Clock();

    //        var command = new AddTransactionCommand(creditAccount, debitAccount, 120);
    //        var handler = new AddTransactionCommandHandler(context, clock);

    //        var result = await handler.ExecuteAsync(command);

    //        Assert.AreEqual(1, 1);
    //    }
    //}
}
