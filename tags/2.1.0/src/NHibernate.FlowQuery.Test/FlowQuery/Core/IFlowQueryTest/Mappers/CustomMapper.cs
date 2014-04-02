using System;
using System.Collections.Generic;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers
{
    using Type = System.Type;

    public class CustomMapper : DefaultMapper
    {
        private readonly Dictionary<Type, Dictionary<Type, Delegate>> _maps;

        public CustomMapper()
        {
            _maps = new Dictionary<Type, Dictionary<Type, Delegate>>();
        }

        public virtual void AddMap<TSource, TDestination>(Func<TSource, TDestination> mappingDelegate)
            where TDestination : new()
        {
            if (!_maps.ContainsKey(typeof(TSource)))
            {
                _maps.Add(typeof(TSource), new Dictionary<Type, Delegate>());
            }

            _maps[typeof(TSource)].Add(typeof(TDestination), mappingDelegate);
        }

        protected override TDestination Map<TSource, TDestination>(TSource source)
        {
            var mappingDelegate = (Func<TSource, TDestination>)_maps[typeof(TSource)][typeof(TDestination)];

            return mappingDelegate(source);
        }
    }
}