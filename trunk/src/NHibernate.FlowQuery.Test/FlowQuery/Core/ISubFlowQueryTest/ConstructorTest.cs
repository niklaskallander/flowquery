using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ConstructorTest : BaseTest
    {
        #region Methods (3)

        [Test]
        public void CanConstruct()
        {
            var query = SubQuery.For<UserEntity>();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void CanConstructFromDetachedCriteria()
        {
            var query = DetachedCriteria.For<UserEntity>().SubQuery<UserEntity>();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsIfDetachedCriteriaIsNull()
        {
            DetachedCriteria criteria = null;

            Assert.That(() => { criteria.SubQuery<UserEntity>(); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SetRootAliasThrowsIfExpressionIsNull()
        {
            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>().SetRootAlias<UserEntity>(null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        #endregion Methods
    }
}