namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class DetachedFlowQueryTest : BaseTest
    {
        [Test]
        public void CanCreateTrulyDetachedQueryUsingStaticHelperWithoutParameters()
        {
            var query = DetachedFlowQuery.For<UserEntity>()
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert.That(query, Is.Not.Null);

            Assert.That(((IFlowQuery)query).Options, Is.Null);
            Assert.That(((IFlowQuery)query).Alias, Is.EqualTo("this"));
            Assert.That(((IFlowQuery)query).CriteriaFactory, Is.Null);
            Assert.That(query.Criteria, Is.Not.Null);
        }

        [Test]
        public void CanCreateTrulyDetachedQueryUsingStaticHelperWithOnlyAlias()
        {
            UserEntity user = null;

            var query = DetachedFlowQuery.For(() => user)
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert.That(query, Is.Not.Null);

            Assert.That(((IFlowQuery)query).Options, Is.Null);
            Assert.That(((IFlowQuery)query).Alias, Is.EqualTo("user"));
            Assert.That(((IFlowQuery)query).CriteriaFactory, Is.Null);
            Assert.That(query.Criteria, Is.Not.Null);
        }

        [Test]
        public void CanCreateTrulyDetachedQueryUsingStaticHelperWithOnlyOptions()
        {
            var options = new FlowQueryOptions();

            var query = DetachedFlowQuery.For<UserEntity>(options)
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert.That(query, Is.Not.Null);

            Assert.That(((IFlowQuery)query).Options, Is.Not.Null);
            Assert.That(((IFlowQuery)query).Alias, Is.EqualTo("this"));
            Assert.That(((IFlowQuery)query).CriteriaFactory, Is.Null);
            Assert.That(query.Criteria, Is.Not.Null);
        }

        [Test]
        public void CanCreateTrulyDetachedQueryUsingStaticHelperWithBothAliasAndOptions()
        {
            var options = new FlowQueryOptions();

            UserEntity user = null;

            var query = DetachedFlowQuery.For(() => user, options)
                .Where(x => x.Id == 1)
                .Select(x => x.Id);

            Assert.That(query, Is.Not.Null);

            Assert.That(((IFlowQuery)query).Options, Is.Not.Null);
            Assert.That(((IFlowQuery)query).Alias, Is.EqualTo("user"));
            Assert.That(((IFlowQuery)query).CriteriaFactory, Is.Null);
            Assert.That(query.Criteria, Is.Not.Null);
        }
    }
}