using System;
using System.Linq;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using FlowQueryIs = NHibernate.FlowQuery.Is;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class JoinTest : BaseTest
    {
        #region Methods (16)

        [Test]
        public void CanFullJoin()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin("Groups", () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinNonCollection()
        {
            Setting setting = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanInnerJoin()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(r.Reveal(x => x.Groups), () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(u => link.Group, () => group)
                .SelectDistinct(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin("Groups", () => link)
                .InnerJoin("link.Group", () => group)
                .SelectDistinct(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoin()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(u => link.Group, () => group)
                .SelectDistinct(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .Join("Groups", () => link)
                .Join("link.Group", () => group)
                .SelectDistinct(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin("Groups", () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoin()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .Join(u => u.Groups, () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var query = SubQuery.For<UserEntity>()
                .Join(r.Reveal(x => x.Groups), () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .Join("Groups", () => link)
                .SelectDistinct(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin("Groups", () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }


        [Test]
        public void CanLeftOuterJoinNonCollection()
        {
            Setting setting = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanRightOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin("Groups", () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinNonCollection()
        {
            Setting setting = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test]
        public void JoiningPropertyMultipleTimesWithSameAliasDoesNotThrow()
        {
            UserGroupLinkEntity link = null;

            Assert.That(() =>
            {
                SubQuery.For<UserEntity>()
                    .InnerJoin(x => x.Groups, () => link)
                    .InnerJoin(x => x.Groups, () => link)
                    .Select();

            }, Throws.Nothing);
        }

        [Test]
        public void JoiningPropertyTwiceWithDifferentAliasesThrows()
        {
            UserGroupLinkEntity link = null, link2 = null;

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .InnerJoin(x => x.Groups, () => link)
                                .InnerJoin(x => x.Groups, () => link2)
                                .Select();

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void UsingSameAliasTwiceThrows()
        {
            object link = null;

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .InnerJoin("Groups", () => link)
                                .InnerJoin("Setting", () => link)
                                .Select();

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CanInnerJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group)
                .SelectDistinct(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .SelectDistinct(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group)
                .SelectDistinct(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group, new CustomConvention(x => x))
                .SelectDistinct(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanFullJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select();

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void JoiningWithProvidedRevealConventionThrowsIfConventionIsNull()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .InnerJoin(u => u.Groups, () => link)
                                .InnerJoin(() => link.Group, () => group, null);

                        }, Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .Join(u => u.Groups, () => link)
                                .Join(() => link.Group, () => group, null);

                        }, Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .FullJoin(u => u.Groups, () => link)
                                .FullJoin(() => link.Group, () => group, null);

                        }, Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .RightOuterJoin(u => u.Groups, () => link)
                                .RightOuterJoin(() => link.Group, () => group, null);

                        }, Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() =>
                        {
                            SubQuery.For<UserEntity>()
                                .LeftOuterJoin(u => u.Groups, () => link)
                                .LeftOuterJoin(() => link.Group, () => group, null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void CanInnerJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = SubQuery.For<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group)
                .InnerJoin(() => group.Customers, () => customerLink)
                .InnerJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanFullJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = SubQuery.For<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group)
                .FullJoin(() => group.Customers, () => customerLink)
                .FullJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = SubQuery.For<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group)
                .Join(() => group.Customers, () => customerLink)
                .Join(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = SubQuery.For<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group)
                .LeftOuterJoin(() => group.Customers, () => customerLink)
                .LeftOuterJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = SubQuery.For<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group)
                .RightOuterJoin(() => group.Customers, () => customerLink)
                .RightOuterJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select();

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }


        #endregion Methods
    }
}