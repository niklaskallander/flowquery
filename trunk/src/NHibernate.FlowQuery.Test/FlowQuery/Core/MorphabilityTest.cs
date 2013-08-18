using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class MorphabilityTest : BaseTest
    {
        [Test]
        public void CanMorphQueries()
        {
            IImmediateFlowQuery<UserEntity> immediateQuery = Query<UserEntity>();

            Assert.That(immediateQuery, Is.Not.Null);

            IDelayedFlowQuery<UserEntity> delayedQuery = immediateQuery
                .Delayed();

            Assert.That(delayedQuery, Is.Not.Null);

            IDetachedFlowQuery<UserEntity> detachedQuery = delayedQuery
                .Detached();

            Assert.That(detachedQuery, Is.Not.Null);

            immediateQuery = delayedQuery
                .Immediate();

            Assert.That(immediateQuery, Is.Not.Null);

            delayedQuery = detachedQuery
                .Delayed();

            Assert.That(delayedQuery, Is.Not.Null);

            immediateQuery = detachedQuery
                .Immediate();

            Assert.That(immediateQuery, Is.Not.Null);
        }
    }
}