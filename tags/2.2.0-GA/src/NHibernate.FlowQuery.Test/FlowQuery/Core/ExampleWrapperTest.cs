// ReSharper disable ExpressionIsAlwaysNull
namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class ExampleWrapperTest : BaseTest
    {
        [Test]
        public void CanConstruct()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            Assert.That(wrapper, Is.Not.Null);
        }

        [Test]
        public void CanEnableLike()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .RestrictByExample
                (
                    new UserEntity
                    {
                        Username = "%m%"
                    },
                    x => x.ExcludeZeroes()
                        .ExcludeNulls()
                        .EnableLike()
                        .ExcludeProperty(u => u.CreatedStamp)
                        .ExcludeProperty(u => u.Role)
                        .ExcludeProperty(u => u.IsOnline)
                        .ExcludeProperty(u => u.NumberOfLogOns)
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.Username.Contains("m"));
            }
        }

        [Test]
        public void CanEnableLikeWithMatchMode()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .RestrictByExample
                (
                    new UserEntity
                    {
                        Username = "m"
                    },
                    x => x.ExcludeZeroes()
                        .ExcludeNulls()
                        .EnableLike(MatchMode.Anywhere)
                        .ExcludeProperty(u => u.CreatedStamp)
                        .ExcludeProperty(u => u.Role)
                        .ExcludeProperty(u => u.IsOnline)
                        .ExcludeProperty(u => u.NumberOfLogOns)
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (UserEntity u in users)
            {
                Assert.That(u.Username.Contains("m"));
            }
        }

        [Test]
        public void CanExcludePropertyUsingString()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .RestrictByExample
                (
                    new UserEntity
                    {
                        IsOnline = true
                    },
                    x => x.ExcludeZeroes()
                        .ExcludeNulls()
                        .ExcludeProperty("CreatedStamp")
                        .ExcludeProperty(u => u.Role)
                        .ExcludeProperty(u => u.NumberOfLogOns)
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanExcludeZeroesNullsAndSpecificProperties()
        {
            FlowQuerySelection<UserEntity> users = Query<UserEntity>()
                .RestrictByExample
                (
                    new UserEntity
                    {
                        IsOnline = true
                    },
                    x => x.ExcludeZeroes()
                        .ExcludeNulls()
                        .ExcludeProperty(u => u.CreatedStamp)
                        .ExcludeProperty(u => u.Role)
                        .ExcludeProperty(u => u.NumberOfLogOns)
                )
                .Select();

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ConstructorThrowsIfExampleIsNull()
        {
            Assert.That(() => new ExampleWrapper<UserEntity>(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExcludePropertyThrowsWhenExpressionIsNull()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            Expression<Func<UserEntity, object>> e = null;

            Assert.That(() => wrapper.ExcludeProperty(e), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExcludePropertyThrowsWhenStringIsEmpty()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            string s = string.Empty;

            Assert.That(() => wrapper.ExcludeProperty(s), Throws.ArgumentException);
        }

        [Test]
        public void ExcludePropertyThrowsWhenStringIsNull()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            string s = null;

            Assert.That(() => wrapper.ExcludeProperty(s), Throws.ArgumentException);
        }
    }
}