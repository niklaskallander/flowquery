namespace NHibernate.FlowQuery
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A static utility class providing default revealing functionality as defined by <see cref="IRevealer" /> and
    ///     <see cref="IRevealer{T}" />.
    /// </summary>
    public static class Reveal
    {
        /// <summary>
        ///     Gets the default <see cref="IRevealConvention" /> instance.
        /// </summary>
        /// <value>
        ///     The default <see cref="IRevealConvention" /> instance.
        /// </value>
        public static IRevealConvention DefaultConvention { get; private set; }

        /// <summary>
        ///     Reveals any hidden members using the provided expression.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        public static string ByConvention(Expression<Func<object>> expression)
        {
            if (DefaultConvention != null)
            {
                return ByConvention(expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal(expression);
        }

        /// <summary>
        ///     Reveals any hidden members using the provided expression.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        public static string ByConvention<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (DefaultConvention != null)
            {
                return ByConvention(expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal(expression);
        }

        /// <summary>
        ///     Reveals any hidden members using the provided expression and revealing convention.
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
        public static string ByConvention(Expression<Func<object>> expression, IRevealConvention convention)
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal(expression);
        }

        /// <summary>
        ///     Reveals any hidden members using the provided expression and revealing convention.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="convention">
        ///     The revealing convention.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        public static string ByConvention<TEntity>
            (
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            )
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal(expression);
        }

        /// <summary>
        ///     Reveals any hidden members using the provided expression.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        public static string ByConvention<TEntity>
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression
            )
        {
            if (DefaultConvention != null)
            {
                return ByConvention(alias, expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal(alias, expression);
        }

        /// <summary>
        ///     Reveals any hidden members using the provided expression and revealing convention.
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
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The hidden member name.
        /// </returns>
        public static string ByConvention<TEntity>
            (
            Expression<Func<TEntity>> alias,
            Expression<Func<TEntity, object>> expression,
            IRevealConvention convention
            )
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal(alias, expression);
        }

        /// <summary>
        ///     Removes the default <see cref="IRevealConvention" /> instance (<seealso cref="DefaultConvention" />).
        /// </summary>
        public static void ClearDefaultConvention()
        {
            DefaultConvention = null;
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer{TEntity}" /> instance using the default
        ///     <see cref="IRevealConvention" /> (<seealso cref="DefaultConvention" />).
        /// </summary>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IRevealer{TEntity}" /> instance.
        /// </returns>
        public static IRevealer<TEntity> CreateRevealer<TEntity>()
        {
            if (DefaultConvention != null)
            {
                return CreateRevealer<TEntity>(DefaultConvention);
            }

            return new Revealer<TEntity>();
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer" /> instance using the default <see cref="IRevealConvention" />
        ///     (<seealso cref="DefaultConvention" />).
        /// </summary>
        /// <returns>
        ///     The created <see cref="IRevealer" /> instance.
        /// </returns>
        public static IRevealer CreateRevealer()
        {
            if (DefaultConvention != null)
            {
                return CreateRevealer(DefaultConvention);
            }

            return new Revealer();
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer{TEntity}" /> instance using the specified
        ///     <see cref="IRevealConvention" /> instance.
        /// </summary>
        /// <param name="convention">
        ///     The <see cref="IRevealConvention" /> instance.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IRevealer{TEntity}" /> instance.
        /// </returns>
        public static IRevealer<TEntity> CreateRevealer<TEntity>(IRevealConvention convention)
        {
            return new Revealer<TEntity>(convention);
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer" /> instance using the specified <see cref="IRevealConvention" /> 
        ///     instance.
        /// </summary>
        /// <param name="convention">
        ///     The <see cref="IRevealConvention" /> instance.
        /// </param>
        /// <returns>
        ///     The created <see cref="IRevealer" /> instance.
        /// </returns>
        public static IRevealer CreateRevealer(IRevealConvention convention)
        {
            return new Revealer(convention);
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer{TEntity}" /> instance using the specified convention.
        /// </summary>
        /// <param name="convention">
        ///     The convention.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The <see cref="System.Type" /> of the entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IRevealer{TEntity}" /> instance.
        /// </returns>
        public static IRevealer<TEntity> CreateRevealer<TEntity>(Func<string, string> convention)
        {
            return new Revealer<TEntity>(new CustomConvention(convention));
        }

        /// <summary>
        ///     Creates a new <see cref="IRevealer" /> instance using the specified convention.
        /// </summary>
        /// <param name="convention">
        ///     The convention.
        /// </param>
        /// <returns>
        ///     The created <see cref="IRevealer" /> instance.
        /// </returns>
        public static IRevealer CreateRevealer(Func<string, string> convention)
        {
            return new Revealer(new CustomConvention(convention));
        }

        /// <summary>
        ///     Sets the default <see cref="IRevealConvention" /> instance (<seealso cref="DefaultConvention" />).
        /// </summary>
        /// <param name="convention">
        ///     The <see cref="IRevealConvention" /> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convention" /> is null.
        /// </exception>
        public static void SetDefaultConvention(IRevealConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            DefaultConvention = convention;
        }

        /// <summary>
        ///     Sets the default <see cref="IRevealConvention" /> instance (<seealso cref="DefaultConvention" />) using
        ///     the specified convention.
        /// </summary>
        /// <param name="convention">
        ///     The convention.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convention" /> is null.
        /// </exception>
        public static void SetDefaultConvention(Func<string, string> convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            DefaultConvention = new CustomConvention(convention);
        }
    }
}