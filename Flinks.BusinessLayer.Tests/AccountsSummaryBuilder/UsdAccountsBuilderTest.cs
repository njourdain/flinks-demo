using System.Linq;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.Repositories.AccountsDetail.Entities;
using Xunit;

namespace Flinks.BusinessLayer.Tests.AccountsSummaryBuilder
{
    public class UsdAccountsBuilderTest
    {
        [Fact]
        public async Task It_ShouldOnlyUseUsdAccounts()
        {
            // Arrange
            var builder = new UsdAccountsBuilder();
            await builder.AddAccountAsync(new Account {Currency = Currency.USD, Balance = new AccountBalance {Current = 1}});
            await builder.AddAccountAsync(new Account {Currency = Currency.CAD, Balance = new AccountBalance {Current = 2}});
            
            // Act
            var accounts = builder.Build();
            
            // Assert
            Assert.Equal(1, accounts.Count());
            Assert.Equal(1, accounts.First().Balance);
        }
        
        [Fact]
        public async Task It_ShouldClearPreviousAccounts()
        {
            // Arrange
            var builder = new UsdAccountsBuilder();
            await builder.AddAccountAsync(new Account {Currency = Currency.USD, Balance = new AccountBalance {Current = 1}});
            
            // Act
            builder.Build();
            builder.ClearAccounts();
            var accounts = builder.Build();
            
            // Assert
            Assert.Empty(accounts);
        }
    }
}