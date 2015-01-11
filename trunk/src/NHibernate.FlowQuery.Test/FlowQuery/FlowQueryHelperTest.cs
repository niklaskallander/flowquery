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

    [TestFixture]
    public class FlowQueryHelperTest : BaseTest
    {
        [Test]
        public void AddMethodCallHandlerReturnsFalseIfMethodNameAlreadyUsed()
        {
            var handler = new SimpleMethodCallHandler();

            bool attempt1 = FlowQueryHelper.AddMethodCallHandler("TestName1", handler);

            Assert.That(attempt1, Is.True);

            bool attempt2 = FlowQueryHelper.AddMethodCallHandler("TestName1", handler);

            Assert.That(attempt2, Is.False);
        }

        [Test]
        public void AddMethodCallHandlerReturnsTrueIfMethodNameAlreadyUsedButWithForceSpecified()
        {
            var handler = new SimpleMethodCallHandler();

            bool attempt1 = FlowQueryHelper.AddMethodCallHandler("TestName2", handler);

            Assert.That(attempt1, Is.True);

            bool attempt2 = FlowQueryHelper.AddMethodCallHandler("TestName2", handler, true);

            Assert.That(attempt2, Is.True);
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsEmptyString()
        {
            Assert
                .That
                (
                    () => FlowQueryHelper.AddMethodCallHandler(string.Empty, new SimpleMethodCallHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsNull()
        {
            Assert
                .That
                (
                    () => FlowQueryHelper.AddMethodCallHandler(null, new SimpleMethodCallHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsWhiteSpace()
        {
            Assert
                .That
                (
                    () => FlowQueryHelper.AddMethodCallHandler("   ", new SimpleMethodCallHandler()),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfResolverIsNull()
        {
            Assert
                .That
                (
                    () => FlowQueryHelper.AddMethodCallHandler("Test", null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void CanAddCustomHandlerToOverrideDefaulImplementationProjectionOnly()
        {
            var mockedResolver = new Mock<IMethodCallExpressionHandler>(MockBehavior.Loose);

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
                            .CanHandleProjection(It.IsAny<MethodCallExpression>(), It.IsAny<HelperContext>())
                )
                .Returns(true);

            bool added = FlowQueryHelper.AddMethodCallHandler("GetHashCode", mockedResolver.Object);

            Assert.That(added, Is.True);

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
            bool added = FlowQueryHelper.AddMethodCallHandler("ToString", new SimpleMethodCallHandler(), true);

            Assert.That(added, Is.True);

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
            bool attempt1 = FlowQueryHelper.AddMethodCallHandler("TestName3", handler);

            IEnumerable<IMethodCallExpressionHandler> handlers = FlowQueryHelper.GetMethodCallHandlers("TestName3");

            Assert.That(attempt1, Is.True);
            Assert.That(handlers.Any());

            // clear the list and assert empty
            FlowQueryHelper.ClearMethodCallHandlers();

            handlers = FlowQueryHelper.GetMethodCallHandlers("TestName3");

            Assert.That(!handlers.Any());

            // add again and assert added
            bool attempt2 = FlowQueryHelper.AddMethodCallHandler("TestName3", handler);

            handlers = FlowQueryHelper.GetMethodCallHandlers("TestName3");

            Assert.That(attempt2, Is.True);
            Assert.That(handlers.Any());
        }

        private class SimpleMethodCallHandler : IMethodCallExpressionHandler
        {
            public bool CanHandleConstruction(MethodCallExpression expression)
            {
                return true;
            }

            public bool CanHandleProjection
                (
                MethodCallExpression expression,
                HelperContext context)
            {
                return true;
            }

            public int Construct
                (
                MethodCallExpression expression,
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
                MethodCallExpression expression,
                HelperContext context
                )
            {
                return ProjectionHelper.GetProjection(expression.Object, context);
            }
        }
    }
}