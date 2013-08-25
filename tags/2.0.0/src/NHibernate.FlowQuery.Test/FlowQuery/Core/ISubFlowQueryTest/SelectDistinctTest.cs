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
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Distinct().Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Distinct().Select(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            var query = Query<UserEntity>().Detached()
                .Where(x => x.IsOnline)
                .Distinct().Select("Id");

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        #endregion Methods
    }
}