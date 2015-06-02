namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ConstructionHelperTest : BaseTest
    {
        [Test]
        public void CanGetListByExpression()
        {
            Expression<Func<UserEntity, object>> expression = x => new Wrapper { Value = x.IsOnline };

            IEnumerable enumerable = Session.CreateCriteria<UserEntity>()
                .SetProjection(Projections.Property("IsOnline"))
                .List();

            IEnumerable<Wrapper> isOnline = ConstructionHelper.GetListByExpression<Wrapper>(expression, enumerable);

            Assert.That(isOnline.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GetListByExpressionReturnsNullIfSelectionIsNull()
        {
            Expression<Func<UserEntity, object>> expression = x => new { x.IsOnline };

            IEnumerable<int> list = ConstructionHelper.GetListByExpression<int>(expression, null);

            Assert.That(list, Is.Null);
        }

        [Test]
        public void GetListByExpressionThrowsWhenExpressionIsNull()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.GetListByExpression<int>(null, new object[] { }),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void GetObjectByExpressionConverterThrowsWhenExpressionIsNull()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.GetObjectByExpressionConverter<int>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void GetObjectByExpressionConverterWorksForMemberInitExpression()
        {
            Expression<Func<int, ObjectWrapper<int>>> memberInitExpression = x => new ObjectWrapper<int>
            {
                Object = x
            };

            Func<object, ObjectWrapper<int>> converter =
                ConstructionHelper.GetObjectByExpressionConverter<ObjectWrapper<int>>(memberInitExpression);

            ObjectWrapper<int> test1 = converter(1);
            ObjectWrapper<int> test2 = converter(2);
            ObjectWrapper<int> test3 = converter(3);

            Assert.That(test1, Is.Not.Null);
            Assert.That(test2, Is.Not.Null);
            Assert.That(test3, Is.Not.Null);

            Assert.That(test1.Object, Is.EqualTo(1));
            Assert.That(test2.Object, Is.EqualTo(2));
            Assert.That(test3.Object, Is.EqualTo(3));
        }

        [Test]
        public void GetObjectByExpressionConverterWorksForNewExpression()
        {
            Expression<Func<int, ObjectWrapper<int>>> newExpression = x => new ObjectWrapper<int>(x);

            Func<object, ObjectWrapper<int>> converter =
                ConstructionHelper.GetObjectByExpressionConverter<ObjectWrapper<int>>(newExpression);

            ObjectWrapper<int> test1 = converter(1);
            ObjectWrapper<int> test2 = converter(2);
            ObjectWrapper<int> test3 = converter(3);

            Assert.That(test1, Is.Not.Null);
            Assert.That(test2, Is.Not.Null);
            Assert.That(test3, Is.Not.Null);

            Assert.That(test1.Object, Is.EqualTo(1));
            Assert.That(test2.Object, Is.EqualTo(2));
            Assert.That(test3.Object, Is.EqualTo(3));
        }

        private class ObjectWrapper<T>
        {
            public ObjectWrapper()
            {
            }

            public ObjectWrapper(T value)
            {
                Object = value;
            }

            public T Object { get; set; }
        }

        private class Wrapper
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public bool Value { get; set; }
        }
    }
}