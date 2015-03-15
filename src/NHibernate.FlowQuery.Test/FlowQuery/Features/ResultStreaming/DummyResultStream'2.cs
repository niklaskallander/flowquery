namespace NHibernate.FlowQuery.Test.FlowQuery.Features.ResultStreaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;

    public class DummyResultStream<TSource, TDestination> : IResultStream<TDestination>
    {
        private readonly Expression<Func<TSource, TDestination>> _expression;

        private bool _endOfStream;

        public DummyResultStream(Expression<Func<TSource, TDestination>> expression = null)
        {
            Items = new List<TDestination>();

            _expression = expression;
        }

        public Expression<Func<TSource, TDestination>> Expression
        {
            get
            {
                return _expression;
            }
        }

        public List<TDestination> Items { get; private set; }

        public void EndOfStream()
        {
            _endOfStream = true;
        }

        public void Receive(TDestination item)
        {
            if (!_endOfStream)
            {
                Items.Add(item);
            }
        }
    }
}