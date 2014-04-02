using System.Linq;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class FetchTest : BaseTest
    {
        [Test]
        public void CanFetchUsingSubselect()
        {
            var stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers").WithSelect()
                .Select()
                .ToArray();

            foreach (var item in stuff)
            {
                var some = item.Group.Customers;

                // ReSharper disable once UnusedVariable
                foreach (var cust in some)
                {
                }

                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");

                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");
            }
        }

        [Test]
        public void CanFetchLazilyUsingString()
        {
            var stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers").Lazily()
                .Select()
                .ToArray();

            foreach (var item in stuff)
            {
                var some = item.Group.Customers;

                // ReSharper disable once UnusedVariable
                foreach (var cust in some)
                {
                }

                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");

                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");
            }
        }

        [Test]
        public void CanFetchEagerlyUsingString()
        {
            var stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers.Customer").WithJoin()
                .Select()
                .ToArray();

            foreach (var item in stuff)
            {
                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");
                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");

                foreach (var itemitem in item.Group.Customers)
                {
                    Assert.That(NHibernateUtil.IsInitialized(itemitem.Customer), "Customer");
                }
            }
        }

        [Test]
        public void CanFetchEagerly()
        {
            CustomerGroupLinkEntity cl = null;

            var stuff = Query<UserGroupLinkEntity>()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly()
                .Fetch(x => cl.Customer).Eagerly()
                .Select()
                .ToArray();

            foreach (var item in stuff)
            {
                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");
                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");

                foreach (var itemitem in item.Group.Customers)
                {
                    Assert.That(NHibernateUtil.IsInitialized(itemitem.Customer), "Customer");
                }
            }
        }

        [Test]
        public void SpecifyingSameFetchTwiceThrowsNothing()
        {
            CustomerGroupLinkEntity cl = null;

            var stuff = Query<UserGroupLinkEntity>()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly();

            Assert.That(stuff, Is.Not.Null);
        }

        [Test]
        public void SpecifyingSameAliasTwiceThrows()
        {
            CustomerGroupLinkEntity cl = null;

            Assert.That(() =>
            {
                Query<UserGroupLinkEntity>()
                    .Fetch(x => x.Group, () => cl).Eagerly()
                    .Fetch(x => x.Group.Customers, () => cl).Eagerly();

            }, Throws.InvalidOperationException);
        }

        [Test]
        public void CanClearFetches()
        {
            var query = DummyQuery<UserGroupLinkEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.Fetches.Count, Is.EqualTo(0));

            query.Fetch(x => x.Group).Eagerly();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(1));

            query.Fetch(x => x.Group.Customers).Eagerly();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(2));

            query.ClearFetches();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(0));
        }
    }
}