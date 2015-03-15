namespace NHibernate.FlowQuery.Test.FlowQuery.Features.ResultStreaming
{
    using System;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    /// <summary>
    ///     Test-suite for issue #10: Add possibility to stream query results
    /// </summary>
    [TestFixture]
    public class ResultStreamingTest : BaseTest
    {
        [Test]
        public void CanStreamAllUsersAsDtosGivenProjectionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream, Projections.Alias(Projections.Property("Id"), "Id"));

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserDto item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Fullname, Is.Null);
                Assert.That(item.IsOnline, Is.False);
            }
        }

        [Test]
        public void CanStreamAllUsersAsDtosGivenStringPropertiesIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream, "Id", "IsOnline");

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserDto item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Fullname, Is.Null);
            }

            Assert.That(stream.Items.Any(x => x.IsOnline));
            Assert.That(stream.Items.Any(x => !x.IsOnline));
        }

        [Test]
        public void CanStreamAllUsersGivenExpressionPropertiesIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserEntity>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream, x => x.Id, x => x.Firstname, x => x.Lastname);

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserEntity item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Firstname, Is.Not.Null);
                Assert.That(item.Lastname, Is.Not.Null);
                Assert.That(item.IsOnline, Is.False);
                Assert.That(item.Password, Is.Null);
            }
        }

        [Test]
        public void CanStreamAllUsersGivenMemberInitExpressionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            Query<UserEntity>()
                .Streamed()
                .Select
                (
                    stream,
                    x => new UserDto(x.Firstname + " " + x.Lastname)
                    {
                        Id = x.Id,
                        IsOnline = x.IsOnline,
                        SettingId = x.Setting.Id,
                        SomeValue = "Boo",
                        Username = x.Username
                    }

                );

            Assert.That(stream.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanStreamAllUsersGivenNewExpressionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            Query<UserEntity>()
                .Streamed()
                .Select
                (
                    stream,
                    x => new UserDto(x.Firstname + " " + x.Lastname)
                );

            Assert.That(stream.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanStreamAllUsersGivenNoProjectionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserEntity>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream);

            Assert.That(stream.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanStreamAllUsersGivenNoProjectionUsingAnonymousType()
        {
            DummyResultStream<UserEntity, RoleEnum> stream = DummyResultStream<UserEntity>.CreateFrom(x => x.Role);

            Query<UserEntity>()
                .Streamed()
                .Select(stream, stream.Expression);

            Assert.That(stream.Items.Count, Is.EqualTo(4));
            Assert.That(stream.Items, Contains.Item(RoleEnum.Webmaster));
            Assert.That(stream.Items, Contains.Item(RoleEnum.Administrator));
            Assert.That(stream.Items, Contains.Item(RoleEnum.Standard));
        }

        [Test]
        public void CanStreamAllUsersGivenPartialSelectionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>();

            PartialSelection<UserEntity, UserDto> selection = query
                .PartialSelect(x => new UserDto(x.Firstname + " " + x.Lastname))
                .Add(x => new UserDto
                {
                    IsOnline = x.IsOnline,
                    Id = x.Id
                });

            Query<UserEntity>()
                .Streamed()
                .Select(stream, selection);

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserDto item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Fullname, Is.Not.Null);
            }

            Assert.That(stream.Items.Any(x => x.IsOnline));
            Assert.That(stream.Items.Any(x => !x.IsOnline));
        }

        [Test]
        public void CanStreamAllUsersGivenProjectionIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserEntity>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream, Projections.Alias(Projections.Property("Id"), "Id"));

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserEntity item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Firstname, Is.Null);
                Assert.That(item.Lastname, Is.Null);
                Assert.That(item.IsOnline, Is.False);
                Assert.That(item.Password, Is.Null);
            }
        }

        [Test]
        public void CanStreamAllUsersGivenSelectSetupIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>();

            ISelectSetup<UserEntity, UserDto> setup = query
                .Select<UserDto>()
                .For(x => x.Fullname).Use(x => x.Firstname + " " + x.Lastname)
                .For(x => x.Id).Use(x => x.Id);

            query
                .Streamed()
                .Select(stream, setup);

            Assert.That(stream.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanStreamAllUsersGivenStringPropertiesIsSpecified()
        {
            var stream = new DummyResultStream<UserEntity, UserEntity>();

            Query<UserEntity>()
                .Streamed()
                .Select(stream, "Id", "Firstname", "Lastname");

            Assert.That(stream.Items.Count, Is.EqualTo(4));

            foreach (UserEntity item in stream.Items)
            {
                Assert.That(item.Id, Is.GreaterThan(0));
                Assert.That(item.Firstname, Is.Not.Null);
                Assert.That(item.Lastname, Is.Not.Null);
                Assert.That(item.IsOnline, Is.False);
                Assert.That(item.Password, Is.Null);
            }
        }

        [Test]
        public void ThrowsGivenResultStreamIsNullAndNoProjectionIsSpecified()
        {
            Assert
                .That
                (
                    () => Query<UserEntity>()
                        .Streamed()
                        .Select(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void TrowsGivenPartialSelectionIsEmpty()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            IPartialSelection<UserEntity, UserDto> selection = new PartialSelection<UserEntity, UserDto>(x => null);

            Assert
                .That
                (
                    () => Query<UserEntity>()
                        .Streamed()
                        .Select(stream, selection),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void TrowsGivenPartialSelectionIsNull()
        {
            var stream = new DummyResultStream<UserEntity, UserDto>();

            Assert
                .That
                (
                    () => Query<UserEntity>()
                        .Streamed()
                        .Select(stream, (IPartialSelection<UserEntity, UserDto>)null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }
    }
}