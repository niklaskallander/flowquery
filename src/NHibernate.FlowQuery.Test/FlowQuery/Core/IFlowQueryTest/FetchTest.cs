namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class FetchTest : BaseTest
    {
        [Test]
        public void CanClearFetches()
        {
            IImmediateFlowQuery<UserGroupLinkEntity> query = DummyQuery<UserGroupLinkEntity>();

            var queryable = (IQueryableFlowQuery)query;

            Assert.That(queryable.Fetches.Count, Is.EqualTo(0));

            query.Fetch(x => x.Group).Eagerly();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(1));

            query.Fetch(x => x.Group.Customers).Eagerly();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(2));

            query.ClearFetches();

            Assert.That(queryable.Fetches.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanFetchEagerly()
        {
            CustomerGroupLinkEntity cl = null;

            UserGroupLinkEntity[] stuff = Query<UserGroupLinkEntity>()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly()
                .Fetch(x => cl.Customer).Eagerly()
                .Select()
                .ToArray();

            foreach (UserGroupLinkEntity item in stuff)
            {
                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");
                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");

                foreach (CustomerGroupLinkEntity itemitem in item.Group.Customers)
                {
                    Assert.That(NHibernateUtil.IsInitialized(itemitem.Customer), "Customer");
                }
            }
        }

        [Test]
        public void CanFetchEagerlyUsingString()
        {
            UserGroupLinkEntity[] stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers.Customer").WithJoin()
                .Select()
                .ToArray();

            foreach (UserGroupLinkEntity item in stuff)
            {
                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");
                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");

                foreach (CustomerGroupLinkEntity itemitem in item.Group.Customers)
                {
                    Assert.That(NHibernateUtil.IsInitialized(itemitem.Customer), "Customer");
                }
            }
        }

        [Test]
        public void CanFetchLazilyUsingString()
        {
            UserGroupLinkEntity[] stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers").Lazily()
                .Select()
                .ToArray();

            foreach (UserGroupLinkEntity item in stuff)
            {
                IList<CustomerGroupLinkEntity> some = item.Group.Customers;

                // ReSharper disable once UnusedVariable
                foreach (CustomerGroupLinkEntity cust in some)
                {
                }

                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");

                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");
            }
        }

        [Test]
        public void CanFetchUsingSubselect()
        {
            UserGroupLinkEntity[] stuff = Query<UserGroupLinkEntity>()
                .Fetch("Group.Customers").WithSelect()
                .Select()
                .ToArray();

            foreach (UserGroupLinkEntity item in stuff)
            {
                IList<CustomerGroupLinkEntity> some = item.Group.Customers;

                // ReSharper disable once UnusedVariable
                foreach (CustomerGroupLinkEntity cust in some)
                {
                }

                Assert.That(NHibernateUtil.IsInitialized(item.Group), "Group");

                Assert.That(NHibernateUtil.IsInitialized(item.Group.Customers), "Customers");
            }
        }

        [Test]
        public void SpecifyingSameAliasTwiceThrows()
        {
            CustomerGroupLinkEntity cl = null;

            Assert
                .That
                (
                    () =>
                    {
                        Query<UserGroupLinkEntity>()
                            .Fetch(x => x.Group, () => cl).Eagerly()
                            .Fetch(x => x.Group.Customers, () => cl).Eagerly();
                    },
                    Throws.InvalidOperationException
                );
        }

        [Test]
        public void SpecifyingSameFetchTwiceThrowsNothing()
        {
            CustomerGroupLinkEntity cl = null;

            IImmediateFlowQuery<UserGroupLinkEntity> stuff = Query<UserGroupLinkEntity>()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly()
                .Fetch(x => x.Group.Customers, () => cl).Eagerly();

            Assert.That(stuff, Is.Not.Null);
        }

        [Test]
        public void FetchBuilderThrowsIfQueryReferencesDoesNotMatch()
        {
            var query1 = DummyQuery<UserEntity>() as IFlowQuery;
            var query2 = DummyQuery<UserEntity>();

            Assert.That(query1, Is.Not.Null);
            Assert.That(query2, Is.Not.Null);

            Assert
                .That
                (
                    () => new FetchBuilder<IImmediateFlowQuery<UserEntity>>
                    (
                        query1,
                        query2,
                        "Groups",
                        "group"
                    ),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void FetchBuilderThrowsIfQueryReferencesMatch()
        {
            var query = DummyQuery<UserEntity>();

            Assert
                .That
                (
                    () => new FetchBuilder<IImmediateFlowQuery<UserEntity>>
                    (
                        query as IFlowQuery,
                        query,
                        "Groups",
                        "group"
                    ),
                    Throws.Nothing
                );
        }
    }
}