namespace NHibernate.FlowQuery.Test.FlowQuery.Features
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class AggregateFromExpressionTest : BaseTest
    {
        [Test]
        public void CanUseAggregateFromExpression()
        {
            Expression<Func<UserEntity, UserDto>> expression = x => new UserDto
            {
                Fullname = x.Firstname + " " + x.Lastname,
                Id = x.Id,
                IsOnline = x.IsOnline,
                SettingId = x.Setting.Id,
                Username = x.Username
            };

            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = Aggregate.FromExpression(expression)
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value, Is.Not.Null);
            }

            Assert.That(users.ElementAt(0).Value.Fullname, Is.EqualTo(Fullnames[0]));
            Assert.That(users.ElementAt(1).Value.Fullname, Is.EqualTo(Fullnames[1]));
            Assert.That(users.ElementAt(2).Value.Fullname, Is.EqualTo(Fullnames[2]));
            Assert.That(users.ElementAt(3).Value.Fullname, Is.EqualTo(Fullnames[3]));
        }

        [Test]
        public void UsingAggregateFromExpressionWithNullExpressionThrows()
        {
            Expression<Func<UserEntity, UserDto>> expression = null;

            Assert
                .That
                (
                    () => Query<UserEntity>().Select(x => Aggregate.FromExpression(expression)),
                    Throws.InstanceOf<NotSupportedException>()
                );
        }
    }
}