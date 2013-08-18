using System;
using NHibernate.FlowQuery.ExtensionHelpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class AggregateExtensionsTest : BaseTest
    {
        #region Methods (14)

        [Test]
        public void AverageThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".Average(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CanAggregateAverage()
        {
            var avgs = Query<UserEntity>()
                .Select(u => u.Id.Average())
                ;

            Assert.That(avgs, Is.Not.Empty);

            foreach (var avg in avgs)
            {
                Assert.That(avg, Is.EqualTo(2.5M));
            }
        }

        [Test]
        public void CanAggregateCount()
        {
            var counts = Query<UserEntity>()
                .Select(u => u.Id.Count())
                ;

            Assert.That(counts, Is.Not.Empty);

            foreach (var count in counts)
            {
                Assert.That(count, Is.EqualTo(4));
            }
        }

        [Test]
        public void CanAggregateDistinctCount()
        {
            var counts = Query<UserEntity>()
                .Select(u => u.Setting.Id.CountDistinct())
                ;

            Assert.That(counts, Is.Not.Empty);

            foreach (var count in counts)
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
                        })
                        ;

            Assert.That(counts, Is.Not.Empty);

            foreach (var count in counts)
            {
                Assert.That(count.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void CanAggregateMax()
        {
            var maxs = Query<UserEntity>()
                .Select(u => u.Id.Max())
                ;

            Assert.That(maxs, Is.Not.Empty);

            foreach (var max in maxs)
            {
                Assert.That(max, Is.EqualTo(4));
            }
        }

        [Test]
        public void CanAggregateMin()
        {
            var mins = Query<UserEntity>()
                .Select(u => u.Id.Min())
                ;

            Assert.That(mins, Is.Not.Empty);

            foreach (var min in mins)
            {
                Assert.That(min, Is.EqualTo(1));
            }
        }

        [Test]
        public void CanAggregateSum()
        {
            var sums = Query<UserEntity>()
                .Select(u => u.Id.Sum())
                ;

            Assert.That(sums, Is.Not.Empty);

            foreach (var sum in sums)
            {
                Assert.That(sum, Is.EqualTo(10));
            }
        }

        [Test]
        public void CountDistinctThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".CountDistinct(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CountThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".Count(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GroupByThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".GroupBy(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MaxThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".Max(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void MinThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".Min(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SumThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => { "".Sum(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        #endregion Methods
    }
}