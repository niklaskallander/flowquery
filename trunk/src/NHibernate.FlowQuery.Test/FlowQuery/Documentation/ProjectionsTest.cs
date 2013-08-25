using System.Collections.Generic;
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
    public class ProjectionsTest : BaseTest
    {
        [Test]
        public void SimpleExample1()
        {
            ISession session = Session;

            IEnumerable<UserEntity> users = session.FlowQuery<UserEntity>()
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample2SingleProperties()
        {
            ISession session = Session;

            IEnumerable<string> usernames = session.FlowQuery<UserEntity>()
                .Select(x => x.Username);

            Assert.That(usernames.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample3MultipleProperties()
        {
            ISession session = Session;

            IEnumerable<UserEntity> users = session.FlowQuery<UserEntity>()
                .Select(x => x.Username, x => x.Firstname, x => x.Lastname);

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample4SingleAndMultiplePropertiesUsingMagicStrings()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>();

            IEnumerable<UserEntity> usernames = query.Select("Username");

            Assert.That(usernames, xIs.Not.Null);
            Assert.That(usernames.Count(), xIs.EqualTo(4));

            IEnumerable<UserEntity> users = query.Select("Username", "Firstname", "Lastname");

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample5SinglePropertyUsingMagicStringWithSpecifiedType()
        {
            ISession session = Session;

            IEnumerable<string> usernames = session.FlowQuery<UserEntity>()
                .Select<string>("Username");

            Assert.That(usernames.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample6UsingIProjection()
        {
            ISession session = Session;

            IEnumerable<object> usernames = session.FlowQuery<UserEntity>()
                .Select(Projections.Property("Username"));

            Assert.That(usernames.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void SimpleExample7UsingIProjectionWithSpecifiedType()
        {
            ISession session = Session;

            IEnumerable<string> usernames = session.FlowQuery<UserEntity>()
                .Select<string>(Projections.Property("Username"));

            Assert.That(usernames.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample1()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    Username = x.Username,
                    IsOnline = x.IsOnline
                });

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample2WithConstructor()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    Username = x.Username,
                    IsOnline = x.IsOnline
                });

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample3Anonymous()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    x.Username,
                    x.IsOnline
                });

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample4AnonymousWithNestedType()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Dto = new UserDto(x.Firstname + " " + x.Lastname)
                    {
                        Username = x.Username,
                        IsOnline = x.IsOnline
                    },

                    x.LastLoggedInStamp,
                    x.Role,
                    x.Id
                });

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void ComplexExample5SelectDictionary()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .SelectDictionary(x => x.Id, x => x.Username);

            Assert.That(users.Count, xIs.EqualTo(4));
        }

        [Test]
        public void AggregationsExample1()
        {
            ISession session = Session;

            var averageLogOnsPerRole = session.FlowQuery<UserEntity>()
                .Select(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns),
                    Role = Aggregate.GroupBy(x.Role)
                });

            Assert.That(averageLogOnsPerRole.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void AggregationsExample2WithSelectSetup()
        {
            ISession session = Session;

            var averageLogOnsPerRole = session.FlowQuery<UserEntity>()
                .Select<AverageLogOnsPerRoleModel>()
                    .For(x => x.AverageLogOns).Use(x => Aggregate.Average(x.NumberOfLogOns))
                    .For(x => x.Role).Use(x => Aggregate.GroupBy(x.Role))
                    .Select();

            Assert.That(averageLogOnsPerRole.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void AggregationsExample3WithPartialSelect()
        {
            ISession session = Session;

            var selectionBuilder = session.FlowQuery<UserEntity>()
                .PartialSelect(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns)
                });

            selectionBuilder
                .Add(x => new AverageLogOnsPerRoleModel
                {
                    Role = Aggregate.GroupBy(x.Role)
                });

            var averageLogOnsPerRole = selectionBuilder
                .Select();

            Assert.That(averageLogOnsPerRole.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void SelectSetupExample1()
        {
            ISession session = Session;

            IEnumerable<UserDto> users = session.FlowQuery<UserEntity>()
                .Select<UserDto>()
                    .For(x => x.Fullname).Use(x => x.Firstname + " " + x.Lastname)
                    .For(x => x.Username).Use(x => x.Username)
                    .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void PartialSelectExample1()
        {
            ISession session = Session;

            var selectionBuilder = session.FlowQuery<UserEntity>()
                .PartialSelect<UserDto>(x => new UserDto(x.Firstname + " " + x.Lastname));

            selectionBuilder
                .Add(x => new UserDto() { Username = x.Username });

            var users = selectionBuilder
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void DistinctExample1()
        {
            ISession session = Session;

            var roles = session.FlowQuery<UserEntity>()
                .Distinct()
                .Select(x => x.Role);

            Assert.That(roles.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void DistinctExample2Indistinct()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>();

            query.Distinct();

            //... code ...//

            var roles = query
                .Indistinct()
                .Select(x => x.Role);

            Assert.That(roles.Count(), xIs.EqualTo(4));
        }

        [Test]
        public void CountExample1()
        {
            ISession session = Session;

            int count = session.FlowQuery<UserEntity>()
                .Count();

            Assert.That(count, xIs.EqualTo(4));
        }

        [Test]
        public void CountExample1DistinctProperty()
        {
            ISession session = Session;

            int count = session.FlowQuery<UserEntity>()
                .Distinct()
                .Count(x => x.Role);

            Assert.That(count, xIs.EqualTo(3));
        }

        [Test]
        public void LimitExample1Limit()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Limit(3, 1)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void LimitExample2Limit()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Limit(3)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void LimitExample3SkipAndTake()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Skip(1)
                .Take(3)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(3));
        }

        [Test]
        public void LimitExample3ClearLimit()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>()
                .Skip(1)
                .Take(3);

            IMorphableFlowQuery morphable = query as IMorphableFlowQuery;

            Assert.That(morphable.ResultsToSkip, xIs.EqualTo(1));
            Assert.That(morphable.ResultsToTake, xIs.EqualTo(3));

            query
                .ClearLimit();

            Assert.That(morphable.ResultsToSkip, xIs.Null);
            Assert.That(morphable.ResultsToTake, xIs.Null);
        }

        [Test]
        public void MiscExample1Comparison()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    IsAdministrator = x.Role == RoleEnum.Administrator
                });

            Assert.That(users.Count(x => x.IsAdministrator), xIs.EqualTo(2));
        }

        [Test]
        public void MiscExample2Ternary()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    IsAdministrator = x.Role == RoleEnum.Administrator
                        ? true
                        : false
                });

            Assert.That(users.Count(x => x.IsAdministrator), xIs.EqualTo(2));
        }

        [Test]
        public void MiscExample3TernaryWithProjections()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Name = x.Role == RoleEnum.Administrator
                        ? x.Lastname + ", " + x.Firstname
                        : x.Username
                });

            Assert.That(users.Count(x => x.Name.Contains(", ")), xIs.EqualTo(2));
        }

        [Test]
        public void MiscExample4Coalesce()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    x.Username,
                    Value = x.Password ?? x.Username
                });

            Assert.That(users.Count(x => x.Username == x.Value), xIs.EqualTo(1));
        }

        [Test]
        public void MiscExample5Substring()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Initials = (x.Firstname.Substring(0, 1) + x.Lastname.Substring(0, 1)),
                    x.Firstname,
                    x.Lastname
                });

            Assert.That(users.All(x => x.Initials.Length == 2));
            Assert.That(users.All(x => x.Initials == (x.Firstname.Substring(0, 1) + x.Lastname.Substring(0, 1))));
        }

        [Test]
        public void MiscExample6StartsWithEndsWithContains()
        {
            ISession session = Session;

            var users = session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    ContainsO = x.Firstname.Contains("o"),
                    StartsWithN = x.Firstname.StartsWith("n"),
                    EndsWithN = x.Firstname.EndsWith("n")
                });

            Assert.That(users.Count(x => x.ContainsO), xIs.EqualTo(2));
            Assert.That(users.Count(x => x.StartsWithN), xIs.EqualTo(1));
            Assert.That(users.Count(x => x.EndsWithN), xIs.EqualTo(1));
        }
    }

    internal class AverageLogOnsPerRoleModel
    {
        public decimal AverageLogOns { get; set; }

        public RoleEnum Role { get; set; }
    }
}