using System.Linq;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.Repositories.AccountsDetail.Entities;
using Xunit;

namespace Flinks.BusinessLayer.Tests.AccountsSummaryBuilder
{
    public class OperationAccountsBuilderTest
    {
        [Fact]
        public async Task It_ShouldOnlyUseOperationAccounts()
        {
            // Arrange
            var builder = new OperationAccountsBuilder();
            await builder.AddAccountAsync(new Account {Category = AccountCategory.Operations, AccountNumber = "1"});
            await builder.AddAccountAsync(new Account {Category = AccountCategory.Credits, AccountNumber = "2"});
            await builder.AddAccountAsync(new Account {Category = AccountCategory.Products, AccountNumber = "3"});
            
            // Act
            var accounts = builder.Build();
            
            // Assert
            Assert.Equal(1, accounts.Count());
            Assert.Equal(1, accounts.First().AccountNumber);
        }

        [Fact]
        public async Task It_ShouldSkipAccount_WhenAccountNumberIsNotANumber()
        {
            // Arrange
            var builder = new OperationAccountsBuilder();
            await builder.AddAccountAsync(new Account {Category = AccountCategory.Operations, AccountNumber = "mock"});
            
            // Act
            var accounts = builder.Build();
            
            // Assert
            Assert.Empty(accounts);
        }
        
        [Fact]
        public async Task It_ShouldClearPreviousAccounts()
        {
            // Arrange
            var builder = new OperationAccountsBuilder();
            await builder.AddAccountAsync(new Account {Category = AccountCategory.Operations, AccountNumber = "1"});
            
            // Act
            builder.Build();
            builder.ClearAccounts();
            var accounts = builder.Build();
            
            // Assert
            Assert.Empty(accounts);
        }
    }
}