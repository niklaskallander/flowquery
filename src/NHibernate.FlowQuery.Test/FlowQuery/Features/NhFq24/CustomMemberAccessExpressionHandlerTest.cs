namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq24
{
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.Members;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class CustomMemberAccessExpressionHandlerTest : BaseTest
    {
        [Test, Category("MySqlUnsupported")]
        public void Given_CustomMemberAccessHandler_When_FilteringOnHandledMember_Then_ReturnsCorrectProjection()
        {
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new DateTimeDayHandler());
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new DateTimeDateHandler());
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new StringLengthHandler());

            var users = Query<UserEntity>()
                .Where(x => x.Firstname.Length == 6)
                .Select(x => new
                {
                    CreatedDayInMonth = x.CreatedStamp.Day,
                    x.LastLoggedInStamp,
                    LastLoggedInDate = x.LastLoggedInStamp.Value.Date,
                    LastLoggedInDayInMonth = x.LastLoggedInStamp.Value.Day,
                    x.Username,
                    x.Firstname,
                    FirstnameLength = x.Firstname.Length
                })
                .ToArray();

            Assert.That(users.Length, Is.EqualTo(2));

            Assert.That(users.Single(x => x.Username == "Wimpy").CreatedDayInMonth, Is.EqualTo(11));
            Assert.That(users.Single(x => x.Username == "Wimpy").LastLoggedInDayInMonth, Is.EqualTo(11));
            Assert.That(users.Single(x => x.Username == "Empor").CreatedDayInMonth, Is.EqualTo(3));
            Assert.That(users.Single(x => x.Username == "Empor").LastLoggedInDayInMonth, Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Firstname.Length, Is.EqualTo(user.FirstnameLength));
                Assert.That(user.Firstname.Length, Is.EqualTo(6));

                var stamp = user.LastLoggedInStamp;

                if (stamp.HasValue)
                {
                    Assert.That(stamp.Value.TimeOfDay, Is.GreaterThan(user.LastLoggedInDate.TimeOfDay));
                }
            }
        }

        [Test, Category("MySqlUnsupported")]
        public void Given_CustomMemberAccessHandler_When_QueryingUsers_Then_ReturnsCorrectProjection()
        {
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new DateTimeDayHandler());
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new DateTimeDateHandler());
            FlowQueryHelper.AddExpressionHandler(ExpressionType.MemberAccess, new StringLengthHandler());

            var users = Query<UserEntity>()
                .Select(x => new
                {
                    CreatedDayInMonth = x.CreatedStamp.Day,
                    x.LastLoggedInStamp,
                    LastLoggedInDate = x.LastLoggedInStamp.Value.Date,
                    LastLoggedInDayInMonth = x.LastLoggedInStamp.Value.Day,
                    x.Username,
                    x.Firstname,
                    FirstnameLength = x.Firstname.Length
                })
                .ToArray();

            Assert.That(users.Length, Is.EqualTo(4));

            Assert.That(users.Single(x => x.Username == "Wimpy").CreatedDayInMonth, Is.EqualTo(11));
            Assert.That(users.Single(x => x.Username == "Wimpy").LastLoggedInDayInMonth, Is.EqualTo(11));
            Assert.That(users.Single(x => x.Username == "Izmid").CreatedDayInMonth, Is.EqualTo(22));
            Assert.That(users.Single(x => x.Username == "Izmid").LastLoggedInDayInMonth, Is.EqualTo(22));
            Assert.That(users.Single(x => x.Username == "Empor").CreatedDayInMonth, Is.EqualTo(3));
            Assert.That(users.Single(x => x.Username == "Empor").LastLoggedInDayInMonth, Is.EqualTo(3));
            Assert.That(users.Single(x => x.Username == "Lajsa").CreatedDayInMonth, Is.EqualTo(4));
            Assert.That(users.Single(x => x.Username == "Lajsa").LastLoggedInDayInMonth, Is.EqualTo(0));

            foreach (var user in users)
            {
                Assert.That(user.Firstname.Length, Is.EqualTo(user.FirstnameLength));

                var stamp = user.LastLoggedInStamp;

                if (stamp.HasValue)
                {
                    Assert.That(stamp.Value.TimeOfDay, Is.GreaterThan(user.LastLoggedInDate.TimeOfDay));
                }
            }
        }
    }
}