namespace NHibernate.FlowQuery.Revealing
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A class providing functionality for member revealing.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The <see cref="System.Type" /> of the entity this <see cref="IRevealer{TEntity}" /> should work with.
    /// </typeparam>
    /// <seealso cref="RevealerBase" />
    /// <seealso cref="Revealer" />
    public class Revealer<TEntity> : Revealer, IRevealer<TEntity>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Revealer{TEntity}"/> class.
        /// </summary>
        /// <param name="convention">
        ///     The convention.
        /// </param>
        public Revealer(IRevealConvention convention)
            : base(convention)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Revealer{TEntity}"/> class. Uses the
        ///     <see cref="MUnderscoreConvention" /> for its <see cref="IRevealConvention" />.
        /// </summary>
        public Revealer()
        {
        }

        /// <inheritdoc />
        public virtual string Reveal(Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(expression, RevealConvention);
        }

        /// <inheritdoc />
        public virtual string Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            return Reveal<TEntity>(expression, convention);
        }

        /// <inheritdoc />
        public virtual string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            return Reveal<TEntity>(alias, expression, RevealConvention);
        }

        /// <inheritdoc />
        public virtual string Reveal
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            )
        {
            return Reveal<TEntity>(alias, expression, convention);
        }
    }
}