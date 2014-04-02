using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.AutoMapping
{
    public class DefaultMapper : IMapper
    {
        protected virtual TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            string[] properties = ReflectionHelper.GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TDestination>();

            System.Type rType = typeof(TDestination);
            System.Type sType = typeof(TSource);

            TDestination item = new TDestination();

            foreach (string property in properties)
            {
                object value = sType.GetProperty(property).GetValue(source, null);

                rType.GetProperty(property).SetValue(item, value, null);
            }

            return item;
        }



        #region IMapper Members

        TDestination IMapper.Map<TSource, TDestination>(TSource source)
        {
            return Map<TSource, TDestination>(source);
        }

        #endregion
    }
}