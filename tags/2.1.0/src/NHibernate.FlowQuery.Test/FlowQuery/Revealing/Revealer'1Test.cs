using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Revealing
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class Revealer1Test
    {
        private IRevealer<UserEntity> Revealer { get; set; }

        [Test]
        public void CanProvideConventionAtInstantiation()
        {
            IRevealer<UserEntity> revealer = new Revealer<UserEntity>(new UnderscoreConvention());

            Assert.That(revealer.Reveal(x => x.Password), Is.EqualTo("_Password"));
        }

        [Test]
        public void CanReveal()
        {
            Assert.That(Revealer.Reveal(x => x.Password), Is.EqualTo("m_Password"));
        }

        [Test]
        public void CanRevealDeep()
        {
            Assert.That(Revealer.Reveal(x => x.Setting.Id), Is.EqualTo("Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(x => u.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromTypedExpressions()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u, x => x.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromUntypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealUsingProvidedConvention()
        {
            Assert.That(Revealer.Reveal(x => x.Password, new UnderscoreConvention()), Is.EqualTo("_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(x => u.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressions()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u, x => x.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressionsUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u, x => x.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressionUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(x => u.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void CanRevealWithAliasFromUntypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromUntypedExpressionUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal(() => u.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void RevealerUsesMUnderscoreConventionByDefault()
        {
            Assert.That(Revealer.Reveal(x => x.Password), Is.EqualTo("m_Password"));
        }

        [Test]
        public void RevealThrowsWhenProvidingExpressionNotPointingToAMemberExpression()
        {
            Assert.That(() => Revealer.Reveal(() => ""), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsAliasExpression()
        {
            Assert.That(() => Revealer.Reveal(null, x => x.Password), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsConvention()
        {
            UserEntity u = null;

            Assert.That(() => Revealer.Reveal(() => u.Password, null), Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() => Revealer.Reveal(x => x.Password, null), Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() => Revealer.Reveal(() => u, x => x.Password, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsExpression()
        {
            Expression<Func<object>> expression1 = null;
            Expression<Func<UserEntity, object>> expression2 = null;

            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(() => Revealer.Reveal(expression1), Throws.InstanceOf<ArgumentNullException>());
            Assert.That(() => Revealer.Reveal(expression2), Throws.InstanceOf<ArgumentNullException>());
            // ReSharper restore ExpressionIsAlwaysNull

            UserEntity u = null;

            Assert.That(() => Revealer.Reveal(() => u, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [SetUp]
        public void SetUp()
        {
            Revealer = new Revealer<UserEntity>();
        }

        [Test]
        public void ThrowsWhenAttemptingInstantiationWithNullAsConvention()
        {
            Assert.That(() => new Revealer(null), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}