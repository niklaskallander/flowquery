using System;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using System.Linq;
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

            var query = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Full.Join(u => link.Group, () => group, () => group.Id == null)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            var query2 = Query<UserEntity>().Detached()
                .Full.Join("Groups", () => link, () => link.Id == null)
                .Select(u => link.Group.Id);

            var groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(groups2.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .Full.Join("Groups", () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinNonCollection()
        {
            Setting setting = null;

            var query = Query<UserEntity>().Detached()
                .Full.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanInnerJoin()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 2)
                .Inner.Join(u => link.Group, () => group, () => group.Id == 2)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(1));

            var query2 = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link, () => link.Id == 2)
                .Distinct().Select(u => link.Group.Id);

            var groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 2)
                .Inner.Join(u => link.Group, () => group, () => group.Id == 2)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(1));

            var query2 = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link, () => link.Id == 2)
                .Distinct().Select(u => link.Group.Id);

            var groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanInnerJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var query = Query<UserEntity>().Detached()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct().Select(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct().Select(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoin()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct().Select(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct().Select(u => group.Name);

            var groups = Query<GroupEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoin()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var query = Query<UserEntity>().Detached()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join("Groups", () => link)
                .Distinct().Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == 2)
                .LeftOuter.Join(u => link.Group, () => group, () => group.Id == 1)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(1));

            var query2 = Query<UserEntity>().Detached()
                .LeftOuter.Join("Groups", () => link, () => link.Id == 2)
                .Select(u => link.Group.Id);

            var groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(groups2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanLeftOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join("Groups", () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }


        [Test]
        public void CanLeftOuterJoinNonCollection()
        {
            Setting setting = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(settings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanRightOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == 2)
                .RightOuter.Join(u => link.Group, () => group, () => group.Id == 2)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(1));

            var query2 = Query<UserEntity>().Detached()
                .RightOuter.Join("Groups", () => link, () => link.Id == 2)
                .Select(u => link.Group.Id);

            var groups2 = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(groups2.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join("Groups", () => link)
                .Select(u => link.Group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinNonCollection()
        {
            Setting setting = null;

            var query = Query<UserEntity>()
                .Detached()
                .RightOuter.Join(x => x.Setting, () => setting)
                .Select(u => setting.Id);

            var settings = Query<Setting>()
                .Where(s => s.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(settings.Count(), Is.EqualTo(6));
        }

        [Test]
        public void JoiningPropertyMultipleTimesWithSameAliasDoesNotThrow()
        {
            UserGroupLinkEntity link = null;

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .Detached()
                    .Inner.Join(x => x.Groups, () => link)
                    .Inner.Join(x => x.Groups, () => link)
                    .Select();

            }, Throws.Nothing);
        }

        [Test]
        public void JoiningPropertyTwiceWithDifferentAliasesThrows()
        {
            UserGroupLinkEntity link = null, link2 = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .Inner.Join(x => x.Groups, () => link)
                                .Inner.Join(x => x.Groups, () => link2)
                                .Select();

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void UsingSameAliasTwiceThrows()
        {
            object link = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .Inner.Join("Groups", () => link)
                                .Inner.Join("Setting", () => link)
                                .Select();

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void CanInnerJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct().Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct().Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct().Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct().Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanFullJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanFullJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => group.Id);

            var groups = Query<GroupEntity>()
                .Where(g => g.Id, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void JoiningWithProvidedRevealConventionDoesNotThrowIfConventionIsNull()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .Inner.Join(u => u.Groups, () => link)
                                .Inner.Join(u => link.Group, () => group, (IRevealConvention)null);

                        }, Throws.Nothing);

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .Inner.Join(u => u.Groups, () => link)
                                .Inner.Join(u => link.Group, () => group, (IRevealConvention)null);

                        }, Throws.Nothing);

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .Full.Join(u => u.Groups, () => link)
                                .Full.Join(u => link.Group, () => group, (IRevealConvention)null);

                        }, Throws.Nothing);

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .RightOuter.Join(u => u.Groups, () => link)
                                .RightOuter.Join(u => link.Group, () => group, (IRevealConvention)null);

                        }, Throws.Nothing);

            Assert.That(() =>
                        {
                            Query<UserEntity>().Detached()
                                .LeftOuter.Join(u => u.Groups, () => link)
                                .LeftOuter.Join(u => link.Group, () => group, (IRevealConvention)null);

                        }, Throws.Nothing);
        }

        [Test]
        public void CanInnerJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            var query2 = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null, null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            var customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(customers2.Count(), Is.EqualTo(0));

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

            var query = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Full.Join(u => group.Customers, () => customerLink)
                .Full.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            var query2 = Query<UserEntity>().Detached()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Full.Join(x => link.Group, () => group, () => group.Id == null)
                .Full.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null, null)
                .Full.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            var customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query2))
                .Select()
                ;

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

            var query = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            var query2 = Query<UserEntity>().Detached()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(x => link.Group, () => group, () => group.Id == null)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null, null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            var customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query2))
                .Select()
                ;

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

            var query = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .LeftOuter.Join(u => group.Customers, () => customerLink)
                .LeftOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            var query2 = Query<UserEntity>().Detached()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .LeftOuter.Join(x => link.Group, () => group, () => group.Id == null)
                .LeftOuter.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null, null)
                .LeftOuter.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            var customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query2))
                .Select()
                ;

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

            var query = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .RightOuter.Join(u => group.Customers, () => customerLink)
                .RightOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            var customers = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            var query2 = Query<UserEntity>().Detached()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .RightOuter.Join(x => link.Group, () => group, () => group.Id == null)
                .RightOuter.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null, null)
                .RightOuter.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            var customers2 = Query<CustomerEntity>()
                .Where(g => g.Name, FlowQueryIs.In(query2))
                .Select()
                ;

            Assert.That(customers.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }


        #endregion Methods
    }
}