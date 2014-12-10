namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class LimitTest : BaseTest
    {
        [Test]
        public void CanConstrainFirstResultWithLimit()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Limit(2, 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainFirstResultWithSkip()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Skip(1)
                .Select();

            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithLimit()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Limit(2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithTake()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Take(2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }
    }
}