namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Joins
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.SqlCommand;

    using NUnit.Framework;

    [TestFixture]
    public class JoinBuilderTest : BaseTest
    {
        [Test]
        public void JoinBuilderThrowsIfImplementorAndQueryIsNotSameReference()
        {
            var implementor = DummyQuery<UserEntity>() as IFlowQuery;

            Assert.That(() => new DummyBuilder(implementor, DummyQuery<UserEntity>()), Throws.ArgumentException);
        }

        [Test]
        public void JoinBuilderThrowsIfImplementorIsNull()
        {
            Assert
                .That
                (
                    () => new DummyBuilder(null, DummyQuery<UserEntity>()),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void JoinBuilderThrowsIfQueryIsNull()
        {
            var implementor = (IFlowQuery)DummyQuery<UserEntity>();

            Assert.That(() => new DummyBuilder(implementor, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void JoinBuilderThrowsNothingWhenNeitherQueryNorImplementorIsNullAndBothAreTheSameReference()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            var implementor = (IFlowQuery)query;

            Assert.That(() => new DummyBuilder(implementor, query), Throws.Nothing);
        }

        internal class DummyBuilder : JoinBuilder<UserEntity, IImmediateFlowQuery<UserEntity>>
        {
            internal DummyBuilder
                (
                IFlowQuery implementor,
                IImmediateFlowQuery<UserEntity> query)
                : base(implementor, query, JoinType.InnerJoin)
            {
            }
        }
    }
}