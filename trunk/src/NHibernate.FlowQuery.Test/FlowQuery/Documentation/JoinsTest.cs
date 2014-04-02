using System.Linq;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class JoinsTest : BaseTest
    {
        [Test]
        public void HowToExample1()
        {
            UserGroupLinkEntity linkAlias = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(linkAlias, Is.Null);
        }

        [Test]
        public void HowToExample2()
        {
            UserGroupLinkEntity linkAlias = null;

            var users = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(5));
        }

        [Test]
        public void HowToExample3WithOnClaus()
        {
            UserGroupLinkEntity linkAlias = null;

            var users = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias, () => linkAlias.Group.Id == 1)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void HowToExample4WithOnClauseAndRevealConvention()
        {
            Assert.That(() =>
            {
                UserGroupLinkEntity linkAlias = null;
                GroupEntity groupAlias = null;

                Session.FlowQuery<UserEntity>()
                    .Inner.Join(x => x.Groups, () => linkAlias)
                    .Inner.Join(x => linkAlias.Group, () => groupAlias, () => groupAlias.Id == 1, new CustomConvention(x => "m_" + x))
                    .Select();

            }, Throws.InstanceOf<QueryException>());
        }

        [Test]
        public void HowToExample4WithOnlyRevealConvention()
        {
            Assert.That(() =>
            {
                UserGroupLinkEntity linkAlias = null;
                GroupEntity groupAlias = null;

                Session.FlowQuery<UserEntity>()
                    .Inner.Join(x => x.Groups, () => linkAlias)
                    .Inner.Join(x => linkAlias.Group, () => groupAlias, new CustomConvention(x => "m_" + x))
                    .Select();

            }, Throws.InstanceOf<QueryException>());
        }

        [Test]
        public void HowToExample6ClearJoins()
        {
            UserGroupLinkEntity linkAlias = null;

            var query = Session.FlowQuery<UserEntity>()
                .Inner.Join(x => x.Groups, () => linkAlias, () => linkAlias.Group.Id == 1);

            var morphable = (IMorphableFlowQuery)query;

            Assert.That(morphable.Joins.Count, Is.EqualTo(1));

            query.ClearJoins();

            Assert.That(morphable.Joins.Count, Is.EqualTo(0));
        }
    }
}