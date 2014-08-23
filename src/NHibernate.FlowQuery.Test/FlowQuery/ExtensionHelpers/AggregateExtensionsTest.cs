namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.ExtensionHelpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class AggregateExtensionsTest : BaseTest
    {
        [Test]
        public void AverageThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.Average(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CanAggregateAverage()
        {
            FlowQuerySelection<int> avgs = Query<UserEntity>()
                .Select(u => (int)u.Id.Average());

            Assert.That(avgs, Is.Not.Empty);

            foreach (int avg in avgs)
            {
                Assert.That(avg, Is.EqualTo(2));
            }
        }

        [Test]
        public void CanAggregateCount()
        {
            FlowQuerySelection<int> counts = Query<UserEntity>()
                .Select(u => u.Id.Count());

            Assert.That(counts, Is.Not.Empty);

            foreach (int count in counts)
            {
                Assert.That(count, Is.EqualTo(4));
            }
        }

        [Test]
        public void CanAggregateDistinctCount()
        {
            FlowQuerySelection<int> counts = Query<UserEntity>()
                .Select(u => u.Setting.Id.CountDistinct());

            Assert.That(counts, Is.Not.Empty);

            foreach (int count in counts)
            {
                Assert.That(count, Is.EqualTo(1));
            }
        }

        [Test]
        public void CanAggregateInGroupBy()
        {
            var counts = Query<UserEntity>()
                .Select(u => new
                {
                    Count = u.Id.Count(), 
                    Username = u.Username.GroupBy()
                });

            Assert.That(counts, Is.Not.Empty);

            foreach (var count in counts)
            {
                Assert.That(count.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void CanAggregateMax()
        {
            FlowQuerySelection<long> maxs = Query<UserEntity>()
                .Select(u => u.Id.Max());

            Assert.That(maxs, Is.Not.Empty);

            foreach (long max in maxs)
            {
                Assert.That(max, Is.EqualTo(4));
            }
        }

        [Test]
        public void CanAggregateMin()
        {
            FlowQuerySelection<long> mins = Query<UserEntity>()
                .Select(u => u.Id.Min());

            Assert.That(mins, Is.Not.Empty);

            foreach (long min in mins)
            {
                Assert.That(min, Is.EqualTo(1));
            }
        }

        [Test]
        public void CanAggregateSum()
        {
            FlowQuerySelection<long> sums = Query<UserEntity>()
                .Select(u => u.Id.Sum());

            Assert.That(sums, Is.Not.Empty);

            foreach (long sum in sums)
            {
                Assert.That(sum, Is.EqualTo(10));
            }
        }

        [Test]
        public void CountDistinctThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.CountDistinct(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.Count(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GroupByThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.GroupBy(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MaxThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.Max(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MinThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.Min(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SumThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.Sum(), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}