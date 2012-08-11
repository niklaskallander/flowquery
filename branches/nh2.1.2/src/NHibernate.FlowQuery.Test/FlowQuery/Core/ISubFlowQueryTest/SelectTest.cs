using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SelectTest : BaseTest
    {
        #region Methods (3)

        [Test]
        public void CanSelectAggregationUsingAggregateHelper()
        {
            //avg

            var aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.Average(u.Id));

            UserEntity[] users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThan(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(2));

            //sum

            aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.Sum(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThan(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(4));

            //min

            aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.Min(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(1));

            //max

            aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.Max(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            //count

            aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.Count(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            //count distinct

            aggregation = SubQuery.For<UserEntity>()
                .Select(u => Aggregate.CountDistinct(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Length, Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingExpression()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingString()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        #endregion Methods
    }
}