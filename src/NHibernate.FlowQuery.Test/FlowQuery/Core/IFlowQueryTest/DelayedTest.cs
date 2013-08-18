using NHibernate.Engine;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Linq;
    using AutoMapper;
    using NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class DelayedTest : BaseTest
    {
        [Test]
        public void CanDelayAnyChecks()
        {
            var anyoneOnline = Query<UserEntity>()
                .Delayed()
                .Any(x => x.IsOnline)
                ;

            var onlineUsers = Query<UserEntity>()
                .Where(x => x.IsOnline)
                .Delayed()
                .Select()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(anyoneOnline.Value, Is.True);
            Assert.That(onlineUsers.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void CanAutoMapDelayedUsingAutoMapMapper()
        {
            Mapper.CreateMap<UserEntity, UserDto>();

            Mapping.SetMapper(new AutoMapMapper());

            var users = Query<UserEntity>()
                .Delayed()
                .Select()
                .ToMap<UserDto>();

            var userCount = Query<UserEntity>()
                .Delayed()
                .Count()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));
            Assert.That(users.Count(), Is.EqualTo(4));

            foreach (var item in users)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Fullname, Is.Null);
                Assert.That(item.SettingId, Is.GreaterThan(0));
            }
        }


        [Test]
        public virtual void CanDelayComplexAndNestedObjects()
        {
            var complexType1 = Query<UserEntity>()
                .Delayed()
                .Select(x => new
                {
                    Name = x.Firstname + " " + x.Lastname,
                    Dto = new UserDto()
                    {
                        Id = x.Id,
                        IsOnline = x.IsOnline
                    }
                })
                ;

            var complexType2 = Query<UserEntity>()
                .Delayed()
                .Select(x => new
                {
                    x.Id,
                    x.IsOnline,
                    Dto = new UserDto(x.Firstname + " " + x.Lastname)
                })
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(complexType1.Count(), Is.EqualTo(complexType2.Count()));
            Assert.That(complexType1.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void CanDelaySelectWithConstructionUsingNonComplexType()
        {
            var users = Query<UserEntity>()
                .Delayed()
                .Select(x => x.Firstname)
                ;

            var userCount = Query<UserEntity>()
                .Delayed()
                .Count()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public virtual void CanDelaySelectSetup()
        {
            var users = Query<UserEntity>()
                .Delayed()
                .Select<UserDto>()
                    .For(x => x.Fullname).Use(x => x.Firstname + " " + x.Lastname)
                    .Select()
                    ;

            var userCount = Query<UserEntity>()
                .Delayed()
                .Count()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public virtual void CanDelaySelectDictionary()
        {
            var firstnamesLastnames = Query<UserEntity>()
                .Delayed()
                .SelectDictionary(x => x.Firstname, x => x.Lastname)
                ;

            var userCount = Query<UserEntity>()
                .Delayed()
                .Count()
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(firstnamesLastnames.Value.Count, Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public virtual void VerifyDelayedQueryUsesMultiCriteria()
        {
            var firstnames = Query<UserEntity>()
                .Delayed()
                .Select(x => x.Firstname)
                ;

            // intentionally not a future query
            var lastnames = Query<UserEntity>()
                .Select(x => x.Lastname)
                ;

            var passwords = Query<UserEntity>()
                .Delayed()
                .Select(x => x.Password)
                ;

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, Is.Not.Null);

            int count = 0;

            Assert.That(() =>
                        {
                            count = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));
        }
    }
}