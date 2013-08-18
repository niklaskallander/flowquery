using System;
using System.Collections;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.ExtensionHelpers
{
    public static class IsExtensions
    {
        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        public static bool IsBetween<TProperty>(this TProperty property, TProperty lowValue, TProperty highValue)
        {
            throw Exception();
        }

        public static bool IsEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsGreaterThan<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsGreaterThanOrEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsIn<TProperty>(this TProperty property, IDetachedImmutableFlowQuery query)
        {
            throw Exception();
        }

        public static bool IsIn<TProperty>(this TProperty property, params TProperty[] values)
        {
            throw Exception();
        }

        public static bool IsIn<TProperty>(this TProperty property, IEnumerable value)
        {
            throw Exception();
        }

        public static bool IsLessThan<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsLessThanOrEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsLike<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        public static bool IsNotNull<TProperty>(this TProperty property)
        {
            throw Exception();
        }

        public static bool IsNull<TProperty>(this TProperty property)
        {
            throw Exception();
        }
    }
}