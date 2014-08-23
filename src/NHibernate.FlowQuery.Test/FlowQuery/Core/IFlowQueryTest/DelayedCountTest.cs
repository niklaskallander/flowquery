namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.Engine;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class DelayedCountTest : BaseTest
    {
        [Test]
        public void CanCountDistinctOnProperty()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Distinct()
                .Count(u => u.IsOnline);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Distinct()
                .Count("IsOnline");

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            Lazy<long> userCount = Query<UserEntity>()
                .Delayed()
                .CountLong();

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(4));
            Assert.That(users.Count(), Is.EqualTo(userCount.Value));
        }

        [Test]
        public void CanCountOnProjection()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count(Projections.Distinct(Projections.Property("IsOnline")));

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnProperty()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count(u => u.IsOnline);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count("IsOnline");

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnStar()
        {
            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count();

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }
    }
}