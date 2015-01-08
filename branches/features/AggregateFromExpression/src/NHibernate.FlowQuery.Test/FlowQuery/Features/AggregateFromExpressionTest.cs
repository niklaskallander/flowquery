namespace NHibernate.FlowQuery.Test.FlowQuery.Features
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;
    using NHibernate.Mapping;
    using NHibernate.Properties;

    using NUnit.Framework;

    [TestFixture]
    public class AggregateFromExpressionTest : BaseTest
    {
        public class SettingModel
        {
            public long Id { get; set; }
        }

        public class UserModel
        {
            public string Fullname { get; set; }

            public long Id { get; set; }

            public bool IsOnline { get; set; }

            public SettingModel Setting { get; set; }

            public string SomeValue { get; set; }

            public string Username { get; set; }
        }

        public class SettingMapper
        {
            public Expression<Func<Setting, SettingModel>> Map
            {
                get
                {
                    Setting setting = null;

                    return x => new SettingModel
                    {
                        Id = setting.Id
                    };
                }
            }
        }

        public class UserMapper
        {
            public Expression<Func<UserEntity, UserModel>> Map
            {
                get
                {
                    var settingMapper = new SettingMapper();

                    return x => new UserModel
                    {
                        Fullname = x.Firstname + " " + x.Lastname,
                        Id = x.Id,
                        IsOnline = x.IsOnline,
                        Setting = Aggregate.FromExpression(settingMapper.Map),
                        Username = x.Username
                    };
                }
            }
        }

        [Test]
        public void CanUseAggregateFromExpressionWithMapperWithAliasOnRoot()
        {
            var settingMapper = new SettingMapper();

            Setting setting = null;

            SettingModel[] settings = Session.FlowQuery(() => setting)
                .Select(settingMapper.Map);

            for (int i = 0; i < settings.Length; i++)
            {
                Assert.That(settings[i].Id, Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void CanUseAggregateFromExpressionWithMapper()
        {
            var userMapper = new UserMapper();

            Setting setting = null;

            UserModel[] users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .Select(userMapper.Map);

            Assert.That(users.Count(), Is.EqualTo(4));

            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];

                Assert.That(user, Is.Not.Null);
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.EqualTo(6));
                Assert.That(user.Fullname, Is.EqualTo(Fullnames[i]));
            }
        }

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