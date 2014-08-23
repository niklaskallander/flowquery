namespace NHibernate.FlowQuery.Revealing
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     An interface defining the functionality required by a member revealer.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The <see cref="System.Type" /> of the entity this <see cref="IRevealer{TEntity}" /> should work with.
    /// </typeparam>
    /// <seealso cref="IRevealerBase" />
    /// <seealso cref="IRevealer" />
    public interface IRevealer<TEntity> : IRevealerBase
    {
        /// <summary>
        ///     Reveals any hidden members using the given expression.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        string Reveal(Expression<Func<TEntity, object>> expression);

        /// <summary>
        ///     Reveals any hidden members using the given expression.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        string Reveal(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression);

        /// <summary>
        ///     Reveals any hidden members using the given expression and revealing convention.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="convention">
        ///     The revealing convention.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        string Reveal(Expression<Func<TEntity, object>> expression, IRevealConvention convention);

        /// <summary>
        ///     Reveals any hidden members using the given expression and revealing convention.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="convention">
        ///     The revealing convention.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        string Reveal
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            );
    }
}