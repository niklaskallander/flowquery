using System;
using NHibernate.FlowQuery.Revealing;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class RevealTest
    {
        [Test]
        public void CanCreateRevealerUsingConventionDelegate()
        {
            IRevealer revealer = Reveal.CreateRevealer(s => "L" + s);

            Assert.That(revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("LPassword"));

            IRevealer<UserEntity> revealerOfUser = Reveal.CreateRevealer<UserEntity>(s => "L" + s);

            Assert.That(revealerOfUser.Reveal(x => x.Password), Is.EqualTo("LPassword"));
        }

        [Test]
        public void CanCreateTypeIgnorantRevealerWithConventionProvided()
        {
            var revealer = Reveal.CreateRevealer(new UnderscoreConvention());

            Assert.That(revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("_Password"));
        }

        [Test]
        public void CanCreateTypeIgnorantRevealerWithDefaultConvention()
        {
            var revealer = Reveal.CreateRevealer();

            Assert.That(revealer.Reveal<UserEntity>(x => x.Username), Is.EqualTo("m_Username"));
        }

        [Test]
        public void CanCreateTypeLockedRevealerWithConventionProvided()
        {
            var revealer = Reveal.CreateRevealer<UserEntity>(new UnderscoreConvention());

            Assert.That(revealer.Reveal(x => x.Password), Is.EqualTo("_Password"));
        }

        [Test]
        public void CanCreateTypeLockedRevealerWithDefaultConvention()
        {
            var revealer = Reveal.CreateRevealer<UserEntity>();

            Assert.That(revealer.Reveal(x => x.Username), Is.EqualTo("m_Username"));
        }

        [Test]
        public void CanRevealDeep()
        {
            Assert.That(Reveal.ByConvention<UserEntity>(x => x.Setting.Id), Is.EqualTo("Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention<UserEntity>(x => u.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromTypedExpressions()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u, x => x.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealDeepWithAliasFromUntypedExpression()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u.Setting.Id), Is.EqualTo("u.Setting.m_Id"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpression()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention<UserEntity>(x => u.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressions()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u, x => x.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressionsUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u, x => x.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void CanRevealWithAliasFromTypedExpressionUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention<UserEntity>(x => u.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void CanRevealWithAliasFromUntypedExpression()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u.Password), Is.EqualTo("u.m_Password"));
        }

        [Test]
        public void CanRevealWithAliasFromUntypedExpressionUsingProvidedConvention()
        {
            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u.Password, new UnderscoreConvention()), Is.EqualTo("u._Password"));
        }

        [Test]
        public void CanRevealWithConventionSpecified()
        {
            Assert.That(Reveal.ByConvention<UserEntity>(x => x.Username, new MConvention()), Is.EqualTo("mUsername"));
        }

        [Test]
        public void CanRevealWithoutSpecifyingDefaultConvention()
        {
            Assert.That(() => Reveal.ByConvention<UserEntity>(x => x.Password), Throws.Nothing);
        }

        [Test]
        public void CanSetDefaultConvention()
        {
            Reveal.SetDefaultConvention(new CustomConvention(s => "ere" + s + "::-"));

            Assert.That(Reveal.ByConvention<UserEntity>(x => x.Username), Is.EqualTo("ereUsername::-"));
        }

        [Test]
        public void CanSetDefaultConventionToConventionDelegate()
        {
            Reveal.SetDefaultConvention(s => "L" + s);

            Assert.That(Reveal.ByConvention<UserEntity>(x => x.Password), Is.EqualTo("LPassword"));

            UserEntity u = null;

            Assert.That(Reveal.ByConvention(() => u.Password), Is.EqualTo("u.LPassword"));
            Assert.That(Reveal.ByConvention(() => u, x => x.Password), Is.EqualTo("u.LPassword"));

            IRevealer revealer = Reveal.CreateRevealer();

            Assert.That(revealer.Reveal<UserEntity>(x => x.Password), Is.EqualTo("LPassword"));

            IRevealer<UserEntity> revealerOfUser = Reveal.CreateRevealer<UserEntity>();

            Assert.That(revealerOfUser.Reveal(x => x.Password), Is.EqualTo("LPassword"));
        }

        [Test]
        public void CustomConventionThrowsWhenAttemptingToInstantiateItWithNullAsConventionDelegate()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.That(() => new CustomConvention(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void SetDefaultConventionThrowsIfConventionIsNull()
        {
            IRevealConvention c = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(() => Reveal.SetDefaultConvention(c), Throws.InstanceOf<ArgumentNullException>());

            Func<string, string> d = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(() => Reveal.SetDefaultConvention(d), Throws.InstanceOf<ArgumentNullException>());
        }

        [TearDown]
        public void TearDown()
        {
            Reveal.ClearDefaultConvention();
        }

        [Test]
        public void UsesMUnderscoreConventionByDefault()
        {
            Assert.That(Reveal.ByConvention<UserEntity>(x => x.Password), Is.EqualTo("m_Password"));
        }
    }
}