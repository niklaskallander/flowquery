using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using xIs = NUnit.Framework.Is;

    [TestFixture]
    public class OrdersTest : BaseTest
    {
        [Test]
        public void HowToExample1()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .OrderByDescending(x => x.IsOnline)
                .OrderBy(x => x.Role)
                .OrderBy(x => x.Username)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void HowToExample2UsingBoolFlag()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .OrderBy(x => x.IsOnline, false)
                .OrderBy(x => x.Role, true)
                .OrderBy(x => x.Username, true)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void HowToExample3UsingDto()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .OrderBy(x => x.IsOnline)
                .OrderBy<UserDto>(x => x.SomeValue)
                .Select(x => new UserDto()
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    Username = x.Username,
                    IsOnline = x.IsOnline,
                    SomeValue = x.Username.Substring(0, 3) + x.Firstname.Substring(0, 3) + x.Lastname.Substring(0, 3)
                });

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void HowToExample4UsingStringsAndIProjection()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .OrderByDescending("IsOnline")
                .OrderBy(Projections.Property("Role"))
                .OrderBy("Username")
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void HowToExample5ClearOrders()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>()
                .OrderBy(x => x.IsOnline, false)
                .OrderBy(x => x.Role, true)
                .OrderBy(x => x.Username, true);

            IMorphableFlowQuery morphable = query as IMorphableFlowQuery;

            Assert.That(morphable.Orders.Count, xIs.EqualTo(3));

            query
                .ClearOrders();

            Assert.That(morphable.Orders.Count, xIs.EqualTo(0));
        }
    }
}