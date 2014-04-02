// ReSharper disable CSharpWarnings::CS0618
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.FlowQuery.ExtensionHelpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class IsExtensionsTest : BaseTest
    {
        [Test]
        public void CanPerformIsBetween()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsBetween(2, 3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (var id in ids)
            {
                Assert.That(id >= 2 && id <= 3);
            }
        }

        [Test]
        public void CanPerformIsEqualTo()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsEqualTo(3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(3));
        }

        [Test]
        public void CanPerformIsGreaterThan()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsGreaterThan(3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));

            foreach (var id in ids)
            {
                Assert.That(id, Is.GreaterThan(3));
            }
        }

        [Test]
        public void CanPerformIsGreaterThanOrEqualTo()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsGreaterThanOrEqualTo(3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (var id in ids)
            {
                Assert.That(id, Is.GreaterThanOrEqualTo(3));
            }
        }

        [Test]
        public void CanPerformIsInWithCollection()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsIn(1, 4))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (var id in ids)
            {
                Assert.That(id == 1 || id == 4);
            }
        }

        [Test]
        public void CanPerformIsInWithStrictEnumerable()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id.IsIn(new TestEnumerable(3, 1, 4)))
                .Select()
                ;

            Assert.That(users.ToArray().Length, Is.EqualTo(3));
            Assert.That(users.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(users.ElementAt(1).Id, Is.EqualTo(3));
            Assert.That(users.ElementAt(2).Id, Is.EqualTo(4));
        }

        [Test]
        public void CanPerformIsInWithSubquery()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsIn(DetachedQuery<UserEntity>().Where(x => x.Id == 3).Select(x => x.Id)))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(3));
        }

        [Test]
        public void CanPerformIsLessThan()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsLessThan(3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(2));

            foreach (var id in ids)
            {
                Assert.That(id, Is.LessThan(3));
            }
        }

        [Test]
        public void CanPerformIsLessThanOrEqualTo()
        {
            var ids = Query<UserEntity>()
                .Where(u => u.Id.IsLessThanOrEqualTo(3))
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(3));

            foreach (var id in ids)
            {
                Assert.That(id, Is.LessThanOrEqualTo(3));
            }
        }

        [Test]
        public void CanPerformIsLike()
        {
            var usernames = Query<UserEntity>()
                .Where(u => u.Username.IsLike("%m%"))
                .Select(u => u.Username)
                ;

            Assert.That(usernames.ToArray().Length, Is.EqualTo(3));

            foreach (var username in usernames)
            {
                Assert.That(username.Contains("m"));
            }
        }

        [Test]
        public void CanPerformIsNotNull()
        {
            var users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp.IsNotNull())
                .Select()
                ;

            Assert.That(users.ToArray().Length, Is.EqualTo(3));
            Assert.That(users.First().LastLoggedInStamp, Is.Not.Null);
        }

        [Test]
        public void CanPerformIsNull()
        {
            var users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp.IsNull())
                .Select()
                ;

            Assert.That(users.ToArray().Length, Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void IsBetweenThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsBetween("", ""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsEqualTo(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsGreaterThanOrEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsGreaterThanOrEqualTo(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsGreaterThanThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsGreaterThan(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsInThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsIn(""), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => 2.IsIn(1, 2, 3), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => 2.IsIn(new List<int>()), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => "".IsIn(DetachedQuery<UserEntity>()), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsLessThanOrEqualToThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsLessThanOrEqualTo(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsLessThanThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsLessThan(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsLikeThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsLike(""), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsNotNullThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsNotNull(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IsNullThrowsWhenCalledOutsideLambdaExpression()
        {
            Assert.That(() => "".IsNull(), Throws.InstanceOf<InvalidOperationException>());
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
// ReSharper restore CSharpWarnings::CS0618