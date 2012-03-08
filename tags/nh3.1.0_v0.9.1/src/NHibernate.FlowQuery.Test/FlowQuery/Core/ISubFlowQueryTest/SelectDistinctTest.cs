using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SelectDistinctTest : BaseTest
    {
		#region Methods (3) 

        [Test]
        public void CanSelectDistinctUsingExpression()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .SelectDistinct(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .SelectDistinct(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.IsOnline)
                .SelectDistinct("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

		#endregion Methods 
    }
}