using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;
using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Setup.Dtos;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.ISubFlowQueryTest
{
    using Is = NUnit.Framework.Is;
    using FlowQueryIs = NHibernate.FlowQuery.Is;

    [TestFixture]
    public class OrderTest : BaseTest
    {
        #region Methods (12)

        [Test]
        public void CanOrderAscending()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderBy(x => x.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingProjection()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderBy(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderAscendingUsingString()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderBy("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lars"));
        }

        [Test]
        public void CanOrderDescending()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderByDescending(u => u.Firstname)
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingProjection()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderByDescending(Projections.Property("Firstname"))
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        [Test]
        public void CanOrderDescendingUsingString()
        {
            var id = SubQuery.For<UserEntity>()
                .OrderByDescending("Firstname")
                .Take(1)
                .Skip(1)
                .Select(x => x.Id);

            var users = Query<UserEntity>()
                .Where(x => x.Id, FlowQueryIs.EqualTo(id))
                .Select();

            Assert.That(users.Count(), Is.EqualTo(1));
            Assert.That(users.ElementAt(0).Firstname, Is.EqualTo("Lotta"));
        }

        #endregion Methods
    }
}