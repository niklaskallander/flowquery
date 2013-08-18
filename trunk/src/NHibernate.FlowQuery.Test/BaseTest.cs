using NHibernate.FlowQuery.Core;
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

        public ISession Session { get; private set; }

        public IStatelessSession StatelessSession { get; private set; }

        public IImmediateFlowQuery<T> Query<T>() where T : class
        {
            return Session.FlowQuery<T>();
        }

        [SetUp]
        public virtual void Setup()
        {
            Usernames = new string[] { "Wimpy", "Empor", "Lajsa", "Izmid" };
            Fullnames = new string[] { "Niklas K�llander", "Lars Wilk", "Kossan Muu", "Lotta Br�k" };
            Firstnames = new string[] { "Niklas", "Lars", "Kossan", "Lotta" };
            Lastnames = new string[] { "K�llander", "Wilk", "Muu", "Br�k" };
            Ids = new long[] { 1, 2, 3, 4 };

            Session = NHibernateConfigurer.GetSessionFactory().OpenSession();
            StatelessSession = NHibernateConfigurer.GetSessionFactory().OpenStatelessSession();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Session.Dispose();
            Session = null;

            StatelessSession.Dispose();
            StatelessSession = null;
        }
    }
}