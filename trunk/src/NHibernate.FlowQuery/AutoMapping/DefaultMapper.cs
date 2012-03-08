using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.AutoMapping
{
    public class DefaultMapper : IMapper
    {
        #region Methods (1)

        protected virtual TReturn Map<TSource, TReturn>(TSource source)
            where TReturn : new()
        {
            string[] properties = ReflectionHelper.GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TReturn>();

            System.Type rType = typeof(TReturn);
            System.Type sType = typeof(TSource);

            TReturn item = new TReturn();
            foreach (string property in properties)
            {
                object value = sType.GetProperty(property).GetValue(source, null);

                rType.GetProperty(property).SetValue(item, value, null);
            }

            return item;
        }

        #endregion Methods



        #region IMapper Members

        TReturn IMapper.Map<TSource, TReturn>(TSource source)
        {
            return Map<TSource, TReturn>(source);
        }

        #endregion
    }
}