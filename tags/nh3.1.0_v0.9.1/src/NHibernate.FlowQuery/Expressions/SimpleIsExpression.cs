namespace NHibernate.FlowQuery.Expressions
{
    public abstract class SimpleIsExpression : IsExpression
    {
        #region Constructors (1)

        protected SimpleIsExpression(object value)
        {
            Value = value;
        }

        #endregion Constructors

        #region Properties (1)

        protected object Value { get; private set; }

        #endregion Properties
    }
}