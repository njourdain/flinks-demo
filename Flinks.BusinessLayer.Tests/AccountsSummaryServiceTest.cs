using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.BusinessLayer.Entities;
using Flinks.Repositories.AccountsDetail;
using Flinks.Repositories.AccountsDetail.Entities;
using Flinks.Repositories.Common;
using Moq;
using Xunit;

namespace Flinks.BusinessLayer.Tests
{
    public class AccountsSummaryServiceTest
    {
        [Fact]
        public async Task It_ShouldReturnAccountSummary_UsingTransactionsOfLast90Days()
        {
            // Arrange
            var mockAccountsDetailRepository = new Mock<IAccountsDetailRepository>();
            mockAccountsDetailRepository.Setup(repo => repo.GetAccountsDetailAsync(It.IsAny<AccountsDetailRequest>()))
                .ReturnsAsync(new AccountsDetailResponse { Login = new LoginDetails() });

            var mockAccountsSummaryBuilder = Mock.Of<IAccountsSummaryBuilder>(b =>
                b.UseAccounts(It.IsAny<Account[]>()) == b &&
                b.UseLoginId(It.IsAny<Guid>()) == b &&
                b.UseRequestId(It.IsAny<Guid>()) == b &&
                b.BuildAsync() == Task.FromResult(new AccountsSummary()));
            
            var service = new AccountsSummaryService(mockAccountsDetailRepository.Object, mockAccountsSummaryBuilder);
            
            // Act
            await service.GetAccountsSummaryAsync(Guid.Empty);
            
            // Assert
            mockAccountsDetailRepository.Verify(m => m.GetAccountsDetailAsync(
                It.Is<AccountsDetailRequest>(r => r.DaysOfTransactions == DaysOfTransaction.Days90)));
        }
    }
}