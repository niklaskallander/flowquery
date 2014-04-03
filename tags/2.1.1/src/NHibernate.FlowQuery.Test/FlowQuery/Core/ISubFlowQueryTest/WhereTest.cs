using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class WhereTest : BaseTest
    {
        [Test]
        public void CorrelatedSubQueriesWorks()
        {
            UserEntity user = null;

            var query = Query<UserEntity>()
                .Detached()
                .SetRootAlias(() => user)
                .Where(x => x.Firstname == user.Firstname)
                .Select(x => x.Id);

            var users = Session.FlowQuery(() => user)
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanCombineMultipleWhereCalls()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.Contains("kl")) // Matches one
                .Where(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 0 && false)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            var query = DetachedQuery<UserEntity>()
                .Where("IsOnline", FqIs.EqualTo(true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 6 || true)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Where()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(x => x.Id, FqIs.Between(2, 3))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => false)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereConstantTrueFetchesAll()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => true)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereEqualTo()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id == 2)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void WhereGreaterThan()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id >= 1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void WhereInSubqueryWithIsHelper()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id, FqIs.In
                                  (
                                      DetachedQuery<UserEntity>()
                                          .Where(x => x.Id == 1 || x.Id == 4)
                                          .Select(x => x.Id)
                                  )
                )
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id, FqIs.In(1, 3))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id < 4)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id <= 4)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void WhereLikeWithIsHelper()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname, FqIs.Like("Nik%"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereNot()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => !u.IsOnline)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereNotEqualTo()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id != 2)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void WhereWithArithmeticOperations()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => (u.Id * 2 + 8) + 10 / 5 - 1 == 11) // Id == 1
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2 + 8) + 10 / 5 - 1, Is.EqualTo(11));
        }

        [Test]
        public void WhereWithConcatenation()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void WhereWithCriterions()
        {
            var query = Query<UserEntity>()
                .Detached()
                .Where(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithCriterionsThrowsNothingIfArrayContainsNullItem()
        {
            var c = new ICriterion[]
            {
                Restrictions.Eq("IsOnline", true),
                null,
                Restrictions.Like("Firstname", "%kl%")
            };

            Assert.That(() =>
                        {
                            DummyQuery<UserEntity>()
                                .Detached()
                                .Where(c)
                                .Select(x => x.Id);

                        }, Throws.Nothing);
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayIsNull()
        {
            ICriterion[] c = null;

            Assert.That(() =>
            {
                DummyQuery<UserEntity>()
                    .Detached()
                    .Where(c)
                    .Select(x => x.Id);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WhereWithDoubleNegation()
        {
            // ReSharper disable once NegativeEqualityExpression
            // ReSharper disable once DoubleNegationOperator
            // ReSharper disable once RedundantBoolCompare
            var query = DetachedQuery<UserEntity>()
                .Where(u => !!(u.IsOnline == true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline ^ (u.Firstname == "Niklas"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
        public void WhereWithIsHelper()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline, FqIs.EqualTo(true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            // ReSharper disable once RedundantBoolCompare
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Id == 1 || u.Id > 3 && u.IsOnline == true)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (var user in users)
            {
                Assert.That(user.Id == 1 || user.Id > 3 && user.IsOnline);
            }
        }

        [Test]
        public void WhereWithNegation()
        {
            // ReSharper disable once RedundantBoolCompare
            // ReSharper disable once NegativeEqualityExpression
            var query = DetachedQuery<UserEntity>()
                .Where(u => !(u.IsOnline == true))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithNotNullCheck()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.LastLoggedInStamp != null)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.LastLoggedInStamp == null)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void WhereWithStringContains()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.Contains("kl"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithNegatedIsExpression()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname, FqIs.Not.EqualTo("Niklas"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Firstname, Is.Not.EqualTo("Niklas"));
            }
        }

        [Test]
        public void WhereWithStringEndsWith()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.EndsWith("las"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void WhereWithStringStartsWith()
        {
            var query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.StartsWith("Nik"))
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }
    }
}