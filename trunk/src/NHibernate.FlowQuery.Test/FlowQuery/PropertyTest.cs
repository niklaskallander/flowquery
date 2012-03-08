using System;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test
{
    [TestFixture]
    public class PropertyTest
    {
		#region Methods (1) 

        [Test]
        public void AsThrowsWhenCalledOutsideOfLambdaExpression()
        {
            Assert.That(() => { Property.As<string>("Tjoho"); }, Throws.InstanceOf<InvalidOperationException>());
        }

		#endregion Methods 
    }
}