namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class GeneralTests : BaseTest
    {
        [Test]
        public void ApplyingNullReferenceAliasOnAliasThrows()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Assert
                .That
                (
                    () => query.ApplyFilterOn(null, Where.Id_Of_Setting_Is(1)),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ApplyingNullReferenceFilterOnAliasThrows()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Setting alias = null;

            Assert.That(() => query.ApplyFilterOn(() => alias, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ApplyingNullReferenceFilterOnSourceThrows()
        {
            IImmediateFlowQuery<UserEntity> query = DummyQuery<UserEntity>();

            Assert.That(() => query.ApplyFilter(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExpresionRebaserTest()
        {
            Expression<Func<Setting, bool>> filter = x => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Expression<Func<UserEntity, bool>> visited = visitor.RebaseTo<UserEntity, bool>(filter);

            Assert.That(visited, Is.Not.Null);
            Assert.That(visited.ToString(), Is.EqualTo("x => (setting.Id == 1)"));
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionHasMoreThanTwoParameters()
        {
            Expression<Func<Setting, string, string, bool>> filter = (x,
                y,
                z) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(filter), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionHasTwoParametersAndSecondParamterIsNotWhereDelegate()
        {
            Expression<Func<Setting, string, bool>> filter = (x,
                y) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(filter), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ExpresionRebaserThrowsIfExpressionIsNull()
        {
            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, bool>(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExpresionRebaserThrowsNothingIfExpressionHasTwoParametersAndSecondParamterIsWhereDelegate()
        {
            Expression<Func<Setting, WhereDelegate, bool>> filter = (x,
                y) => x.Id == 1;

            var visitor = new ExpressionRebaser(typeof(Setting), "setting");

            Assert.That(() => visitor.RebaseTo<UserEntity, WhereDelegate, bool>(filter), Throws.Nothing);
        }
    }
}