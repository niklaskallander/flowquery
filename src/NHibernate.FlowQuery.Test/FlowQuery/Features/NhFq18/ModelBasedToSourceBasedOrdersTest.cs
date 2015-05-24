namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq18
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ModelBasedToSourceBasedOrdersTest : BaseTest
    {
        private static Expression<Func<UserEntity, UserDto>> Projection
        {
            get
            {
                return x => new UserDto
                {
                    SomeValue = x.Firstname + " " + x.Lastname
                };
            }
        }

        [Test]
        public void Given_NullAsProjection_When_AskedToTransformOrders_OrderHelperThrowsArgNullException()
        {
            var statements = new OrderByStatement[0];
            var data = new QueryHelperData(new Dictionary<string, string>(), new List<Join>());

            Assert.That(() => OrderHelper.GetSourceBasedOrdersFrom(statements, null, data), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Given_NullAsData_When_AskedToTransformOrders_OrderHelperThrowsArgNullException()
        {
            var statements = new OrderByStatement[0];

            Assert.That(() => OrderHelper.GetSourceBasedOrdersFrom(statements, Projection, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Given_NullAsOrderByStatements_When_AskedToTransformOrders_OrderHelperThrowsArgNullException()
        {
            var data = new QueryHelperData(new Dictionary<string, string>(), new List<Join>());

            Assert.That(() => OrderHelper.GetSourceBasedOrdersFrom(null, Projection, data), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void Given_AllSourceBasedListOfOrderByStatements_When_AskedToTransformOrders_OrderHelperReturnsGivenRefernce()
        {
            var data = new QueryHelperData(new Dictionary<string, string>(), new List<Join>());

            var orders = new[]
            {
                new OrderByStatement
                {
                    IsBasedOnSource = true
                }
            };

            var newOrders = OrderHelper.GetSourceBasedOrdersFrom(orders, Projection, data);

            Assert.That(newOrders, Is.SameAs(orders));
        }

        [Test]
        public void Given_MixedListOfOrderByStatements_When_AskedToTransformOrders_OrderHelperAlreadySourceBasedOrdersAreAlsoReturnedAndInCorrectOrder()
        {
            var data = new QueryHelperData(new Dictionary<string, string>(), new List<Join>());

            var orders = new[]
            {
                new OrderByStatement
                {
                    IsBasedOnSource = true
                },

                new OrderByStatement
                {
                    IsBasedOnSource = false,
                    Property = "SomeValue"
                }
            };

            var newOrders = OrderHelper.GetSourceBasedOrdersFrom(orders, Projection, data);

            Assert.That(newOrders.Count(), Is.EqualTo(2));
            Assert.That(newOrders.ElementAt(0).Order, Is.Null);
            Assert.That(newOrders.ElementAt(1).Order, Is.Not.Null);
        }

        [Test, Category("MySqlUnsupported")]
        public void Given_DelayedQueryWithModelBasedOrders_When_LaterUsedAsSubquery_IsSortedByAppropriateSourceProjection()
        {
            IDelayedFlowQuery<UserEntity> query = Query<UserEntity>()
                .OrderBy<UserDto>(x => x.SomeValue)
                .Delayed();

            FlowQuerySelection<UserDto> allUsers = query
                .Select(Projection);

            IDetachedFlowQuery<UserEntity> subquery = query
                .Detached()
                .Limit(2, 1)
                .Select(x => x.Id);

            FlowQuerySelection<UserDto> subsetOfUsers = Query<UserEntity>()
                .Delayed()
                .Where(x => x.Id, NHibernate.FlowQuery.Is.In(subquery))
                .Select(Projection);

            Assert.That(allUsers.Count(), Is.EqualTo(4));
            Assert.That(subsetOfUsers.Count(), Is.EqualTo(2));
            Assert.That(subsetOfUsers.ElementAt(0).SomeValue, Is.EqualTo("Lars Wilk"));
            Assert.That(subsetOfUsers.ElementAt(1).SomeValue, Is.EqualTo("Lotta Brak"));
        }

        [Test, Category("MySqlUnsupported")]
        public void Given_SubQueryWithModelBasedOrders_When_UsingUglyHack_Then_IsSortedByAppropriateSourceProjection()
        {
            IDetachedFlowQuery<UserEntity> subquery = Query<UserEntity>()
                .OrderBy<UserDto>(x => x.SomeValue)
                .Detached()
                .Limit(2, 1)
                .Select(x => x.Id);

            var subqueryAsFlowQuery = subquery as IFlowQuery;

            Assert.That(subqueryAsFlowQuery != null);

            OrderByStatement[] orders = subqueryAsFlowQuery.Orders.ToArray();
            QueryHelperData data = subqueryAsFlowQuery.Data;

            subquery.ClearOrders();

            IEnumerable<OrderByStatement> newOrders = OrderHelper.GetSourceBasedOrdersFrom(orders, Projection, data);

            subqueryAsFlowQuery.Orders.AddRange(newOrders);

            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Where(x => x.Id, NHibernate.FlowQuery.Is.In(subquery))
                .Select(Projection);

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Lars Wilk"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta Brak"));
        }
    }
}