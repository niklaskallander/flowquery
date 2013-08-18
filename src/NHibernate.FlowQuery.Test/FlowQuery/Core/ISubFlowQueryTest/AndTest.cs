using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class AndTest : BaseTest
    {
        #region Methods (30)

        [Test]
        public void And()
        {
            var query = Query<UserEntity>()
                .Detached()
                .And(u => u.IsOnline)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndBetweenWithIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id, FlowQueryIs.Between(2, 3))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.InRange(2, 3));
            }
        }

        [Test]
        public void AndConstantFalseFetchesNone()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => false)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AndConstantTrueFetchesAll()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => true)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        [Test]
        public void AndEqualTo()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id == 2)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

        [Test]
        public void AndGreaterThan()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id > 1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThan(1));
            }
        }

        [Test]
        public void AndGreaterThanOrEqualTo()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id >= 1)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.GreaterThanOrEqualTo(1));
            }
        }

        [Test]
        public void AndInSubqueryWithIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id, FlowQueryIs.In
                                  (
                                      Query<UserEntity>().Detached()
                                          .Where(x => x.Id == 1 || x.Id == 4)
                                          .Select(x => x.Id)
                                  )
                )
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 4 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndInWithIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id, FlowQueryIs.In(1, 3))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(2));

            foreach (var user in users)
            {
                Assert.That(new long[] { 1, 3 }, Contains.Item(user.Id));
            }
        }

        [Test]
        public void AndLessThan()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id < 4)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThan(4));
            }
        }

        [Test]
        public void AndLessThanOrEqualTo()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id <= 4)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var user in users)
            {
                Assert.That(user.Id, Is.LessThanOrEqualTo(4));
            }
        }

        [Test]
        public void AndLikeWithIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname, FlowQueryIs.Like("Nik%"))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void AndNot()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => !u.IsOnline)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndNotEqualTo()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id != 2)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.First().Id, Is.Not.EqualTo(2));
        }

        [Test]
        public void AndWithArithmeticOperations()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => (u.Id * 2 + 8) + 10 / 5 - 1 == 11) // Id == 1
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That((users.First().Id * 2 + 8) + 10 / 5 - 1, Is.EqualTo(11));
        }

        [Test]
        public void AndWithConcatenation()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname + " " + u.Lastname == "Niklas Källander")
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname + " " + users.First().Lastname, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void AndWithCriterions()
        {
            var query = Query<UserEntity>().Detached()
                .And(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.Single().IsOnline);
            Assert.That(users.Single().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithDoubleNegation()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => !!(u.IsOnline == true))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.IsOnline, Is.True);
            }
        }

        [Test]
        public void AndWithIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.IsOnline, FlowQueryIs.EqualTo(true))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithMultipleBinaryExpressions()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id == 1 || u.Id > 3 && u.IsOnline == true)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));

            foreach (var user in users)
            {
                Assert.That(user.Id == 1 || user.Id > 3 && user.IsOnline == true);
            }
        }

        [Test]
        public void AndWithNegation()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => !(u.IsOnline == true))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().IsOnline, Is.False);
        }

        [Test]
        public void AndWithNotNullCheck()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.LastLoggedInStamp != null)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var user in users)
            {
                Assert.That(user.LastLoggedInStamp, Is.Not.Null);
            }
        }

        [Test]
        public void AndWithNullCheck()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.LastLoggedInStamp == null)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().LastLoggedInStamp, Is.Null);
        }

        [Test]
        public void AndWithStringAndIsHelper()
        {
            var query = Query<UserEntity>().Detached()
                .And("IsOnline", FlowQueryIs.EqualTo(true))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.IsOnline);
            }
        }

        [Test]
        public void AndWithStringContains()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname.Contains("kl"))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.Contains("kl"));
        }

        [Test]
        public void AndWithStringEndsWith()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname.EndsWith("las"))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.EndsWith("las"));
        }

        [Test]
        public void AndWithStringStartsWith()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname.StartsWith("Nik"))
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Firstname.StartsWith("Nik"));
        }

        [Test]
        public void CanCombineMultipleAndCalls()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Firstname.Contains("kl")) // Matches one
                .And(u => !u.IsOnline) // In combination with above, matches zero, otherwise 1
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalAndWithConstantFalseFetchesNone()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id > 0 && false)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(0));
        }

        [Test]
        public void LogicalOrWithConstantTrueFetchesAll()
        {
            var query = Query<UserEntity>().Detached()
                .And(u => u.Id > 6 || true)
                .Select(u => u.Id);

            var users = Query<UserEntity>()
                .Where(u => u.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
        }

        #endregion Methods
    }
}