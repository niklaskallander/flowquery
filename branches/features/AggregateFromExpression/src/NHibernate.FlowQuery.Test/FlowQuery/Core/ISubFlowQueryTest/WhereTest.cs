namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class WhereTest : BaseTest
    {
        [Test]
        public void CanCombineMultipleWhereCalls()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.Contains("kl")) // Matches one
                .Where(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CorrelatedSubQueriesWorks()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> query = Query<UserEntity>()
                .Detached()
                .SetRootAlias(() => user)
                .Where(x => x.Firstname == user.Firstname)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 0 && false)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where("IsOnline", FqIs.EqualTo(true))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 6 || true)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void Where()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereBetweenWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(x => x.Id, FqIs.Between(2, 3))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void WhereConstantFalseFetchesNone()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => false)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhereConstantTrueFetchesAll()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => true)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void WhereEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id == 2)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void WhereGreaterThan()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id > 1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void WhereGreaterThanOrEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id >= 1)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void WhereInSubqueryWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Id == 1 || x.Id == 4)
                .Select(x => x.Id);

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id, FqIs.In(subquery))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereInWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id, FqIs.In(1, 3))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void WhereLessThan()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id < 4)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void WhereLessThanOrEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id <= 4)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void WhereLikeWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname, FqIs.Like("Nik%"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void WhereNot()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => !u.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereNotEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Id != 2)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void WhereWithArithmeticOperations()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => (u.Id * 2) + 8 + (10 / 5) - 1 == 11) // Id == 1
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2) + 8 + (10 / 5) - 1, Is.EqualTo(11));
        }

        [Test]
        public void WhereWithConcatenation()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void WhereWithCriterions()
        {
            IDetachedFlowQuery<UserEntity> query = Query<UserEntity>()
                .Detached()
                .Where(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithCriterionsThrowsIfArrayIsNull()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Detached()
                            .Where((ICriterion[])null)
                            .Select(x => x.Id);
                    }, 
                    Throws.InstanceOf<ArgumentNullException>()
                );
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

            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Detached()
                            .Where(c)
                            .Select(x => x.Id);
                    }, 
                    Throws.Nothing
                );
        }

        [Test]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1003:SymbolsMustBeSpacedCorrectly", 
            Justification = "Reviewed. Suppression is OK here.")]
        public void WhereWithDoubleNegation()
        {
            // ReSharper disable once NegativeEqualityExpression, DoubleNegationOperator, RedundantBoolCompare
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => !!(u.IsOnline == true))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void WhereWithExclusiveOr()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline ^ (u.Firstname == "Niklas"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity u in users)
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
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.IsOnline, FqIs.EqualTo(true))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void WhereWithMultipleBinaryExpressions()
        {
            // ReSharper disable once RedundantBoolCompare
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => (u.Id == 1 || u.Id > 3) && u.IsOnline == true)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (UserEntity user in users)
            {
                Assert.That((user.Id == 1 || user.Id > 3) && user.IsOnline);
            }
        }

        [Test]
        public void WhereWithNegatedIsExpression()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname, FqIs.Not.EqualTo("Niklas"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Firstname, Is.Not.EqualTo("Niklas"));
            }
        }

        [Test]
        public void WhereWithNegation()
        {
            // ReSharper disable once RedundantBoolCompare, NegativeEqualityExpression
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => !(u.IsOnline == true))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void WhereWithNotNullCheck()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.LastLoggedInStamp != null)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void WhereWithNullCheck()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.LastLoggedInStamp == null)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void WhereWithStringContains()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.Contains("kl"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void WhereWithStringEndsWith()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.EndsWith("las"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void WhereWithStringStartsWith()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Where(u => u.Firstname.StartsWith("Nik"))
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }
    }
}