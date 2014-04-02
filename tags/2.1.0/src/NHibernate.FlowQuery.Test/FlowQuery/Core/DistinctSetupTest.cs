using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Core.Selection;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class DistinctSetupTest : BaseTest
    {
        protected ISelectSetup<UserEntity, UserDto> CreateSetup()
        {
            return DummyQuery<UserEntity>()
                .Distinct()
                .Select<UserDto>();
        }
            
        [Test]
        public void CanConstruct()
        {
            var setup = CreateSetup();

            Assert.That(setup, Is.Not.Null);
        }

        [Test]
        public void CanUseExpressionInForCall()
        {
            var setupPart = CreateSetup()
                .For(x => x.IsOnline);

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void CanUseStringInForCall()
        {
            var setupPart = CreateSetup()
                .For("IsOnline");

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsWhenSelectionBuilderIsNull()
        {
            Assert.That(() => new SelectSetup<UserEntity, UserDto>(null, null), Throws.InstanceOf<ArgumentNullException>());
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
    }
}