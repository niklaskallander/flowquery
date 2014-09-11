namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class LimitTest : BaseTest
    {
        [Test]
        public void CanConstrainFirstAndMaxResultsWithTakeAndSkip()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Take(2)
                .Skip(1)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainFirstResultWithLimit()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Limit(2, 1)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithLimit()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Limit(2)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }
    }
}