using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Core.SelectSetup;
using NHibernate.FlowQuery.Test.Setup.Dtos;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class IDistinctSetupTest : BaseTest
    {
        #region Methods (9)

        [Test]
        public void CanConstruct()
        {
            var setup = Query<UserEntity>().Distinct().Select<UserDto>();

            Assert.That(setup, Is.Not.Null);
        }

        [Test]
        public void CanUseExpressionInForCall()
        {
            var setupPart = Query<UserEntity>()
                .Distinct().Select<UserDto>()
                    .For(x => x.IsOnline);

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void CanUseStringInForCall()
        {
            var setupPart = Query<UserEntity>()
                .Distinct().Select<UserDto>()
                    .For("IsOnline");

            Assert.That(setupPart, Is.Not.Null);
        }

        [Test]
        public void ConstructorThrowsWhenSelectionBuilderIsNull()
        {
            Assert.That(() =>
                        {
                            new SelectSetup<UserEntity, UserDto>(null, null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ForThrowsIfExpressionDoesNotPointToProperty()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>()
                                .Distinct().Select<UserDto>()
                                    .For(x => true);

                        }, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void ForThrowsIfExpressionIsNull()
        {
            Assert.That(() =>
                        {
                            Expression<Func<UserDto, object>> s = null;

                            Query<UserEntity>().Distinct().Select<UserDto>().For(s);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ForThrowsIfStringIsEmpty()
        {
            Assert.That(() =>
                        {
                            string s = string.Empty;

                            Query<UserEntity>().Distinct().Select<UserDto>().For(s);

                        }, Throws.ArgumentException);
        }

        [Test]
        public void ForThrowsIfStringIsNull()
        {
            Assert.That(() =>
                        {
                            string s = null;

                            Query<UserEntity>().Distinct().Select<UserDto>().For(s);

                        }, Throws.ArgumentException);
        }

        [Test]
        public void SelectThrowsIfNoSetupHasBeenProvided()
        {
            Assert.That(() =>
                        {
                            Query<UserEntity>().Distinct().Select<UserDto>().Select();

                        }, Throws.InstanceOf<InvalidOperationException>());
        }

        #endregion Methods
    }
}