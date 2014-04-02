using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class RestrictionsTest : BaseTest
    {
        [Test]
        public void SimpleExample1()
        {
            var onlineUsers = Session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select();

            Assert.That(onlineUsers.Count(), Is.EqualTo(3));
            Assert.That(onlineUsers.All(x => x.IsOnline), Is.True);
        }

        [Test]
        public void SimpleExample2And()
        {
            var onlineAdministrators = Session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline && x.Role == RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), Is.EqualTo(2));
            Assert.That(onlineAdministrators.All(x => x.IsOnline), Is.True);
        }

        [Test]
        public void SimpleExample3Or()
        {
            var onlineOrStandardUsers = Session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline || x.Role == RoleEnum.Standard)
                .Select();

            Assert.That(onlineOrStandardUsers.Count(), Is.EqualTo(4));
            Assert.That(onlineOrStandardUsers.All(x => x.IsOnline), Is.False);
        }

        [Test]
        public void SimpleExample4UsingIsHelperEqualTo()
        {
            var administrators = Session.FlowQuery<UserEntity>()
                .Where(x => x.Role, FqIs.EqualTo(RoleEnum.Administrator))
                .Select();

            Assert.That(administrators.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SimpleExample5UsingIsHelperIn()
        {
            var privilegedUsers = Session.FlowQuery<UserEntity>()
                .Where(x => x.Role, FqIs.In(RoleEnum.Administrator, RoleEnum.Webmaster))
                .Select();

            Assert.That(privilegedUsers.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ComplexExample1UsingSubquery()
        {
            var subquery = Session.FlowQuery<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Session.FlowQuery<UserEntity>()
                .Where(x => x.Id, FqIs.In(subquery))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample2UsingWhereDelegate()
        {
            var onlineOrPrivilegedUsers = Session.FlowQuery<UserEntity>()
                .Where((x, where) => x.IsOnline || where(x.Role, FqIs.In(RoleEnum.Administrator, RoleEnum.Webmaster)))
                .Select();

            Assert.That(onlineOrPrivilegedUsers.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ComplexExample3UsingMiscContains()
        {
            var usersHavingOInFistname = Session.FlowQuery<UserEntity>()
                .Where(x => x.Firstname.Contains("o"))
                .Select();

            Assert.That(usersHavingOInFistname.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ComplexExample4UsingSubstring()
        {
            var usersWithNameStartingOnN = Session.FlowQuery<UserEntity>()
                .Where(x => x.Firstname.Substring(0, 1) == "N")
                .Select();

            Assert.That(usersWithNameStartingOnN.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ComplexExample5UsingDetachedCriteria()
        {
            var criteria = DetachedCriteria.For<UserEntity>()
                .Add(Restrictions.Between("Id", 2, 3))
                .SetProjection(Projections.Property("Id"));

            var users = Session.FlowQuery<UserEntity>()
                .Where(x => x.Id, FqIs.In(criteria))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void SimpleExample6EmptyCollection()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Where(x => x.Groups, FqIs.Empty())
                .Select(x => new
                {
                    Username = x.Username
                });

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Username, Is.EqualTo("Lajsa"));
        }

        [Test]
        public void ComplexExample6EmptySubquery()
        {
            UserEntity user = null;

            var subquery = Session.DetachedFlowQuery<UserGroupLinkEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.User.Id == user.Id)
                .Select(x => x.Id);

            var users = Session.FlowQuery<UserEntity>(() => user)
                .Where(subquery, FqIs.Empty())
                .Select(x => new
                {
                    Username = x.Username
                });

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Username, Is.EqualTo("Lajsa"));
        }

        [Test]
        public void MiscExample1RestrictByExample()
        {
            var users = Session.FlowQuery<UserEntity>()
                .RestrictByExample(new UserEntity { Firstname = "Niklas", Role = RoleEnum.Administrator }, x =>
                {
                    x.ExcludeProperty(u => u.CreatedStamp);
                    x.ExcludeProperty(u => u.IsOnline);
                    x.ExcludeProperty(u => u.NumberOfLogOns);
                    x.ExcludeZeroes();
                    x.ExcludeNulls();
                })
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Role, Is.EqualTo(RoleEnum.Administrator));
            Assert.That(users.First().Firstname, Is.EqualTo("Niklas"));
        }
    }
}