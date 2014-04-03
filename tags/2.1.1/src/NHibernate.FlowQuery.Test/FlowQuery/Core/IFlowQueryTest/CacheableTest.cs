using System.Data;
using System.Linq;
using System.Reflection;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.Impl;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    // Tests ported from NHibernate.Test.NHSpecificTest.NH1989.Fixture.cs
    [TestFixture]
    public class CacheableTest : BaseTest
    {
        public override void OnTearDown()
        {
            using (ISession session  = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete("from CacheableUser");

                    transaction.Commit();
                }
            }
        }

        private static void Delete(ISession s)
        {
            using (IDbCommand command = s.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM CacheableUser";
                command.ExecuteNonQuery();
            }
        }

        public override void OnSetup()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var user = new CacheableUser
                    {
                        Name = "test"
                    };

                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        [Test]
        public void SecondLevelCacheWithSingleCacheableFuture()
        {
            using (ISession session  = SessionFactory.OpenSession())
            {
                // Query results should be cached
                CacheableUser user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(user, Is.Not.Null);

                Delete(session);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                CacheableUser user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(user, Is.Not.Null,
                    "entity not retrieved from cache");
            }
        }

        [Test]
        public void SecondLevelCacheWithDifferentRegionsFuture()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                // Query results should be cached
                CacheableUser user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region1")
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(user, Is.Not.Null);

                Delete(session);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                // Query results should be cached
                CacheableUser user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region2")
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(user, Is.Null,
                    "entity from different region should not be retrieved");
            }
        }

        [Test]
        public void SecondLevelCacheWithMixedCacheableAndNonCacheableFuture()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                // Query results should be cached
                var user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                // non cacheable Future causes batch to be non-cacheable
                var userCount = session.DelayedFlowQuery<CacheableUser>()
                    .Count();

                Assert.That(user.Single(), Is.Not.Null);
                Assert.That(userCount.Value, Is.EqualTo(1));

                Delete(session);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                var user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                var userCount = session.DelayedFlowQuery<CacheableUser>()
                    .Count();

                Assert.That(user.SingleOrDefault(), Is.Null,
                    "query results should not come from cache");

                Assert.That(userCount.Value, Is.EqualTo(0));
            }
        }

        [Test]
        public void SecondLevelCacheWithMixedCacheRegionsFuture()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                // Query results should be cached
                var user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region1")
                    .Where(x => x.Name == "test")
                    .Select();

                // non cacheable Future causes batch to be non-cacheable
                var userCount = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region2")
                    .Count();

                Assert.That(user.Single(), Is.Not.Null);
                Assert.That(userCount.Value, Is.EqualTo(1));

                Delete(session);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                var user = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region1")
                    .Where(x => x.Name == "test")
                    .Select();

                var userCount = session.DelayedFlowQuery<CacheableUser>()
                    .Cacheable("region2")
                    .Count();

                Assert.That(user.SingleOrDefault(), Is.Null,
                    "query results should not come from cache");

                Assert.That(userCount.Value, Is.EqualTo(0));
            }
        }

        [Test]
        public void SecondLevelCacheWithSingleCacheableQueryFuture()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                // Query results should be cached
                CacheableUser temp = session.FlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(temp, Is.Not.Null);

                Delete(session);
            }

            using (ISession session = SessionFactory.OpenSession())
            {
                CacheableUser temp = session.FlowQuery<CacheableUser>()
                    .Cacheable()
                    .Where(x => x.Name == "test")
                    .Select();

                Assert.That(
                    temp,
                    Is.Not.Null,
                    "entity not retrieved from cache");
            }
        }

        [Test]
        public void TestPopulatesCriteriaCacheableProperly()
        {
            var query = Session.FlowQuery<CacheableUser>()
                .Cacheable();

            var criteria = (CriteriaImpl)CriteriaHelper
                .BuildCriteria<CacheableUser, CacheableUser>
                (
                    QuerySelection.Create(query as IQueryableFlowQuery)

                );

            Assert.That(criteria.Cacheable, Is.True);
        }

        [Test]
        public void TestPopulatesCriteriaCacheableAndCacheRegionProperly()
        {
            var query = Session.FlowQuery<CacheableUser>()
                .Cacheable("Region1");

            var criteria = (CriteriaImpl)CriteriaHelper
                .BuildCriteria<CacheableUser, CacheableUser>
                (
                    QuerySelection.Create(query as IQueryableFlowQuery)
                );

            Assert.That(criteria.Cacheable, Is.True);
            Assert.That(criteria.CacheRegion, Is.EqualTo("Region1"));
        }

        [Test]
        public void TestPopulatesCriteriaCacheableCachRegionAndCacheModeProperly()
        {
            var query = Session.FlowQuery<CacheableUser>()
                .Cacheable("Region1", CacheMode.Put);

            var criteria = (CriteriaImpl)CriteriaHelper
                .BuildCriteria<CacheableUser, CacheableUser>
                (
                    QuerySelection.Create(query as IQueryableFlowQuery)
                );

            Assert.That(criteria.Cacheable, Is.True);
            Assert.That(criteria.CacheRegion, Is.EqualTo("Region1"));

            var type = typeof(CriteriaImpl);

            var field = type.GetField("cacheMode", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null, "FieldInfo");

            var mode = field
                .GetValue(criteria);

            Assert.That(mode, Is.EqualTo(CacheMode.Put));
        }

        [Test]
        public void TestPopulatesCriteriaCacheableAndCacheModeProperly()
        {
            var query = Session.FlowQuery<CacheableUser>()
                .Cacheable(CacheMode.Put);

            var criteria = (CriteriaImpl)CriteriaHelper
                .BuildCriteria<CacheableUser, CacheableUser>
                (
                    QuerySelection.Create(query as IQueryableFlowQuery)
                );

            Assert.That(criteria.Cacheable, Is.True);
            Assert.That(criteria.CacheRegion, Is.Null);

            var type = typeof(CriteriaImpl);

            var field = type.GetField("cacheMode", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null, "FieldInfo");

            var mode = field
                .GetValue(criteria);

            Assert.That(mode, Is.EqualTo(CacheMode.Put));
        }

        [Test]
        public void TestPopulatesDetachedCriteriaCacheableProperly()
        {
            var detachedCriteria = Session.DetachedFlowQuery<CacheableUser>()
                .Cacheable()
                .Criteria;

            var field = typeof(DetachedCriteria)
                .GetField("impl", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null);

            var impl = (CriteriaImpl)field.GetValue(detachedCriteria);

            Assert.That(impl.Cacheable, Is.True);
        }

        [Test]
        public void TestPopulatesDetachedCriteriaCacheableAndCacheRegionProperly()
        {
            var detachedCriteria = Session.DetachedFlowQuery<CacheableUser>()
                .Cacheable("Region1")
                .Criteria;

            var field = typeof(DetachedCriteria)
                .GetField("impl", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null);

            var impl = (CriteriaImpl)field.GetValue(detachedCriteria);

            Assert.That(impl.Cacheable, Is.True);
            Assert.That(impl.CacheRegion, Is.EqualTo("Region1"));
        }

        [Test]
        public void TestPopulatesDetachedCriteriaCacheableCachRegionAndCacheModeProperly()
        {
            var detachedCriteria = Session.DetachedFlowQuery<CacheableUser>()
                .Cacheable("Region1", CacheMode.Put)
                .Criteria;

            var field = typeof(DetachedCriteria)
                .GetField("impl", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null);

            var impl = (CriteriaImpl)field.GetValue(detachedCriteria);

            Assert.That(impl.Cacheable, Is.True);
            Assert.That(impl.CacheRegion, Is.EqualTo("Region1"));

            var cacheModeField = typeof(CriteriaImpl).GetField("cacheMode", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(cacheModeField, Is.Not.Null);

            var mode = cacheModeField
                .GetValue(impl);

            Assert.That(mode, Is.EqualTo(CacheMode.Put));
        }

        [Test]
        public void TestPopulatesDetachedCriteriaCacheableAndCacheModeProperly()
        {
            var detachedCriteria = Session.DetachedFlowQuery<CacheableUser>()
                .Cacheable(CacheMode.Put)
                .Criteria;

            var field = typeof(DetachedCriteria)
                .GetField("impl", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null);

            var impl = (CriteriaImpl)field.GetValue(detachedCriteria);

            Assert.That(impl.Cacheable, Is.True);
            Assert.That(impl.CacheRegion, Is.Null);

            var cacheModeField = typeof(CriteriaImpl).GetField("cacheMode", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(cacheModeField, Is.Not.Null);

            var mode = cacheModeField
                .GetValue(impl);

            Assert.That(mode, Is.EqualTo(CacheMode.Put));
        }
    }
}