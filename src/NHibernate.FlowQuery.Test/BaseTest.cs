namespace NHibernate.FlowQuery.Test
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Test.Setup;

    using NUnit.Framework;

    public class BaseTest
    {
        public static readonly string[] Firstnames;

        public static readonly string[] Fullnames;

        public static readonly long[] Ids;

        public static readonly string[] Lastnames;

        public static readonly string[] Usernames;

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
                "Niklas Källander", 
                "Lars Wilk", 
                "Kossan Muu", 
                "Lotta Bråk"
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
                "Källander", 
                "Wilk", 
                "Muu", 
                "Bråk"
            };

            Ids = new long[]
            {
                1, 
                2, 
                3, 
                4
            };
        }

        public ISession Session
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

        public ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory;
            }
        }

        public IStatelessSession StatelessSession
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

        public IDetachedFlowQuery<TSource> DetachedDummyQuery<TSource>()
            where TSource : class
        {
            return new DetachedQueryClass<TSource>((x, y) => null);
        }

        public IDetachedFlowQuery<TSource> DetachedQuery<TSource>()
            where TSource : class
        {
            return new DetachedQueryClass<TSource>(Session.CreateCriteria);
        }

        public IImmediateFlowQuery<TSource> DummyQuery<TSource>()
            where TSource : class
        {
            return new QueryClass<TSource>((x, y) => null);
        }

        public virtual void OnSetup()
        {
        }

        public virtual void OnTearDown()
        {
        }

        public IImmediateFlowQuery<TSource> Query<TSource>()
            where TSource : class
        {
            return new QueryClass<TSource>(Session.CreateCriteria);
        }

        [SetUp]
        public virtual void Setup()
        {
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

        public class DetachedQueryClass<TSource> : DetachedFlowQuery<TSource>
            where TSource : class
        {
            public DetachedQueryClass
                (Func<Type, string, ICriteria> criteriaFactory)
                : base(criteriaFactory, "this")
            {
            }
        }

        public class QueryClass<TSource> : ImmediateFlowQuery<TSource>
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