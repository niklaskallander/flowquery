namespace NHibernate.FlowQuery.Test.FlowQuery.Revealing
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NHibernate.FlowQuery.Revealing;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class RevealerTest
    {
        private IRevealer Revealer { get; set; }

        [Test]
        public void CanProvideConventionAtInstantiation()
        {
            IRevealer revealer = new Revealer(new UnderscoreConvention());

            Assert.That(revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("_Password"));
        }

        [Test]
        public void CanReveal()
        {
            Assert.That(Revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("m_Password"));
        }

        [Test]
        public void CanRevealDeep()
        {
            Assert.That(Revealer.Reveal<UserEntity>(x => x.Setting.Id), Is.EqualTo("Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal<UserEntity>(x => u.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
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
        public void CanRevealUsingExposedRevealConvention()
        {
            Assert.That(Revealer.RevealConvention.RevealFrom("Password"), Is.EqualTo("m_Password"));
        }

        [Test]
        public void CanRevealUsingProvidedConvention()
        {
            Assert
                .That
                (
                    Revealer.Reveal<UserEntity>(x => x.Password, new UnderscoreConvention()), 
                    Is.EqualTo("_Password")
                );
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Revealer.Reveal<UserEntity>(x => u.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressionUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert
                .That
                (
                    Revealer.Reveal<UserEntity>(x => u.Password, new UnderscoreConvention()), 
                    Is.EqualTo("u._Password")
                );
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

            Assert
                .That(Revealer.Reveal(() => u, x => x.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
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
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", 
            Justification = "Reviewed. Suppression is OK here.")]
        public void RevealThrowsWhenProvidingExpressionNotPointingToAMemberExpression()
        {
            Assert.That(() => Revealer.Reveal(() => ""), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsAliasExpression()
        {
            Assert
                .That
                (
                    () => Revealer.Reveal<UserEntity>(null, x => x.Password), 
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsConvention()
        {
            UserEntity u = null;

            Assert.That(() => Revealer.Reveal(() => u.Password, null), Throws.InstanceOf<ArgumentNullException>());

            Assert
                .That
                (
                    () => Revealer.Reveal<UserEntity>(x => x.Password, null), 
                    Throws.InstanceOf<ArgumentNullException>()
                );

            Assert
                .That
                (
                    () => Revealer.Reveal(() => u, x => x.Password, null), 
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void RevealThrowsWhenProvidingNullAsExpression()
        {
            UserEntity u = null;

            Assert.That(() => Revealer.Reveal(null), Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() => Revealer.Reveal<UserEntity>(null), Throws.InstanceOf<ArgumentNullException>());

            Assert.That(() => Revealer.Reveal(() => u, null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void RevealerUsesMUnderscoreConventionByDefault()
        {
            Assert.That(Revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("m_Password"));
        }

        [SetUp]
        public void SetUp()
        {
            Revealer = new Revealer();
        }

        [Test]
        public void ThrowsWhenAttemptingInstantiationWithNullAsConvention()
        {
            Assert.That(() => new Revealer(null), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}