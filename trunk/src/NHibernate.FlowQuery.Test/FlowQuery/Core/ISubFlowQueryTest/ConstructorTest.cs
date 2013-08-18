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
            var query = Query<UserEntity>().Detached();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void CanConstructFromDetachedCriteria()
        {
            var query = DetachedCriteria.For<UserEntity>()
                .DetachedFlowQuery();

            Assert.That(query, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsIfDetachedCriteriaIsNull()
        {
            DetachedCriteria criteria = null;

            Assert.That(() => { criteria.DetachedFlowQuery(); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SetRootAliasThrowsIfExpressionIsNull()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached().SetRootAlias<UserEntity>(null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        #endregion Methods
    }
}