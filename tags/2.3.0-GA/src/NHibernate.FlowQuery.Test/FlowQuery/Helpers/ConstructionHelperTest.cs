namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
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
        public void CanHandleSortsOutLambdaExpression()
        {
            Expression<Func<UserEntity, UserDto>> expression =
                x => new UserDto { Fullname = x.Firstname + " " + x.Lastname };

            bool canHandle = ConstructionHelper.CanHandle(expression);

            Assert.That(canHandle, Is.True);
        }

        [Test]
        public void CanHandleThrowsWhenExpressionIsNull()
        {
            Assert.That(() => ConstructionHelper.CanHandle(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetListByExpressionReturnsNullIfSelectionIsNull()
        {
            Expression<Func<UserEntity, object>> expression = x => new { x.IsOnline };

            IEnumerable<int> list = ConstructionHelper.GetListByExpression<int>(expression, null);

            Assert.That(list, Is.Null);
        }

        [Test]
        public void GetListByExpressionReturnsNullIfUnhandlableExpression()
        {
            IEnumerable enumerable = Session.CreateCriteria<UserEntity>()
                .SetProjection(Projections.Property("IsOnline"))
                .List();

            Expression<Func<UserEntity, object>> expression = x => x.IsOnline;

            IEnumerable<int> list = ConstructionHelper.GetListByExpression<int>(expression, enumerable);

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

        private class Wrapper
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public bool Value { get; set; }
        }
    }
}