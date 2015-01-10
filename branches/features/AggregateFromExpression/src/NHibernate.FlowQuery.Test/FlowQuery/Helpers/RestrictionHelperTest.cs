namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class RestrictionHelperTest
    {
        [Test]
        public void GetProjectionProjectionCriterionThrowsIfNodeTypeIsInvalid()
        {
            Assert
                .That
                (
                    () => { RestrictionHelper.GetProjectionProjectionCriterion(null, null, ExpressionType.Block); },
                    Throws.InstanceOf<NotSupportedException>()
                );
        }

        [Test]
        public void GetProjectionValueCriterionThrowsIfNodeTypeIsInvalid()
        {
            var data = new QueryHelperData(new Dictionary<string, string>(), new List<Join>());

            var context = new HelperContext(data, (string)null, HelperType.Filter);

            Assert
                .That
                (
                    () =>
                    {
                        RestrictionHelper
                            .GetProjectionValueCriterion
                            (
                                Expression.Equal(Expression.Constant(true), Expression.Constant(true)),
                                true,
                                ExpressionType.ListInit,
                                context,
                                false
                            );
                    },
                    Throws.InstanceOf<NotSupportedException>()
                );
        }
    }
}