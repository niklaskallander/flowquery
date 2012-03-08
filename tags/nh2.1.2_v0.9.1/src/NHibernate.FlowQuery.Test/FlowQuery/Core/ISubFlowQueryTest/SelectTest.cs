using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SelectTest : BaseTest
    {
		#region Methods (3) 

        [Test]
        public void CanSelectUsingExpression()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectUsingString()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .Select("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

		#endregion Methods 
    }
}