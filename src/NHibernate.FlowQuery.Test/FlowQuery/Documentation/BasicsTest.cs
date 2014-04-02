using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class BasicsTest : BaseTest
    {

        [Test]
        public virtual void ConstructionExample1()
        {
            var query = Session.FlowQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample2WithAlias()
        {
            UserEntity alias = null;

            var query = Session.FlowQuery(() => alias);

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample3WithOptions()
        {
            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            var query = Session.FlowQuery<UserEntity>(options);

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public virtual void ConstructionExample4WithAliasAndOptions()
        {
            UserEntity alias = null;

            var options = new FlowQueryOptions();

            options.Add(criteria => criteria.SetCacheMode(CacheMode.Refresh));

            var query = Session.FlowQuery(() => alias, options);

            Assert.That(query, Is.Not.Null);
        }
    }
}