namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class SuppressionTest : BaseTest
    {
        [Test]
        public void Example1GlobalSuppression()
        {
            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = true;

            FlowQuerySelection<UserDto> users = Session.FlowQuery<UserEntity>()
                .OrderBy<UserDto>(x => x.Fullname)
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = false;
        }

        [Test]
        public void Example2LocalSuppression()
        {
            var options = new FlowQueryOptions
            {
                ShouldSuppressOrderByProjectionErrors = true
            };

            FlowQuerySelection<UserDto> users = Session.FlowQuery<UserEntity>(options)
                .OrderBy<UserDto>(x => x.Fullname)
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
        }
    }
}