using System;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery
{
    [TestFixture]
    public class PropertyTest
    {
        [Test]
        public void AsThrowsWhenCalledOutsideOfLambdaExpression()
        {
            Assert.That(() => Property.As<string>("Tjoho"), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}