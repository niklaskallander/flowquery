namespace NHibernate.FlowQuery.Expressions
{
    public abstract class SimpleIsExpression : IsExpression
    {
        protected SimpleIsExpression(object value)
        {
            Value = value;
        }

        protected object Value { get; private set; }
    }
}