using System;
using NHibernate.FlowQuery.ExtensionHelpers;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.ExtensionHelpers
{
    using System.Collections.Generic;
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class PropertyExtensionsTest : BaseTest
    {
        #region Methods (3)

        [Test]
        public void CanExposePropertyUsingAs()
        {
            List<long> ids = Query<UserEntity>()
                .Where(u => "u.Username".As<string>() == "Wimpy")
                .Select(u => u.Id);

            Assert.That(ids.Count, Is.EqualTo(1));
            Assert.That(ids[0], Is.EqualTo(1));
        }

        [Test]
        public void CanExposePropertyUsingAsAndPerformIsHelperExtensionOnResult()
        {
            List<string> names = Query<UserEntity>()
                .Where(u => "u.Username".As<string>().IsLike("%m%"))
                .Select(u => u.Username);

            Assert.That(names.Count, Is.EqualTo(3));
            foreach (var name in names)
            {
                Assert.That(name.Contains("m"));
            }
        }

        [Test]
        public void ThrowsWhenCalledOutsideOfLambdaExpression()
        {
            Assert.That(() => { "".As<string>(); }, Throws.InstanceOf<InvalidOperationException>());
        }

        #endregion Methods
    }
}