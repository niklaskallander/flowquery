using System;
using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class DistinctTest : BaseTest
    {
        [Test]
        public void CanMakeDistinctImmediateQueryIndistinct()
        {
            int immediateCount = Query<UserEntity>()
                .Distinct()
                .Indistinct()
                .Count(u => u.IsOnline)
                ;

            Assert.That(immediateCount, Is.EqualTo(4));
        }

        [Test]
        public void CanMakeDistinctDelayedQueryIndistinct()
        {
            Lazy<int> delayedCount = Query<UserEntity>()
                .Delayed()
                .Distinct()
                .Indistinct()
                .Count(u => u.IsOnline)
                ;

            Assert.That(delayedCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanMakeDistinctDetachedQueryIndistinct()
        {
            var detachedCount = Query<UserEntity>()
                .Detached()
                .Distinct()
                .Indistinct()
                .Count(u => u.IsOnline)
                ;

            var query = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(detachedCount))
                .Select();

            Assert.That(query.Count(), Is.EqualTo(1));
            Assert.That(query.First().Id, Is.EqualTo(4));
        }
    }
}
