using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.Repositories.AccountsDetail.Entities;
using Xunit;

namespace Flinks.BusinessLayer.Tests.AccountsSummaryBuilder
{
    public class BiggestCreditTrxIdBuilderTest
    {
        [Fact]
        public async Task It_ShouldReturnDefaultGuidIfNoTransactions()
        {
            // Arrange
            var builder = new BiggestCreditTrxIdBuilder();
            
            // Act
            await builder.AddAccountAsync(new Account());
            var id = builder.Build();
            
            // Assert
            Assert.Equal(default(Guid), id);
        }
        
        [Fact]
        public async Task It_ShouldReturnIdOfTransactionWithBiggestCredit()
        {
            // Arrange
            var builder = new BiggestCreditTrxIdBuilder();
            
            var biggestTransaction = new Transaction {Credit = 2, Id = Guid.NewGuid()};
            
            await builder.AddAccountAsync(new Account
            {
                Transactions = new []
                {
                    new Transaction {Credit = 1, Id = Guid.NewGuid()},
                    biggestTransaction
                }
            });
            
            // Act
            var id = builder.Build();
            
            // Assert
            Assert.Equal(biggestTransaction.Id, id);
        }
        
        [Fact]
        public async Task It_ShouldClearPreviousAccounts()
        {
            // Arrange
            var builder = new BiggestCreditTrxIdBuilder();
            await builder.AddAccountAsync(new Account
            {
                Transactions = new []
                {
                    new Transaction {Credit = 1, Id = Guid.NewGuid()}
                }
            });
            
            // Act
            builder.Build();
            builder.ClearAccounts();
            var id = builder.Build();
            
            // Assert
            Assert.Equal(default(Guid), id);
        }
    }
}