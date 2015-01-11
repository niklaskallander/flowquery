namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using NHibernate.Engine;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class DelayedTest : BaseTest
    {
        [Test]
        public void CanAutoMapDelayedUsingAutoMapMapper()
        {
            Mapper.CreateMap<UserEntity, UserDto>();

            Mapping.SetMapper(new AutoMapMapper());

            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Delayed()
                .Select()
                .ToMap<UserDto>();

            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));
            Assert.That(userCount.Value, Is.EqualTo(4));

            foreach (UserDto item in users)
            {
                Assert.That(Usernames, Contains.Item(item.Username));
                Assert.That(Ids, Contains.Item(item.Id));
                Assert.That(item.Fullname, Is.Null);
                Assert.That(item.SettingId, Is.GreaterThan(0));
            }
        }

        [Test]
        public void CanDelayAnyChecks()
        {
            Lazy<bool> anyoneOnline = Query<UserEntity>()
                .Delayed()
                .Any(x => x.IsOnline);

            FlowQuerySelection<UserEntity> onlineUsers = Query<UserEntity>()
                .Where(x => x.IsOnline)
                .Delayed()
                .Select();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(anyoneOnline.Value, Is.True);
            Assert.That(onlineUsers.Count(), Is.GreaterThan(0));
        }

        [Test]
        public virtual void CanDelayComplexAndNestedObjects()
        {
            var complexType1 = Query<UserEntity>()
                .Delayed()
                .Select(x => new
                {
                    Name = x.Firstname + " " + x.Lastname, 
                    Dto = new UserDto
                    {
                        Id = x.Id, 
                        IsOnline = x.IsOnline
                    }
                });

            var complexType2 = Query<UserEntity>()
                .Delayed()
                .Select(x => new
                {
                    x.Id, 
                    x.IsOnline, 
                    Dto = new UserDto(x.Firstname + " " + x.Lastname)
                });

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(complexType1.Count(), Is.EqualTo(complexType2.Count()));
            Assert.That(complexType1.Count(), Is.GreaterThan(0));
        }

        [Test]
        public virtual void CanDelaySelectDictionary()
        {
            Lazy<Dictionary<string, string>> firstnamesLastnames = Query<UserEntity>()
                .Delayed()
                .SelectDictionary(x => x.Firstname, x => x.Lastname);

            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(firstnamesLastnames.Value.Count, Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public virtual void CanDelaySelectSetup()
        {
            FlowQuerySelection<UserDto> users = Query<UserEntity>()
                .Delayed()
                .Select<UserDto>()
                .For(x => x.Fullname).Use(x => x.Firstname + " " + x.Lastname)
                .Select();

            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public void CanDelaySelectWithConstructionUsingNonComplexType()
        {
            FlowQuerySelection<string> users = Query<UserEntity>()
                .Delayed()
                .Select(x => x.Firstname);

            Lazy<int> userCount = Query<UserEntity>()
                .Delayed()
                .Count();

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));

            Assert.That(users.Count(), Is.EqualTo(userCount.Value));
            Assert.That(userCount.Value, Is.GreaterThan(0));
        }

        [Test]
        public virtual void VerifyDelayedQueryUsesMultiCriteria()
        {
            Query<UserEntity>()
                .Delayed()
                .Select(x => x.Firstname);

            // intentionally not a future query
            Query<UserEntity>()
                .Select(x => x.Lastname);

            Query<UserEntity>()
                .Delayed()
                .Select(x => x.Password);

            var sessionImpl = (ISessionImplementor)Session;

            int count = 0;

            Assert.That(() => count = sessionImpl.FutureCriteriaBatch.Results.Count, Throws.Nothing);

            Assert.That(count, Is.EqualTo(2));
        }
    }
}