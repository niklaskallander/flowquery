// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Revealing;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class JoinTest : BaseTest
    {
        [Test, Category("MySqlUnsupported")]
        public void CanFullJoin()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(9));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            FlowQuerySelection<string> groups1 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Full.Join(u => group.Customers, () => customerLink)
                .Full.Join(u => customerLink.Customer, () => customer)
                .Distinct().Select(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(5));

            FlowQuerySelection<string> groups2 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Full.Join(u => link.Group, () => group, () => group.Id == null)
                .Full.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null)
                .Full.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct().Select(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(5));

            Reveal.ClearDefaultConvention();
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .Full.Join(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(9));

            var settings2 = Query<UserEntity>()
                .Full.Join(x => x.Setting, () => setting, () => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(10));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .Full.Join("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .Full.Join("Groups", () => link, () => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(9));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Full.Join(u => link.Group, () => group, () => group.Id == null, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(11));
        }

        [Test, Category("MySqlUnsupported")]
        public void CanFullJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link)
                .Full.Join(u => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .Full.Join(u => u.Groups, () => link, () => link.Id == null)
                .Full.Join(u => link.Group, () => group, () => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(11));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct().Select(u => new { link.Group });

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

            FlowQuerySelection<string> groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            FlowQuerySelection<string> groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(0));
            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            IRevealer<UserEntity> r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var groups = Query<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link, () => link.Id == null)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null, new CustomConvention(x => x))
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == 0)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            FlowQuerySelection<string> groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Inner.Join(u => group.Customers, () => customerLink)
                .Inner.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            FlowQuerySelection<string> groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null)
                .Inner.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null)
                .Inner.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(0));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            IRevealer<UserEntity> r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var groups1 = Query<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(r.Reveal(x => x.Groups), () => link, () => link.Id == null)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinMultipleTimesWithSpecifiedOnClauses()
        {
            UserGroupLinkEntity link = null;

            UserEntity user = null;

            GroupEntity group = null;

            var groups = Session.FlowQuery(() => user)
                .Inner.Join(u => u.Groups, () => link, () => link.Id == user.Id)
                .Inner.Join(u => link.Group, () => group, () => group.Name == "A1")
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanJoinOnJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinOnJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Inner.Join("link.Group", () => group)
                .Distinct()
                .Select(u => new { group.Name });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Inner.Join("Groups", () => link)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group, () => group.Id == null, new CustomConvention(x => x))
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => new { group.Id });

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .Inner.Join(u => u.Groups, () => link, () => link.Id == null)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(0));

            var groups2 = Query<UserEntity>()
                .Inner.Join("Groups", () => link, () => link.Id == null)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinWithoutInParameterAndWithoutConvention()
        {
            UserGroupLinkEntity link = null;

            UserEntity user = null;

            GroupEntity group = null;

            var groups = Session.FlowQuery(() => user)
                .Inner.Join(u => u.Groups, () => link)
                .Inner.Join(u => link.Group, () => group)
                .Distinct()
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanLeftOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanLeftOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            FlowQuerySelection<string> groups = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .LeftOuter.Join(u => group.Customers, () => customerLink)
                .LeftOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups.Count(), Is.EqualTo(5));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .LeftOuter.Join(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(4));

            var settings2 = Query<UserEntity>()
                .LeftOuter.Join(x => x.Setting, () => setting, () => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanLeftOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .LeftOuter.Join("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuter.Join("Groups", () => link, () => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanLeftOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .LeftOuter.Join(u => link.Group, () => group, () => group.Id == null, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanLeftOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link)
                .LeftOuter.Join(u => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .LeftOuter.Join(u => link.Group, () => group, () => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(5));
        }

        [Test]
        public void CanRightOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            FlowQuerySelection<string> groups1 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .RightOuter.Join(u => group.Customers, () => customerLink)
                .RightOuter.Join(u => customerLink.Customer, () => customer)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            FlowQuerySelection<string> groups2 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .RightOuter.Join(u => link.Group, () => group, () => group.Id == null)
                .RightOuter.Join(u => group.Customers, () => customerLink, () => customerLink.Id == null)
                .RightOuter.Join(u => customerLink.Customer, () => customer, () => customer.Id == null)
                .Distinct()
                .Select(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .RightOuter.Join(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(9));

            var settings2 = Query<UserEntity>()
                .RightOuter.Join(x => x.Setting, () => setting, () => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanRightOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .RightOuter.Join("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuter.Join("Groups", () => link, () => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(5));
        }

        [Test]
        public void CanRightOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .RightOuter.Join(u => link.Group, () => group, () => group.Id == null, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanRightOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link)
                .RightOuter.Join(u => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuter.Join(u => u.Groups, () => link, () => link.Id == null)
                .RightOuter.Join(u => link.Group, () => group, () => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
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
                            .Inner.Join(x => x.Groups, () => link)
                            .Inner.Join(x => x.Groups, () => link)
                            .Select();
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
                        DummyQuery<UserEntity>()
                            .Inner.Join(x => x.Groups, () => link)
                            .Inner.Join(x => x.Groups, () => link2)
                            .Select();
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
                        DummyQuery<UserEntity>()
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
                        DummyQuery<UserEntity>()
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
                        DummyQuery<UserEntity>()
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
                        DummyQuery<UserEntity>()
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
                        DummyQuery<UserEntity>()
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
                        DummyQuery<UserEntity>()
                            .Inner.Join("Groups", () => link)
                            .Inner.Join("Setting", () => link)
                            .Select();
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }
    }
}