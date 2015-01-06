namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Moq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectionHelperTest : BaseTest
    {
        [Test]
        public void AddMethodCallHandlerReturnsFalseIfMethodNameAlreadyUsed()
        {
            var resolver = new SimpleMethodCallHandler(Projections.Sum);

            bool attempt1 = ProjectionHelper.AddMethodCallHandler("TestName", resolver);

            Assert.That(attempt1, Is.True);

            bool attempt2 = ProjectionHelper.AddMethodCallHandler("TestName", resolver);

            Assert.That(attempt2, Is.False);
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsEmptyString()
        {
            Assert
                .That
                (
                    () =>
                        ProjectionHelper
                            .AddMethodCallHandler(string.Empty, new SimpleMethodCallHandler(Projections.Sum)),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsNull()
        {
            Assert
                .That
                (
                    () => ProjectionHelper.AddMethodCallHandler(null, new SimpleMethodCallHandler(Projections.Sum)),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfMethodNameIsWhiteSpace()
        {
            Assert
                .That
                (
                    () =>
                        ProjectionHelper
                            .AddMethodCallHandler("   ", new SimpleMethodCallHandler(Projections.Sum)),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void AddMethodCallHandlerThrowsIfResolverIsNull()
        {
            Assert
                .That
                (
                    () =>
                        ProjectionHelper
                            .AddMethodCallHandler("Test", null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SimpleMethodHandlerThrowsIfDelegateIsNull()
        {
            Assert
                .That
                (
                    () => new SimpleMethodCallHandler(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void CanAddCustomHandlerToOverrideDefaulImplementation()
        {
            var mockedResolver = new Mock<IMethodCallProjectionHandler>(MockBehavior.Loose);

            mockedResolver
                .Setup
                (
                    resolver =>
                        resolver
                            .Handle
                            (
                                It.IsAny<MethodCallExpression>(),
                                It.IsAny<string>(),
                                It.IsAny<QueryHelperData>()
                            )
                )
                .Returns(Projections.Constant(123));

            bool added = ProjectionHelper.AddMethodCallHandler("GetHashCode", mockedResolver.Object);

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
    }
}