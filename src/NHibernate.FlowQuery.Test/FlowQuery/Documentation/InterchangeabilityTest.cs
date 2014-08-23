namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using System;
    using System.Linq;

    using NHibernate.Engine;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class InterchangeabilityTest : BaseTest
    {
        [Test]
        public void DefinitionExample1()
        {
            IDelayedFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .Delayed()
                .Where(x => x.IsOnline);

            Lazy<int> count = query.Count();

            FlowQuerySelection<UserEntity> users = query
                .Take(10)
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            Assert.That(sessionImpl, Is.Not.Null);

            int queryCount = 0;

            Assert.That(() => queryCount = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(queryCount, Is.EqualTo(2));

            Assert.That(count.Value, Is.EqualTo(3));
            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ReusabilityMechanismsClear()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            // reset fetch instructions specified using query.Fetch(..)
            query.ClearFetches();

            // reset group by columns specified using query.GroupBy(..)
            query.ClearGroupBys();

            // reset joins specified using query.Inner/Full/LeftOuter/RightOuter
            query.ClearJoins();

            // reset limit/take/skip specified using query.Take(..)/Skip(..)/Limit(..)
            query.ClearLimit();

            // reset locks specified using query.Lock(..)
            query.ClearLocks();

            // reset orders specified using query.OrderBy(..)/OrderByDescending(..)
            query.ClearOrders();

            // reset restrictions specified using query.Where(..)/And(..)/RestrictByExample(..)
            query.ClearRestrictions();

            // reset timeout specified using query.Timeout(..)/TimeoutAfter(..)
            query.ClearTimeout();
        }

        [Test]
        public void ReusabilityMechanismsClearRestrictions()
        {
            IDelayedFlowQuery<UserEntity> query = Session.DelayedFlowQuery<UserEntity>()
                .OrderBy(x => x.Username);

            FlowQuerySelection<UserEntity> onlineAdministrators = query
                .Where(x => x.IsOnline && x.Role == RoleEnum.Administrator)
                .Select();

            FlowQuerySelection<UserEntity> onlineNonAdministrators = query
                .ClearRestrictions()
                .Where(x => x.IsOnline && x.Role != RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), Is.EqualTo(2));
            Assert.That(onlineNonAdministrators.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ReusabilityMechanismsCopy()
        {
            IDelayedFlowQuery<UserEntity> baseQuery = Session.DelayedFlowQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .OrderBy(x => x.Username);

            FlowQuerySelection<UserEntity> onlineAdministrators = baseQuery
                .Copy()
                .Where(x => x.Role == RoleEnum.Administrator)
                .Select();

            FlowQuerySelection<UserEntity> onlineNonAdministrators = baseQuery
                .Copy()
                .Where(x => x.Role != RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), Is.EqualTo(2));
            Assert.That(onlineNonAdministrators.Count(), Is.EqualTo(1));
        }
    }
}