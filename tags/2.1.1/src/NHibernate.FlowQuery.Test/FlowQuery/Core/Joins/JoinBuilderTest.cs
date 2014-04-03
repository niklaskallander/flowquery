using System;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.SqlCommand;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable SuspiciousTypeConversion.Global
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Joins
{
    [TestFixture]
    public class JoinBuilderTest : BaseTest
    {
        [Test]
        public void JoinBuilderThrowsIfQueryIsNull()
        {
            var implementor = (FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>)DummyQuery<UserEntity>();

            Assert.That(() => new DummyBuilder(implementor, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void JoinBuilderThrowsIfImplementorIsNull()
        {
            Assert.That(() => new DummyBuilder(null, DummyQuery<UserEntity>()), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void JoinBuilderThrowsIfImplementorAndQueryIsNotSameReference()
        {
            var implementor = (FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>)DummyQuery<UserEntity>();

            Assert.That(() => new DummyBuilder(implementor, DummyQuery<UserEntity>()), Throws.ArgumentException);
        }

        [Test]
        public void JoinBuilderThrowsNothingWhenNeitherQueryNorImplementorIsNullAndBothAreTheSameReference()
        {
            var query = DummyQuery<UserEntity>();

            var implementor = (FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>>)query;

            Assert.That(() => new DummyBuilder(implementor, query), Throws.Nothing);
        }
    }

    internal class DummyBuilder : JoinBuilder<UserEntity, IImmediateFlowQuery<UserEntity>>
    {
        internal DummyBuilder(FlowQueryImplementor<UserEntity, IImmediateFlowQuery<UserEntity>> implementor, IImmediateFlowQuery<UserEntity> query)
            : base(implementor, query, JoinType.InnerJoin)
        { }
    }
}