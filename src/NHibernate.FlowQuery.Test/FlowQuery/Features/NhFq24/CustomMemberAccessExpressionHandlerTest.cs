namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq24
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Expression = System.Linq.Expressions.Expression;

    [TestFixture]
    public class CustomMemberAccessExpressionHandlerTest : BaseTest
    {
        [Test]
        public void Given_CustomMemberAccessHandler_When_QueryingUsers_Then_ReturnsCorrectProjection()
        {
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new DayInMonthHandler());

            var users = Query<UserEntity>()
                .Select(x => new
                {
                    CreatedDayInMonth = x.CreatedStamp.Day,
                    x.Username
                })
                .ToArray();

            Assert.That(users.Length, Is.EqualTo(4));

            Assert.That(users.Single(x => x.Username == "Wimpy").CreatedDayInMonth, Is.EqualTo(11));
            Assert.That(users.Single(x => x.Username == "Izmid").CreatedDayInMonth, Is.EqualTo(22));
            Assert.That(users.Single(x => x.Username == "Empor").CreatedDayInMonth, Is.EqualTo(3));
            Assert.That(users.Single(x => x.Username == "Lajsa").CreatedDayInMonth, Is.EqualTo(4));
        }

        private class DayInMonthHandler : IExpressionHandler
        {
            public bool CanHandleConstructionOf(Expression expression)
            {
                return false;
            }

            public bool CanHandleProjectionOf(Expression expression, HelperContext context)
            {
                if (expression.NodeType != ExpressionType.MemberAccess)
                {
                    return false;
                }

                var member = (MemberExpression)expression;

                if (member.Expression.Type != typeof(DateTime))
                {
                    return false;
                }

                return member.Member.Name == "Day";
            }

            public int Construct(Expression expression, object[] arguments, out object value, out bool wasHandled)
            {
                value = null;
                wasHandled = false;

                return 0;
            }

            public IProjection Project(Expression expression, HelperContext context)
            {
                var member = (MemberExpression)expression;

                return Projections
                    .SqlFunction
                    (
                        "day",
                        NHibernateUtil.Int32,
                        ProjectionHelper.GetProjection(member.Expression, context)
                    );
            }
        }
    }
}