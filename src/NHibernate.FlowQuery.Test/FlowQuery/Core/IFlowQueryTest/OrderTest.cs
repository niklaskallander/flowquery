namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class OrderTest : BaseTest
    {
        [Test]
        public void CanOrderAscending()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderBy(u => u.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderAscendingUsingProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderBy(Projections.Property("Firstname"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderAscendingUsingString()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderBy("Firstname")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderByProjectionPropertyAscending()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderBy<UserDto>(x => x.SomeValue)
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname + " " + x.Lastname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Kossan Muu"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lars Wilk"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lotta Brak"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Niklas Kallander"));
        }

        [Test]
        public void CanOrderByProjectionPropertyAscendingUsingString()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderBy<UserDto>("SomeValue")
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderByProjectionPropertyAscendingUsingStringCaseInsensitive()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderBy<UserDto>("somevalue")
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderByProjectionPropertyDescending()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderByDescending<UserDto>(x => x.SomeValue)
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderByProjectionPropertyDescendingComplex()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderByDescending<UserDto>(x => x.SomeValue)
                .Distinct()
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname + " " + x.Lastname + " | " + x.Username
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Niklas Kallander | Wimpy"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta Brak | Lajsa"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lars Wilk | Izmid"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Kossan Muu | Empor"));
        }

        [Test]
        public void CanOrderByProjectionPropertyDescendingUsingString()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .OrderByDescending<UserDto>("SomeValue")
                .Select(x => new UserDto
                {
                    SomeValue = x.Firstname
                });

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderByTernaryProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderBy(x => x.IsOnline ? 1 : 0)
                .OrderBy(x => x.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderDescending()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderByDescending(u => u.Firstname)
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderDescendingUsingProjection()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderByDescending(Projections.Property("Firstname"))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderDescendingUsingString()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .OrderByDescending("Firstname")
                .Select();

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void OrderByProjectionWithoutProjectingThePropertyThrows()
        {
            Assert
                .That
                (
                    () =>
                    {
                        Query<UserEntity>()
                            .OrderBy<UserDto>(x => x.SomeValue)
                            .Select(x => new UserDto
                            {
                                Username = x.Firstname
                            });
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void OrderByProjectionWithoutProjectingThePropertyThrowsNothingIfErrorsAreSuppressedGlobally()
        {
            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = true;

            Assert
                .That
                (
                    () =>
                    {
                        Query<UserEntity>()
                            .OrderBy<UserDto>(x => x.SomeValue)
                            .Select(x => new UserDto
                            {
                                Username = x.Firstname
                            });
                    },
                    Throws.Nothing
                );

            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = false;
        }

        [Test]
        public void OrderByProjectionWithoutProjectingThePropertyThrowsNothingIfErrorsAreSuppressedNonGlobally()
        {
            FlowQueryOptions options = new FlowQueryOptions()
                .SuppressOrderByProjectionErrors();

            Assert
                .That
                (
                    () =>
                    {
                        Session.FlowQuery<UserEntity>(options)
                            .OrderBy<UserDto>(x => x.SomeValue)
                            .Select(x => new UserDto
                            {
                                Username = x.Firstname
                            });
                    },
                    Throws.Nothing
                );
        }

        [Test]
        public void OrderByProjectionWithoutProjectingToProjectionTypeThrows()
        {
            Assert
                .That
                (
                    () =>
                    {
                        Query<UserEntity>()
                            .OrderBy<UserDto>(x => x.SomeValue)
                            .Select(x => new UserEntity
                            {
                                Username = x.Firstname
                            });
                    },
                    Throws.InstanceOf<InvalidOperationException>()
                );
        }

        [Test]
        public void OrderByProjectionWithoutProjectingToProjectionTypeThrowsNothingIfErrorsAreSuppressed()
        {
            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = true;

            Assert
                .That
                (
                    () =>
                    {
                        Query<UserEntity>()
                            .OrderBy<UserDto>(x => x.SomeValue)
                            .Select(x => new UserEntity
                            {
                                Username = x.Firstname
                            });
                    },
                    Throws.Nothing
                );

            FlowQueryOptions.GloballySuppressOrderByProjectionErrors = false;
        }
    }
}