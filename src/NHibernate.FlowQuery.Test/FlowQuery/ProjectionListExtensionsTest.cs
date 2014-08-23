namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectionListExtensionsTest
    {
        [Test]
        public void AddPropertiesUsingStringsThrowsIfListIsNull()
        {
            Assert
                .That
                (
                    () => ((ProjectionList)null).AddProperties("Id", "CreatedStamp"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void AddPropertiesUsingExpressionsThrowsIfListIsNull()
        {
            Assert
                .That
                (
                    () => ((ProjectionList)null).AddProperties<UserEntity>(null, x => x.Id, x => x.CreatedStamp),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void AddPropertyUsingStringThrowsIfListIsNull()
        {
            Assert
                .That
                (
                    () => ((ProjectionList)null).AddProperty("Id"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void AddPropertyUsingStringWithAliasThrowsIfListIsNull()
        {
            Assert
                .That
                (
                    () => ((ProjectionList)null).AddProperty("Id", "Id"),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void AddPropertyUsingExpressionThrowsIfListIsNull()
        {
            Assert
                .That
                (
                    () => ((ProjectionList)null).AddProperty<UserEntity, string>(x => x.Username, null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }
    }
}