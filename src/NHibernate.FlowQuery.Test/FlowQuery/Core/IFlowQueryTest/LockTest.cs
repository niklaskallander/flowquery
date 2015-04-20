namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Linq;
    using System.Reflection;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.Impl;

    using NUnit.Framework;

    [TestFixture]
    public class LockTest : BaseTest
    {
        [Test]
        public void CanClearLocks()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Lock().Upgrade()
                .Lock("user").Read()
                .Lock("testAlias").Force();

            var info = (IFlowQuery)query;

            Assert.That(info.Locks.Count, Is.EqualTo(3));

            query.ClearLocks();

            Assert.That(info.Locks.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanGetLockBuilder()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock();

            Assert.That(builder, Is.Not.Null);
        }

        [Test]
        public void CanGetLockBuilderUsingExpressionAlias()
        {
            UserEntity user = null;

            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock(() => user);

            Assert.That(builder, Is.Not.Null);
        }

        [Test]
        public void CanGetLockBuilderUsingStringAlias()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock("user");

            Assert.That(builder, Is.Not.Null);
        }

        [Test]
        public void CanSetLockModeWithAliasUsingExpression()
        {
            UserEntity user = null;

            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock(() => user);

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));
        }

        [Test]
        public void CanSetLockModeWithAliasUsingString()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock("user");

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));
        }

        [Test]
        public void CanSetLockModeWithoutAlias()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock();

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.Null);
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));
        }

        [Test]
        public void CanSetMultipleLocksOnDifferentAliases()
        {
            var query = (IFlowQuery)DummyQuery<UserEntity>()
                .Lock().Upgrade()
                .Lock("user").Read()
                .Lock("testAlias").Force();

            Assert.That(query.Locks.Count, Is.EqualTo(3));

            foreach (Lock lockInfo in query.Locks)
            {
                switch (lockInfo.Alias)
                {
                    case null:
                        Assert.That(lockInfo.LockMode, Is.EqualTo(LockMode.Upgrade));
                        break;

                    case "user":
                        Assert.That(lockInfo.LockMode, Is.EqualTo(LockMode.Read));
                        break;

                    case "testAlias":
                        Assert.That(lockInfo.LockMode, Is.EqualTo(LockMode.Force));
                        break;
                }
            }
        }

        [Test]
        public void CanUpdateLockForAliasSetUsingExpressionByAliasSetUsingString()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Lock(() => user).Upgrade();

            var info = (IFlowQuery)query;

            Assert.That(info.Locks.Count, Is.EqualTo(1));

            Lock only = info.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));

            query.Lock("user").Force();

            Assert.That(info.Locks.Count, Is.EqualTo(1));

            only = info.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Force));
        }

        [Test]
        public void CanUpdateLockForAliasSetUsingStringByAliasSetUsingExpression()
        {
            UserEntity user = null;

            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>()
                .Lock("user").Upgrade();

            var info = (IFlowQuery)query;

            Assert.That(info.Locks.Count, Is.EqualTo(1));

            Lock only = info.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));

            query.Lock(() => user).Force();

            Assert.That(info.Locks.Count, Is.EqualTo(1));

            only = info.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Force));
        }

        [Test]
        public void CanUpdateLockModeWithAliasUsingExpression()
        {
            UserEntity user = null;

            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock(() => user);

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));

            builder.Read();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Read));
        }

        [Test]
        public void CanUpdateLockModeWithAliasUsingString()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock("user");

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));

            builder.Read();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            only = query.Locks.Single();

            Assert.That(only.Alias, Is.EqualTo("user"));
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Read));
        }

        [Test]
        public void CanUpdateLockModeWithoutAlias()
        {
            ILockBuilder<IImmediateFlowQuery<UserEntity>> builder = DummyQuery<UserEntity>()
                .Lock();

            Assert.That(builder, Is.Not.Null);

            var query = (IFlowQuery)builder.Upgrade();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            Lock only = query.Locks.Single();

            Assert.That(only.Alias, Is.Null);
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Upgrade));

            builder.Read();

            Assert.That(query.Locks.Count, Is.EqualTo(1));

            only = query.Locks.Single();

            Assert.That(only.Alias, Is.Null);
            Assert.That(only.LockMode, Is.EqualTo(LockMode.Read));
        }

        [Test]
        public void LocksArePopulatedOnCriteria()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Lock().Upgrade()
                .Lock("user").Read()
                .Lock("testAlias").Force();

            ICriteria criteria = new CriteriaBuilder()
                .Build<UserEntity, UserEntity>(QuerySelection.Create((IQueryableFlowQuery)query));

            Assert.That(criteria, Is.Not.Null);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.LockModes.Keys, Contains.Item("this"));
            Assert.That(impl.LockModes.Keys, Contains.Item("user"));
            Assert.That(impl.LockModes.Keys, Contains.Item("testAlias"));

            Assert.That(impl.LockModes["this"], Is.EqualTo(LockMode.Upgrade));
            Assert.That(impl.LockModes["user"], Is.EqualTo(LockMode.Read));
            Assert.That(impl.LockModes["testAlias"], Is.EqualTo(LockMode.Force));
        }

        [Test]
        public void LocksArePopulatedOnDetachedCriteria()
        {
            var query = DetachedQuery<UserEntity>()
                .Lock().None()
                .Lock("user").Write()
                .Lock("testAlias").UpgradeNoWait() as DetachedFlowQuery<UserEntity>;

            DetachedCriteria detachedCriteria = new CriteriaBuilder()
                .Build<UserEntity>(query);

            Assert.That(detachedCriteria, Is.Not.Null);

            FieldInfo field = typeof(DetachedCriteria)
                .GetField("impl", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(field, Is.Not.Null);

            // ReSharper disable once PossibleNullReferenceException
            object criteria = field
                .GetValue(detachedCriteria);

            var impl = (CriteriaImpl)criteria;

            Assert.That(impl.LockModes.Keys, Contains.Item("this"));
            Assert.That(impl.LockModes.Keys, Contains.Item("user"));
            Assert.That(impl.LockModes.Keys, Contains.Item("testAlias"));

            Assert.That(impl.LockModes["this"], Is.EqualTo(LockMode.None));
            Assert.That(impl.LockModes["user"], Is.EqualTo(LockMode.Write));
            Assert.That(impl.LockModes["testAlias"], Is.EqualTo(LockMode.UpgradeNoWait));
        }
    }
}