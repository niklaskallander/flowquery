namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class CountTest : BaseTest
    {
        [Test]
        public void CanCountDistinctOnProperty()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Distinct()
                .Count(x => x.Id);

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Distinct()
                .Count("Id");

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .CountLong();

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProjection()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Count(Projections.Distinct(Projections.Property("Id")));

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnProperty()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Count(x => x.Id);

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Count("Id");

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCountOnStar()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Count();

            IImmediateFlowQuery<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.EqualTo(query));

            Assert.That(users.Count(), Is.EqualTo(1));
        }
    }
}