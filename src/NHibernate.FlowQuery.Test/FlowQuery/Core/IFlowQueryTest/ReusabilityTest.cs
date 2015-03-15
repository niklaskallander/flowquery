namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.FlowQuery.Features.ResultStreaming;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using FqIs = Is;

    [TestFixture]
    public class ReusabilityTest : BaseTest
    {
        [Test]
        public virtual void TestBasicReuseSelectPlusCount()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            int count = query.Count();

            FlowQuerySelection<UserEntity> users = query.Select();

            Assert.That(count, Is.EqualTo(3));
            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));
        }

        [Test]
        public virtual void TestClearRestrictions()
        {
            IImmediateFlowQuery<UserEntity> query1 = Query<UserEntity>()
                .Where(x => x.IsOnline);

            IImmediateFlowQuery<UserEntity> query2 = query1.Copy()
                .ClearRestrictions();

            FlowQuerySelection<UserEntity> users1 = query1.Select();

            FlowQuerySelection<UserEntity> users2 = query2.Select();

            Assert.That(users1.Count(), Is.EqualTo(3));
            Assert.That(users2.Count(), Is.EqualTo(4));
        }

        [Test]
        public virtual void TestDelayedCopy()
        {
            IDelayedFlowQuery<UserEntity> query1 = Query<UserEntity>()
                .Delayed();

            IDelayedFlowQuery<UserEntity> query2 = query1.Copy()
                .Where(x => x.IsOnline);

            FlowQuerySelection<UserEntity> users1 = query1.Select();

            FlowQuerySelection<UserEntity> users2 = query2.Select();

            Assert.That(users1.Count(), Is.EqualTo(4));
            Assert.That(users2.Count(), Is.EqualTo(3));
        }

        [Test]
        public virtual void TestDetachedCopy()
        {
            IDetachedFlowQuery<UserEntity> subquery1 = Query<UserEntity>()
                .Detached()
                .Select(x => x.Id);

            IDetachedFlowQuery<UserEntity> subquery2 = subquery1.Copy()
                .Where(x => x.IsOnline);

            FlowQuerySelection<UserEntity> users1 = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(subquery1))
                .Select();

            FlowQuerySelection<UserEntity> users2 = Query<UserEntity>()
                .Where(x => x.Id, FqIs.In(subquery2))
                .Select();

            Assert.That(users1.Count(), Is.EqualTo(4));
            Assert.That(users2.Count(), Is.EqualTo(3));
        }

        [Test]
        public virtual void TestImmediateCopy()
        {
            IImmediateFlowQuery<UserEntity> query1 = Query<UserEntity>();

            IImmediateFlowQuery<UserEntity> query2 = query1.Copy()
                .Where(x => x.IsOnline);

            FlowQuerySelection<UserEntity> users1 = query1.Select();

            FlowQuerySelection<UserEntity> users2 = query2.Select();

            Assert.That(users1.Count(), Is.EqualTo(4));
            Assert.That(users2.Count(), Is.EqualTo(3));
        }

        [Test]
        public virtual void TestStreamedCopy()
        {
            IStreamedFlowQuery<UserEntity> query1 = Query<UserEntity>()
                .Streamed();

            IStreamedFlowQuery<UserEntity> query2 = query1.Copy()
                .Where(x => x.IsOnline);

            var users1 = new DummyResultStream<UserEntity, UserEntity>();
            var users2 = new DummyResultStream<UserEntity, UserEntity>();

            query1.Select(users1);
            query2.Select(users2);

            Assert.That(users1.Items.Count(), Is.EqualTo(4));
            Assert.That(users2.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public virtual void TestReuseAsSubqueryForRootQuery()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            IDetachedFlowQuery<UserEntity> detached = query
                .Detached()
                .Where(x => x.Lastname.StartsWith("K"))
                .Select(x => x.Id);

            query.Where(x => x.Id, FqIs.In(detached));

            FlowQuerySelection<UserEntity> users = query.Select();

            Assert.That(users.Count(), Is.EqualTo(1));
        }

        [Test]
        public virtual void TestReuseCleanSelectPlusComplexSelect()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            FlowQuerySelection<UserEntity> users = query.Select();

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },
                x.LastLoggedInStamp
            });

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseComplexSelectPlusCleanSelect()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },
                x.LastLoggedInStamp
            });

            FlowQuerySelection<UserEntity> users = query.Select();

            Assert.That(users.Count(), Is.EqualTo(3));
            Assert.That(users.All(x => x.IsOnline));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseComplexSelectPlusSelectDictionary()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => x.IsOnline);

            var complexUsers = query.Select(x => new
            {
                User = new UserDto(x.Firstname + " " + x.Lastname)
                {
                    Id = x.Id,
                    IsOnline = x.IsOnline,
                    SettingId = x.Setting.Id,
                    Username = x.Username
                },
                x.LastLoggedInStamp
            });

            Dictionary<long, bool> userDictionary = query
                .SelectDictionary(x => x.Id, x => x.IsOnline);

            Assert.That(userDictionary.Count(), Is.EqualTo(3));
            Assert.That(userDictionary.All(x => x.Value));

            Assert.That(complexUsers.Count(), Is.EqualTo(3));
            Assert.That(complexUsers.All(x => x.User != null));
            Assert.That(complexUsers.All(x => x.User.IsOnline));
        }

        [Test]
        public virtual void TestReuseSelectSetupPlusSelectDictionary()
        {
            IImmediateFlowQuery<UserEntity> query = Query<UserEntity>()
                .Where(x => !x.IsOnline);

            FlowQuerySelection<UserDto> setupUsers = query
                .Select<UserDto>()
                .For(x => x.Id).Use(x => x.Id)
                .For(x => x.IsOnline).Use(x => x.IsOnline)
                .For(x => x.Username).Use(x => x.Username)
                .Select();

            Dictionary<long, bool> userDictionary = query
                .SelectDictionary(x => x.Id, x => x.IsOnline);

            Assert.That(userDictionary.Count(), Is.EqualTo(1));
            Assert.That(userDictionary.All(x => !x.Value));

            Assert.That(setupUsers.Count(), Is.EqualTo(1));
            Assert.That(setupUsers.All(x => !x.IsOnline));
        }
    }
}