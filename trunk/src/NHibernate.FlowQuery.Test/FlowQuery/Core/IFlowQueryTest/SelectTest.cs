using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using NHibernate.Criterion;
using NHibernate.FlowQuery.AutoMapping;
using NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;
    using Property = NHibernate.FlowQuery.Property;

    [TestFixture]
    public class SelectTest : BaseTest
    {
        #region Methods (53)

        [Test]
        public void AttemptToSelectInvalidAggregationThrows()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Select(x => x.Id.GetHashCode());

                        }, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void CanAutoMapSpecificPropertiesUsingExpressions()
        {
            var users = Query<UserEntity>()
                .Select(x => x.Id, x => x.Username)
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var u in users)
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

            var dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));
            foreach (var item in dtos)
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
            CustomMapper mapper = new CustomMapper();

            mapper.AddMap<UserEntity, UserDto>(x => new UserDto(x.Firstname + " " + x.Lastname)
                                               {
                                                   Id = x.Id,
                                                   IsOnline = x.IsOnline,
                                                   Username = x.Username
                                               });

            Mapping.SetMapper(mapper);

            var dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));
            foreach (var item in dtos)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(Fullnames, Contains.Item(item.Fullname));
            }
        }

        [Test]
        public void CanAutoMapUsingStrings()
        {
            var users = Query<UserEntity>()
                .Select("Username")
                .ToMap<UserDto>();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var u in users)
            {
                Assert.That(Usernames, Contains.Item(u.Username));
            }
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
                switch (a.Role)
                {
                    case RoleEnum.Administrator:
                        Assert.That(a.Count, Is.EqualTo(2));
                        break;
                    default:
                        Assert.That(a.Count, Is.EqualTo(1));
                        break;
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
            var users = Query<UserEntity>().Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectAnonymous()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new { u.Username });

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
                            Value = ((u.Id - 1) * 2 + 15) / u.Id,
                            u.Id
                        });

            Assert.That(anonymous.Count(), Is.EqualTo(4));
            foreach (var item in anonymous)
            {
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Value, Is.EqualTo(((item.Id - 1) * 2 + 15) / item.Id));
            }
        }

        [Test]
        public void CanSelectBinaryExpressions()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator, u.Role });

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
                .Select(u => new { Fullname = u.Firstname + (" " + u.Lastname) });

            Assert.That(anonymous.Count(), Is.EqualTo(4));
            foreach (var item in anonymous)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
            }
        }

        [Test]
        public void CanSelectConcatenatedPropertiesWithoutNewExpression()
        {
            var names = Query<UserEntity>().Select(u => u.Firstname + " " + u.Lastname);

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
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator ? true : false, u.Role });

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

        [Test, Ignore("NHibernate currently translates the system types to mismatching ITypes making NHibernate throw exceptions. YesNoType is not the same type as BooleanType. Have not found a way to work-around this issue yet.")]
        public void CanSelectConditionalsWithMixedProjectionAndConstant()
        {
            var anonymous = Query<UserEntity>()
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator ? u.IsOnline : false, u.Role, u.IsOnline });

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
        public void CanSelectConditionalsWithMixedProjectionAndLocalVariable()
        {
            bool notTrue = false;

            var anonymous = Query<UserEntity>()
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator ? true : notTrue, u.Role });

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
                .Join(u => u.Groups, () => link)
                .Select(u => new { link.Id });

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
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator && u.Id < 3, u.Role, u.Id });

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
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator || u.Id < 3, u.Role, u.Id });

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
                .Select(u => new { u.Username, Name = new { u.Firstname, u.Lastname } });

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
                .Select(u => new { IsAdministrator = u.Role == RoleEnum.Administrator ? true : u.Id < 3 ? true : false, u.Role, u.Id });

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
            var ids = Query<UserEntity>().Select(u => u.Setting.Id);

            Assert.That(ids.Count(), Is.EqualTo(4));
            foreach (long id in ids)
            {
                Assert.That(id, Is.EqualTo(6));
            }
        }

        [Test]
        public void CanSelectNestedMemberInits()
        {
            var users = Query<UserEntity>()
                .Select(x => new UserEntity()
                        {
                            Id = x.Id,
                            Setting = new Setting() { Id = x.Setting.Id }
                        });

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(user.Setting, Is.Not.Null);
                Assert.That(user.Setting.Id, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanSelectSingleProperty()
        {
            var names = Query<UserEntity>().Select(u => u.Username);

            Assert.That(names.Count(), Is.EqualTo(4));
            foreach (string name in names)
            {
                Assert.That(Usernames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectSpecificPropertiesUsingExpressions()
        {
            var users = Query<UserEntity>()
                .Select(u => u.Id, u => u.Role);

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
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
            var users = Query<UserEntity>()
                .Select(u => new UserEntity() { Id = u.Id, Role = u.Role });

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
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
            var users = Query<UserEntity>()
                .Select("Id", "Role");

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
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
            var shortNames = Query<UserEntity>().Select(x => x.Username.Substring(0, 2));

            Assert.That(shortNames.Count(), Is.EqualTo(4));
            foreach (string shortName in shortNames)
            {
                Assert.That(shortName.Length, Is.EqualTo(1)); // somehow you always get what you want - 1 ( database error or something ).
            }
        }

        [Test]
        public void CanSelectToClassWithPublicFields()
        {
            var puffClasses = Query<UserEntity>()
                .Select(x => new PuffClass() { Puff = x.Firstname + " " + x.Lastname });

            Assert.That(puffClasses.Count(), Is.EqualTo(4));
            foreach (PuffClass pc in puffClasses)
            {
                Assert.That(Fullnames, Contains.Item(pc.Puff));
            }
        }

        [Test]
        public void CanSelectTypedUsingProjection()
        {
            var users = Query<UserEntity>()
                .Select<UserDto>(Projections.Alias(Projections.Property("Username"), "Username"));

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var u in users)
            {
                Assert.That(Usernames, Contains.Item(u.Username));
            }
        }

        [Test]
        public void CanSelectTypedUsingPropertyProjection()
        {
            var names = Query<UserEntity>()
                .Select<string>(Projections.Property("Firstname"));

            Assert.That(names.Count(), Is.EqualTo(4));
            foreach (var name in names)
            {
                Assert.That(Firstnames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectUsingAutoMapping()
        {
            var dtos = Query<UserEntity>()
                .Select()
                .ToMap<UserDto>();

            Assert.That(dtos.Count(), Is.EqualTo(4));
            foreach (var item in dtos)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Fullname, Is.Null);
            }
        }

        [Test]
        public void CanSelectUsingMemberInit()
        {
            var dtos = Query<UserEntity>()
                .Select(u => new UserDto() { Fullname = u.Firstname + " " + u.Lastname, Id = u.Id });

            Assert.That(dtos.Count(), Is.EqualTo(4));
            foreach (var item in dtos)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(string.IsNullOrEmpty(item.Username));
            }
        }

        [Test]
        public void CanSelectUsingMemberInitWithConstructor()
        {
            var dtos = Query<UserEntity>()
                .Select(u => new UserDto(u.Firstname + " " + u.Lastname) { Id = u.Id });

            Assert.That(dtos.Count(), Is.EqualTo(4));
            foreach (var item in dtos)
            {
                Assert.That(Fullnames, Contains.Item(item.Fullname));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(string.IsNullOrEmpty(item.Username));
            }
        }

        [Test]
        public void CanSelectUsingProjection()
        {
            var users = Query<UserEntity>()
                .Select(Projections.Alias(Projections.Property("Firstname"), "Firstname"));

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var u in users)
            {
                Assert.That(u.Firstname.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanSelectUsingPropertyHelper()
        {
            var users = Query<UserEntity>()
                .Select(u => new UserEntity() { Id = Property.As<long>("u.Id") });

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var item in users)
            {
                Assert.That(item.Id, Is.Not.EqualTo(0));
            }
        }

        [Test]
        public void CanSelectUsingPropertyProjection()
        {
            var names = Query<UserEntity>()
                .Select(Projections.Property("Firstname"));

            Assert.That(names.Count(), Is.EqualTo(4));
            foreach (var name in names)
            {
                Assert.That(Firstnames, Contains.Item(name));
            }
        }

        [Test]
        public void CanSelectUsingSelectSetup()
        {
            var users = Query<UserEntity>()
                            .Select<UserDto>()
                                .For(x => x.IsOnline).Use(x => x.IsOnline)
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithProjections()
        {
            var users = Query<UserEntity>()
                            .Select<UserDto>()
                                .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithStrings()
        {
            var users = Query<UserEntity>()
                            .Select<UserDto>()
                                .For("IsOnline").Use(x => x.IsOnline)
                                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectUsingSelectSetupWithStringsInUseStatement()
        {
            var users = Query<UserEntity>()
                .Select<UserDto>()
                    .For(x => x.IsOnline).Use("IsOnline")
                    .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanSelectWithProxyInterface()
        {
            var users = Query<IUserEntity>().Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
            {
                Assert.That(Ids, Contains.Item(user.Id));
                Assert.That(Usernames, Contains.Item(user.Username));
            }
        }

        [Test]
        public void SelectUsingExpressionsThrowsIfArrayContainsNull()
        {
            Expression<Func<UserEntity, object>>[] array = new Expression<Func<UserEntity, object>>[] { null };

            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Select(array);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectUsingExpressionsThrowsIfExpressionArrayIsNull()
        {
            Expression<Func<UserEntity, object>>[] e = null;

            Assert.That(() =>
            {
                Query<UserEntity>().Select(e);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectUsingProjectionThrowsIfProjectionIsNull()
        {
            IProjection p = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>().Select(p);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectUsingPropertyProjectionThrowsIfPropertyProjectionIsNull()
        {
            PropertyProjection p = null;

            Assert.That(() =>
            {
                Query<UserEntity>().Select(p);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SelectUsingSelectSetupThrowsIfNoSetupIsMade()
        {
            Assert.That(() =>
            {
                Query<UserEntity>()
                    .Select<UserDto>()
                        .Select();

            }, Throws.InvalidOperationException);
        }

        [Test]
        public void SelectUsingSelectSetupWithStringInUseStatementThrowsIfStringIsEmpty()
        {
            Assert.That(() =>
            {
                Query<UserEntity>()
                    .Select<UserDto>()
                        .For(x => x.IsOnline).Use(string.Empty)
                        .Select();

            }, Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingSelectSetupWithStringInUseStatementThrowsIfStringIsNull()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Select<UserDto>()
                                    .For(x => x.IsOnline).Use(null as string)
                                    .Select();

                        }, Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayContainsEmptyStrings()
        {
            string[] strings = new string[] { "UserName", string.Empty };

            Assert.That(() =>
            {
                Query<UserEntity>().Select(strings);

            }, Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayContainsNull()
        {
            string[] strings = new string[] { "UserName", null };

            Assert.That(() =>
                        {
                            Query<UserEntity>().Select(strings);

                        }, Throws.ArgumentException);
        }

        [Test]
        public void SelectUsingStringsThrowsIfStringArrayIsNull()
        {
            string[] strings = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>().Select(strings);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SetMapperThrowsIfMapperIsNull()
        {
            Assert.That(() =>
                        {
                            Mapping.SetMapper(null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            Mapping.SetMapper(new DefaultMapper());
        }

        #endregion Methods

        #region Nested Classes (1)

        public class PuffClass
        {
            #region Fields (1)

            public string Puff;

            #endregion Fields
        }

        #endregion Nested Classes
    }
}