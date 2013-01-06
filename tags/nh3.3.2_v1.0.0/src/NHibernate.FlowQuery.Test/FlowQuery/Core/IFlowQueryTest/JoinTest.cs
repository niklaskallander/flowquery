using System;
using System.Linq;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class JoinTest : BaseTest
    {
        #region Join Tests

        [Test]
        public void CanJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group, new CustomConvention(x => x))
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group, x => group.Id == null, new CustomConvention(x => x))
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group)
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups.Count(), Is.EqualTo(2));

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

            var groups1 = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(() => link.Group, () => group)
                .Join(() => group.Customers, () => customerLink)
                .Join(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            var groups2 = Query<UserEntity>()
                .Join(u => u.Groups, () => link, x => link.Id == null)
                .Join(() => link.Group, () => group, x => group.Id == null)
                .Join(() => group.Customers, () => customerLink, x => customerLink.Id == null)
                .Join(() => customerLink.Customer, () => customer, x => customer.Id == null)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(0));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .Join(u => u.Groups, () => link, u => link.Id == null)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(0));

            var groups2 = Query<UserEntity>()
                .Join("Groups", () => link, u => link.Id == null)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var groups1 = Query<UserEntity>()
                .Join(r.Reveal(x => x.Groups), () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Join(r.Reveal(x => x.Groups), () => link, x => link.Id == null)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinOnJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .Join(u => u.Groups, () => link)
                .Join(u => link.Group, () => group)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .Join(u => u.Groups, () => link, x => link.Id == null)
                .Join(u => link.Group, () => group, x => group.Id == null)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinOnJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .Join("Groups", () => link)
                .Join("link.Group", () => group)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .Join("Groups", () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        #endregion

        #region Inner Join Tests

        [Test]
        public void CanInnerJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group, x => group.Id == null, new CustomConvention(x => x))
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group)
                .SelectDistinct(u => new { group.Id });

            Assert.That(groups.Count(), Is.EqualTo(2));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var groups1 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group)
                .InnerJoin(() => group.Customers, () => customerLink)
                .InnerJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            var groups2 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link, x => link.Id == null)
                .InnerJoin(() => link.Group, () => group, x => group.Id == null)
                .InnerJoin(() => group.Customers, () => customerLink, x => customerLink.Id == null)
                .InnerJoin(() => customerLink.Customer, () => customer, x => customer.Id == null)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(0));
            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanInnerJoinWithSpecifiedOnClause()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link, u => link.Id == 0)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanJoinMultipleTimesWithSpecifiedOnClauses()
        {
            UserGroupLinkEntity link = null;

            UserEntity user = null;

            GroupEntity group = null;

            var groups = Session.FlowQuery<UserEntity>(() => user)
                .InnerJoin(u => u.Groups, () => link, u => link.Id == user.Id)
                .InnerJoin(u => link.Group, () => group, u => group.Name == "A1")
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanJoinWithoutInParameterAndWithoutConvention()
        {
            UserGroupLinkEntity link = null;

            UserEntity user = null;

            GroupEntity group = null;

            var groups = Session.FlowQuery<UserEntity>(() => user)
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(() => link.Group, () => group)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinInCombinationWithRevealFramework()
        {
            UserGroupLinkEntity link = null;

            var r = Reveal.CreateRevealer<UserEntity>(new CustomConvention(s => s));

            var groups = Query<UserEntity>()
                .InnerJoin(r.Reveal(x => x.Groups), () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .InnerJoin(r.Reveal(x => x.Groups), () => link, x => link.Id == null)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntity()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link)
                .InnerJoin(u => link.Group, () => group)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups1.Count(), Is.EqualTo(2));

            var groups2 = Query<UserEntity>()
                .InnerJoin(u => u.Groups, () => link, x => link.Id == null)
                .InnerJoin(u => link.Group, () => group, x => group.Id == null)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups2.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanInnerJoinOnInnerJoinedEntityUsingString()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups = Query<UserEntity>()
                .InnerJoin("Groups", () => link)
                .InnerJoin("link.Group", () => group)
                .SelectDistinct(u => new { group.Name });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanInnerJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .InnerJoin("Groups", () => link)
                .SelectDistinct(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(2));
        }

        #endregion

        #region Left Outer Join Tests

        [Test]
        public void CanLeftOuterJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .LeftOuterJoin(() => link.Group, () => group, x => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .LeftOuterJoin(() => link.Group, () => group, x => group.Id == null, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanLeftOuterJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var groups = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .LeftOuterJoin(() => link.Group, () => group)
                .LeftOuterJoin(() => group.Customers, () => customerLink)
                .LeftOuterJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups.Count(), Is.EqualTo(5));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanLeftOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var groups = Query<UserEntity>()
                .LeftOuterJoin(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups.Count(), Is.EqualTo(6));
        }

        [Test]
        public void CanLeftOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .LeftOuterJoin("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .LeftOuterJoin("Groups", () => link, x => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CanLeftOuterJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .LeftOuterJoin(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(4));

            var settings2 = Query<UserEntity>()
                .LeftOuterJoin(x => x.Setting, () => setting, x => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(4));
        }

        #endregion

        #region Right Outer Join Tests

        [Test]
        public void CanRightOuterJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .RightOuterJoin(() => link.Group, () => group, x => group.Id == null, new CustomConvention(x => x))
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
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .RightOuterJoin(() => link.Group, () => group, x => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(2));

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

            var groups1 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .RightOuterJoin(() => link.Group, () => group)
                .RightOuterJoin(() => group.Customers, () => customerLink)
                .RightOuterJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(4));

            var groups2 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .RightOuterJoin(() => link.Group, () => group, x => group.Id == null)
                .RightOuterJoin(() => group.Customers, () => customerLink, x => customerLink.Id == null)
                .RightOuterJoin(() => customerLink.Customer, () => customer, x => customer.Id == null)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(4));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanRightOuterJoin()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuterJoin(u => u.Groups, () => link, x => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(5));
        }

        [Test]
        public void CanRightOuterJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .RightOuterJoin("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .RightOuterJoin("Groups", () => link, x => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(5));
        }

        [Test]
        public void CanRightOuterJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .RightOuterJoin(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(9));

            var settings2 = Query<UserEntity>()
                .RightOuterJoin(x => x.Setting, () => setting, x => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(6));
        }

        #endregion

        #region Full Join Tests

        [Test]
        public void CanFullJoinWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group)
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link, x => link.Id == null)
                .FullJoin(() => link.Group, () => group, x => group.Id == null)
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(11));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanFullJoinWithProvidedRevealConvention()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            var groups1 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link, x => link.Id == null)
                .FullJoin(() => link.Group, () => group, x => group.Id == null, new CustomConvention(x => x))
                .Select(u => new { group.Id });

            Assert.That(groups2.Count(), Is.EqualTo(11));
        }

        [Test]
        public void CanFullJoinCollectionWithRevealAndLambdas()
        {
            Reveal.SetDefaultConvention(x => x);

            UserGroupLinkEntity link = null;
            GroupEntity group = null;
            CustomerGroupLinkEntity customerLink = null;
            CustomerEntity customer = null;

            var groups1 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .FullJoin(() => link.Group, () => group)
                .FullJoin(() => group.Customers, () => customerLink)
                .FullJoin(() => customerLink.Customer, () => customer)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups1.Count(), Is.EqualTo(5));

            var groups2 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link, x => link.Id == null)
                .FullJoin(() => link.Group, () => group, x => group.Id == null)
                .FullJoin(() => group.Customers, () => customerLink, x => customerLink.Id == null)
                .FullJoin(() => customerLink.Customer, () => customer, x => customer.Id == null)
                .SelectDistinct(u => customer.Name);

            Assert.That(groups2.Count(), Is.EqualTo(5));

            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void CanFullJoin()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .FullJoin(u => u.Groups, () => link, x => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(9));
        }

        [Test]
        public void CanFullJoinUsingString()
        {
            UserGroupLinkEntity link = null;

            var groups1 = Query<UserEntity>()
                .FullJoin("Groups", () => link)
                .Select(u => new { link.Group });

            Assert.That(groups1.Count(), Is.EqualTo(6));

            var groups2 = Query<UserEntity>()
                .FullJoin("Groups", () => link, x => link.Id == null)
                .Select(u => new { link.Group });

            Assert.That(groups2.Count(), Is.EqualTo(9));
        }

        [Test]
        public void CanFullJoinNonCollection()
        {
            Setting setting = null;

            var settings1 = Query<UserEntity>()
                .FullJoin(x => x.Setting, () => setting)
                .Select(u => new { setting.Id });

            Assert.That(settings1.Count(), Is.EqualTo(9));

            var settings2 = Query<UserEntity>()
                .FullJoin(x => x.Setting, () => setting, x => setting.Id == null)
                .Select(u => new { setting.Id });

            Assert.That(settings2.Count(), Is.EqualTo(10));
        }

        #endregion

        #region Overall Tests

        [Test]
        public void JoiningWithProvidedRevealConventionDoesNotThrowIfConventionIsNull()
        {
            UserGroupLinkEntity link = null;
            GroupEntity group = null;

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .InnerJoin(u => u.Groups, () => link)
                    .InnerJoin(() => link.Group, () => group, (IRevealConvention)null);

            }, Throws.Nothing);

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .Join(u => u.Groups, () => link)
                    .Join(() => link.Group, () => group, (IRevealConvention)null);

            }, Throws.Nothing);

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .FullJoin(u => u.Groups, () => link)
                    .FullJoin(() => link.Group, () => group, (IRevealConvention)null);

            }, Throws.Nothing);

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .RightOuterJoin(u => u.Groups, () => link)
                    .RightOuterJoin(() => link.Group, () => group, (IRevealConvention)null);

            }, Throws.Nothing);

            Assert.That(() =>
            {
                Query<UserEntity>()
                    .LeftOuterJoin(u => u.Groups, () => link)
                    .LeftOuterJoin(() => link.Group, () => group, (IRevealConvention)null);

            }, Throws.Nothing);
        }

        [Test]
        public void JoiningPropertyMultipleTimesWithSameAliasDoesNotThrow()
        {
            UserGroupLinkEntity link = null;

            Assert.That(() =>
            {
                Query<UserEntity>()
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
                Query<UserEntity>()
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
                Query<UserEntity>()
                    .InnerJoin("Groups", () => link)
                    .InnerJoin("Setting", () => link)
                    .Select();

            }, Throws.InstanceOf<InvalidOperationException>());
        }

        #endregion
    }
}