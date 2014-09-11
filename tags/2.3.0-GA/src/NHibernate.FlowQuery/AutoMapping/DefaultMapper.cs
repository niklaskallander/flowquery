namespace NHibernate.FlowQuery.AutoMapping
{
    using System;

    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     The default mapper used by FlowQuery.
    /// </summary>
    public class DefaultMapper : IMapper
    {
        /// <inheritdoc />
        public virtual TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            string[] properties = ReflectionHelper
                .GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TDestination>();

            Type destinationType = typeof(TDestination);
            Type sourceType = typeof(TSource);

            var item = new TDestination();

            foreach (string property in properties)
            {
                object value = sourceType.GetProperty(property).GetValue(source, null);

                destinationType.GetProperty(property).SetValue(item, value, null);
            }

            return item;
        }
    }
}