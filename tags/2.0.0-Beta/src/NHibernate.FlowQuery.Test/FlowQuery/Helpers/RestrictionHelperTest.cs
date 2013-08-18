using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    [TestFixture]
    public class RestrictionHelperTest
    {
        #region Methods (1)

        [Test]
        public void GetProjectionValueCriterionThrowsIfNodeTypeIsInvalid()
        {
            Assert.That(() =>
                        {
                            RestrictionHelper.GetProjectionValueCriterion(Expression.Equal(Expression.Constant(true), Expression.Constant(true)), true, ExpressionType.ListInit, null, new Dictionary<string, string>(), false);

                        }, Throws.InstanceOf<NotSupportedException>());
        }

        #endregion Methods
    }
}