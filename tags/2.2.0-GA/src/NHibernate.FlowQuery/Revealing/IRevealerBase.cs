namespace NHibernate.FlowQuery.Revealing
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     An interface defining the base functionality and structure required by a member revealer.
    /// </summary>
    /// <seealso cref="IRevealer" />
    /// <seealso cref="IRevealer{TEntity}" />
    public interface IRevealerBase
    {
        /// <summary>
        ///     Gets the <see cref="IRevealConvention" /> convention used by this <see cref="IRevealerBase" /> revealer.
        /// </summary>
        /// <value>
        ///     The <see cref="IRevealConvention" /> convention.
        /// </value>
        /// <seealso cref="Conventions" />
        /// <seealso cref="IRevealConvention" />
        /// <seealso cref="CustomConvention" />
        /// <seealso cref="MConvention" />
        /// <seealso cref="MUnderscoreConvention" />
        /// <seealso cref="UnderscoreConvention" />
        IRevealConvention RevealConvention { get; }

        /// <summary>
        ///     Reveals any hidden members using the given expression.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        string Reveal(Expression<Func<object>> expression);

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
        string Reveal(Expression<Func<object>> expression, IRevealConvention convention);
    }
}