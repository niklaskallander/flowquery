namespace NHibernate.FlowQuery.Core
{
    public class Pair<TKey, TValue>
    {
        public virtual TKey Key { get; set; }

        public virtual TValue Value { get; set; }
    }
}