using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ProjectionsTest : BaseTest
    {
        [Test]
        public void SimpleExample1()
        {
            IEnumerable<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample2SingleProperties()
        {
            IEnumerable<string> usernames = Session.FlowQuery<UserEntity>()
                .Select(x => x.Username);

            Assert.That(usernames.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample3MultipleProperties()
        {
            IEnumerable<UserEntity> users = Session.FlowQuery<UserEntity>()
                .Select(x => x.Username, x => x.Firstname, x => x.Lastname);

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample4SingleAndMultiplePropertiesUsingMagicStrings()
        {
            var query = Session.FlowQuery<UserEntity>();

            IEnumerable<UserEntity> usernames = query.Select("Username");

            Assert.That(usernames, Is.Not.Null);
            Assert.That(usernames.Count(), Is.EqualTo(4));

            IEnumerable<UserEntity> users = query.Select("Username", "Firstname", "Lastname");

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample5SinglePropertyUsingMagicStringWithSpecifiedType()
        {
            IEnumerable<string> usernames = Session.FlowQuery<UserEntity>()
                .Select<string>("Username");

            Assert.That(usernames.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample6UsingIProjection()
        {
            IEnumerable<object> usernames = Session.FlowQuery<UserEntity>()
                .Select(Projections.Property("Username"));

            Assert.That(usernames.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SimpleExample7UsingIProjectionWithSpecifiedType()
        {
            IEnumerable<string> usernames = Session.FlowQuery<UserEntity>()
                .Select<string>(Projections.Property("Username"));

            Assert.That(usernames.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample1()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    Username = x.Username,
                    IsOnline = x.IsOnline
                });

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample2WithConstructor()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new UserDto
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    Username = x.Username,
                    IsOnline = x.IsOnline
                });

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample3Anonymous()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Fullname = x.Firstname + " " + x.Lastname,
                    x.Username,
                    x.IsOnline
                });

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample4AnonymousWithNestedType()
        {
            var users = Session.FlowQuery<UserEntity>()
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

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ComplexExample5ProjectingSubquery()
        {
            UserEntity user = null;

            var groupCountSubquery = Session.DetachedFlowQuery<UserGroupLinkEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.User.Id == user.Id)
                .Count();

            var userModels = Session.FlowQuery<UserEntity>(() => user)
                .Select(x => new
                {
                    Username = x.Username,
                    NumberOfGroups = Aggregate.Subquery<int>(groupCountSubquery)
                });

            Assert.That(userModels.Count(), Is.EqualTo(4));

            foreach (var userModel in userModels)
            {
                if (userModel.Username == "Lajsa")
                {
                    Assert.That(userModel.NumberOfGroups, Is.EqualTo(0));
                }
                else
                {
                    Assert.That(userModel.NumberOfGroups, Is.GreaterThan(0));
                }
            }
        }

        [Test]
        public void ComplexExample6TypeCast()
        {
            var average = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    AverageIdValue = (decimal)Aggregate.Average(x.Id)
                });

            Assert.That(average.Count(), Is.EqualTo(1));
            Assert.That(average.First().AverageIdValue, Is.EqualTo(2.5M));
        }

        [Test]
        public void SelectDictionaryExample1()
        {
            var users = Session.FlowQuery<UserEntity>()
                .SelectDictionary(x => x.Id, x => x.Username);

            Assert.That(users.Count, Is.EqualTo(4));
        }

        [Test]
        public void AggregationsExample1()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .Select(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns),
                    Role = Aggregate.GroupBy(x.Role)
                });

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample2WithSelectSetup()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .Select<AverageLogOnsPerRoleModel>()
                    .For(x => x.AverageLogOns).Use(x => Aggregate.Average(x.NumberOfLogOns))
                    .For(x => x.Role).Use(x => Aggregate.GroupBy(x.Role))
                    .Select();

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample3WithPartialSelect()
        {
            var selectionBuilder = Session.FlowQuery<UserEntity>()
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

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample4_Example1WithInferredGroupBy()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .Select(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns),
                    Role = x.Role
                });

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample5_Example2WithInferredGroupBy()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .Select<AverageLogOnsPerRoleModel>()
                    .For(x => x.AverageLogOns).Use(x => Aggregate.Average(x.NumberOfLogOns))
                    .For(x => x.Role).Use(x => x.Role)
                    .Select();

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample6_Example3WithInferredGroupBy()
        {
            var selectionBuilder = Session.FlowQuery<UserEntity>()
                .PartialSelect(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns)
                });

            selectionBuilder
                .Add(x => new AverageLogOnsPerRoleModel
                {
                    Role = x.Role
                });

            var averageLogOnsPerRole = selectionBuilder
                .Select();

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }


        [Test]
        public void AggregationsExample7_Example1WithSeparateGroupBy()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .GroupBy(x => x.Role)
                .Select(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns),
                    Role = x.Role
                });

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AggregationsExample8_Example1WithSeparateGroupByWithoutProjection()
        {
            var averageLogOnsPerRole = Session.FlowQuery<UserEntity>()
                .GroupBy(x => x.Role)
                .Select(x => new AverageLogOnsPerRoleModel
                {
                    AverageLogOns = Aggregate.Average(x.NumberOfLogOns)
                });

            Assert.That(averageLogOnsPerRole.Count(), Is.EqualTo(3));
        }

        [Test]
        public void SelectSetupExample1()
        {
            IEnumerable<UserDto> users = Session.FlowQuery<UserEntity>()
                .Select<UserDto>()
                    .For(x => x.Fullname).Use(x => x.Firstname + " " + x.Lastname)
                    .For(x => x.Username).Use(x => x.Username)
                    .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void PartialSelectExample1()
        {
            var selectionBuilder = Session.FlowQuery<UserEntity>()
                .PartialSelect<UserDto>(x => new UserDto(x.Firstname + " " + x.Lastname));

            selectionBuilder
                .Add(x => new UserDto { Username = x.Username });

            var users = selectionBuilder
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void DistinctExample1()
        {
            var roles = Session.FlowQuery<UserEntity>()
                .Distinct()
                .Select(x => x.Role);

            Assert.That(roles.Count(), Is.EqualTo(3));
        }

        [Test]
        public void DistinctExample2Indistinct()
        {
            var query = Session.FlowQuery<UserEntity>();

            query.Distinct();

            //... code ...//

            var roles = query
                .Indistinct()
                .Select(x => x.Role);

            Assert.That(roles.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CountExample1()
        {
            int count = Session.FlowQuery<UserEntity>()
                .Count();

            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void CountExample1DistinctProperty()
        {
            int count = Session.FlowQuery<UserEntity>()
                .Distinct()
                .Count(x => x.Role);

            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void LimitExample1Limit()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Limit(3, 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void LimitExample2Limit()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Limit(3)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void LimitExample3SkipAndTake()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Skip(1)
                .Take(3)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void LimitExample3ClearLimit()
        {
            var query = Session.FlowQuery<UserEntity>()
                .Skip(1)
                .Take(3);

            var morphable = (IMorphableFlowQuery)query;

            Assert.That(morphable.ResultsToSkip, Is.EqualTo(1));
            Assert.That(morphable.ResultsToTake, Is.EqualTo(3));

            query
                .ClearLimit();

            Assert.That(morphable.ResultsToSkip, Is.Null);
            Assert.That(morphable.ResultsToTake, Is.Null);
        }

        [Test]
        public void MiscExample1Comparison()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    IsAdministrator = x.Role == RoleEnum.Administrator
                });

            Assert.That(users.Count(x => x.IsAdministrator), Is.EqualTo(2));
        }

        [Test]
        public void MiscExample2Ternary()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    IsAdministrator = x.Role == RoleEnum.Administrator
                        ? true
                        : false
                });

            Assert.That(users.Count(x => x.IsAdministrator), Is.EqualTo(2));
        }

        [Test]
        public void MiscExample3TernaryWithProjections()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    Name = x.Role == RoleEnum.Administrator
                        ? x.Lastname + ", " + x.Firstname
                        : x.Username
                });

            Assert.That(users.Count(x => x.Name.Contains(", ")), Is.EqualTo(2));
        }

        [Test]
        public void MiscExample4Coalesce()
        {
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    x.Username,
                    Value = x.Password ?? x.Username
                });

            Assert.That(users.Count(x => x.Username == x.Value), Is.EqualTo(1));
        }

        [Test]
        public void MiscExample5Substring()
        {
            var users = Session.FlowQuery<UserEntity>()
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
            var users = Session.FlowQuery<UserEntity>()
                .Select(x => new
                {
                    ContainsO = x.Firstname.Contains("o"),
                    StartsWithN = x.Firstname.StartsWith("n"),
                    EndsWithN = x.Firstname.EndsWith("n")
                });

            Assert.That(users.Count(x => x.ContainsO), Is.EqualTo(2));
            Assert.That(users.Count(x => x.StartsWithN), Is.EqualTo(1));
            Assert.That(users.Count(x => x.EndsWithN), Is.EqualTo(1));
        }
    }

    internal class AverageLogOnsPerRoleModel
    {
        public double AverageLogOns { get; set; }

        public RoleEnum Role { get; set; }
    }
}