namespace NHibernate.FlowQuery.Test
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup;

    using NUnit.Framework;

    public class BaseTest
    {
        protected static readonly string[] Firstnames;

        protected static readonly string[] Fullnames;

        protected static readonly long[] Ids;

        protected static readonly string[] Lastnames;

        protected static readonly string[] Usernames;

        private ISession _session;

        private ISessionFactory _sessionFactory;

        private IStatelessSession _statelessSession;

        static BaseTest()
        {
            NHibernateConfigurer.Configure();

            Usernames = new[]
            {
                "Wimpy", 
                "Empor", 
                "Lajsa", 
                "Izmid"
            };

            Fullnames = new[]
            {
                "Niklas Kallander", 
                "Lars Wilk", 
                "Kossan Muu", 
                "Lotta Brak"
            };

            Firstnames = new[]
            {
                "Niklas", 
                "Lars", 
                "Kossan", 
                "Lotta"
            };

            Lastnames = new[]
            {
                "Kallander", 
                "Wilk", 
                "Muu", 
                "Brak"
            };

            Ids = new long[]
            {
                1, 
                2, 
                3, 
                4
            };
        }

        protected ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = _sessionFactory.OpenSession();
                }

                return _session;
            }
        }

        protected ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory;
            }
        }

        protected IStatelessSession StatelessSession
        {
            get
            {
                if (_statelessSession == null)
                {
                    _statelessSession = _sessionFactory.OpenStatelessSession();
                }

                return _statelessSession;
            }
        }

        [SetUp]
        public virtual void Setup()
        {
            // to always start on a clean slate
            FlowQueryHelper.ClearExpressionHandlers();

            _sessionFactory = NHibernateConfigurer.GetSessionFactory();

            OnSetup();
        }

        [TearDown]
        public virtual void TearDown()
        {
            OnTearDown();

            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }

            if (_statelessSession != null)
            {
                _statelessSession.Dispose();
                _statelessSession = null;
            }
        }

        protected static IDetachedFlowQuery<TSource> DetachedDummyQuery<TSource>()
            where TSource : class
        {
            return new DetachedQueryClass<TSource>((x,
                y) => null);
        }

        protected IDetachedFlowQuery<TSource> DetachedQuery<TSource>()
            where TSource : class
        {
            return new DetachedQueryClass<TSource>(Session.CreateCriteria);
        }

        protected IImmediateFlowQuery<TSource> DummyQuery<TSource>()
            where TSource : class
        {
            return new QueryClass<TSource>((x,
                y) => null);
        }

        protected virtual void OnSetup()
        {
        }

        protected virtual void OnTearDown()
        {
        }

        protected IImmediateFlowQuery<TSource> Query<TSource>()
            where TSource : class
        {
            return new QueryClass<TSource>(Session.CreateCriteria);
        }

        private class DetachedQueryClass<TSource> : DetachedFlowQuery<TSource>
            where TSource : class
        {
            public DetachedQueryClass
                (Func<Type, string, ICriteria> criteriaFactory)
                : base(criteriaFactory, "this")
            {
            }
        }

        private class QueryClass<TSource> : ImmediateFlowQuery<TSource>
            where TSource : class
        {
            public QueryClass
                (Func<Type, string, ICriteria> criteriaFactory)
                : base(criteriaFactory, "this")
            {
            }
        }
    }
}