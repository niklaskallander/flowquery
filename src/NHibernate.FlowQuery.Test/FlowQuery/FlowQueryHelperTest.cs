namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Moq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Expression = System.Linq.Expressions.Expression;

    [TestFixture]
    public class FlowQueryHelperTest : BaseTest
    {
        [Test]
        public void AddMethodCallHandlerThrowsIfResolverIsNull()
        {
            Assert
                .That
                (
                    () => FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void CanAddCustomHandlerToOverrideDefaulImplementationProjectionOnly()
        {
            var mockedResolver = new Mock<IExpressionHandler>(MockBehavior.Loose);

            mockedResolver
                .Setup
                (
                    resolver =>
                        resolver
                            .Project(It.IsAny<MethodCallExpression>(), It.IsAny<HelperContext>())
                )
                .Returns(Projections.Constant(123));

            mockedResolver
                .Setup
                (
                    resolver =>
                        resolver
                            .CanHandleProjectionOf(It.IsAny<MethodCallExpression>(), It.IsAny<HelperContext>())
                )
                .Returns(true);

            FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, mockedResolver.Object);

            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Hash = x.GetHashCode()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            mockedResolver.VerifyAll();

            foreach (var user in users)
            {
                Assert.That(user.Hash, Is.EqualTo(123));
            }
        }

        [Test]
        public void CanAddCustomHandlerToOverrideDefaulImplementationWithConstruction()
        {
            FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, new SimpleMethodCallHandler());

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

        [Test]
        public void CanClearCustomMethodCallHandlers()
        {
            var handler = new SimpleMethodCallHandler();

            // add one and assert added
            FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, handler);

            IEnumerable<IExpressionHandler> handlers = FlowQueryHelper
                .GetExpressionHandlers(ExpressionType.Call);

            Assert.That(handlers, Has.Member(handler));

            // clear the list and assert empty
            FlowQueryHelper.ClearExpressionHandlers();

            handlers = FlowQueryHelper.GetExpressionHandlers(ExpressionType.Call);

            Assert.That(handlers, Has.No.Member(handler));

            // add again and assert added
            FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, handler);

            handlers = FlowQueryHelper.GetExpressionHandlers(ExpressionType.Call);

            Assert.That(handlers, Has.Member(handler));
        }

        [Test]
        public void Given_UnsupportedExpressionType_When_GettingExpressionHandlers_Then_ReturnsEmptySetOfHandlers()
        {
            var handlers = FlowQueryHelper.GetExpressionHandlers(ExpressionType.NewArrayBounds);

            Assert.That(handlers.Count(), Is.EqualTo(0));
        }

        private class SimpleMethodCallHandler : IMethodCallExpressionHandler
        {
            public bool CanHandleConstructionOf(Expression expression)
            {
                return true;
            }

            public bool CanHandleProjectionOf
                (
                Expression expression,
                HelperContext context
                )
            {
                return true;
            }

            public int Construct
                (
                Expression expression,
                object[] arguments,
                out object value,
                out bool wasHandled
                )
            {
                value = "Hello World";
                wasHandled = true;

                return 0;
            }

            public IProjection Project
                (
                Expression expression,
                HelperContext context
                )
            {
                var methodCall = (MethodCallExpression)expression;

                return ProjectionHelper.GetProjection(methodCall.Object, context);
            }
        }
    }
}