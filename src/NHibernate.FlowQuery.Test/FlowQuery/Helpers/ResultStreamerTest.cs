namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;

    using Moq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Helpers;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    [TestFixture]
    public class ResultStreamerTest
    {
        private static InstanceOfTypeConstraint ThrowsNotImplemented
        {
            get
            {
                return Throws.InstanceOf<NotImplementedException>();
            }
        }

        private static InstanceOfTypeConstraint ThrowsArgumentNull
        {
            get
            {
                return Throws.InstanceOf<ArgumentNullException>();
            }
        }

        [Test]
        public void ThrowsIfResultStreamIsNull()
        {
            Assert.That(() => new ResultStreamer<int>(CreateResultStream<int>().Object, null), ThrowsArgumentNull);
        }

        [Test]
        public void ThrowsIfConverterIsNull()
        {
            Assert.That(() => new ResultStreamer<int>(null, x => (int)x), ThrowsArgumentNull);
        }

        [Test]
        public void AddShouldSendConvertedItemsToResultStreamsReceiveMethod()
        {
            Mock<IResultStream<int>> stream;

            ResultStreamer<int> streamer = CreateStreamer(out stream);

            stream.Setup(x => x.Receive(1));
            stream.Setup(x => x.Receive(2));
            stream.Setup(x => x.Receive(3));

            streamer.Add(1);
            streamer.Add(2);
            streamer.Add(3);

            stream.VerifyAll();
        }

        [Test]
        public void ClearShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(streamer.Clear, ThrowsNotImplemented);
        }

        [Test]
        public void ContainsShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.Contains(1), ThrowsNotImplemented);
        }

        [Test]
        public void CopyToShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.CopyTo(new[] { 1 }, 1), ThrowsNotImplemented);
        }

        [Test]
        public void CountShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.Count, ThrowsNotImplemented);
        }

        [Test]
        public void GetEnumeratorShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.GetEnumerator(), ThrowsNotImplemented);
        }

        [Test]
        public void IndexOfShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.IndexOf(1), ThrowsNotImplemented);
        }

        [Test]
        public void IndexerShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer[1], ThrowsNotImplemented);
            Assert.That(() => streamer[1] = 1, ThrowsNotImplemented);
        }

        [Test]
        public void InsertShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.Insert(1, 1), ThrowsNotImplemented);
        }

        [Test]
        public void IsFixedSizeShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.IsFixedSize, ThrowsNotImplemented);
        }

        [Test]
        public void IsReadOnlyShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.IsReadOnly, ThrowsNotImplemented);
        }

        [Test]
        public void IsSynchronizedShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.IsSynchronized, ThrowsNotImplemented);
        }

        [Test]
        public void RemoveAtShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.RemoveAt(1), ThrowsNotImplemented);
        }

        [Test]
        public void RemoveShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.Remove(1), ThrowsNotImplemented);
        }

        [Test]
        public void SyncRootShouldNotBeImplemented()
        {
            ResultStreamer<int> streamer = CreateStreamer<int>();

            Assert.That(() => streamer.SyncRoot, ThrowsNotImplemented);
        }

        private static Mock<IResultStream<T>> CreateResultStream<T>()
        {
            return new Mock<IResultStream<T>>();
        }

        private static ResultStreamer<T> CreateStreamer<T>
            (
            out Mock<IResultStream<T>> resultStream,
            Func<object, T> converter = null
            )
        {
            resultStream = CreateResultStream<T>();

            return new ResultStreamer<T>(resultStream.Object, converter ?? (x => (T)x));
        }

        private static ResultStreamer<T> CreateStreamer<T>()
        {
            Mock<IResultStream<T>> resultStream;

            return CreateStreamer(out resultStream);
        }
    }
}