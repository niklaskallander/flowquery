namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class SelectTest : BaseTest
    {
        [Test]
        public void CanSelectAggregationUsingAggregateHelper()
        {
            // avg
            IDetachedFlowQuery<UserEntity> aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.Average(u.Id));

            IEnumerable<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.GreaterThan(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            // sum
            aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.Sum(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.LessThan(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            // min
            aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.Min(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(1));

            // max
            aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.Max(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            // count
            aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.Count(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            // count distinct
            aggregation = DetachedQuery<UserEntity>()
                .Select(u => Aggregate.CountDistinct(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            // group by
            aggregation = DetachedQuery<UserEntity>()
                .Select(x => Aggregate.GroupBy(x.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(aggregation))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingExpression()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(Projections.Property("Id"));

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingString()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select("Id");

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }
    }
}