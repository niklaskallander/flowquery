namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using System;
    using System.Collections.Generic;

    using NHibernate.FlowQuery.Core;

    using NUnit.Framework;

    [TestFixture]
    public class FlowQuerySelectionTest
    {
        [Test]
        public void CanImplicitlyCastToArray()
        {
            int[] array = new FlowQuerySelection<int>(new[] { 1, 2, 3, 4, 5 });

            Assert.That(array.Length, Is.EqualTo(5));

            for (int i = 0; i < 5; i++)
            {
                Assert.That(array[i], Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void CanImplicitlyCastToList()
        {
            List<int> list = new FlowQuerySelection<int>(new[] { 1, 2, 3, 4, 5 });

            Assert.That(list.Count, Is.EqualTo(5));

            for (int i = 0; i < 5; i++)
            {
                Assert.That(list[i], Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void CanImplicitlyCastToSingleTSourceItem()
        {
            int singleItem = new FlowQuerySelection<int>(new[] { 7, 2, 3, 4, 5 });

            Assert.That(singleItem, Is.EqualTo(7));
        }

        [Test]
        public void ConstructorThrowsIfDelayedSelectionIsNull()
        {
            Assert
                .That
                (
                    () => new FlowQuerySelection<object>((Func<IEnumerable<object>>)null), 
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ConstructorThrowsIfSelectionIsNull()
        {
            Assert
                .That
                (
                    () => new FlowQuerySelection<object>((IEnumerable<object>)null), 
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void ImplicitOperatorToArrayReturnsNullIfSelectionIsNull()
        {
            int[] array = (FlowQuerySelection<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(array, Is.Null);
        }

        [Test]
        public void ImplicitOperatorToListReturnsNullIfSelectionIsNull()
        {
            List<int> list = (FlowQuerySelection<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.That(list, Is.Null);
        }

        [Test]
        public void ImplicitOperatorToSingleItemReturnsDefaultValuesIfSelectionIsNullOrEmpty()
        {
            FlowQuerySelection<int> selection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            int singleItem = selection;

            Assert.That(singleItem, Is.EqualTo(0));

            selection = new FlowQuerySelection<int>(new int[0]);

            singleItem = selection;

            Assert.That(singleItem, Is.EqualTo(0));
        }
    }
}