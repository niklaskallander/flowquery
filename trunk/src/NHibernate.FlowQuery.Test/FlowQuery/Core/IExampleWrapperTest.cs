using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System.Linq;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class IExampleWrapperTest : BaseTest
    {
        #region Methods (9)

        [Test]
        public void CanConstruct()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            Assert.That(wrapper, Is.Not.Null);
        }

        [Test]
        public void CanEnableLike()
        {
            var users = Query<UserEntity>()
                .RestrictByExample(new UserEntity()
                                     {
                                         Username = "%m%"

                                     }, x => x.ExcludeZeroes()
                                              .ExcludeNulls()
                                              .EnableLike()
                                              .ExcludeProperty(u => u.CreatedStamp)
                                              .ExcludeProperty(u => u.Role)
                                              .ExcludeProperty(u => u.IsOnline)
                                              .ExcludeProperty(u => u.NumberOfLogOns))

                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.Username.Contains("m"));
            }
        }

        [Test]
        public void CanEnableLikeWithMatchMode()
        {
            var users = Query<UserEntity>()
                .RestrictByExample(new UserEntity()
                                     {
                                         Username = "m"

                                     }, x => x.ExcludeZeroes()
                                              .ExcludeNulls()
                                              .EnableLike(MatchMode.Anywhere)
                                              .ExcludeProperty(u => u.CreatedStamp)
                                              .ExcludeProperty(u => u.Role)
                                              .ExcludeProperty(u => u.IsOnline)
                                              .ExcludeProperty(u => u.NumberOfLogOns))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));

            foreach (var u in users)
            {
                Assert.That(u.Username.Contains("m"));
            }
        }

        [Test]
        public void CanExcludePropertyUsingString()
        {
            var users = Query<UserEntity>()
                .RestrictByExample(new UserEntity()
                                     {
                                         IsOnline = true

                                     }, x => x.ExcludeZeroes()
                                              .ExcludeNulls()
                                              .ExcludeProperty("CreatedStamp")
                                              .ExcludeProperty(u => u.Role)
                                              .ExcludeProperty(u => u.NumberOfLogOns))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void CanExcludeZeroesNullsAndSpecificProperties()
        {
            var users = Query<UserEntity>()
                .RestrictByExample(new UserEntity()
                                     {
                                         IsOnline = true

                                     }, x => x.ExcludeZeroes()
                                              .ExcludeNulls()
                                              .ExcludeProperty(u => u.CreatedStamp)
                                              .ExcludeProperty(u => u.Role)
                                              .ExcludeProperty(u => u.NumberOfLogOns))
                .Select()
                ;

            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ConstructorThrowsIfExampleIsNull()
        {
            Assert.That(() =>
                        {
                            new ExampleWrapper<UserEntity>(null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExcludePropertyThrowsWhenExpressionIsNull()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            Expression<Func<UserEntity, object>> e = null;

            Assert.That(() =>
                        {
                            wrapper.ExcludeProperty(e);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ExcludePropertyThrowsWhenStringIsEmpty()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            string s = string.Empty;

            Assert.That(() =>
                        {
                            wrapper.ExcludeProperty(s);

                        }, Throws.ArgumentException);
        }

        [Test]
        public void ExcludePropertyThrowsWhenStringIsNull()
        {
            IExampleWrapper<UserEntity> wrapper = new ExampleWrapper<UserEntity>(Example.Create(new UserEntity()));

            string s = null;

            Assert.That(() =>
                        {
                            wrapper.ExcludeProperty(s);

                        }, Throws.ArgumentException);
        }

        #endregion Methods
    }
}