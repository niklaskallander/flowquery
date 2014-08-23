namespace NHibernate.FlowQuery.Test.FlowQuery.Core.Selection
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class PartialSelectionTest : BaseTest
    {
        [Test]
        public void PartialSelectionReturnsNullAtCompileIfEmpty()
        {
            IPartialSelection<UserEntity, UserDto> selection = DummyQuery<UserEntity>()
                .PartialSelect<UserDto>();

            Expression<Func<UserEntity, UserDto>> expression = selection.Compile();

            Assert.That(expression, Is.Null);
        }

        [Test]
        public void PartialSelectionThrowsIfBuilderIsNull()
        {
            Assert
                .That
                (
                    () => new PartialSelection<UserEntity, UserDto>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void PartialSelectionThrowsIfSelectionIsEmpty()
        {
            Assert.That(() => DummyQuery<UserEntity>().PartialSelect<UserDto>().Select(), Throws.ArgumentException);
        }

        [Test]
        public void PartialSelectionThrowsIfSelectionIsNull()
        {
            Assert
                .That
                (
                    () => DummyQuery<UserEntity>().Select((PartialSelection<UserEntity, UserDto>)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }
    }
}