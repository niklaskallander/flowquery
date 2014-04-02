using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public abstract class RevealerBase : IRevealerBase
    {
        protected RevealerBase(IRevealConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            RevealConvention = convention;
        }

        public virtual IRevealConvention RevealConvention { get; private set; }

        public virtual string Reveal(Expression<Func<object>> expression)
        {
            return Reveal(expression, RevealConvention);
        }

        public virtual string Reveal(string name, IRevealConvention convention)
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

        public virtual string Reveal(Expression<Func<object>> expression, IRevealConvention convention)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body);

            return Reveal(property, convention);
        }
    }
}