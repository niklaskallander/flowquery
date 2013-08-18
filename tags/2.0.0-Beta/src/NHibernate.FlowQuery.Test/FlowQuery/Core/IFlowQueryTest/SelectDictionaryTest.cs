using System.Linq;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest
{
    using System.Collections.Generic;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class SelectDictionaryTest : BaseTest
    {
        [Test]
        public virtual void CanSelectToDictionary()
        {
            Dictionary<long, string> userDictionary = Query<UserEntity>()
                .Order.By(x => x.Id)
                .SelectDictionary(x => x.Id, x => x.Username)
                ;

            Assert.That(userDictionary.Count, Is.EqualTo(4));
            Assert.That(userDictionary.ElementAt(0).Key, Is.EqualTo(1));
            Assert.That(userDictionary.ElementAt(1).Key, Is.EqualTo(2));
            Assert.That(userDictionary.ElementAt(2).Key, Is.EqualTo(3));
            Assert.That(userDictionary.ElementAt(3).Key, Is.EqualTo(4));
            Assert.That(userDictionary.ElementAt(0).Value, Is.EqualTo("Wimpy"));
            Assert.That(userDictionary.ElementAt(1).Value, Is.EqualTo("Izmid"));
            Assert.That(userDictionary.ElementAt(2).Value, Is.EqualTo("Empor"));
            Assert.That(userDictionary.ElementAt(3).Value, Is.EqualTo("Lajsa"));
        }
    }
}
