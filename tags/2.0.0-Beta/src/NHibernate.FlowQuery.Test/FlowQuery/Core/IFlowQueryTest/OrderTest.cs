using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Linq;
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class OrderTest : BaseTest
    {
        #region Methods (12)

        [Test]
        public void CanOrderAscending()
        {
            var users = Query<UserEntity>()
                .Order.By(u => u.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderAscendingUsingProjection()
        {
            var users = Query<UserEntity>()
                .Order.By(Projections.Property("Firstname"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderAscendingUsingString()
        {
            var users = Query<UserEntity>()
                .Order.By("Firstname")
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderByProjectionPropertyAscending()
        {
            var users = Query<UserEntity>()
                .Order.By<UserDto>(x => x.SomeValue)
                .Select(x => new UserDto()
                {
                    SomeValue = x.Firstname + " " + x.Lastname
                })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Kossan Muu"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lars Wilk"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lotta Bråk"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Niklas Källander"));
        }

        [Test]
        public void CanOrderByProjectionPropertyAscendingUsingString()
        {
            var users = Query<UserEntity>()
                .Order.By<UserDto>("SomeValue")
                .Select(x => new UserDto()
                {
                    SomeValue = x.Firstname
                })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Kossan"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Niklas"));
        }

        [Test]
        public void CanOrderByProjectionPropertyDescending()
        {
            var users = Query<UserEntity>()
                .Order.ByDescending<UserDto>(x => x.SomeValue)
                .Select(x => new UserDto()
                {
                    SomeValue = x.Firstname
                })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderByProjectionPropertyDescendingUsingString()
        {
            var users = Query<UserEntity>()
                .Order.ByDescending<UserDto>("SomeValue")
                .Select(x => new UserDto()
                {
                    SomeValue = x.Firstname
                })
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).SomeValue, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).SomeValue, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).SomeValue, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).SomeValue, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderDescending()
        {
            var users = Query<UserEntity>()
                .Order.ByDescending(u => u.Firstname)
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderDescendingUsingProjection()
        {
            var users = Query<UserEntity>()
                .Order.ByDescending(Projections.Property("Firstname"))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void CanOrderDescendingUsingString()
        {
            var users = Query<UserEntity>()
                .Order.ByDescending("Firstname")
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(4));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Niklas"));
            Assert.That(users.ElementAt(1).Firstname, Is.EqualTo("Lotta"));
            Assert.That(users.ElementAt(2).Firstname, Is.EqualTo("Lars"));
            Assert.That(users.ElementAt(3).Firstname, Is.EqualTo("Kossan"));
        }

        [Test]
        public void OrderByProjectionWithoutProjectingThePropertyThrows()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Order.By<UserDto>(x => x.SomeValue)
                                .Select(x => new UserDto()
                                {
                                    Username = x.Firstname
                                })
                                ;

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void OrderByProjectionWithoutProjectingToProjectionTypeThrows()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Order.By<UserDto>(x => x.SomeValue)
                                .Select(x => new UserEntity()
                                {
                                    Username = x.Firstname
                                })
                                ;

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        #endregion Methods
    }
}