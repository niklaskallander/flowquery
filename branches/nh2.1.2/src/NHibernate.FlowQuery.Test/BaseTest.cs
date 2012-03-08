using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    public class BaseTest
    {
        #region Fields (6)

        protected string[] Firstnames;
        protected string[] Fullnames;
        protected long[] Ids;
        protected string[] Lastnames;
        protected string[] Usernames;

        #endregion Fields

        #region Constructors (1)

        static BaseTest()
        {
            NHibernateConfigurer.Configure();
            NHibernateConfigurer.AddData();
        }

        #endregion Constructors

        #region Properties (1)

        public ISession Session { get; private set; }

        #endregion Properties

        #region Methods (3)

        public IFlowQuery<T> Query<T>() where T : class
        {
            return Session.FlowQuery<T>();
        }

        [SetUp]
        public virtual void Setup()
        {
            Usernames = new string[] { "Wimpy", "Empor", "Lajsa", "Izmid" };
            Fullnames = new string[] { "Niklas Källander", "Lars Wilk", "Kossan Muu", "Lotta Bråk" };
            Firstnames = new string[] { "Niklas", "Lars", "Kossan", "Lotta" };
            Lastnames = new string[] { "Källander", "Wilk", "Muu", "Bråk" };
            Ids = new long[] { 1, 2, 3, 4 };

            Session = NHibernateConfigurer.GetSessionFactory().OpenSession();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Session.Dispose();
            Session = null;
        }

        #endregion Methods
    }
}