using System.Linq.Expressions;
namespace NHibernate.FlowQuery.Helpers
{
    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression m_From;
        private readonly ParameterExpression m_To;

        public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
        {
            m_From = from;
            m_To = to;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == m_From
                ? m_To
                : base.VisitParameter(node);
        }
    }
}