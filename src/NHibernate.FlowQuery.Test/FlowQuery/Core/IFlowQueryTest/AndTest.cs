namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class AndTest : BaseTest
    {
        [Test]
        public void And()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndBetweenWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id, FqIs.Between(2, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void AndConstantFalseFetchesNone()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AndConstantTrueFetchesAll()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void AndEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id == 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void AndGreaterThan()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id > 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void AndGreaterThanOrEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id >= 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void AndInSubqueryWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Id == 1 || x.Id == 4)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And
                (
                    u => u.Id,
                    FqIs.In(subquery)
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndInWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id, FqIs.In(1, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (UserEntity user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndLessThan()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id < 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void AndLessThanOrEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id <= 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (UserEntity user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void AndLikeWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname, FqIs.Like("Nik%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void AndNot()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => !u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndNotEqualTo()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id != 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void AndWithArithmeticOperations()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => (u.Id * 2) + 8 + (10 / 5) - 1 == 11) // Id == 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2) + 8 + (10 / 5) - 1, Is.EqualTo(11));
        }

        [Test]
        public void AndWithConcatenation()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname + " " + u.Lastname == "Niklas Kallander")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Kallander"));
        }

        [Test]
        public void AndWithCriterions()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1003:SymbolsMustBeSpacedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public void AndWithDoubleNegation()
        {
            // ReSharper disable once NegativeEqualityExpression, DoubleNegationOperator, RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => !!(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndWithIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.IsOnline, FqIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithJoinedEntityProjection()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .And(u => u.Id == link.Id)
                .Select(u => new { UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().UserId, Is.EqualTo(users.First().LinkId));
        }

        [Test]
        public void AndWithMultipleBinaryExpressions()
        {
            // ReSharper disable once RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => (u.Id == 1 || u.Id > 3) && u.IsOnline == true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (UserEntity user in users)
            {
                Assert.That((user.Id == 1 || user.Id > 3) && user.IsOnline);
            }
        }

        [Test]
        public void AndWithNegation()
        {
            // ReSharper disable once NegativeEqualityExpression, RedundantBoolCompare
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => !(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndWithNotNullCheck()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.LastLoggedInStamp != null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void AndWithNullCheck()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.LastLoggedInStamp == null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void AndWithStringAndIsHelper()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And("IsOnline", FqIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithStringContains()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname.Contains("kl"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithStringEndsWith()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname.EndsWith("las"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void AndWithStringStartsWith()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname.StartsWith("Nik"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void AndWithWhereDelegateHelper()
        {
            var lastNames = new object[] { "Nilsson", "Kallander" };

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And((u, where) => u.Firstname == "Niklas"
                    && (where(u.Lastname, FqIs.In(lastNames))
                        || where(u.IsOnline, FqIs.Not.EqualTo(true))))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanCombineMultipleAndCalls()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Firstname.Contains("kl")) // Matches one
                .And(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRestrictEmptyCollection()
        {
            int count = Query<UserEntity>()
                .And(x => x.Groups, FqIs.Empty())
                .Count();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void CanRestrictNotEmptyCollection()
        {
            int count = Query<UserEntity>()
                .And(x => x.Groups, FqIs.Not.Empty())
                .Count();

            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void CanRestrictOnExistsGreedily()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .And(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(subquery, FqIs.Not.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanRestrictOnExistsNonGreedily()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .And(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .And(subquery, FqIs.Not.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanRestrictOnNotExistsGreedily()
        {
            IDetachedImmutableFlowQuery subquery = DetachedQuery<UserEntity>()
                .And(x => x.IsOnline)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(subquery, FqIs.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRestrictOnNotExistsNonGreedily()
        {
            UserEntity user = null;

            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .And(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id);

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .And(subquery, FqIs.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanRestrictOnNotExistsNonGreedilyUsingDetachedCriteria()
        {
            UserEntity user = null;

            DetachedCriteria subquery = DetachedQuery<UserEntity>()
                .SetRootAlias(() => user)
                .And(x => x.IsOnline && x.Id == user.Id)
                .Select(x => x.Id)
                .Criteria;

            FlowQuerySelection<UserEntity> users = Session.FlowQuery(() => user)
                .And(subquery, FqIs.Empty())
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id > 0 && false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .And(u => u.Id > 6 || true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }
    }
}