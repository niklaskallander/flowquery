namespace NHibernate.FlowQuery.Test.FlowQuery.Features
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class FlowQueryHelperProjectTest : BaseTest
    {
        [Test]
        public void CanUseFlowQueryHelperProject()
        {
            Expression<Func<UserEntity, UserDto>> expression = x => new UserDto
            {
                Fullname = x.Firstname + " " + x.Lastname,
                Id = x.Id,
                IsOnline = x.IsOnline,
                SettingId = x.Setting.Id,
                Username = x.Username
            };

            var helper = new FlowQueryHelper();

            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = helper.Project(x, expression)
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
        public void CanUseFlowQueryHelperProjectWithMapper()
        {
            var userMapper = new UserMapper();

            Setting setting = null;

            UserModel[] users = Query<UserEntity>()
                .Inner.Join(x => x.Setting, () => setting)
                .Select(userMapper.Map);

            Assert.That(users.Count(), Is.EqualTo(4));

            for (int i = 0; i < users.Length; i++)
            {
                UserModel user = users[i];

                Assert.That(user, Is.Not.Null);
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.EqualTo(6));
                Assert.That(user.Fullname, Is.EqualTo(Fullnames[i]));
            }
        }

        [Test]
        public void CanUseFlowQueryHelperProjectWithMapperWithAliasOnRoot()
        {
            var settingMapper = new SettingMapper();

            SettingModel[] settings = Session.FlowQuery<Setting>()
                .Select(settingMapper.Map);

            for (int i = 0; i < settings.Length; i++)
            {
                Assert.That(settings[i].Id, Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void UsingFlowQueryHelperProjectDirectlyThrows()
        {
            Expression<Func<UserEntity, UserDto>> expression = x => new UserDto();

            UserEntity y = null;

            var helper = new FlowQueryHelper();

            Assert
                .That
                (
                    () => helper.Project(y, expression),
                    Throws.InstanceOf<NotImplementedException>()
                );
        }

        [Test]
        public void UsingFlowQueryHelperProjectWithNullExpressionThrows()
        {
            Expression<Func<UserEntity, UserDto>> expression = null;

            var helper = new FlowQueryHelper();

            Assert
                .That
                (
                    () => Query<UserEntity>().Select(x => helper.Project(x, expression)),
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        public class SettingMapper
        {
            public Expression<Func<Setting, SettingModel>> Map
            {
                get
                {
                    return setting => new SettingModel
                    {
                        Id = setting.Id
                    };
                }
            }
        }

        public class SettingModel
        {
            public long Id { get; set; }
        }

        public class UserMapper
        {
            public Expression<Func<UserEntity, UserModel>> Map
            {
                get
                {
                    Setting setting = null;

                    var settingMapper = new SettingMapper();

                    var helper = new FlowQueryHelper();

                    return x => new UserModel
                    {
                        Fullname = x.Firstname + " " + x.Lastname,
                        Id = x.Id,
                        IsOnline = x.IsOnline,
                        Setting = helper.Project(setting, settingMapper.Map),
                        Username = x.Username
                    };
                }
            }
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
    }
}