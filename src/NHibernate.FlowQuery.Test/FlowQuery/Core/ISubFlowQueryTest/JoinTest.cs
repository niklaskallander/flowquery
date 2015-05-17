namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Revealing;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class JoinTest : BaseTest
    {
        [Test, Category("MySqlUnsupported")]
        public void CanFullJoin()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Full.Join(u => group.Customers, () => customerLink)
                .Full.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == 0)
                .Full.Join(x => link.Group, () => group, () => group.Id == 0)
                .Full.Join(u => group.Customers, () => customerLink, () => customerLink.Id == 0, null)
                .Full.Join(u => customerLink.Customer, () => customer, () => customer.Id == 0)
                .Distinct().Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query2))
                .Select();

            Assert.That(customers2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinNonCollection()
        {
            Setting setting = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            FlowQuerySelection<Setting> settings = Query<Setting>()
                .Where(s => s.Id, FqIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join("Groups", () => link)
                .Distinct().Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == 0)
                .Full.Join(u => link.Group, () => group, () => group.Id == 0)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Full.Join("Groups", () => link, () => link.Id == 0)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query2))
                .Select();

            Assert.That(groups2.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoin()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct().Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 0)
                .Inner.Join(u => link.Group, () => group, () => group.Id == 0)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == 0, null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == 0)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query2))
                .Select();

            Assert.That(customers2.Count(), Is.EqualTo(0));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            IRevealer<UserEntity> r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct().Select(u => group.Name);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct().Select(u => group.Name);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoin()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => group.Name);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct()
                .Select(u => group.Name);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoin()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            IRevealer<UserEntity> r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct()
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 2)
                .Inner.Join(u => link.Group, () => group, () => group.Id == 2)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(1));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link, () => link.Id == 2)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query2))
                .Select();

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 0)
                .Inner.Join(x => link.Group, () => group, () => group.Id == 0)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == 0, null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == 0)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query2))
                .Select();

            Assert.That(customers2.Count(), Is.EqualTo(0));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct()
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 2)
                .Inner.Join(u => link.Group, () => group, () => group.Id == 2)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(1));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .Inner.Join("Groups", () => link, () => link.Id == 2)
                .Distinct()
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query2))
                .Select();

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanLeftOuterJoin()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .LeftOuter.Join(u => group.Customers, () => customerLink)
                .LeftOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == 0)
                .LeftOuter.Join(x => link.Group, () => group, () => group.Id == 0)
                .LeftOuter.Join(u => group.Customers, () => customerLink, () => customerLink.Id == 0, null)
                .LeftOuter.Join(u => customerLink.Customer, () => customer, () => customer.Id == 0)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query2))
                .Select();

            Assert.That(customers2.Count(), Is.EqualTo(0));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinNonCollection()
        {
            Setting setting = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            FlowQuerySelection<Setting> settings = Query<Setting>()
                .Where(s => s.Id, FqIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanLeftOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join("Groups", () => link)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == 2)
                .LeftOuter.Join(u => link.Group, () => group, () => group.Id == 1)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(1));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .LeftOuter.Join("Groups", () => link, () => link.Id == 2)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query2))
                .Select();

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanRightOuterJoin()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .RightOuter.Join(u => group.Customers, () => customerLink)
                .RightOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == 0)
                .RightOuter.Join(x => link.Group, () => group, () => group.Id == 0)
                .RightOuter.Join(u => group.Customers, () => customerLink, () => customerLink.Id == 0, null)
                .RightOuter.Join(u => customerLink.Customer, () => customer, () => customer.Id == 0)
                .Distinct()
                .Select(u => customer.Name);

            FlowQuerySelection<CustomerEntity> customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FqIs.In(query2))
                .Select();

            Assert.That(customers2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinNonCollection()
        {
            Setting setting = null;

            IDetachedFlowQuery<UserEntity> query = Query<UserEntity>()
                .Detached()
                .RightOuter.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            FlowQuerySelection<Setting> settings = Query<Setting>()
                .Where(s => s.Id, FqIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanRightOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join("Groups", () => link)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            IDetachedFlowQuery<UserEntity> query = DetachedQuery<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == 2)
                .RightOuter.Join(u => link.Group, () => group, () => group.Id == 2)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(1));

            IDetachedFlowQuery<UserEntity> query2 = DetachedQuery<UserEntity>()
                .RightOuter.Join("Groups", () => link, () => link.Id == 2)
                .Select(u => link.Group.Id);

            FlowQuerySelection<GroupEntity> groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FqIs.In(query2))
                .Select();

            Assert.That(groups2.Count(), Is.EqualTo(2));
        }

        [Test]
        public void JoiningPropertyMultipleTimesWithSameAliasDoesNotThrow()
        {
            UserGroupLinkEntity link = null;

            Assert
                .That
                (
                    () =>
                    {
                        Query<UserEntity>()
                            .Detached()
                            .Inner.Join(x => x.Groups, () => link)
                            .Inner.Join(x => x.Groups, () => link)
                            .Select(x => x.Id);
                    },
                    Throws.Nothing
                );
        }

        [Test]
        public void JoiningPropertyTwiceWithDifferentAliasesThrows()
        {
            UserGroupLinkEntity link = null, link2 = null;

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .Inner.Join(x => x.Groups, () => link)
                            .Inner.Join(x => x.Groups, () => link2)
                            .Select(x => x.Id);
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void JoiningWithProvidedRevealConventionDoesNotThrowIfConventionIsNull()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .Inner.Join(u => u.Groups, () => link)
                            .Inner.Join(u => link.Group, () => group, (IRevealConvention)null);
                    },
                    Throws.Nothing
                );

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .Inner.Join(u => u.Groups, () => link)
                            .Inner.Join(u => link.Group, () => group, (IRevealConvention)null);
                    },
                    Throws.Nothing
                );

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .Full.Join(u => u.Groups, () => link)
                            .Full.Join(u => link.Group, () => group, (IRevealConvention)null);
                    },
                    Throws.Nothing
                );

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .RightOuter.Join(u => u.Groups, () => link)
                            .RightOuter.Join(u => link.Group, () => group, (IRevealConvention)null);
                    },
                    Throws.Nothing
                );

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .LeftOuter.Join(u => u.Groups, () => link)
                            .LeftOuter.Join(u => link.Group, () => group, (IRevealConvention)null);
                    },
                    Throws.Nothing
                );
        }

        [Test]
        public void UsingSameAliasTwiceThrows()
        {
            object link = null;

            Assert
                .That
                (
                    () =>
                    {
                        DetachedQuery<UserEntity>()
                            .Inner.Join("Groups", () => link)
                            .Inner.Join("Setting", () => link)
                            .Select(x => x.Id);
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }
    }
}