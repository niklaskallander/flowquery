using System;
using System.Collections.Generic;
using NHibernate.FlowQuery.Core;
using NUnit.Framework;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core
{
    using Is = NUnit.Framework.Is;

    [TestFixture]
    public class FlowQuerySelectionTest
    {
        #region Methods (5)

        [Test]
        public void CanImplicitlyCastToArray()
        {
            int[] array = new FlowQuerySelection<int>(new int[] { 1, 2, 3, 4, 5 });

            Assert.That(array.Length, Is.EqualTo(5));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(array[i], Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void CanImplicitlyCastToSingleTSourceItem()
        {
            int singleItem = new FlowQuerySelection<int>(new int[] { 7, 2, 3, 4, 5 });

            Assert.That(singleItem, Is.EqualTo(7));
        }

        [Test]
        public void ImplicitOperatorToSingleItemReturnsDefaultValuesIfSelectionIsNullOrEmpty()
        {
            FlowQuerySelection<int> selection = null;

            int singleItem = selection;

            Assert.That(singleItem, Is.EqualTo(default(int)));

            selection = new FlowQuerySelection<int>(new int[0]);

            singleItem = selection;

            Assert.That(singleItem, Is.EqualTo(default(int)));
        }

        [Test]
        public void CanImplicitlyCastToList()
        {
            List<int> list = new FlowQuerySelection<int>(new int[] { 1, 2, 3, 4, 5 });

            Assert.That(list.Count, Is.EqualTo(5));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(list[i], Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void ConstructorThrowsIfSelectionIsNull()
        {
            Assert.That(() =>
                        {
                            new FlowQuerySelection<object>((IEnumerable<object>)null);

                        }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorThrowsIfDelayedSelectionIsNull()
        {
            Assert.That(() =>
            {
                new FlowQuerySelection<object>((Func<IEnumerable<object>>)null);

            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void ImplicitOperatorToArrayReturnsNullIfSelectionIsNull()
        {
            FlowQuerySelection<int> selection = null;

            int[] array = selection;

            Assert.That(array, Is.Null);
        }

        [Test]
        public void ImplicitOperatorToListReturnsNullIfSelectionIsNull()
        {
            FlowQuerySelection<int> selection = null;

            List<int> list = selection;

            Assert.That(list, Is.Null);
        }

        #endregion Methods
    }
}