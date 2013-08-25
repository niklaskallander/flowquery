using System.Linq;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using xIs = NUnit.Framework.Is;

    [TestFixture]
    public class JoinsTest : BaseTest
    {
        [Test]
        public void HowToExample1()
        {
            UserGroupLinkEntity linkAlias = null;

            Assert.That(linkAlias, xIs.Null);
        }

        [Test]
        public void HowToExample2()
        {
            ISession session = Session;

            UserGroupLinkEntity linkAlias = null;

            var users = session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(5));
        }

        [Test]
        public void HowToExample3WithOnClaus()
        {
            ISession session = Session;

            UserGroupLinkEntity linkAlias = null;

            var users = session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias, () => linkAlias.Group.Id == 1)
                .Select();

            Assert.That(users.Count(), xIs.EqualTo(2));
        }

        [Test]
        public void HowToExample4WithOnClauseAndRevealConvention()
        {
            ISession session = Session;

            Assert.That(() =>
                        {
                            UserGroupLinkEntity linkAlias = null;
                            GroupEntity groupAlias = null;

                            var users = session.FlowQuery<UserEntity>()
                                .Inner.Join(x => x.Groups, () => linkAlias)
                                .Inner.Join(x => linkAlias.Group, () => groupAlias, () => groupAlias.Id == 1, new CustomConvention(x => "m_" + x))
                                .Select();

                        }, Throws.InstanceOf<QueryException>());
        }

        [Test]
        public void HowToExample4WithOnlyRevealConvention()
        {
            ISession session = Session;

            Assert.That(() =>
                        {
                            UserGroupLinkEntity linkAlias = null;
                            GroupEntity groupAlias = null;

                            var users = session.FlowQuery<UserEntity>()
                                .Inner.Join(x => x.Groups, () => linkAlias)
                                .Inner.Join(x => linkAlias.Group, () => groupAlias, new CustomConvention(x => "m_" + x))
                                .Select();

                        }, Throws.InstanceOf<QueryException>());
        }

        [Test]
        public void HowToExample6ClearJoins()
        {
            ISession session = Session;

            UserGroupLinkEntity linkAlias = null;

            var query = session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias, () => linkAlias.Group.Id == 1);

            IMorphableFlowQuery morphable = query as IMorphableFlowQuery;

            Assert.That(morphable.Joins.Count, xIs.EqualTo(1));

            query
                .ClearJoins();

            Assert.That(morphable.Joins.Count, xIs.EqualTo(0));
        }
    }
}