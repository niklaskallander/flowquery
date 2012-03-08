using System;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    [TestFixture]
    public class AggregateHelperTest
    {
		#region�Methods�(7)�

        [Test]
        public void AverageThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.Average(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountDistinctThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.CountDistinct(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.Count(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GroupByThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.GroupBy(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MaxThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.Max(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MinThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.Min(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SumThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { Aggregate.Sum(3); }, Throws.InstanceOf<InvalidOperationException>());
        }

		#endregion�Methods�
    }
}