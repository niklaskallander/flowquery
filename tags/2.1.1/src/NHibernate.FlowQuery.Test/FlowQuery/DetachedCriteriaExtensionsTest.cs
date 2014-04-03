using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using FqIs = Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class DetachedCriteriaExtensionTest : BaseTest
    {
        [Test]
        public void CanCreateSubqueryFromDetachedCriteria()
        {
            var criteria = DetachedCriteria.For<UserEntity>()
                .Add(Restrictions.Eq("Id", (long)2))
                .SetProjection(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(criteria.DetachedFlowQuery()))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }
    }
}