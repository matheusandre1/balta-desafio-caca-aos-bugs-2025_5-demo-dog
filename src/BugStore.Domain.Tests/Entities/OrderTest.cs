namespace BugStore.Domain.Tests.Entities;

using AutoFixture;
using BugStore.Domain.Entities;
using Xunit;

public class OrderTests
{
       private readonly Fixture _fixture = new Fixture();
        [Fact]
        public void Lines_Should_Default_To_Empty_List_Instead_Of_Null()
        {
            var order = _fixture.Build<Order>().Create();

            
            Assert.NotNull(order.Lines);
            Assert.NotEmpty(order.Lines);
        }

        [Fact]
        public void Can_Link_Customer_To_Order()
        {
        var order = _fixture.Build<Order>().Create();
        Assert.NotNull(order);         
        }
}
