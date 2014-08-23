namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;

    using NHibernate.Type;

    using NUnit.Framework;

    [TestFixture]
    public class TypeHelperTest
    {
        [Test]
        public void ResolvesNullableTypes()
        {
            IType type = NHibernate.FlowQuery.Helpers.TypeHelper.GuessType(typeof(int?));

            Assert.That(type, Is.Not.Null);
            Assert.That(type.Name, Is.EqualTo("Int32"));
        }

        [Test]
        public void ResortsToNHibernateUtilWhenTypeUnknown()
        {
            IType type = NHibernate.FlowQuery.Helpers.TypeHelper.GuessType(typeof(Exception));

            Assert.That(type, Is.Not.Null);
            Assert.That(type.Name, Is.EqualTo("System.Exception"));
        }
    }
}