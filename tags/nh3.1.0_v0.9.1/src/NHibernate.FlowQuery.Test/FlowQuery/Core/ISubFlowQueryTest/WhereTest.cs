using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class WhereTest : BaseTest
    {
        #region Methods (33)

        [Test]
        public void CorrelatedSubQueriesWorks()
        {
            UserEntity user = null;

            var query = SubQuery.For<UserEntity>()
                .SetRootAlias(() => user)
                .Where(x => x.Firstname == user.Firstname)
                .Select(x => x.Id);

            var users = Session.FlowQuery<UserEntity>(() => user)
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanCombineMultipleWhereCalls()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname.Contains("kl")) // Matches one
                .Where(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id > 0 && false)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where("IsOnline", FlowQueryIs.EqualTo(true))
                .Select(x => x.Id);


            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id > 6 || true)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Where()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.IsOnline)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereBetweenWithIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.Between(2, 3))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void WhereConstantFalseFetchesNone()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => false)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereConstantTrueFetchesAll()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => true)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereEqualTo()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id == 2)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void WhereGreaterThan()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id > 1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualTo()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id >= 1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void WhereInSubqueryWithIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In
                                  (
                                      SubQuery.For<UserEntity>()
                                          .Where(x => x.Id == 1 || x.Id == 4)
                                          .Select(x => x.Id)
                                  )
                )
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereInWithIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(1, 3))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereLessThan()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id < 4)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualTo()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id <= 4)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void WhereLikeWithIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname, FlowQueryIs.Like("Nik%"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereNot()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => !u.IsOnline)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereNotEqualTo()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id != 2)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void WhereWithArithmeticOperations()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => (u.Id * 2 + 8) + 10 / 5 - 1 == 11) // Id == 1
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2 + 8) + 10 / 5 - 1, Is.EqualTo(11));
        }

        [Test]
        public void WhereWithConcatenation()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void WhereWithCriterions()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayContainsNullItem()
        {
            ICriterion[] c = new ICriterion[]
            {
                Restrictions.Eq("IsOnline", true),
                null,
                Restrictions.Like("Firstname", "%kl%")
            };

            Assert.That(() =>
            {
                var query = SubQuery.For<UserEntity>()
                    .Where(c)
                    .Select(x => x.Id);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayIsNull()
        {
            ICriterion[] c = null;

            Assert.That(() =>
            {
                var query = SubQuery.For<UserEntity>()
                    .Where(c)
                    .Select(x => x.Id);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WhereWithDoubleNegation()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => !!(u.IsOnline == true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereWithExclusiveOr()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.IsOnline ^ (u.Firstname == "Niklas"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

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
        public void WhereWithIsHelper()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.IsOnline, FlowQueryIs.EqualTo(true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void WhereWithMultipleBinaryExpressions()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Id == 1 || u.Id > 3 && u.IsOnline == true)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            foreach (var user in users)
            {
                Assert.That(user.Id == 1 || user.Id > 3 && user.IsOnline == true);
            }
        }

        [Test]
        public void WhereWithNegation()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => !(u.IsOnline == true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithNotNullCheck()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.LastLoggedInStamp != null)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void WhereWithNullCheck()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.LastLoggedInStamp == null)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void WhereWithStringContains()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname.Contains("kl"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithNegatedIsExpression()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname, FlowQueryIs.Not.EqualTo("Niklas"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Firstname, Is.Not.EqualTo("Niklas"));
            }
        }

        [Test]
        public void WhereWithStringEndsWith()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname.EndsWith("las"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void WhereWithStringStartsWith()
        {
            var query = SubQuery.For<UserEntity>()
                .Where(u => u.Firstname.StartsWith("Nik"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        #endregion Methods
    }
}