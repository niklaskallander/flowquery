using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using xIs = NUnit.Framework.Is;

    [TestFixture]
    public class RestrictionsTest : BaseTest
    {
        [Test]
        public void SimpleExample1()
        {
            ISession session = Session;

            var onlineUsers = session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline)
                .Select();

            Assert.That(onlineUsers.Count(), xIs.EqualTo(3));
            Assert.That(onlineUsers.All(x => x.IsOnline), xIs.True);
        }

        [Test]
        public void SimpleExample2And()
        {
            ISession session = Session;

            var onlineAdministrators = session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline && x.Role == RoleEnum.Administrator)
                .Select();

            Assert.That(onlineAdministrators.Count(), xIs.EqualTo(2));
            Assert.That(onlineAdministrators.All(x => x.IsOnline), xIs.True);
        }

        [Test]
        public void SimpleExample3Or()
        {
            ISession session = Session;

            var onlineOrStandardUsers = session.FlowQuery<UserEntity>()
                .Where(x => x.IsOnline || x.Role == RoleEnum.Standard)
                .Select();

            Assert.That(onlineOrStandardUsers.Count(), xIs.EqualTo(4));
            Assert.That(onlineOrStandardUsers.All(x => x.IsOnline), xIs.False);
        }

        [Test]
        public void SimpleExample4UsingIsHelperEqualTo()
        {
            ISession session = Session;

            var administrators = session.FlowQuery<UserEntity>()
                .Where(x => x.Role, Is.EqualTo(RoleEnum.Administrator))
                .Select();

            Assert.That(administrators.Count(), xIs.EqualTo(2));
        }

        [Test]
        public void SimpleExample5UsingIsHelperIn()
        {
            ISession session = Session;

            var privilegedUsers = session.FlowQuery<UserEntity>()
                .Where(x => x.Role, Is.In(RoleEnum.Administrator, RoleEnum.Webmaster))
                .Select();

            Assert.That(privilegedUsers.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void ComplexExample1UsingSubQuery()
        {
            ISession session = Session;

            var subquery = session.FlowQuery<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = session.FlowQuery<UserEntity>()
                .Where(x => x.Id, Is.In(subquery))
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample2UsingWhereDelegate()
        {
            ISession session = Session;

            var onlineOrPrivilegedUsers = session.FlowQuery<UserEntity>()
                .Where((x, where) => x.IsOnline || where(x.Role, Is.In(RoleEnum.Administrator, RoleEnum.Webmaster)))
                .Select();

            Assert.That(onlineOrPrivilegedUsers.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void ComplexExample3UsingMiscContains()
        {
            ISession session = Session;

            var usersHavingOInFistname = session.FlowQuery<UserEntity>()
                .Where(x => x.Firstname.Contains("o"))
                .Select();

            Assert.That(usersHavingOInFistname.Count(), xIs.EqualTo(2));
        }

        [Test]
        public void ComplexExample4UsingSubstring()
        {
            ISession session = Session;

            var usersWithNameStartingOnN = session.FlowQuery<UserEntity>()
                .Where(x => x.Firstname.Substring(0, 1) == "N")
                .Select();

            Assert.That(usersWithNameStartingOnN.Count(), xIs.EqualTo(1));
        }

        [Test]
        public void MiscExample1RestrictByExample()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .RestrictByExample(new UserEntity() { Firstname = "Niklas", Role = RoleEnum.Administrator }, x =>
                {
                    x.ExcludeProperty(u => u.CreatedStamp);
                    x.ExcludeProperty(u => u.IsOnline);
                    x.ExcludeProperty(u => u.NumberOfLogOns);
                    x.ExcludeZeroes();
                    x.ExcludeNulls();
                })
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(1));
            Assert.That(users.First().Role, xIs.EqualTo(RoleEnum.Administrator));
            Assert.That(users.First().Firstname, xIs.EqualTo("Niklas"));
        }
    }
}