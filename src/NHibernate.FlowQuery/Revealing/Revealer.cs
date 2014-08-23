namespace NHibernate.FlowQuery.Revealing
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A class providing functionality for member revealing.
    /// </summary>
    /// <seealso cref="RevealerBase" />
    /// <seealso cref="Revealer{TEntity}" />
    public class Revealer : RevealerBase, IRevealer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Revealer" /> class.
        /// </summary>
        /// <param name="convention">
        ///     The <see cref="IRevealConvention" /> convention.
        /// </param>
        public Revealer(IRevealConvention convention)
            : base(convention)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Revealer" /> class. Uses the
        ///     <see cref="MUnderscoreConvention" /> for its <see cref="IRevealConvention" />.
        /// </summary>
        public Revealer()
            : base(new MUnderscoreConvention())
        {
        }

        /// <inheritdoc />
        public virtual string Reveal<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return Reveal(expression, RevealConvention);
        }

        /// <inheritdoc />
        public virtual string Reveal<TEntity>
            (
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return Reveal(property, convention);
        }

        /// <inheritdoc />
        public virtual string Reveal<TEntity>
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression
            )
        {
            return Reveal(alias, expression, RevealConvention);
        }

        /// <inheritdoc />
        public virtual string Reveal<TEntity>
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (alias == null)
            {
                throw new ArgumentNullException("alias");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            string aliasName = ExpressionHelper.GetPropertyName(alias.Body);

            return Reveal(aliasName + "." + property, convention);
        }
    }
}