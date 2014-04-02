using System;
using NHibernate.FlowQuery.Core.Selection;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Selection
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class PartialSelectionTest : BaseTest
    {
        [Test]
        public void PartialSelectionThrowsIfBuilderIsNull()
        {
            Assert.That(() => new PartialSelection<UserEntity, UserDto>(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void PartialSelectionThrowsIfSelectionIsEmpty()
        {
            Assert.That(() => DummyQuery<UserEntity>().PartialSelect<UserDto>().Select(), Throws.ArgumentException);
        }

        [Test]
        public void PartialSelectionThrowsIfSelectionIsNull()
        {
            PartialSelection<UserEntity, UserDto> selection = null;

            Assert.That(() => DummyQuery<UserEntity>().Select(selection), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void PartialSelectionReturnsNullAtCompileIfEmpty()
        {
            var selection = DummyQuery<UserEntity>()
                .PartialSelect<UserDto>();

            var expression = selection.Compile();

            Assert.That(expression, Is.Null);
        }
    }
}
