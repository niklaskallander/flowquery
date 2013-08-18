using System;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Core.Orders;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Orders
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class OrderBuilderTest : BaseTest
    {
        [Test]
        public void OrderBuilderThrowsIfQueryIsNull()
        {
            var implementor = Query<UserEntity>() as FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>;

            Assert.That(implementor, Is.Not.Null);

            Assert.That(() =>
                        {
                            new DummyBuilder(implementor, null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void OrderBuilderThrowsIfImplementorIsNull()
        {
            Assert.That(() =>
                        {
                            new DummyBuilder(null, Query<UserEntity>());

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void OrderBuilderThrowsIfImplementorAndQueryIsNotSameReference()
        {
            var implementor = Query<UserEntity>() as FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>;

            Assert.That(implementor, Is.Not.Null);

            Assert.That(() =>
                        {
                            new DummyBuilder(implementor, Query<UserEntity>());

                        }, Throws.ArgumentException);
        }

        [Test]
        public void OrderBuilderThrowsNothingWhenNeitherQueryNorImplementorIsNullAndBothAreTheSameReference()
        {
            var query = Query<UserEntity>();

            var implementor = query as FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>;

            Assert.That(implementor, Is.Not.Null);

            Assert.That(() =>
                        {
                            new DummyBuilder(implementor, query);

                        }, Throws.Nothing);
        }
    }

    internal class DummyBuilder : OrderBuilder<UserEntity, IImmediateFlowQuery<UserEntity>>
    {
        internal DummyBuilder(FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>> implementor, IImmediateFlowQuery<UserEntity> query)
            : base(implementor, query)
        { }
    }
}
