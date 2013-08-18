using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class WhereTest : BaseTest
    {
        #region Methods (88)

        [Test]
        public void CanCombineMultipleWhereCalls()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname.Contains("kl")) // Matches one
                .Where(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRestrictByExample()
        {
            var users = Query<UserEntity>()
                .RestrictByExample(new UserEntity() { Firstname = "Niklas", Role = RoleEnum.Administrator }, x =>
                {
                    x.ExcludeProperty(u => u.CreatedStamp);
                    x.ExcludeProperty(u => u.IsOnline);
                    x.ExcludeZeroes();
                    x.ExcludeNulls();
                })
                .Select()
                ;

            Assert.That(users, Is.Not.Null);

            Assert.That(users.Count(), Is.GreaterThan(0));

            foreach (var user in users)
            {
                Assert.That(user.Firstname, Is.EqualTo("Niklas"));
            }
        }

        [Test]
        public void RestrictByExampleThrowsIfExampleInstanceIsNull()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                    .RestrictByExample(null, x => { });

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RestrictByExampleThrowsIfExampleIsNull()
        {
            Assert.That(() =>
            {
                Query<UserEntity>()
                        .RestrictByExample(new UserEntity() { Firstname = "Niklas", Role = RoleEnum.Administrator }, null);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        protected bool GetFalse()
        {
            return false;
        }

        [Test]
        public void IsEqualToAllWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualToAll(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsEqualToWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsGreaterThanAllWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThanAll(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsGreaterThanOrEqualToAllWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThanOrEqualToAll(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsGreaterThanOrEqualToSomeWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThanOrEqualToSome(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsGreaterThanOrEqualToWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThanOrEqualTo(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsGreaterThanSomeWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThanSome(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsGreaterThanWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.GreaterThan(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsLessThanAllWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThanAll(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void IsLessThanOrEqualToAllWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThanOrEqualToAll(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsLessThanOrEqualToSomeWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThanOrEqualToSome(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsLessThanOrEqualToWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThanOrEqualTo(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsLessThanSomeWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThanSome(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsLessThanWithSubquery()
        {
            var subquery = Query<UserEntity>()
                .Detached()
                .Where(x => x.Username == Usernames[0])
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.LessThan(subquery))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id > 0 && false)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            var users = Query<UserEntity>()
                .Where("IsOnline", FlowQueryIs.EqualTo(true))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id > 6 || true)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Where()
        {
            var users = Query<UserEntity>()
                .Where(u => u.IsOnline)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereBetweenWithIsHelper()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.Between(2, 3))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void WhereConstantFalseFetchesNone()
        {
            var users = Query<UserEntity>()
                .Where(u => false)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereConstantTrueFetchesAll()
        {
            var users = Query<UserEntity>()
                .Where(u => true)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id == 2)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void WhereGreaterThan()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id > 1)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id >= 1)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualToWithValueLeftAndProjectionRight()
        {
            var users = Query<UserEntity>()
                .Where(u => 4 >= u.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereGreaterThanWithValueLeftAndProjectionRight()
        {
            var users = Query<UserEntity>()
                .Where(u => 4 > u.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereInSubqueryWithIsHelper()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In
                                  (
                                      Query<UserEntity>()
                                          .Detached()
                                          .Where(x => x.Id == 1 || x.Id == 4)
                                          .Select(x => x.Id)
                                  )
                )
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereInWithIsHelper()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(1, 3))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereLessThan()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id < 4)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id <= 4)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualToWithValueLeftAndProjectionRight()
        {
            var users = Query<UserEntity>()
                .Where(u => 1 <= u.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereLessThanWithValueLeftAndProjectionRight()
        {
            var users = Query<UserEntity>()
                .Where(u => 1 < u.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereLikeWithIsHelper()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname, FlowQueryIs.Like("Nik%"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereNot()
        {
            var users = Query<UserEntity>()
                .Where(u => !u.IsOnline)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereNotEqualTo()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id != 2)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void WhereProjectionEqualToProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 2) == x.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionEqualToProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 2) == x.Firstname.Substring(0, 2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionEqualToProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username == x.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionEqualToProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username == x.Firstname.Substring(0, 2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) >= x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) >= (x.Setting.Id + x.Setting.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id >= x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanOrEqualToProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Setting.Id >= (x.Id + x.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) > x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) > (x.Setting.Id + x.Setting.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id > x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereProjectionGreaterThanProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Setting.Id > (x.Id + x.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) <= x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) <= (x.Setting.Id + x.Setting.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id <= x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanOrEqualToProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Setting.Id <= (x.Id + x.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) < x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhereProjectionLessThanProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => (x.Id + x.Id) < (x.Setting.Id + x.Setting.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Id < x.Setting.Id)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionLessThanProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Setting.Id < (x.Id + x.Id))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionLeftNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 2) != x.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionNoMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username.Substring(0, 2) != x.Firstname.Substring(0, 2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionOnlyMembers()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username != x.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereProjectionNotEqualToProjectionRightNotMember()
        {
            var users = Query<UserEntity>()
                .Where(x => x.Username != x.Firstname.Substring(0, 2))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithArithmeticOperations()
        {
            var users = Query<UserEntity>()
                .Where(u => (u.Id * 2 + 8) + 10 / 5 - 1 == 11) // Id == 1
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2 + 8) + 10 / 5 - 1, Is.EqualTo(11));
        }

        [Test]
        public void WhereWithConcatenation()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void WhereWithConstantValue()
        {
            var users = Query<UserEntity>()
                .Where(u => u.IsOnline != true)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithCriterions()
        {
            var users = Query<UserEntity>()
                .Where(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithCriterionsDoesNotThrowIfArrayContainsNullItem()
        {
            ICriterion[] c = new ICriterion[]
            {
                Restrictions.Eq("IsOnline", true),
                null,
                Restrictions.Like("Firstname", "%kl%")
            };

            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Where(c)
                                .Select()
                                ;

                        }, Throws.Nothing);
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayIsNull()
        {
            ICriterion[] c = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Where(c)
                                .Select()
                                ;

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WhereWithCustomInvocationExpression()
        {
            Func<bool> func = () => true;

            var users = Query<UserEntity>()
                .Where(u => func())
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithDoubleNegation()
        {
            var users = Query<UserEntity>()
                .Where(u => !!(u.IsOnline == true))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereWithExclusiveOr()
        {
            var users = Query<UserEntity>()
                .Where(u => u.IsOnline ^ (u.Firstname == "Niklas"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var u in users)
            {
                if (u.IsOnline)
                {
                    Assert.That(u.Firstname, Is.Not.EqualTo("Niklas"));
                }
            }
        }

        [Test]
        public void WhereWithWhereDelegateHelper()
        {
            var users = Query<UserEntity>()
                .Where((u, where) => u.Firstname == "Niklas"
                                  && (where(u.Lastname, FlowQueryIs.In(new string[] { "Nilsson", "Källander" }))
                                  || where(u.IsOnline, FlowQueryIs.Not.EqualTo(true))))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithWhereDelegateUsingString()
        {
            var users = Query<UserEntity>()
                .Where((u, where) => u.Firstname == "Niklas"
                                  && (where("Lastname", FlowQueryIs.In(new string[] { "Nilsson", "Källander" }))
                                  || where("u.IsOnline", FlowQueryIs.Not.EqualTo(true))))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithInvalidMethodCallThrows()
        {
            Assert.That(() =>
                        {
                            var users = Query<UserEntity>()
                                .Where(u => u.Firstname.Any())
                                .Select()
                                ;

                        }, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void WhereWithIsHelper()
        {
            var users = Query<UserEntity>()
                .Where(u => u.IsOnline, FlowQueryIs.EqualTo(true))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void WhereWithMultipleBinaryExpressions()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Id == 1 || u.Id > 3 && u.IsOnline == true)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (var user in users)
            {
                Assert.That(user.Id == 1 || user.Id > 3 && user.IsOnline == true);
            }
        }

        [Test]
        public void WhereWithNegation()
        {
            var users = Query<UserEntity>()
                .Where(u => !(u.IsOnline == true))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithNotNullCheck()
        {
            var users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp != null)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void WhereWithNullCheck()
        {
            var users = Query<UserEntity>()
                .Where(u => u.LastLoggedInStamp == null)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void WhereWithProjection1EqualProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id == link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithProjection1GreaterThanOrEqualToProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id >= link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhereWithProjection1GreaterThanProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id > link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereWithProjection1LessThanOrEqualToProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id <= link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void WhereWithProjection1LessThanProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id < link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithProjection1NotEqualProjection2()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Where(u => u.Id != link.Id)
                .Select(u => new { u.Username, UserId = u.Id, LinkId = link.Id })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereWithStringContains()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname.Contains("kl"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithStringEndsWith()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname.EndsWith("las"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void WhereWithStringStartsWith()
        {
            var users = Query<UserEntity>()
                .Where(u => u.Firstname.StartsWith("Nik"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereWithValueFromExternalMethod()
        {
            var users = Query<UserEntity>()
                .Where(u => GetFalse())
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        #endregion Methods
    }
}