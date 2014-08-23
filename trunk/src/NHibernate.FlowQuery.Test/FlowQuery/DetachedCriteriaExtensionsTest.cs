namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class DetachedCriteriaExtensionTest : BaseTest
    {
        [Test]
        public void CanCreateSubqueryFromDetachedCriteria()
        {
            DetachedCriteria criteria = DetachedCriteria.For<UserEntity>()
                .Add(Restrictions.Eq("Id", (long)2))
                .SetProjection(Projections.Property("Id"));

            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(criteria.DetachedFlowQuery()))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }
    }
}