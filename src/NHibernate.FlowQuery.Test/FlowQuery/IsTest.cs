using System.Collections;
using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    using Is = NUnit.Framework.Is;
    using QIs = NHibernate.FlowQuery.Is;

    [TestFixture]
    public class IsTest : BaseTest
    {
        #region Methods (14)

        [Test]
        public void CanFilterBetween()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.Between(2, 4))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.Id, Is.InRange(2, 4));
            }
        }

        [Test]
        public void CanFilterEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.EqualTo(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void CanFilterGreaterThan()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.GreaterThan(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.GreaterThan(2));
            Assert.That(users.ElementAt(1).Id, Is.GreaterThan(2));
        }

        [Test]
        public void CanFilterGreaterThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.GreaterThanOrEqualTo(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.GreaterThanOrEqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.GreaterThanOrEqualTo(2));
            Assert.That(users.ElementAt(2).Id, Is.GreaterThanOrEqualTo(2));
        }

        [Test]
        public void CanFilterInWithEnumerableCollection()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.In(new long[] { 1, 3 }))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
        }

        [Test]
        public void CanFilterInWithStrictEnumerable()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.In(new TestEnumerable(3, 1, 4)))
                .Select();
            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
            Assert.That(users.ElementAt(2).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanFilterInWithSubQuery()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.In(SubQuery.For<UserEntity>().Where(x => x.IsOnline).Select(x => x.Id)))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void CanFilterInWithValues()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.In(2, 4))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.EqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanFilterLessThan()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.LessThan(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.LessThan(2));
        }

        [Test]
        public void CanFilterLessThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.LessThanOrEqualTo(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().Id, Is.LessThanOrEqualTo(2));
            Assert.That(users.ElementAt(1).Id, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void CanFilterLike()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Firstname, QIs.Like("%i%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("i"));
        }

        [Test]
        public void CanFilterNotNull()
        {
            var users = Query<UserEntity>()
                .Where(x => x.LastLoggedInStamp, QIs.Not.Null())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void CanFilterNull()
        {
            var users = Query<UserEntity>()
                .Where(x => x.LastLoggedInStamp, QIs.Null())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void CanNegateFilter()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.Not.EqualTo(2))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.Id, Is.Not.EqualTo(2));
            }
        }

        #endregion Methods



        #region TestEnumerable

        public class TestEnumerable : IEnumerable
        {
            private object[] m_Values;

            public TestEnumerable(params object[] values)
            {
                m_Values = values;
            }

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return m_Values.GetEnumerator();
            }

            #endregion
        }
        #endregion !TestEnumerable
    }
}