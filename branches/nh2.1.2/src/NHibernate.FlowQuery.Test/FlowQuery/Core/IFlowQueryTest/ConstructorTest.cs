using System;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ConstructorTest : BaseTest
    {
        #region Methods (4)

        [Test]
        public void CanCreateFlowQuery()
        {
            var q = Session.FlowQuery<UserEntity>();

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithAlias()
        {
            UserEntity user = null;

            var q = Session.FlowQuery<UserEntity>(() => user);

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            var options = new FlowQueryOptions()
                .Add(c => c.SetMaxResults(5));

            var q = Session.FlowQuery<UserEntity>(options);

            Assert.That(q, Is.Not.Null);

            Assert.That(q.Criteria.GetRootEntityTypeIfAvailable(), Is.EqualTo(typeof(UserEntity)));
        }

        [Test]
        public void CanCreateFlowQueryWithAliasAndOptions()
        {
            UserEntity user = null;

            var q = Session.FlowQuery<UserEntity>(() => user, new FlowQueryOptions());

            Assert.That(q, Is.Not.Null);
        }

        [Test]
        public void ThrowsWhenOptionsIsNull()
        {
            FlowQueryOptions o = null;

            Assert.That(() => { Session.FlowQuery<UserEntity>(o); }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ThrowsWhenSessionIsNull()
        {
            ISession s = null;

            Assert.That(() => { s.FlowQuery<UserEntity>(); }, Throws.InstanceOf<ArgumentNullException>());
        }

        #endregion Methods
    }
}