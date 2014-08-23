namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.ExtensionHelpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class IsExtensionsTest : BaseTest
    {
        [Test]
        public void CanPerformIsBetween()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsBetween(2, 3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (long id in ids)
            {
                Assert.That(id >= 2 && id <= 3);
            }
        }

        [Test]
        public void CanPerformIsEqualTo()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsEqualTo(3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(3));
        }

        [Test]
        public void CanPerformIsGreaterThan()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsGreaterThan(3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));

            foreach (long id in ids)
            {
                Assert.That(id, Is.GreaterThan(3));
            }
        }

        [Test]
        public void CanPerformIsGreaterThanOrEqualTo()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsGreaterThanOrEqualTo(3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (long id in ids)
            {
                Assert.That(id, Is.GreaterThanOrEqualTo(3));
            }
        }

        [Test]
        public void CanPerformIsInWithCollection()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsIn(1, 4))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (long id in ids)
            {
                Assert.That(id == 1 || id == 4);
            }
        }

        [Test]
        public void CanPerformIsInWithStrictEnumerable()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id.IsIn(new TestEnumerable(3, 1, 4)))
                .Select();

            Assert.That(users.ToArray().Length, Is.EqualTo(3));
            Assert.That(users.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
            Assert.That(users.ElementAt(2).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanPerformIsInWithSubquery()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsIn(DetachedQuery<UserEntity>().Where(x => x.Id == 3).Select(x => x.Id)))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(3));
        }

        [Test]
        public void CanPerformIsLessThan()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsLessThan(3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (long id in ids)
            {
                Assert.That(id, Is.LessThan(3));
            }
        }

        [Test]
        public void CanPerformIsLessThanOrEqualTo()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => u.Id.IsLessThanOrEqualTo(3))
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(3));

            foreach (long id in ids)
            {
                Assert.That(id, Is.LessThanOrEqualTo(3));
            }
        }

        [Test]
        public void CanPerformIsLike()
        {
            FlowQuerySelection<string> usernames = Query<UserEntity>()
                .Where(u => u.Username.IsLike("%m%"))
                .Select(u => u.Username);

            Assert.That(usernames.ToArray().Length, Is.EqualTo(3));

            foreach (string username in usernames)
            {
                Assert.That(username.Contains("m"));
            }
        }

        [Test]
        public void CanPerformIsNotNull()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp.IsNotNull())
                .Select();

            Assert.That(users.ToArray().Length, Is.EqualTo(3));
            Assert.That(users.First().LastLoggedInStamp, Is.Not.Null);
        }

        [Test]
        public void CanPerformIsNull()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp.IsNull())
                .Select();

            Assert.That(users.ToArray().Length, Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void IsBetweenThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert
                .That
                (
                    () => string.Empty.IsBetween(string.Empty, string.Empty), 
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void IsEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsEqualTo(string.Empty), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsGreaterThanOrEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert
                .That
                (
                    () => string.Empty.IsGreaterThanOrEqualTo(string.Empty), 
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void IsGreaterThanThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsGreaterThan(string.Empty), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsInThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsIn(string.Empty), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => 2.IsIn(1, 2, 3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => 2.IsIn(new List<int>()), Throws.InstanceOf<InvalidOperationException>());
            Assert
                .That
                (
                    () => string.Empty.IsIn(DetachedQuery<UserEntity>()), 
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void IsLessThanOrEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert
                .That
                (
                    () => string.Empty.IsLessThanOrEqualTo(string.Empty), 
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void IsLessThanThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsLessThan(string.Empty), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsLikeThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsLike(string.Empty), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsNotNullThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsNotNull(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsNullThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => string.Empty.IsNull(), Throws.InstanceOf<InvalidOperationException>());
        }

        public class TestEnumerable : IEnumerable
        {
            private readonly object[] _values;

            public TestEnumerable(params object[] values)
            {
                _values = values;
            }

            public IEnumerator GetEnumerator()
            {
                return _values.GetEnumerator();
            }
        }
    }
}