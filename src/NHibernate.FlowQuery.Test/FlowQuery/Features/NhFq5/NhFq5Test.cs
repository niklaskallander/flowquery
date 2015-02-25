namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq5
{
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    /// <summary>
    ///     Tests for issue-5: Add possibility to use non-local variable aliases.
    /// </summary>
    [TestFixture]
    public class NhFq5Test : BaseTest
    {
        private static readonly GroupEntity StaticGroupField = null;

        private readonly GroupEntity _groupField = null;

        private static GroupEntity StaticGroupProperty
        {
            get
            {
                return StaticGroupField;
            }
        }

        private GroupEntity GroupProperty
        {
            get
            {
                return _groupField;
            }
        }

        [Test]
        public void CanUseExternalFieldAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            var aliases = new NhFq5Aliases();

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => aliases._groupField)
                .Select(x => aliases._groupField.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUseExternalPropertyAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            var aliases = new NhFq5Aliases();

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => aliases.GroupProperty)
                .Select(x => aliases.GroupProperty.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUseExternalStaticFieldAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => NhFq5Aliases.StaticGroupField)
                .Select(x => NhFq5Aliases.StaticGroupField.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUseExternalStaticPropertyAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => NhFq5Aliases.StaticGroupProperty)
                .Select(x => NhFq5Aliases.StaticGroupProperty.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUsePrivateFieldAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => _groupField)
                .Select(x => _groupField.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUsePrivatePropertyAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => GroupProperty)
                .Select(x => GroupProperty.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUsePrivateStaticFieldAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => StaticGroupField)
                .Select(x => StaticGroupField.Id);

            Assert.That(groupIds.Any());
        }

        [Test]
        public void CanUsePrivateStaticPropertyAsAliasForJoin()
        {
            UserGroupLinkEntity groupLink = null;

            FlowQuerySelection<long> groupIds = Query<UserEntity>()
                .Inner.Join(x => x.Groups, () => groupLink)
                .Inner.Join(x => groupLink.Group, () => StaticGroupProperty)
                .Select(x => StaticGroupProperty.Id);

            Assert.That(groupIds.Any());
        }
    }
}