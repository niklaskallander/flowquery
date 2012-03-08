using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class ConstructionHelperTest : BaseTest
    {
        #region Methods (4)

        [Test]
        public void CanGetListByExpression()
        {
            Expression<Func<UserEntity, object>> expression = x => new Wrapper { Value = x.IsOnline };

            var isOnline = ConstructionHelper.GetListByExpression<Wrapper>(expression, Projections.Property("IsOnline"), Session.CreateCriteria<UserEntity>());

            Assert.That(isOnline.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GetListByExpressionThrowsWhenCriteriaIsNull()
        {
            Assert.That(() =>
            {
                Expression<Func<UserEntity, object>> expression = x => new { x.IsOnline };

                ConstructionHelper.GetListByExpression<int>(expression, Projections.Property("SomeProperty"), null);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetListByExpressionThrowsWhenExpressionIsNull()
        {
            Assert.That(() =>
                        {
                            var criteria = Session.CreateCriteria<UserEntity>();

                            ConstructionHelper.GetListByExpression<int>(null, Projections.Property("SomeProperty"), criteria);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void GetListByExpressionThrowsWhenProjectionIsNull()
        {
            Assert.That(() =>
                        {
                            var criteria = Session.CreateCriteria<UserEntity>();

                            Expression<Func<UserEntity, object>> expression = x => new { x.IsOnline };

                            ConstructionHelper.GetListByExpression<int>(expression, null, criteria);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        #endregion Methods

        #region Nested Classes (1)

        private class Wrapper
        {
            #region Properties (1)

            public bool Value { get; set; }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}