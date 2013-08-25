using System;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Test.Setup;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    public class BaseTest
    {
        protected string[] Firstnames;
        protected string[] Fullnames;
        protected long[] Ids;
        protected string[] Lastnames;
        protected string[] Usernames;

        static BaseTest()
        {
            NHibernateConfigurer.Configure();

            NHibernateConfigurer.AddData();
        }

        private ISessionFactory m_SessionFactory;
        private ISession m_Session;
        private IStatelessSession m_StatelessSession;

        public ISession Session
        {
            get
            {
                if (m_Session == null)
                {
                    m_Session = m_SessionFactory.OpenSession();
                }

                return m_Session;
            }
        }

        public IStatelessSession StatelessSession
        {
            get
            {
                if (m_StatelessSession == null)
                {
                    m_StatelessSession = m_SessionFactory.OpenStatelessSession();
                }

                return m_StatelessSession;
            }
        }

        public IImmediateFlowQuery<TSource> Query<TSource>()
            where TSource : class
        {
            return new Query<TSource>((t, a) => Session.CreateCriteria(t, a));
        }

        [SetUp]
        public virtual void Setup()
        {
            Usernames = new string[] { "Wimpy", "Empor", "Lajsa", "Izmid" };
            Fullnames = new string[] { "Niklas Källander", "Lars Wilk", "Kossan Muu", "Lotta Bråk" };
            Firstnames = new string[] { "Niklas", "Lars", "Kossan", "Lotta" };
            Lastnames = new string[] { "Källander", "Wilk", "Muu", "Bråk" };
            Ids = new long[] { 1, 2, 3, 4 };

            m_SessionFactory = NHibernateConfigurer.GetSessionFactory();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (m_Session != null)
            {
                m_Session.Dispose();
                m_Session = null;
            }

            if (m_StatelessSession != null)
            {
                m_StatelessSession.Dispose();
                m_StatelessSession = null;
            }
        }
    }

    public class Query<TSource> : ImmediateFlowQueryImplementor<TSource>
         where TSource : class
    {
        public Query(Func<System.Type, string, ICriteria> criteriaFactory)
            : base(criteriaFactory, "this")
        { }
    }
}