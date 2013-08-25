using System.Linq;
using NHibernate.Engine;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Documentation
{
    using xIs = NUnit.Framework.Is;

    [TestFixture]
    public class InterchangeabilityTest : BaseTest
    {
        [Test]
        public void DefinitionExample1()
        {
            ISession session = Session;

            var query = session.FlowQuery<UserEntity>()
                .Delayed()
                .Where(x => x.IsOnline);

            var count = query.Count();

            var users = query
                .Take(10)
                .Select();

            ISessionImplementor sessionImpl = Session as ISessionImplementor;

            Assert.That(sessionImpl, xIs.Not.Null);

            int queryCount = 0;

            Assert.That(() =>
                        {
                            queryCount = sessionImpl.FutureCriteriaBatch.Results.Count;
                        },

                        Throws.Nothing);

            Assert.That(queryCount, xIs.EqualTo(2));

            Assert.That(count.Value, xIs.EqualTo(3));
            Assert.That(users.Count(), xIs.EqualTo(3));
        }
    }
}
