namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Members
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;

    /// <summary>
    ///     Handles projections of <see cref="string.Length" />.
    /// </summary>
    public class StringLengthHandler : AbstractMemberHandler<string>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StringLengthHandler" /> class.
        /// </summary>
        public StringLengthHandler()
            : base(x => x.Length)
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
                    new StandardSQLFunction("len"),
                    NHibernateUtil.Int32,
                    memberOwnerProjection
                );
        }
    }
}