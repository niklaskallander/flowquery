namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
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
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.IsOnline)
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
        public void AndBetweenWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id, FqIs.Between(2, 3))
                .Select(u => u.Id);

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
        public void AndConstantFalseFetchesNone()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => false)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AndConstantTrueFetchesAll()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => true)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void AndEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id == 2)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void AndGreaterThan()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id > 1)
                .Select(u => u.Id);

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
        public void AndGreaterThanOrEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id >= 1)
                .Select(u => u.Id);

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
        public void AndInSubqueryWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> subquery = DetachedQuery<UserEntity>()
                .Where(x => x.Id == 1 || x.Id == 4)
                .Select(x => x.Id);

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id, FqIs.In(subquery))
                .Select(u => u.Id);

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
        public void AndInWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id, FqIs.In(1, 3))
                .Select(u => u.Id);

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
        public void AndLessThan()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id < 4)
                .Select(u => u.Id);

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
        public void AndLessThanOrEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id <= 4)
                .Select(u => u.Id);

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
        public void AndLikeWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname, FqIs.Like("Nik%"))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void AndNot()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => !u.IsOnline)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndNotEqualTo()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id != 2)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void AndWithArithmeticOperations()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => (u.Id * 2) + 8 + (10 / 5) - 1 == 11) // Id == 1
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2) + 8 + (10 / 5) - 1, Is.EqualTo(11));
        }

        [Test]
        public void AndWithConcatenation()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void AndWithCriterions()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
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
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => !!(u.IsOnline == true))
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
        public void AndWithIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.IsOnline, FqIs.EqualTo(true))
                .Select(u => u.Id);

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
        public void AndWithMultipleBinaryExpressions()
        {
            // ReSharper disable once RedundantBoolCompare
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => (u.Id == 1 || u.Id > 3) && u.IsOnline == true)
                .Select(u => u.Id);

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
        public void AndWithNegation()
        {
            // ReSharper disable once RedundantBoolCompare, NegativeEqualityExpression
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => !(u.IsOnline == true))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndWithNotNullCheck()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.LastLoggedInStamp != null)
                .Select(u => u.Id);

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
        public void AndWithNullCheck()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.LastLoggedInStamp == null)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void AndWithStringAndIsHelper()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And("IsOnline", FqIs.EqualTo(true))
                .Select(u => u.Id);

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
        public void AndWithStringContains()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname.Contains("kl"))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithStringEndsWith()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname.EndsWith("las"))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void AndWithStringStartsWith()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname.StartsWith("Nik"))
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void CanCombineMultipleAndCalls()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Firstname.Contains("kl")) // Matches one
                .And(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id > 0 && false)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .And(u => u.Id > 6 || true)
                .Select(u => u.Id);

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(u => u.Id, FqIs.In(query))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }
    }
}