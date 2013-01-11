using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class AndTest : BaseTest
    {
        #region Methods (33)

        [Test]
        public void And()
        {
            var users = Query<UserEntity>()
                .And(u => u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndBetweenWithIsHelper()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id, FlowQueryIs.Between(2, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void AndConstantFalseFetchesNone()
        {
            var users = Query<UserEntity>()
                .And(u => false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AndConstantTrueFetchesAll()
        {
            var users = Query<UserEntity>()
                .And(u => true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void AndEqualTo()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id == 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void AndGreaterThan()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id > 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void AndGreaterThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id >= 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void AndInSubqueryWithIsHelper()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id, FlowQueryIs.In
                                  (
                                      SubQuery.For<UserEntity>()
                                          .Where(x => x.Id == 1 || x.Id == 4)
                                          .Select(x => x.Id)
                                  )
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndInWithIsHelper()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id, FlowQueryIs.In(1, 3))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndLessThan()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id < 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void AndLessThanOrEqualTo()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id <= 4)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void AndLikeWithIsHelper()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname, FlowQueryIs.Like("Nik%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void AndNot()
        {
            var users = Query<UserEntity>()
                .And(u => !u.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndNotEqualTo()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id != 2)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void AndWithArithmeticOperations()
        {
            var users = Query<UserEntity>()
                .And(u => (u.Id * 2 + 8) + 10 / 5 - 1 == 11) // Id == 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2 + 8) + 10 / 5 - 1, Is.EqualTo(11));
        }

        [Test]
        public void AndWithConcatenation()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void AndWithCriterions()
        {
            var users = Query<UserEntity>()
                .And(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithDoubleNegation()
        {
            var users = Query<UserEntity>()
                .And(u => !!(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndWithWhereDelegateHelper()
        {
            var users = Query<UserEntity>()
                .And((u, where) => u.Firstname == "Niklas"
                                  && (where(u.Lastname, FlowQueryIs.In(new string[] { "Nilsson", "Källander" }))
                                  || where(u.IsOnline, FlowQueryIs.Not.EqualTo(true))))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public void AndWithIsHelper()
        {
            var users = Query<UserEntity>()
                .And(u => u.IsOnline, FlowQueryIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithJoinedEntityProjection()
        {
            UserGroupLinkEntity link = null;

            var users = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .And(u => u.Id == link.Id)
                .Select(u => new { UserId = u.Id, LinkId = link.Id });

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().UserId, Is.EqualTo(users.First().LinkId));
        }

        [Test]
        public void AndWithMultipleBinaryExpressions()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id == 1 || u.Id > 3 && u.IsOnline == true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            foreach (var user in users)
            {
                Assert.That(user.Id == 1 || user.Id > 3 && user.IsOnline == true);
            }
        }

        [Test]
        public void AndWithNegation()
        {
            var users = Query<UserEntity>()
                .And(u => !(u.IsOnline == true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndWithNotNullCheck()
        {
            var users = Query<UserEntity>()
                .And(u => u.LastLoggedInStamp != null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void AndWithNullCheck()
        {
            var users = Query<UserEntity>()
                .And(u => u.LastLoggedInStamp == null)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void AndWithStringAndIsHelper()
        {
            var users = Query<UserEntity>()
                .And("IsOnline", FlowQueryIs.EqualTo(true))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithStringContains()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname.Contains("kl"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithStringEndsWith()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname.EndsWith("las"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void AndWithStringStartsWith()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname.StartsWith("Nik"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void CanCombineMultipleAndCalls()
        {
            var users = Query<UserEntity>()
                .And(u => u.Firstname.Contains("kl")) // Matches one
                .And(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id > 0 && false)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            var users = Query<UserEntity>()
                .And(u => u.Id > 6 || true)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        #endregion Methods
    }
}