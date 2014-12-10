namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    using AutoMapper;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.AutoMapping;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using Property = NHibernate.FlowQuery.Property;

    [TestFixture]
    public class SelectTest : BaseTest
    {
        [Test]
        public void CanTrimStartOfStringWithoutAlsoTrimmingEnd()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = "     " + x.Username + "     ",
                    TrimmedValue = ("     " + x.Username + "     ").TrimStart()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.TrimmedValue);
                Assert.That(user.Value.TrimStart() == user.TrimmedValue);
                Assert.That(user.TrimmedValue.Last() == ' ');
            }
        }

        [Test]
        public void CanTrimEndOfStringWithoutAlsoTrimmingStart()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = "     " + x.Username + "     ",
                    TrimmedValue = ("     " + x.Username + "     ").TrimEnd()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.TrimmedValue);
                Assert.That(user.Value.TrimEnd() == user.TrimmedValue);
                Assert.That(user.TrimmedValue.First() == ' ');
            }
        }

        [Test]
        public void CanTrimString()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = "     " + x.Username + "     ",
                    TrimmedValue = ("     " + x.Username + "     ").Trim()
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.TrimmedValue);
                Assert.That(user.Value.Trim() == user.TrimmedValue);
            }
        }

        [Test]
        public void CanUseMathRoundInProjectionReturningDecimal()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = x.NumberOfLogOns * 1.234M,
                    RoundedValue = Math.Round(x.NumberOfLogOns * 1.234M)
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.RoundedValue);
                Assert.That(Math.Round(user.Value) == user.RoundedValue);
            }
        }

        [Test]
        public void CanUseMathRoundInProjectionSpecifyingDecimalPointsReturningDecimal()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = x.NumberOfLogOns * 1.234567M,
                    RoundedValue = Math.Round(x.NumberOfLogOns * 1.234567M, 3)
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.RoundedValue);
                Assert.That(Math.Round(user.Value, 3) == user.RoundedValue);
            }
        }

        [Test]
        public void CanUseMathRoundInProjectionReturningDouble()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = x.NumberOfLogOns * 1.234D,
                    RoundedValue = Math.Round(x.NumberOfLogOns * 1.234D)
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.RoundedValue);
                Assert.That(Math.Round(user.Value) == user.RoundedValue);
            }
        }

        [Test]
        public void CanUseMathRoundInProjectionSpecifyingDecimalPointsReturningDouble()
        {
            var users = Query<UserEntity>()
                .Select(x => new
                {
                    Value = x.NumberOfLogOns * 1.234567D,
                    RoundedValue = Math.Round(x.NumberOfLogOns * 1.234567D, 3)
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Value != user.RoundedValue);
                Assert.That(Math.Round(user.Value, 3) == user.RoundedValue);
            }
        }

        [Test]
        public void AttemptToSelectInvalidAggregationThrows()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select(x => x.Id.GetHashCode()),
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        [Test]
        public void CanAutoMapSpecificPropertiesUsingExpressions()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select(x => x.Id, x => x.Username)
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserDto u in users)
            {
                Assert.That(u.Id, Is.GreaterThan(0));
                Assert.That(u.Username.Length, Is.GreaterThan(0));
                Assert.That(u.Fullname, Is.Null);
            }
        }

        [Test]
        public void CanAutoMapUsingAutoMapMapper()
        {
            Mapper.CreateMap<UserEntity, UserDto>();

            Mapping.SetMapper(new AutoMapMapper());

            FlowQuerySelection<UserDto> dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));

            foreach (UserDto item in dtos)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Fullname, Is.Null);
                Assert.That(item.SettingId, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanAutoMapUsingCustomMapper()
        {
            var mapper = new CustomMapper();

            mapper
                .AddMap<UserEntity, UserDto>(x => new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    Username = x.Username
                });

            Mapping.SetMapper(mapper);

            FlowQuerySelection<UserDto> dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));

            foreach (UserDto item in dtos)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(Fullnames, Contains.Item(item.Fullname));
            }
        }

        [Test]
        public void CanAutoMapUsingStrings()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select("Username")
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserDto u in users)
            {
                Assert.That(Usernames, Contains.Item(u.Username));
            }
        }

        [Test]
        public void CanClearGroupBys()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.GroupBys.Count, Is.EqualTo(0));

            query.GroupBy(x => x.Id);

            Assert.That(queryable.GroupBys.Count, Is.EqualTo(1));

            query.GroupBy(x => x.IsOnline);

            Assert.That(queryable.GroupBys.Count, Is.EqualTo(2));

            query.ClearGroupBys();

            Assert.That(queryable.GroupBys.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanSelectAggregationInGroupByUsingAggregateHelper()
        {
            var aggregations = Query<UserEntity>()
                .Select(u => new
                {
                    Count = Aggregate.Count(u.Id),
                    Role = Aggregate.GroupBy(u.Role)
                });

            Assert.That(aggregations.Count(), Is.EqualTo(3));

            foreach (var a in aggregations)
            {
                if (a.Role == RoleEnum.Administrator)
                {
                    Assert.That(a.Count, Is.EqualTo(2));
                }
                else
                {
                    Assert.That(a.Count, Is.EqualTo(1));
                }
            }
        }

        [Test]
        public void CanSelectAggregationUsingAggregateHelper()
        {
            var aggregation = Query<UserEntity>()
                .Select(u => new
                {
                    Avg = Aggregate.Average(u.Id),
                    Sum = Aggregate.Sum(u.Id),
                    Min = Aggregate.Min(u.Id),
                    Max = Aggregate.Max(u.Id),
                    Cnt = Aggregate.Count(u.Id),
                    Cnd = Aggregate.CountDistinct(u.Id)
                });

            Assert.That(aggregation.Count(), Is.EqualTo(1));

            var a = aggregation.First();
            Assert.That(a.Avg, Is.EqualTo(2.5));
            Assert.That(a.Sum, Is.EqualTo(10));
            Assert.That(a.Min, Is.EqualTo(1));
            Assert.That(a.Max, Is.EqualTo(4));
            Assert.That(a.Cnt, Is.EqualTo(4));
            Assert.That(a.Cnd, Is.EqualTo(4));
        }

        [Test]
        public void CanSelectAll()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>().Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    u.Username
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
            }
        }

        [Test]
        public void CanSelectArithmeticOperations()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    Value = (((u.Id - 1) * 2) + 15) / u.Id,
                    u.Id
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Value, Is.EqualTo((((item.Id - 1) * 2) + 15) / item.Id));
            }
        }

        [Test]
        public void CanSelectBinaryExpressions()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    IsAdministrator = u.Role == RoleEnum.Administrator,
                    u.Role
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectConcatenatedProperties()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    Fullname = u.Firstname + (" " + u.Lastname)
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
            }
        }

        [Test]
        public void CanSelectConcatenatedPropertiesWithoutNewExpression()
        {
            FlowQuerySelection<string> names = Query<UserEntity>()
                .Select(u => u.Firstname + " " + u.Lastname);

            Assert.That(names.Count(), Is.EqualTo(4));

            foreach (string name in names)
            {
                Assert.That(Fullnames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectConditionals()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    // ReSharper disable once RedundantTernaryExpression
                    IsAdministrator = u.Role == RoleEnum.Administrator ? true : false,
                    u.Role
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectConditionalsWithMixedProjectionAndConstant()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    // ReSharper disable once SimplifyConditionalTernaryExpression
                    IsAdministrator = u.Role == RoleEnum.Administrator ? u.IsOnline : false,
                    u.Role,
                    u.IsOnline
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator && item.IsOnline)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectConditionalsWithMixedProjectionAndLocalConstant()
        {
            const bool NotTrue = false;

            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    // ReSharper disable once SimplifyConditionalTernaryExpression
                    IsAdministrator = u.Role == RoleEnum.Administrator ? true : NotTrue,
                    u.Role
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectConditionalsWithMixedProjectionAndLocalVariable()
        {
            // ReSharper disable once ConvertToConstant.Local
            bool notTrue = false;

            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    // ReSharper disable once SimplifyConditionalTernaryExpression
                    IsAdministrator = u.Role == RoleEnum.Administrator ? true : notTrue,
                    u.Role
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectFromJoinedEntityProjections()
        {
            UserGroupLinkEntity link = null;

            var ids = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Select(u => new
                {
                    link.Id
                });

            Assert.That(ids, Is.Not.Empty);

            foreach (var id in ids)
            {
                Assert.That(id.Id, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanSelectMemberInitNestedInAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    Something = u.CreatedStamp,
                    Member = new UserDto(u.Firstname + " " + u.Lastname)
                    {
                        Id = u.Id,
                        Username = u.Username
                    }
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                Assert.That(item.Something, Is.Not.EqualTo(new DateTime()));
                Assert.That(item.Member, Is.Not.Null);
                Assert.That(Ids, Contains.Item(item.Member.Id));
                Assert.That(Fullnames, Contains.Item(item.Member.Fullname));
                Assert.That(Usernames, Contains.Item(item.Member.Username));
            }
        }

        [Test]
        public void CanSelectMultipleBinaryExpressions()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    IsAdministrator = u.Role == RoleEnum.Administrator && u.Id < 3,
                    u.Role,
                    u.Id
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator && item.Id < 3)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectMultipleBinaryExpressionsWithOr()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    IsAdministrator = u.Role == RoleEnum.Administrator || u.Id < 3,
                    u.Role,
                    u.Id
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator || item.Id < 3)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectNestedAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    u.Username,
                    Name = new
                    {
                        u.Firstname,
                        u.Lastname
                    }
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Firstnames, Contains.Item(item.Name.Firstname));
                Assert.That(Lastnames, Contains.Item(item.Name.Lastname));
            }
        }

        [Test]
        public void CanSelectNestedConditionals()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new
                {
                    // ReSharper disable once SimplifyConditionalTernaryExpression, RedundantTernaryExpression
                    IsAdministrator = u.Role == RoleEnum.Administrator
                        ? true
                        : u.Id < 3
                            ? true
                            : false,
                    u.Role,
                    u.Id
                });

            Assert.That(anonymous.Count(), Is.EqualTo(4));

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator || item.Id < 3)
                {
                    Assert.That(item.IsAdministrator, Is.True);
                }
                else
                {
                    Assert.That(item.IsAdministrator, Is.False);
                }
            }
        }

        [Test]
        public void CanSelectNestedEntitiysIdWithoutJoin()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>().Select(u => u.Setting.Id);

            Assert.That(ids.Count(), Is.EqualTo(4));

            foreach (long id in ids)
            {
                Assert.That(id, Is.EqualTo(6));
            }
        }

        [Test]
        public void CanSelectNestedMemberInits()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select(x => new UserEntity
                {
                    Id = x.Id,
                    Setting = new Setting
                    {
                        Id = x.Setting.Id
                    }
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanSelectPartially()
        {
            IPartialSelection<UserEntity, UserDto> partialSelection = Query<UserEntity>()
                .PartialSelect(x => new UserDto(x.Firstname + " " + x.Lastname));

            partialSelection
                .Add(x => new UserDto
                {
                    Id = x.Id
                });

            partialSelection
                .Add(x => new UserDto
                {
                    Username = x.Username
                });

            FlowQuerySelection<UserDto> users = partialSelection
                .Select();

            foreach (UserDto user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(Fullnames, Contains.Item(user.Fullname));
                Assert.That(Usernames, Contains.Item(user.Username));
            }
        }

        [Test]
        public void CanSelectSingleProperty()
        {
            FlowQuerySelection<string> names = Query<UserEntity>().Select(u => u.Username);

            Assert.That(names.Count(), Is.EqualTo(4));

            foreach (string name in names)
            {
                Assert.That(Usernames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectSinglePropertyWithSeparateGroupBy()
        {
            bool[] values = Query<UserEntity>()
                .OrderBy(x => x.IsOnline)
                .GroupBy(x => x.IsOnline)
                .Select<bool>(Projections.Property("IsOnline"))
                .ToArray();

            Assert.That(values.Length, Is.EqualTo(2));
            Assert.That(values[0], Is.False);
            Assert.That(values[1], Is.True);
        }

        [Test]
        public void CanSelectSpecificPropertiesUsingExpressions()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select(u => u.Id, u => u.Role);

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.Not.EqualTo(0));
                Assert.That(user.Role, Is.Not.EqualTo(RoleEnum.Default));
                Assert.That(user.Firstname, Is.Null);
                Assert.That(user.Lastname, Is.Null);
                Assert.That(user.Username, Is.Null);
                Assert.That(user.Password, Is.Null);
                Assert.That(user.Groups.Count(), Is.EqualTo(0));
                Assert.That(user.CreatedStamp, Is.EqualTo(new DateTime()));
            }
        }

        [Test]
        public void CanSelectSpecificPropertiesUsingMemberInit()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select(u => new UserEntity
                {
                    Id = u.Id,
                    Role = u.Role
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.Not.EqualTo(0));
                Assert.That(user.Role, Is.Not.EqualTo(RoleEnum.Default));
                Assert.That(user.Firstname, Is.Null);
                Assert.That(user.Lastname, Is.Null);
                Assert.That(user.Username, Is.Null);
                Assert.That(user.Password, Is.Null);
                Assert.That(user.Groups.Count(), Is.EqualTo(0));
                Assert.That(user.CreatedStamp, Is.EqualTo(new DateTime()));
            }
        }

        [Test]
        public void CanSelectSpecificPropertiesUsingStrings()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select("Id", "Role");

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.Not.EqualTo(0));
                Assert.That(user.Role, Is.Not.EqualTo(RoleEnum.Default));
                Assert.That(user.Firstname, Is.Null);
                Assert.That(user.Lastname, Is.Null);
                Assert.That(user.Username, Is.Null);
                Assert.That(user.Password, Is.Null);
                Assert.That(user.Groups.Count(), Is.EqualTo(0));
                Assert.That(user.CreatedStamp, Is.EqualTo(new DateTime()));
            }
        }

        [Test]
        public void CanSelectSubstring()
        {
            FlowQuerySelection<string> shortNames = Query<UserEntity>().Select(x => x.Username.Substring(0, 1));

            Assert.That(shortNames.Count(), Is.EqualTo(4));

            foreach (string shortName in shortNames)
            {
                Assert.That(shortName.Length, Is.EqualTo(1));
                Assert.That(shortName.Trim(), Is.Not.EqualTo(string.Empty));
            }
        }

        [Test]
        public void CanSelectToClassWithPublicFields()
        {
            FlowQuerySelection<PuffClass> puffClasses = Query<UserEntity>()
                .Select(x => new PuffClass
                {
                    Puff = x.Firstname + " " + x.Lastname
                });

            Assert.That(puffClasses.Count(), Is.EqualTo(4));

            foreach (PuffClass pc in puffClasses)
            {
                Assert.That(Fullnames, Contains.Item(pc.Puff));
            }
        }

        [Test]
        public void CanSelectTypedUsingProjection()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select<UserDto>(Projections.Alias(Projections.Property("Username"), "Username"));

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserDto u in users)
            {
                Assert.That(Usernames, Contains.Item(u.Username));
            }
        }

        [Test]
        public void CanSelectTypedUsingPropertyProjection()
        {
            FlowQuerySelection<string> names = Query<UserEntity>()
                .Select<string>(Projections.Property("Firstname"));

            Assert.That(names.Count(), Is.EqualTo(4));

            foreach (string name in names)
            {
                Assert.That(Firstnames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectUsingAutoMapping()
        {
            FlowQuerySelection<UserDto> dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));

            foreach (UserDto item in dtos)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Fullname, Is.Null);
            }
        }

        [Test]
        public void CanSelectUsingMemberInit()
        {
            FlowQuerySelection<UserDto> dtos = Query<UserEntity>()
                .Select(u => new UserDto
                {
                    Fullname = u.Firstname + " " + u.Lastname,
                    Id = u.Id
                });

            Assert.That(dtos.Count(), Is.EqualTo(4));

            foreach (UserDto item in dtos)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(string.IsNullOrEmpty(item.Username));
            }
        }

        [Test]
        public void CanSelectUsingMemberInitWithConstructor()
        {
            FlowQuerySelection<UserDto> dtos = Query<UserEntity>()
                .Select(u => new UserDto(u.Firstname + " " + u.Lastname)
                {
                    Id = u.Id
                });

            Assert.That(dtos.Count(), Is.EqualTo(4));

            foreach (UserDto item in dtos)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(string.IsNullOrEmpty(item.Username));
            }
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select(Projections.Alias(Projections.Property("Firstname"), "Firstname"));

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity u in users)
            {
                Assert.That(u.Firstname.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanSelectUsingPropertyHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Select(u => new UserEntity
                {
                    Id = Property.As<long>("u.Id")
                });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity item in users)
            {
                Assert.That(item.Id, Is.Not.EqualTo(0));
            }
        }

        [Test]
        public void CanSelectUsingPropertyProjection()
        {
            FlowQuerySelection<string> names = Query<UserEntity>()
                .Select<string>(Projections.Property("Firstname"));

            Assert.That(names.Count(), Is.EqualTo(4));

            foreach (string name in names)
            {
                Assert.That(Firstnames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectUsingSelectSetup()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select<UserDto>()
                .For(x => x.IsOnline).Use(x => x.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithProjections()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select<UserDto>()
                .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithStrings()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select<UserDto>()
                .For("IsOnline").Use(x => x.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithStringsInUseStatement()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Select<UserDto>()
                .For(x => x.IsOnline).Use("IsOnline")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectWithProxyInterface()
        {
            FlowQuerySelection<IUserEntity> users = Query<IUserEntity>()
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (IUserEntity user in users)
            {
                Assert.That(Ids, Contains.Item(user.Id));
                Assert.That(Usernames, Contains.Item(user.Username));
            }
        }

        [Test]
        public void CanSelectWithStringEmptyInProjections()
        {
            var anonymous = Query<UserEntity>()
                .Select(x => new
                {
                    Prop = x.Role == RoleEnum.Administrator ? "Admin" : string.Empty,
                    x.Role
                });

            foreach (var item in anonymous)
            {
                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.Prop, Is.EqualTo("Admin"));
                }
                else
                {
                    Assert.That(item.Prop, Is.Not.EqualTo("Admin"));
                }
            }
        }

        [Test]
        public void CanUseLocalVariableInProjections()
        {
            const string Local = "TEST";

            var anonymous = Query<UserEntity>()
                .Select(x => new
                {
                    Prop = x.Role == RoleEnum.Administrator ? Local : x.Username,
                    local = Local,
                    x.Role,
                    x.Username
                });

            foreach (var item in anonymous)
            {
                Assert.That(item.local, Is.EqualTo(Local));

                if (item.Role == RoleEnum.Administrator)
                {
                    Assert.That(item.Prop, Is.EqualTo(Local));
                }
                else
                {
                    Assert.That(item.Prop, Is.EqualTo(item.Username));
                }
            }
        }

        [Test]
        public void GroupByThrowsIfProjectionIsAggregate()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .GroupBy(x => Aggregate.Average(x.Id));
                    },
                    Throws.InvalidOperationException
                );
        }

        [Test]
        public void GroupByThrowsIfProjectionIsGroupBy()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .GroupBy(x => Aggregate.GroupBy(x.Id));
                    },
                    Throws.InvalidOperationException
                );
        }

        [Test]
        public void ProjectUsingSelectSetupThrowsIfSetupIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((ISelectSetup<UserEntity, UserDto>)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectUsingExpressionsThrowsIfArrayContainsNull()
        {
            var array = new Expression<Func<UserEntity, object>>[]
            {
                null
            };

            Assert.That(() => DummyQuery<UserEntity>().Select(array), Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingExpressionsThrowsIfExpressionArrayIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((Expression<Func<UserEntity, object>>[])null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectUsingProjectionThrowsIfProjectionIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((IProjection)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectUsingPropertyProjectionThrowsIfPropertyProjectionIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((PropertyProjection)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SelectUsingSelectSetupThrowsIfNoSetupIsMade()
        {
            Assert.That(() => DummyQuery<UserEntity>().Select<UserDto>().Select(), Throws.InvalidOperationException);
        }

        [Test]
        public void SelectUsingSelectSetupWithStringInUseStatementThrowsIfStringIsEmpty()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Select<UserDto>()
                            .For(x => x.IsOnline).Use(string.Empty)
                            .Select();
                    },
                    Throws.ArgumentException
                );
        }

        [Test]
        public void SelectUsingSelectSetupWithStringInUseStatementThrowsIfStringIsNull()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Select<UserDto>()
                            .For(x => x.IsOnline).Use(null as string)
                            .Select();
                    },
                    Throws.ArgumentException
                );
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayContainsEmptyStrings()
        {
            var strings = new[]
            {
                "UserName", 
                string.Empty
            };

            Assert.That(() => DummyQuery<UserEntity>().Select(strings), Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayContainsNull()
        {
            var strings = new[]
            {
                "UserName", 
                null
            };

            Assert.That(() => DummyQuery<UserEntity>().Select(strings), Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((string[])null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void SetMapperThrowsIfMapperIsNull()
        {
            Assert.That(() => Mapping.SetMapper(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            Mapping.SetMapper(new DefaultMapper());
        }

        [Test]
        public void TestCasting()
        {
            TestCastingModel[] stuff = Query<UserEntity>()
                .Where(x => x.Id == 1)
                .Select(x => new TestCastingModel
                {
                    Test1 = x.Id,
                    Test2 = (decimal)Aggregate.Average(x.Id),
                    Test3 = Aggregate.Average(x.Id),
                    Test4 = x.Id,
                    Test5 = x.Id,
                    Test6 = x.Id
                })
                .ToArray();

            Assert.That(stuff.Length, Is.EqualTo(1));
            Assert.That(stuff[0].Test1, Is.EqualTo(1));
            Assert.That(stuff[0].Test2, Is.EqualTo(1));
            Assert.That(stuff[0].Test3, Is.EqualTo(1));
            Assert.That(stuff[0].Test4, Is.EqualTo(1));
            Assert.That(stuff[0].Test5, Is.EqualTo(1));
            Assert.That(stuff[0].Test6, Is.EqualTo(1));
        }

        [Test]
        public void TestGroupByWithSelectUsingICriteria()
        {
            IList stuff = Session.CreateCriteria<UserEntity>()
                .AddOrder(Order.Asc(Projections.Property("IsOnline")))
                .SetProjection
                (
                    new FqProjectionList()
                        .Add(Projections.Count("Id"))
                        .Add(new FqGroupByProjection(Projections.Property("IsOnline")))
                )
                .List();

            var stuff1 = (object[])stuff[0];
            var stuff2 = (object[])stuff[1];

            Assert.That(stuff1[0], Is.EqualTo(1));
            Assert.That(stuff1[1], Is.False);

            Assert.That(stuff2[0], Is.EqualTo(3));
            Assert.That(stuff2[1], Is.True);
        }

        [Test]
        public void TestGroupByWithoutSelectUsingICriteria()
        {
            IList stuff = Session.CreateCriteria<UserEntity>()
                .AddOrder(Order.Asc(Projections.Property("IsOnline")))
                .SetProjection
                (
                    new FqProjectionList()
                        .Add(Projections.Count("Id"))
                        .Add(new FqGroupByProjection(Projections.Property("IsOnline"), false))
                )
                .List();

            Assert.That(stuff[0], Is.EqualTo(1));
            Assert.That(stuff[1], Is.EqualTo(3));
        }

        [Test]
        public void TestInferredGrouping()
        {
            var stuff = Query<UserEntity>()
                .Select(x => new
                {
                    Count = Aggregate.Count(x.Id),
                    x.IsOnline
                })
                .ToArray();

            Assert.That(stuff[0].Count, Is.GreaterThan(0));
            Assert.That(stuff[1].Count, Is.GreaterThan(0));
        }

        [Test]
        public void TestSelectByAliasReturnsNull()
        {
            UserGroupLinkEntity groupLink = null;
            GroupEntity group = null;

            GroupEntity[] stuff1 = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => group)
                .Select(x => group)
                .ToArray();

            foreach (GroupEntity item in stuff1)
            {
                Assert.That(item, Is.Null);
            }
        }

        [Test]
        public void TestSelectRootByParameterNameThrows()
        {
            Assert.That(() => DummyQuery<UserEntity>().Select(x => x), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void TestSelectSubquery()
        {
            UserEntity root = null;

            IDetachedFlowQuery<UserGroupLinkEntity> subquery = Query<UserGroupLinkEntity>()
                .Detached()
                .SetRootAlias(() => root)
                .Where(x => x.User.Id == root.Id)
                .Count();

            TestSelectSubqueryModel[] stuff = Session.FlowQuery(() => root)
                .OrderBy<TestSelectSubqueryModel>(x => x.NumberOfGroups)
                .Select(x => new TestSelectSubqueryModel
                {
                    Name = x.Firstname,
                    NumberOfGroups = Aggregate.Subquery<int>(subquery)
                })
                .ToArray();

            Assert.That(stuff.Length, Is.EqualTo(4));
            Assert.That(stuff[0].NumberOfGroups, Is.EqualTo(0));
            Assert.That(stuff[1].NumberOfGroups, Is.EqualTo(1));
            Assert.That(stuff[2].NumberOfGroups, Is.EqualTo(2));
            Assert.That(stuff[3].NumberOfGroups, Is.EqualTo(2));
        }

        [Test]
        public void TestSelectSubqueryThrowsIfSubqueryIsNull()
        {
            DetachedCriteria subquery = null;

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Select(x => new
                            {
                                NumberOfGroups = Aggregate.Subquery<int>(subquery)
                            });
                    },
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        [Test]
        public void TestSelectSubqueryWithGroupBy()
        {
            UserEntity root = null;

            IDetachedFlowQuery<UserGroupLinkEntity> subquery = Query<UserGroupLinkEntity>()
                .Detached()
                .SetRootAlias(() => root)
                .Where(x => x.User.Id == root.Id)
                .Count();

            TestSelectSubqueryModel[] stuff = Session.FlowQuery(() => root)
                .OrderBy<TestSelectSubqueryModel>(x => x.NumberOfGroups)
                .GroupBy(x => x.Id)
                .Select(x => new TestSelectSubqueryModel
                {
                    Name = x.Firstname,
                    NumberOfGroups = Aggregate.Subquery<int>(subquery)
                })
                .ToArray();

            Assert.That(stuff.Length, Is.EqualTo(4));
            Assert.That(stuff[0].NumberOfGroups, Is.EqualTo(0));
            Assert.That(stuff[1].NumberOfGroups, Is.EqualTo(1));
            Assert.That(stuff[2].NumberOfGroups, Is.EqualTo(2));
            Assert.That(stuff[3].NumberOfGroups, Is.EqualTo(2));
        }

        public class PuffClass
        {
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate",
                Justification = "Reviewed. Suppression is OK here.")]
            public string Puff;
        }

        public class TestCastingModel
        {
            public long? Test1 { get; set; }

            public decimal Test2 { get; set; }

            public double Test3 { get; set; }

            public decimal Test4 { get; set; }

            public float Test5 { get; set; }

            public object Test6 { get; set; }
        }

        public class TestSelectSubqueryModel
        {
            public virtual string Name { get; set; }

            public virtual int NumberOfGroups { get; set; }
        }
    }
}