namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Members
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     Handles projections of <see cref="DateTime.Day" />.
    /// </summary>
    public class DateTimeDayHandler : AbstractMemberHandler<DateTime>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DateTimeDayHandler" /> class.
        /// </summary>
        public DateTimeDayHandler()
            : base(x => x.Day)
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
            return Projections
                .SqlFunction
                (
                    "day",
                    NHibernateUtil.Int32,
                    memberOwnerProjection
                );
        }
    }
}