namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class UtilitiesTest : BaseTest
    {
        [Test]
        public void SetCacheableExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Cacheable()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCacheableExample2()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Cacheable("Region1")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCacheableExample3()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Cacheable("Region1", CacheMode.Normal)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCacheableExample4()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Cacheable(CacheMode.Normal)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCacheableExample5()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Cacheable(false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCommentExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Comment("This is an example comment.")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetCommentExample2()
        {
            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .Comment("This is an example comment.");

            // ... code ...
            FlowQuerySelection<UserEntity> users = query
                .Comment(null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetFetchModeExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Fetch(x => x.Groups).Eagerly()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(6));
        }

        [Test]
        public void SetFetchModeExample2()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Fetch(x => x.Groups, () => groupLink).Eagerly()
                .Fetch(x => groupLink.Group).Eagerly()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(6));

            foreach (var user in users)
            {
                Assert.That(NHibernateUtil.IsInitialized(user.Groups));

                foreach (var group in user.Groups)
                {
                    Assert.That(NHibernateUtil.IsInitialized(group.Group));
                }
            }
        }

        [Test]
        public void SetFetchModeExample3()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Fetch("Groups.Group").Eagerly()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(6));

            foreach (var user in users)
            {
                Assert.That(NHibernateUtil.IsInitialized(user.Groups));

                foreach (var group in user.Groups)
                {
                    Assert.That(NHibernateUtil.IsInitialized(group.Group));
                }
            }
        }

        [Test]
        public void SetFetchModeExample4()
        {
            UserGroupLinkEntity groupLink = null;

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .Fetch(x => x.Groups, () => groupLink).Eagerly()
                .Fetch(x => groupLink.Group.Customers).Eagerly();

            // ... code ...
            FlowQuerySelection<UserEntity> users = query
                .ClearFetches()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetFetchSizeExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .FetchSize(50)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetFetchSizeExample2()
        {
            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .FetchSize(50);

            // ... code ...
            FlowQuerySelection<UserEntity> users = query
                .FetchSize(0)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetLockModeExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Lock().Write()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetLockModeExample2()
        {
            UserEntity user = null;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>(() => user)
                .Lock(() => user).Write()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetLockModeExample3()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Lock(() => groupLink).Write()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void SetLockModeExample4()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Lock("groupLink").Write()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void SetLockModeExample5()
        {
            UserGroupLinkEntity groupLink = null;

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Lock().Write()
                .Lock(() => groupLink).Write();

            // ... code ...
            FlowQuerySelection<UserEntity> users = query
                .ClearLocks()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void SetReadOnlyExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .ReadOnly()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetReadOnlyExample2()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .ReadOnly(false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetTimeoutExample1()
        {
            FlowQuerySelection<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Timeout(10)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SetTimeoutExample2()
        {
            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>()
                .Timeout(10);

            // ... code ...
            FlowQuerySelection<UserEntity> users = query
                .ClearTimeout()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }
    }
}