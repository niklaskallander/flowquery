using System;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.SqlCommand;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Joins
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class JoinBuilderTest : BaseTest
    {
        [Test]
        public void JoinBuilderThrowsIfQueryIsNull()
        {
            var implementor = Query<UserEntity>() as FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>;

            Assert.That(implementor, Is.Not.Null);

            Assert.That(() =>
                        {
                            new DummyBuilder(implementor, null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void JoinBuilderThrowsIfImplementorIsNull()
        {
            Assert.That(() =>
                        {
                            new DummyBuilder(null, Query<UserEntity>());

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void JoinBuilderThrowsIfImplementorAndQueryIsNotSameReference()
        {
            var implementor = Query<UserEntity>() as FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>;

            Assert.That(implementor, Is.Not.Null);

            Assert.That(() =>
                        {
                            new DummyBuilder(implementor, Query<UserEntity>());

                        }, Throws.ArgumentException);
        }

        [Test]
        public void JoinBuilderThrowsNothingWhenNeitherQueryNorImplementorIsNullAndBothAreTheSameReference()
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

    internal class DummyBuilder : JoinBuilder<UserEntity, IImmediateFlowQuery<UserEntity>>
    {
        internal DummyBuilder(FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>> implementor, IImmediateFlowQuery<UserEntity> query)
            : base(implementor, query, JoinType.InnerJoin)
        { }
    }
}