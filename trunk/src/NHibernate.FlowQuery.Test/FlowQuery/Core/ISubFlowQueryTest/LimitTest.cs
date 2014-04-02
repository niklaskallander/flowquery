using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class LimitTest : BaseTest
    {
        [Test]
        public void CanConstrainFirstAndMaxResultsWithTakeAndSkip()
        {
            var query = DetachedQuery<UserEntity>()
                .Take(2)
                .Skip(1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainFirstResultWithLimit()
        {
            var query = DetachedQuery<UserEntity>()
                .Limit(2, 1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithLimit()
        {
            var query = DetachedQuery<UserEntity>()
                .Limit(2)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }
    }
}