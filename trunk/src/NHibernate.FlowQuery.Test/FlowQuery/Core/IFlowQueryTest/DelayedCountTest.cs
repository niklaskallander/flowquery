using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Linq;
    using NHibernate.Criterion;
    using NHibernate.Engine;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class DelayedCountTest : BaseTest
    {
        #region Methods (7)

        [Test]
        public void CanCountDistinctOnProperty()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Distinct()
                .Count(u => u.IsOnline)
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
        }

        [Test]
        public void CanCountDistinctOnPropertyUsingString()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Distinct()
                .Count("IsOnline")
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
        }

        [Test]
        public void CanCountLongOnStar()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .CountLong()
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnProjection()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Count(Projections.Distinct(Projections.Property("IsOnline")))
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(userCount.Value, Is.EqualTo(2));
        }

        [Test]
        public void CanCountOnProperty()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Count(u => u.IsOnline)
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnPropertyUsingString()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Count("IsOnline")
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        [Test]
        public void CanCountOnStar()
        {
            var userCount = Query<UserEntity>()
                .Delayed()
                .Count()
                ;

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));

            Assert.That(userCount.Value, Is.EqualTo(4));
        }

        #endregion Methods
    }
}