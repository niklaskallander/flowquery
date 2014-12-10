namespace NHibernate.FlowQuery.Revealing
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A class providing the base functionality and structure for member revealing.
    /// </summary>
    /// <seealso cref="Revealer" />
    /// <seealso cref="Revealer{TEntity}" />
    public abstract class RevealerBase : IRevealerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RevealerBase" /> class.
        /// </summary>
        /// <param name="convention">
        ///     The <see cref="IRevealConvention" /> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convention" /> is null.
        /// </exception>
        protected RevealerBase(IRevealConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            RevealConvention = convention;
        }

        /// <inheritdoc />
        public virtual IRevealConvention RevealConvention { get; private set; }

        /// <inheritdoc />
        public virtual string Reveal(Expression<Func<object>> expression)
        {
            return Reveal(expression, RevealConvention);
        }

        /// <inheritdoc />
        public virtual string Reveal(Expression<Func<object>> expression, IRevealConvention convention)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body);

            return Reveal(property, convention);
        }

        /// <summary>
        ///     Reveals any hidden members using the given property name and revealing convention.
        /// </summary>
        /// <param name="name">
        ///     The property name.
        /// </param>
        /// <param name="convention">
        ///     The convention.
        /// </param>
        /// <returns>
        ///     The name of the hidden member.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="name" /> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convention" /> is null.
        /// </exception>
        protected virtual string Reveal(string name, IRevealConvention convention)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            if (name.Contains("."))
            {
                string[] splits = name.Split('.');

                string temp = string.Empty;

                for (int i = 0; i < splits.Length - 1; i++)
                {
                    temp += splits[i] + ".";
                }

                return temp + convention.RevealFrom(splits[splits.Length - 1]);
            }

            return convention.RevealFrom(name);
        }
    }
}