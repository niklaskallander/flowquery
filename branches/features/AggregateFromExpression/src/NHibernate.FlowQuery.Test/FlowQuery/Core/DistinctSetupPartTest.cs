namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class DistinctSetupPartTest : BaseTest
    {
        [Test]
        public void CanConstruct()
        {
            ISelectSetup<UserEntity, UserDto> setup = DummyQuery<UserEntity>()
                .Select<UserDto>();

            var part = new SelectSetupPart<UserEntity, UserDto>("IsOnline", setup, null);

            Assert.That(part, Is.Not.Null);
        }

        [Test]
        public void CanUseExpressionInUseCall()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct()
                .Select<UserDto>()
                .For(x => x.IsOnline).Use(x => x.IsOnline)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanUseProjectionInUseCall()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Distinct()
                .Select<UserDto>()
                .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void ConstructorThrowsIfSetupIsNull()
        {
            Assert
                .That
                (
                    () => new SelectSetupPart<UserEntity, UserDto>("IsOnline", null, null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ConstructorThrowsIfStringIsEmpty()
        {
            ISelectSetup<UserEntity, UserDto> setup = DummyQuery<UserEntity>()
                .Select<UserDto>();

            Assert
                .That
                (
                    () => new SelectSetupPart<UserEntity, UserDto>(string.Empty, setup, null),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void ConstructorThrowsIfStringIsNull()
        {
            ISelectSetup<UserEntity, UserDto> setup = DummyQuery<UserEntity>()
                .Select<UserDto>();

            Assert.That(() => new SelectSetupPart<UserEntity, UserDto>(null, setup, null), Throws.ArgumentException);
        }

        [Test]
        public void UseThrowsIfExpressionIsNull()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>()
                            .For(x => x.IsOnline).Use((Expression<Func<UserEntity, object>>)null);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void UseThrowsIfProjectionIsNull()
        {
            Assert
                .That
                (
                    () =>
                    {
                        DummyQuery<UserEntity>()
                            .Distinct().Select<UserDto>()
                            .For(x => x.IsOnline).Use((IProjection)null);
                    },
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }
    }
}