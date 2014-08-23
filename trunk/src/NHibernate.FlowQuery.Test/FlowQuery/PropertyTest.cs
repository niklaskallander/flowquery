namespace NHibernate.FlowQuery.Test.FlowQuery
{
    using System;

    using NUnit.Framework;

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