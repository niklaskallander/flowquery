namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Expression = System.Linq.Expressions.Expression;

    [TestFixture]
    public class ProjectionExtensionTest : BaseTest
    {
        [Test]
        public void Example1()
        {
            // add the handler
            FlowQueryHelper.AddExpressionHandler(ExpressionType.Call, new ToStringHandler());

            var userIds = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    IdAsString = x.Id.ToString()
                });

            Assert.That(userIds.Count(), Is.EqualTo(4));
            Assert.That(userIds.ElementAt(0).IdAsString, Is.EqualTo("1"));
            Assert.That(userIds.ElementAt(1).IdAsString, Is.EqualTo("2"));
            Assert.That(userIds.ElementAt(2).IdAsString, Is.EqualTo("3"));
            Assert.That(userIds.ElementAt(3).IdAsString, Is.EqualTo("4"));
        }
    }

    public class ToStringHandler : IMethodCallExpressionHandler
    {
        public bool CanHandleConstructionOf(Expression expression)
        {
            return false;
        }

        public bool CanHandleProjectionOf(Expression expression, HelperContext context)
        {
            var methodCall = expression as MethodCallExpression;

            return methodCall != null
                && methodCall.Method.Name == "ToString";
        }

        public int Construct(Expression expression, object[] arguments, out object value, out bool wasHandled)
        {
            value = null;
            wasHandled = false;

            return 0;
        }

        public IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var methodCall = (MethodCallExpression)expression;

            // return null if called statically
            if (methodCall.Object == null)
            {
                return null;
            }

            // return null if called for non-ToString method
            if (methodCall.Method.Name != "ToString")
            {
                return null;
            }

            // resolve a projection for the property
            IProjection property = ProjectionHelper
                .GetProjection(methodCall.Object, context);

            // return null if no projection could be resolved
            if (property == null)
            {
                return null;
            }

            // create a cast projection property
            return new CastProjection
                (
                NHibernateUtil.String,
                property
                );
        }
    }
}