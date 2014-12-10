namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectionExtensionTest : BaseTest
    {
        [Test]
        public void Example1()
        {
            // add the handler
            ProjectionHelper.AddMethodCallHandler("ToString", new ToStringHandler());

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

    public class ToStringHandler : IMethodCallProjectionHandler
    {
        public IProjection Handle
            (
            MethodCallExpression expression,
            string root,
            QueryHelperData data
            )
        {
            // return null if called statically
            if (expression.Object == null)
            {
                return null;
            }

            // return null if called for non-ToString method
            if (expression.Method.Name != "ToString")
            {
                return null;
            }

            // resolve a projection for the property
            IProjection property = ProjectionHelper
                .GetProjection(expression.Object, root, data);

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