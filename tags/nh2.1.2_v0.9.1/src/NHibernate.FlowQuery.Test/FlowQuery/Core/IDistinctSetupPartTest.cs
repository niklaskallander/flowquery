using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class IDistinctSetupPartTest : BaseTest
    {
		#region�Methods�(8)�

        [Test]
        public void CanConstruct()
        {
            var setup = new SelectSetup<UserEntity, UserDto>(Query<UserEntity>());

            var part = new SelectSetupPart<UserEntity, UserDto>("IsOnline", setup);

            Assert.That(part, Is.Not.Null);
        }

        [Test]
        public void CanUseExpressionInUseCall()
        {
            var users = Query<UserEntity>()
                .SelectDistinct<UserDto>()
                    .For(x => x.IsOnline).Use(x => x.IsOnline)
                    .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void CanUseProjectionInUseCall()
        {
            var users = Query<UserEntity>()
                .SelectDistinct<UserDto>()
                    .For(x => x.IsOnline).Use(Projections.Property("IsOnline"))
                    .Select();

            Assert.That(users.Count(), Is.EqualTo(2));
            Assert.That(users.First().IsOnline, Is.False);
            Assert.That(users.Last().IsOnline, Is.True);
        }

        [Test]
        public void ConstructorThrowsIfSetupIsNull()
        {
            Assert.That(() =>
            {
                new SelectSetupPart<UserEntity, UserDto>("IsOnline", null);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorThrowsIfStringIsEmpty()
        {
            var setup = new SelectSetup<UserEntity, UserDto>(Query<UserEntity>());

            Assert.That(() =>
            {
                new SelectSetupPart<UserEntity, UserDto>(string.Empty, setup);

            }, Throws.ArgumentException);
        }

        [Test]
        public void ConstructorThrowsIfStringIsNull()
        {
            var setup = new SelectSetup<UserEntity, UserDto>(Query<UserEntity>());

            Assert.That(() =>
                        {
                            new SelectSetupPart<UserEntity, UserDto>(null, setup);

                        }, Throws.ArgumentException);
        }

        [Test]
        public void UseThrowsIfExpressionIsNull()
        {
            Assert.That(() =>
            {
                Expression<Func<UserEntity, object>> e = null;

                Query<UserEntity>()
                    .SelectDistinct<UserDto>()
                        .For(x => x.IsOnline).Use(e);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void UseThrowsIfProjectionIsNull()
        {
            Assert.That(() =>
                        {
                            IProjection p = null;

                            Query<UserEntity>()
                                .SelectDistinct<UserDto>()
                                    .For(x => x.IsOnline).Use(p);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

		#endregion�Methods�
    }
}