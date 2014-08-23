namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class DistinctSetupTest : BaseTest
    {
        [Test]
        public void CanConstruct()
        {
            ISelectSetup<UserEntity, UserDto> setup = CreateSetup();

            Assert.That(setup, Is.Not.Null);
        }

        [Test]
        public void CanUseExpressionInForCall()
        {
            ISelectSetupPart<UserEntity, UserDto> setupPart = CreateSetup()
                .For(x => x.IsOnline);

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void CanUseStringInForCall()
        {
            ISelectSetupPart<UserEntity, UserDto> setupPart = CreateSetup()
                .For("IsOnline");

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsWhenSelectionBuilderIsNull()
        {
            Assert
                .That
                (
                    () => new SelectSetup<UserEntity, UserDto>(null, null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ForThrowsIfExpressionDoesNotPointToProperty()
        {
            Assert.That(() => CreateSetup().For(x => true), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void ForThrowsIfExpressionIsNull()
        {
            Expression<Func<UserDto, object>> s = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(() => CreateSetup().For(s), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ForThrowsIfStringIsEmpty()
        {
            string s = string.Empty;

            Assert.That(() => CreateSetup().For(s), Throws.ArgumentException);
        }

        [Test]
        public void ForThrowsIfStringIsNull()
        {
            string s = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(() => CreateSetup().For(s), Throws.ArgumentException);
        }

        [Test]
        public void SelectThrowsIfNoSetupHasBeenProvided()
        {
            Assert.That(() => CreateSetup().Select(), Throws.InstanceOf<InvalidOperationException>());
        }

        protected ISelectSetup<UserEntity, UserDto> CreateSetup()
        {
            return DummyQuery<UserEntity>()
                .Distinct()
                .Select<UserDto>();
        }
    }
}