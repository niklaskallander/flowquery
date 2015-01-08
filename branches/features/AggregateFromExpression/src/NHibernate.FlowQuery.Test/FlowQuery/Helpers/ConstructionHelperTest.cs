namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ConstructionHandlers.MethodCalls;
    using NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls;
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

        [Test]
        public void AddMethodCallHandlerReturnsFalseIfMethodNameAlreadyUsed()
        {
            var handler = new SimpleMethodCallConstructionHandler();

            bool attempt1 = ConstructionHelper.AddMethodCallHandler("TestName", handler);

            Assert.That(attempt1, Is.True);

            bool attempt2 = ConstructionHelper.AddMethodCallHandler("TestName", handler);

            Assert.That(attempt2, Is.False);
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsEmptyString()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.AddMethodCallHandler(string.Empty, new SimpleMethodCallConstructionHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsNull()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.AddMethodCallHandler(null, new SimpleMethodCallConstructionHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsWhiteSpace()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.AddMethodCallHandler("   ", new SimpleMethodCallConstructionHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfResolverIsNull()
        {
            Assert
                .That
                (
                    () => ConstructionHelper.AddMethodCallHandler("Test", null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void CanAddCustomHandlerToOverrideDefaulImplementation()
        {
            bool added;

            added = ConstructionHelper
                .AddMethodCallHandler("ToString", new SimpleMethodCallConstructionHandler());

            Assert.That(added, Is.True);

            added = ProjectionHelper
                .AddMethodCallHandler("ToString", new SimpleMethodCallProjectionHandler());


            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = x.Id.ToString()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value, Is.EqualTo("Hello World"));
            }
        }


        private class SimpleMethodCallProjectionHandler : IMethodCallProjectionHandler
        {
            public IProjection Handle(MethodCallExpression expression, string root, QueryHelperData data)
            {
                return ProjectionHelper.GetProjection(expression.Object, root, data);
            }
        }

        private class SimpleMethodCallConstructionHandler : IMethodCallConstructionHandler
        {
            public int Handle(MethodCallExpression expression, object[] arguments, out object value, out bool wasHandled)
            {
                value = "Hello World";
                wasHandled = true;

                return 0;
            }
        }

        private class Wrapper
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public bool Value { get; set; }
        }
    }
}