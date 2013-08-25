using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class AnyTest : BaseTest
    {
        #region Methods (88)

        [Test]
        public void CanAnyWithExpressions()
        {
            var any = Query<UserEntity>()
                .Any(u => u.Firstname.Contains("kl")  // Matches one
                       && !u.IsOnline) // In combination with above, matches zero, otherwise 1
                ;

            Assert.That(any, Is.False);
        }

        [Test]
        public void CanAnyWithSubQuery()
        {
            var subquery = Query<UserEntity>().Detached()
                .Select(x => x.Id);

            var any = Query<UserEntity>()
                .Any(x => x.Id, FlowQueryIs.In(subquery))
                ;

            Assert.That(any, Is.True);
        }

        [Test]
        public void LogicalAndWithConstantFalseResultsInNone()
        {
            var any = Query<UserEntity>()
                .Any(u => u.Id > 0 && false)
                ;

            Assert.That(any, Is.False);
        }

        [Test]
        public void LogicalAndWithStringAndIsHelper()
        {
            var any = Query<UserEntity>()
                .Any("IsOnline", FlowQueryIs.EqualTo(true))
                ;

            Assert.That(any, Is.True);
        }

        [Test]
        public void AnyWithCriterions()
        {
            var any = Query<UserEntity>()
                .Any(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                ;

            Assert.That(any, Is.True);
        }

        [Test]
        public void AnyWithWhereDelegateHelper()
        {
            var any = Query<UserEntity>()
                .Any((u, where) => u.Firstname == "Niklas"
                                  && (where(u.Lastname, FlowQueryIs.In(new string[] { "Nilsson", "Källander" }))
                                  || where(u.IsOnline, FlowQueryIs.Not.EqualTo(true))))
                ;

            Assert.That(any, Is.True);
        }

        [Test]
        public void DelayedCanAnyWithExpressions()
        {
            var any = Query<UserEntity>()
                .Delayed()
                .Any(u => u.Firstname.Contains("kl")  // Matches one
                       && !u.IsOnline) // In combination with above, matches zero, otherwise 1
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.False);
        }

        [Test]
        public void DelayedCanAnyWithSubQuery()
        {
            var subquery = Query<UserEntity>().Detached()
                .Select(x => x.Id);

            var any = Query<UserEntity>()
                .Delayed()
                .Any(x => x.Id, FlowQueryIs.In(subquery))
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.True);
        }

        [Test]
        public void DelayedLogicalAndWithConstantFalseResultsInNone()
        {
            var any = Query<UserEntity>()
                .Delayed()
                .Any(u => u.Id > 0 && false)
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.False);
        }

        [Test]
        public void DelayedLogicalAndWithStringAndIsHelper()
        {
            var any = Query<UserEntity>()
                .Delayed()
                .Any("IsOnline", FlowQueryIs.EqualTo(true))
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.True);
        }

        [Test]
        public void DelayedAnyWithCriterions()
        {
            var any = Query<UserEntity>()
                .Delayed()
                .Any(Restrictions.Eq("IsOnline", true), Restrictions.Like("Firstname", "%kl%"))
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.True);
        }

        [Test]
        public void DelayedAnyWithWhereDelegateHelper()
        {
            var any = Query<UserEntity>()
                .Delayed()
                .Any((u, where) => u.Firstname == "Niklas"
                                  && (where(u.Lastname, FlowQueryIs.In(new string[] { "Nilsson", "Källander" }))
                                  || where(u.IsOnline, FlowQueryIs.Not.EqualTo(true))))
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(1));

            Assert.That(any.Value, Is.True);
        }

        #endregion Methods
    }
}