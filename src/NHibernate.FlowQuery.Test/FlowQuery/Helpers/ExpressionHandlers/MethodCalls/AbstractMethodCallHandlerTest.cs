namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls;

    using NUnit.Framework;

    using Expression = System.Linq.Expressions.Expression;

    [TestFixture]
    public class AbstractMethodCallHandlerTest
    {
        [Test]
        public void Given_EmptySetOfSupportedMethodNames_When_Constructed_Then_ThrowsArgumentException()
        {
            Assert.That(() => new MethodCallHandler(), Throws.ArgumentException);
        }

        [Test]
        public void Given_NullReferenceAsSupportedMethodNames_When_Constructed_Then_ThrowsArgumentNullException()
        {
            Assert.That(() => new MethodCallHandler(null), Throws.InstanceOf<ArgumentNullException>());
        }

        private class MethodCallHandler : AbstractMethodCallHandler
        {
            public MethodCallHandler(params string[] supportedMethodNames)
                : base(supportedMethodNames)
            {
            }

            protected override IProjection ProjectCore
                (
                MethodCallExpression expression, 
                Expression subExpression, 
                IProjection projection, 
                HelperContext context
                )
            {
                return null;
            }
        }
    }
}