using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class LimitTest : BaseTest
    {
        #region Methods (3)

        [Test]
        public void CanConstrainFirstAndMaxResultsWithTakeAndSkip()
        {
            var query = Query<UserEntity>().Detached()
                .Take(2)
                .Skip(1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainFirstResultWithLimit()
        {
            var query = Query<UserEntity>().Detached()
                .Limit(2, 1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithLimit()
        {
            var query = Query<UserEntity>().Detached()
                .Limit(2)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        #endregion Methods
    }
}