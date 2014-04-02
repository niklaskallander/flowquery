using System;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Test.Setup;
using NHibernate.Metadata;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    public class BaseTest
    {
        public static readonly string[] Firstnames;
        public static readonly string[] Fullnames;
        public static readonly long[] Ids;
        public static readonly string[] Lastnames;
        public static readonly string[] Usernames;

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

        private ISessionFactory _sessionFactory;
        private ISession _session;
        private IStatelessSession _statelessSession;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
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

        public IDetachedFlowQuery<TSource> DetachedQuery<TSource>()
            where TSource : class
        {
            return new DetachedQuery<TSource>(Session.CreateCriteria, Session.SessionFactory.GetClassMetadata);
        }

        public IDetachedFlowQuery<TSource> DetachedDummyQuery<TSource>()
            where TSource : class
        {
            return new DetachedQuery<TSource>((x, y) => null, x => null);
        }

        public IImmediateFlowQuery<TSource> Query<TSource>()
            where TSource : class
        {
            return new Query<TSource>(Session.CreateCriteria, Session.SessionFactory.GetClassMetadata);
        }

        public IImmediateFlowQuery<TSource> DummyQuery<TSource>()
            where TSource : class
        {
            return new Query<TSource>((x, y) => null, x => null);
        }

        public virtual void OnSetup()
        {
            
        }

        [SetUp]
        public virtual void Setup()
        {
            _sessionFactory = NHibernateConfigurer.GetSessionFactory();

            OnSetup();
        }

        public virtual void OnTearDown()
        {

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
    }

    public class Query<TSource> : ImmediateFlowQueryImplementor<TSource>
         where TSource : class
    {
        public Query(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory)
            : base(criteriaFactory, metaDataFactory, "this")
        { }
    }

    public class DetachedQuery<TSource> : DetachedFlowQueryImplementor<TSource>
         where TSource : class
    {
        public DetachedQuery(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory)
            : base(criteriaFactory, metaDataFactory, "this")
        { }
    }
}