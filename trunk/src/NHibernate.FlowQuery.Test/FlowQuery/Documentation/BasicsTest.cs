using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using xIs = NUnit.Framework.Is;

    [TestFixture]
    public class BasicsTest : BaseTest
    {

        [Test]
        public virtual void ConstructionExample1()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>();

            Assert.That(query, xIs.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample2WithAlias()
        {
            ISession session = Session;

            UserEntity alias = null;

            var query = session.FlowQuery<UserEntity>(() => alias);

            Assert.That(query, xIs.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample3WithOptions()
        {
            ISession session = Session;

            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            var query = session.FlowQuery<UserEntity>(options);

            Assert.That(query, xIs.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample4WithAliasAndOptions()
        {
            ISession session = Session;

            UserEntity alias = null;

            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            var query = session.FlowQuery<UserEntity>(() => alias, options);

            Assert.That(query, xIs.Not.Null);
        }
    }
}
