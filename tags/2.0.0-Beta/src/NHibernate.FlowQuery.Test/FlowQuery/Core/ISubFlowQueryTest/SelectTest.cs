using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Criterion;
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

            var aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.Average(u.Id));

            IEnumerable<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThan(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            //sum

            aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.Sum(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThan(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            //min

            aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.Min(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(1));

            //max

            aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.Max(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            //count

            aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.Count(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            //count distinct

            aggregation = Query<UserEntity>().Detached()
                .Select(u => Aggregate.CountDistinct(u.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(4));

            //group by

            aggregation = Query<UserEntity>().Detached()
                .Select(x => Aggregate.GroupBy(x.Id));

            users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(aggregation))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingExpression()
        {
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Select(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingString()
        {
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Select("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        #endregion Methods
    }
}