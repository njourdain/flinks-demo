using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.BusinessLayer.Entities;
using Flinks.Repositories.AccountsDetail.Entities;
using Moq;
using Xunit;

namespace Flinks.BusinessLayer.Tests.AccountsSummaryBuilder
{
    public class AccountsSummaryBuilderTest
    {
        [Fact]
        public async Task It_ShouldReturnDefaultAccountsSummaryIfThereAreNoAccounts()
        {
            var requestId = Guid.NewGuid();
            var loginId = Guid.NewGuid();

            var builder = new Builders.AccountsSummaryBuilder(null, null, null);
            
            // Act
            var summary = await builder.UseRequestId(requestId)
                .UseLoginId(loginId)
                .BuildAsync();
            
            // Assert
            Assert.Equal(requestId, summary.RequestId);
            Assert.Equal(loginId, summary.LoginId);
        }
        
        [Fact]
        public async Task It_ShouldClearPreviousAccountsBeforeAddingToInnerBuilders()
        {
            // Arrange
            var opsSequence = new MockSequence();
            var mockOpsBuilder = new Mock<IOperationAccountsBuilder>(MockBehavior.Strict);
            
            mockOpsBuilder.InSequence(opsSequence).Setup(b => b.ClearAccounts());
            mockOpsBuilder.InSequence(opsSequence).Setup(b => b.AddAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);
            mockOpsBuilder.Setup(b => b.Build()).Returns(new OperationAccount[0]);
            
            var usdSequence = new MockSequence();
            var mockUsdAccountsBuilder = new Mock<IUsdAccountsBuilder>(MockBehavior.Strict);
            
            mockUsdAccountsBuilder.InSequence(usdSequence).Setup(b => b.ClearAccounts());
            mockUsdAccountsBuilder.InSequence(usdSequence).Setup(b => b.AddAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);
            mockUsdAccountsBuilder.Setup(b => b.Build()).Returns(new UsdAccount[0]);
            
            var bigSequence = new MockSequence();
            var mockBiggestTrxBuilder = new Mock<IBiggestCreditTrxIdBuilder>(MockBehavior.Strict);
            
            mockBiggestTrxBuilder.InSequence(bigSequence).Setup(b => b.ClearAccounts());
            mockBiggestTrxBuilder.InSequence(bigSequence).Setup(b => b.AddAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);
            mockBiggestTrxBuilder.Setup(b => b.Build()).Returns(Guid.Empty);

            var builder =
                new Builders.AccountsSummaryBuilder(mockOpsBuilder.Object, mockUsdAccountsBuilder.Object, mockBiggestTrxBuilder.Object);

            builder.UseAccounts(new[] {new Account()});
            
            // Act
            await builder.BuildAsync();
            
            // Assert - test will fail if mocked methods are called in correct order
            
        }
        
        [Fact]
        public async Task It_ShouldUseTheFirstAccountForTheHolderNameAndEmail()
        {
            // Arrange
            var mockOpsBuilder = Mock.Of<IOperationAccountsBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask);
            
            var mockUsdAccountsBuilder = Mock.Of<IUsdAccountsBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask);
            
            var mockBiggestTrxBuilder = Mock.Of<IBiggestCreditTrxIdBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask);

            var builder =
                new Builders.AccountsSummaryBuilder(mockOpsBuilder, mockUsdAccountsBuilder, mockBiggestTrxBuilder);

            builder.UseAccounts(new[] 
            {
                new Account
                {
                    Holder = new AccountHolder
                    {
                        Email = "mockEmail", Name = "mockName"
                    }
                },
                new Account
                {
                    Holder = new AccountHolder
                    {
                        Email = "mockEmail2", Name = "mockName2"
                    }
                }
            });
            
            // Act
            var summary = await builder.BuildAsync();

            // Assert
            Assert.Equal("mockEmail", summary.Holder.Email);
            Assert.Equal("mockName", summary.Holder.Name);
        }
        
        [Fact]
        public async Task It_ShouldUseInnerBuildersResultsInSummary()
        {
            // Arrange
            var mockOpsAccounts = new OperationAccount[0];
            var mockOpsBuilder = Mock.Of<IOperationAccountsBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask &&
                b.Build() == mockOpsAccounts);

            var mockUsdAccounts = new UsdAccount[0];
            var mockUsdAccountsBuilder = Mock.Of<IUsdAccountsBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask &&
                b.Build() == mockUsdAccounts);

            var mockBiggestTrx = Guid.NewGuid();
            var mockBiggestTrxBuilder = Mock.Of<IBiggestCreditTrxIdBuilder>(b =>
                b.AddAccountAsync(It.IsAny<Account>()) == Task.CompletedTask &&
                b.Build() == mockBiggestTrx);

            var builder =
                new Builders.AccountsSummaryBuilder(mockOpsBuilder, mockUsdAccountsBuilder, mockBiggestTrxBuilder);

            builder.UseAccounts(new[] {new Account()});
            
            // Act
            var summary = await builder.BuildAsync();
            
            // Assert
            Assert.Equal(mockOpsAccounts, summary.OperationAccounts);
            Assert.Equal(mockUsdAccounts, summary.USDAccounts);
            Assert.Equal(mockBiggestTrx, summary.BiggestCreditTrxId);
        }
    }
}