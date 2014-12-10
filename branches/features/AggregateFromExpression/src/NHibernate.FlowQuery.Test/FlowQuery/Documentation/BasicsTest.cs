namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class BasicsTest : BaseTest
    {
        [Test]
        public virtual void ConstructionExample1()
        {
            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample2WithAlias()
        {
            UserEntity alias = null;

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery(() => alias);

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample3WithOptions()
        {
            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery<UserEntity>(options);

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample4WithAliasAndOptions()
        {
            UserEntity alias = null;

            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            IImmediateFlowQuery<UserEntity> query = Session.FlowQuery(() => alias, options);

            Assert.That(query, Is.Not.Null);
        }
    }
}