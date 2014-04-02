using System;
using System.Linq;
using NHibernate.FlowQuery.ExtensionHelpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class PropertyExtensionsTest : BaseTest
    {
        [Test]
        public void CanExposePropertyUsingAs()
        {
            var ids = Query<UserEntity>()
                .Where(u => "u.Username".As<string>() == "Wimpy")
                .Select(u => u.Id)
                ;

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(1));
        }

        [Test]
        public void CanExposePropertyUsingAsAndPerformIsHelperExtensionOnResult()
        {
            var names = Query<UserEntity>()
                .Where(u => "u.Username".As<string>().IsLike("%m%"))
                .Select(u => u.Username)
                ;

            Assert.That(names.ToArray().Length, Is.EqualTo(3));

            foreach (var name in names)
            {
                Assert.That(name.Contains("m"));
            }
        }

        [Test]
        public void ThrowsWhenCalledOutsideOfLambdaExpression()
        {
            Assert.That(() => "".As<string>(), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}