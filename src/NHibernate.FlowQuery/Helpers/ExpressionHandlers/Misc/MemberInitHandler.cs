namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection and construction of <see cref="MemberInitExpression"/> expressions.
    /// </summary>
    public class MemberInitHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleConstructionOf(Expression expression)
        {
            return expression.NodeType == ExpressionType.MemberInit;
        }

        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression.NodeType == ExpressionType.MemberInit;
        }

        /// <inheritdoc />
        public override int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            var memberInit = (MemberInitExpression)expression;

            int i = ConstructionHelper.Invoke(memberInit.NewExpression, arguments, out value);

            foreach (MemberBinding binding in memberInit.Bindings)
            {
                var memberAssignment = binding as MemberAssignment;

                if (memberAssignment != null)
                {
                    object temp;

                    i += ConstructionHelper.Invoke(memberAssignment.Expression, arguments.Skip(i).ToArray(), out temp);

                    SetValue(binding.Member, value, temp);
                }
            }

            wasHandled = true;

            return i;
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var memberInit = (MemberInitExpression)expression;

            var list = Projections.ProjectionList();

            var newProjection = ProjectionHelper.GetProjection(memberInit.NewExpression, context)
                as ProjectionList;

            if (newProjection != null)
            {
                for (int i = 0; i < newProjection.Length; i++)
                {
                    list.Add(newProjection[i]);
                }
            }

            foreach (MemberBinding memberBinding in memberInit.Bindings)
            {
                var memberAssigment = memberBinding as MemberAssignment;

                if (memberAssigment != null)
                {
                    IProjection projection = ProjectionHelper.GetProjection(memberAssigment.Expression, context);

                    var innerList = projection as ProjectionList;

                    if (innerList != null)
                    {
                        for (int i = 0; i < innerList.Length; i++)
                        {
                            list.Add(innerList[i]);
                        }
                    }
                    else
                    {
                        string member = memberAssigment.Member.Name;

                        list.Add(new FqAliasProjection(projection, member));

                        if (!context.Data.Mappings.ContainsKey(member))
                        {
                            context.Data.Mappings.Add(member, projection);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        ///     Sets the value of the <see cref="MemberInfo" /> member on <paramref name="instance" /> to
        ///     <paramref name="value" />.
        /// </summary>
        /// <param name="memberInfo">
        ///     The <see cref="MemberInfo" /> member to set.
        /// </param>
        /// <param name="instance">
        ///     The instance to update.
        /// </param>
        /// <param name="value">
        ///     The value to use.
        /// </param>
        /// <remarks>
        ///     Borrowed from Linq to NHibernate.
        /// </remarks>
        private static void SetValue
            (
            MemberInfo memberInfo,
            object instance,
            object value
            )
        {
            var field = memberInfo as FieldInfo;

            if (field != null)
            {
                field.SetValue(instance, value);
            }
            else
            {
                var prop = memberInfo as PropertyInfo;

                if (prop != null)
                {
                    prop.SetValue(instance, value, null);
                }
            }
        }
    }
}