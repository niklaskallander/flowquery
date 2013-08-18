using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ReusabilityTest : BaseTest
    {
        [Test]
        public virtual void TestBasicReuseSelectPlusCount()
        {
            var query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var count = query.Count();

            var users = query.Select();

            Assert.That(count, Is.EqualTo(3));
            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));
        }

        [Test]
        public virtual void TestReuseCleanSelectPlusComplexSelect()
        {
            var query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var users = query.Select();

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },

                x.LastLoggedInStamp
            });

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseComplexSelectPlusCleanSelect()
        {
            var query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },

                x.LastLoggedInStamp
            });

            var users = query.Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseComplexSelectPlusSelectDictionary()
        {
            var query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },

                x.LastLoggedInStamp
            });

            var userDictionary = query
                .SelectDictionary(x => x.Id, x => x.IsOnline);

            Assert.That(userDictionary.Count(), Is.EqualTo(3));
            Assert.That(userDictionary.All(x => x.Value));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseSelectSetupPlusSelectDictionary()
        {
            var query = Query<UserEntity>()
                .Where(x => !x.IsOnline);

            var setupUsers = query
                .Select<UserDto>()
                    .For(x => x.Id).Use(x => x.Id)
                    .For(x => x.IsOnline).Use(x => x.IsOnline)
                    .For(x => x.Username).Use(x => x.Username)
                .Select();

            var userDictionary = query
                .SelectDictionary(x => x.Id, x => x.IsOnline);

            Assert.That(userDictionary.Count(), Is.EqualTo(1));
            Assert.That(userDictionary.All(x => !x.Value));

            Assert.That(setupUsers.Count(), Is.EqualTo(1));
            Assert.That(setupUsers.All(x => !x.IsOnline));
        }

        [Test]
        public virtual void TestReuseAsSubQueryForRootQuery()
        {
            var query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var detached = query
                .Detached()
                .Where(x => x.Lastname.StartsWith("K"))
                .Select(x => x.Id);

            query.Where(x => x.Id, FlowQueryIs.In(detached));

            var users = query.Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }
    }
}