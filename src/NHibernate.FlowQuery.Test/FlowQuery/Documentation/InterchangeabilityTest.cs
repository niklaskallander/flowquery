using System.Linq;
using NHibernate.Engine;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class InterchangeabilityTest : BaseTest
    {
        [Test]
        public void DefinitionExample1()
        {
            var query = Session.FlowQuery<UserEntity>()
                .Delayed()
                .Where(x => x.IsOnline);

            var count = query.Count();

            var users = query
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
        public void ReusabilityMechanismsCopy()
        {
            var baseQuery = Session.DelayedFlowQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .OrderBy(x => x.Username);

            var onlineAdministrators = baseQuery
                .Copy()
                .Where(x => x.Role == RoleEnum.Administrator)
                .Select();

            var onlineNonAdministrators = baseQuery
                .Copy()
                .Where(x => x.Role != RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), Is.EqualTo(2));
            Assert.That(onlineNonAdministrators.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ReusabilityMechanismsClearRestrictions()
        {
            var query = Session.DelayedFlowQuery<UserEntity>()
                .OrderBy(x => x.Username);

            var onlineAdministrators = query
                .Where(x => x.IsOnline && x.Role == RoleEnum.Administrator)
                .Select();

            var onlineNonAdministrators = query
                .ClearRestrictions()
                .Where(x =>  x.IsOnline && x.Role != RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), Is.EqualTo(2));
            Assert.That(onlineNonAdministrators.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ReusabilityMechanismsClear()
        {
            var query = DummyQuery<UserEntity>();

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
    }
}