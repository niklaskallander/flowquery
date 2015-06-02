namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls;

    using NUnit.Framework;

    [TestFixture]
    public class SimpleMethodCallHandlerTest
    {
        [Test]
        public void ConstructorThrowsIfResolverIsNull()
        {
            Assert
                .That
                (
                    () => new SimpleMethodCallHandler(null, "Avg"),
                    Throws.TypeOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ConstructorDoesNotThrowGivenAProperResolver()
        {
            Assert
                .That
                (
                    () => new SimpleMethodCallHandler(Projections.Count, "Count"),
                    Throws.Nothing
                );
        }
    }
}