namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Members
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;

    /// <summary>
    ///     Handles projections of <see cref="DateTime.Date" />.
    /// </summary>
    public class DateTimeDateHandler : AbstractMemberHandler<DateTime>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DateTimeDateHandler" /> class.
        /// </summary>
        public DateTimeDateHandler()
            : base(x => x.Date)
        {
        }

        /// <inheritdoc />
        protected override IProjection Project
            (
            MemberExpression expression,
            IProjection memberOwnerProjection,
            HelperContext context
            )
        {
            return new FqCastProjection(NHibernateUtil.Date, memberOwnerProjection);
        }
    }
}