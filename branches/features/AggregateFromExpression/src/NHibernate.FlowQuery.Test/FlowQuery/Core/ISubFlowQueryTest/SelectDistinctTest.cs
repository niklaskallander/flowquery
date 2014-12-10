namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class SelectDistinctTest : BaseTest
    {
        [Test]
        public void CanSelectDistinctUsingExpression()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingProjection()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select(Projections.Property("Id"));

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanSelectDistinctUsingString()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Distinct().Select("Id");

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }
    }
}