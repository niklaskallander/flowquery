using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery
{
    public static class Reveal
    {
        #region Properties (1)

        public static IRevealConvention DefaultConvention
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (15)

        public static string ByConvention(Expression<Func<object>> expression)
        {
            if (DefaultConvention != null)
            {
                return ByConvention(expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal(expression);
        }

        public static string ByConvention<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (DefaultConvention != null)
            {
                return ByConvention(expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal<TEntity>(expression);
        }

        public static string ByConvention(Expression<Func<object>> expression, IRevealConvention convention)
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal(expression);
        }

        public static string ByConvention<TEntity>(Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal<TEntity>(expression);
        }

        public static string ByConvention<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression)
        {
            if (DefaultConvention != null)
            {
                return ByConvention(alias, expression, DefaultConvention);
            }

            IRevealer revealer = new Revealer();

            return revealer.Reveal<TEntity>(alias, expression);
        }

        public static string ByConvention<TEntity>(Expression<Func<TEntity>> alias, Expression<Func<TEntity, object>> expression, IRevealConvention convention)
        {
            IRevealer revealer = new Revealer(convention);

            return revealer.Reveal<TEntity>(alias, expression);
        }

        public static void ClearDefaultConvention()
        {
            DefaultConvention = null;
        }

        public static IRevealer<TEntity> CreateRevealer<TEntity>()
        {
            if (DefaultConvention != null)
            {
                return CreateRevealer<TEntity>(DefaultConvention);
            }
            return new Revealer<TEntity>();
        }

        public static IRevealer CreateRevealer()
        {
            if (DefaultConvention != null)
            {
                return CreateRevealer(DefaultConvention);
            }
            return new Revealer();
        }

        public static IRevealer<TEntity> CreateRevealer<TEntity>(IRevealConvention convention)
        {
            return new Revealer<TEntity>(convention);
        }

        public static IRevealer CreateRevealer(IRevealConvention convention)
        {
            return new Revealer(convention);
        }

        public static IRevealer<TEntity> CreateRevealer<TEntity>(Func<string, string> convention)
        {
            return new Revealer<TEntity>(new CustomConvention(convention));
        }

        public static IRevealer CreateRevealer(Func<string, string> convention)
        {
            return new Revealer(new CustomConvention(convention));
        }

        public static void SetDefaultConvention(IRevealConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            DefaultConvention = convention;
        }

        public static void SetDefaultConvention(Func<string, string> convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            DefaultConvention = new CustomConvention(convention);
        }

        #endregion Methods
    }
}