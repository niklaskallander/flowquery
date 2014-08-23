namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using System;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.ExtensionHelpers;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    [TestFixture]
    public class PropertyExtensionsTest : BaseTest
    {
        [Test]
        public void CanExposePropertyUsingAs()
        {
            FlowQuerySelection<long> ids = Query<UserEntity>()
                .Where(u => "u.Username".As<string>() == "Wimpy")
                .Select(u => u.Id);

            Assert.That(ids.ToArray().Length, Is.EqualTo(1));
            Assert.That(ids.First(), Is.EqualTo(1));
        }

        [Test]
        public void CanExposePropertyUsingAsAndPerformIsHelperExtensionOnResult()
        {
            FlowQuerySelection<string> names = Query<UserEntity>()
                .Where(u => "u.Username".As<string>().IsLike("%m%"))
                .Select(u => u.Username);

            Assert.That(names.ToArray().Length, Is.EqualTo(3));

            foreach (string name in names)
            {
                Assert.That(name.Contains("m"));
            }
        }

        [Test]
        public void ThrowsWhenCalledOutsideOfLambdaExpression()
        {
            Assert.That(() => string.Empty.As<string>(), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}