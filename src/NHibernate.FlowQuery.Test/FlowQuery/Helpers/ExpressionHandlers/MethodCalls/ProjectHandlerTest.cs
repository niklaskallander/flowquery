
namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectHandlerTest
    {
        [Test]
        public void DoesNotAttemptToHandeIfExpressionIsNull()
        {
            var handler = new ProjectHandler();

            bool wasHandled;
            object value;

            int usedArguments = handler.Construct(null, new object[0], out value, out wasHandled);

            Assert.That(usedArguments, Is.EqualTo(0));
            Assert.That(wasHandled, Is.False);
            Assert.That(value, Is.Null);

        }
    }
}
