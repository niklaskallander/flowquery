using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class LimitTest : BaseTest
    {
        #region Methods (4)

        [Test]
        public void CanConstrainFirstResultWithLimit()
        {
            var users = Query<UserEntity>()
                .Limit(2, 1)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainFirstResultWithSkip()
        {
            var users = Query<UserEntity>()
                .Skip(1)
                .Select()
                ;

            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithLimit()
        {
            var users = Query<UserEntity>()
                .Limit(2)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanConstrainMaxResultsWithTake()
        {
            var users = Query<UserEntity>()
                .Take(2)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        #endregion Methods
    }
}