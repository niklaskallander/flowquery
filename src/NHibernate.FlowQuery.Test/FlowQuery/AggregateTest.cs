using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    [TestFixture]
    public class AggregateHelperTest
    {
        [Test]
        public void AverageThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Average(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Average((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountDistinctThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.CountDistinct(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.CountDistinct((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Count(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Count((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GroupByThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.GroupBy(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.GroupBy((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MaxThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Max(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Max((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MinThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Min(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Min((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SumThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Sum(3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Sum((int?)3), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SubqueryThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => Aggregate.Subquery<int>(null as DetachedCriteria), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => Aggregate.Subquery<int>(null as IDetachedImmutableFlowQuery), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}