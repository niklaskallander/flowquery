using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    [TestFixture]
    public class SessionExtensionTest : BaseTest
    {
		#region Methods (2) 

        [Test]
        public void CanCreateFlowQuery()
        {
            Assert.That(() => { Session.FlowQuery<UserEntity>(); }, Throws.Nothing);

            IFlowQuery<UserEntity> fq = Session.FlowQuery<UserEntity>();

            Assert.That(fq != null);
        }

        [Test]
        public void CanCreateFlowQueryWithOptions()
        {
            Assert.That(() => { Session.FlowQuery<UserEntity>(new FlowQueryOptions()); }, Throws.Nothing);

            IFlowQuery<UserEntity> fq = Session.FlowQuery<UserEntity>(new FlowQueryOptions());

            Assert.That(fq != null);
        }

		#endregion Methods 
    }
}