using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    using Is = NUnit.Framework.Is;
    using QIs = NHibernate.FlowQuery.Is;

    [TestFixture]
    public class DetachedCriteriaExtensionTest : BaseTest
    {
		#region Methods (1) 

        [Test]
        public void CanCreateSubQueryFromDetachedCriteria()
        {
            var criteria = DetachedCriteria.For<UserEntity>()
                .Add(Restrictions.Eq("Id", (long)2))
                .SetProjection(Projections.Property("Id"));

            var users = Query<UserEntity>()
                .Where(x => x.Id, QIs.In(criteria.SubQuery<UserEntity>()))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.First().Id, Is.EqualTo(2));
        }

		#endregion Methods 
    }
}