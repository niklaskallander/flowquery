namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Members
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from
    ///     <see cref="MemberExpression" />.
    /// </summary>
    /// <typeparam name="TMemberOwner">
    ///     The <see cref="Type" /> of the handled member's owner.
    /// </typeparam>
    /// <remarks>
    ///     The projection implementation defined in this base class will unwrap any given <see cref="Nullable{T}" />
    ///     expressions to make sure that implementing handlers can work with both nullable and non-nullable members
    ///     (for instance <see cref="DateTime" /> and <see cref="Nullable{DateTime}" />).
    /// </remarks>
    public abstract class AbstractMemberHandler<TMemberOwner> : IExpressionHandler
    {
        /// <summary>
        ///     The name of the handled member.
        /// </summary>
        private readonly string _memberName;

        /// <summary>
        ///     The <see cref="Type" /> of the handled member's owner.
        /// </summary>
        private readonly Type _memberOwnerType;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractMemberHandler{TMemberOwner}" /> class.
        /// </summary>
        /// <param name="memberName">
        ///     The name of the handled member.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="memberName" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="memberName" /> is empty or only whitespace.
        /// </exception>
        protected AbstractMemberHandler(string memberName)
        {
            if (memberName == null)
            {
                throw new ArgumentNullException("memberName");
            }

            if (memberName.Trim() == string.Empty)
            {
                throw new ArgumentException("memberName must have a value", "memberName");
            }

            _memberName = memberName.Trim();
            _memberOwnerType = typeof(TMemberOwner);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractMemberHandler{TMemberOwner}" /> class.
        /// </summary>
        /// <param name="memberNameExpression">
        ///     The name of the handled member.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="memberNameExpression" /> is null.
        /// </exception>
        protected AbstractMemberHandler(Expression<Func<TMemberOwner, object>> memberNameExpression)
        {
            if (memberNameExpression == null)
            {
                throw new ArgumentNullException("memberNameExpression");
            }

            _memberName = ExpressionHelper.GetPropertyName(memberNameExpression);
            _memberOwnerType = typeof(TMemberOwner);
        }

        /// <summary>
        ///     Gets a value indicating whether the inheriting class can handle construction.
        /// </summary>
        /// <value>
        ///     Indicates whether the inheriting class can handle construction.
        /// </value>
        protected virtual bool CanHandleConstruction
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the inheriting class can handle projection.
        /// </summary>
        /// <value>
        ///     Indicates whether the inheriting class can handle construction.
        /// </value>
        protected virtual bool CanHandleProjection
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public virtual bool CanHandleConstructionOf(Expression expression)
        {
            return CanHandleConstruction
                && ExpressionIsOfDesiredKind((MemberExpression)expression);
        }

        /// <inheritdoc />
        public virtual bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return CanHandleProjection
                && ExpressionIsOfDesiredKind((MemberExpression)expression);
        }

        /// <inheritdoc />
        public virtual int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            value = null;
            wasHandled = false;

            return 0;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This method will unwrap <see cref="Nullable{T}" /> expressions to make sure that handlers can work with
        ///     both nullable and non-nullable members (e.g. <see cref="DateTime" /> and
        ///     <see cref="Nullable{DateTime}" />).
        /// </remarks>
        public virtual IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var member = (MemberExpression)expression;

            if (member.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var innerMember = (MemberExpression)member.Expression;

                if (innerMember.Member.Name == "Value")
                {
                    Type type = innerMember.Expression.Type;

                    // unwrap if nullable (NHibernate doesn't know about "Value")
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        member = innerMember;
                    }
                }
            }

            IProjection memberOwnerProjection = ProjectionHelper.GetProjection(member.Expression, context);

            return Project(member, memberOwnerProjection, context);
        }

        /// <summary>
        ///     Verifies that the given <see cref="MemberExpression" /> expression is of desired kind.
        /// </summary>
        /// <param name="member">
        ///     The given <see cref="MemberExpression" /> expression.
        /// </param>
        /// <returns>
        ///     If the given <see cref="MemberExpression" /> is of desired kind; return true, otherwise false.
        /// </returns>
        protected virtual bool ExpressionIsOfDesiredKind(MemberExpression member)
        {
            return member.Expression.Type == _memberOwnerType
                && member.Member.Name == _memberName;
        }

        /// <summary>
        ///     The project.
        /// </summary>
        /// <param name="member">
        ///     The member.
        /// </param>
        /// <param name="memberOwnerProjection">
        ///     The member owner projection.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" />.
        /// </returns>
        protected abstract IProjection Project
            (
            MemberExpression member,
            IProjection memberOwnerProjection,
            HelperContext context
            );
    }
}