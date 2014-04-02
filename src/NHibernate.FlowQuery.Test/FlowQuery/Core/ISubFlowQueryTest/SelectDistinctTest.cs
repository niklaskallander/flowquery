using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SelectDistinctTest : BaseTest
    {
        [Test]
        public void CanSelectDistinctUsingExpression()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select(new[] { "Id" });

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }
    }
}