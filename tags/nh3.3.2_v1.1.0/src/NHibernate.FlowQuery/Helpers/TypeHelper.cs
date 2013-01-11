using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Type;

namespace NHibernate.FlowQuery.Helpers
{
    public static class TypeHelper
    {
        private static readonly Dictionary<System.Type, IType> m_ClrTypeToNHibernateType = new Dictionary<System.Type, IType>();

        static TypeHelper()
        {
            PropertyInfo[] properties = typeof(TypeHelper).GetProperties();

            foreach (PropertyInfo info in properties)
            {
                if (typeof(IType).IsAssignableFrom(info.PropertyType) == false)
                {
                    continue;
                }

                IType type = (IType)info.GetValue(null, null);

                m_ClrTypeToNHibernateType[type.ReturnedClass] = type;
            }
        }

        public static IType GuessType(System.Type type)
        {
            if (type.IsGenericType && typeof(Nullable<>).Equals(type.GetGenericTypeDefinition()))
            {
                type = type.GetGenericArguments()[0];
            }

            if (m_ClrTypeToNHibernateType.ContainsKey(type))
            {
                return m_ClrTypeToNHibernateType[type];
            }

            return NHibernateUtil.GuessType(type);
        }

        /// <summary>
        /// NHibernate boolean type
        /// </summary>
        public static NullableType Boolean { get { return NHibernateUtil.Boolean; } }

        /// <summary>
        /// NHibernate date type
        /// </summary>
        public static NullableType DateTime { get { return NHibernateUtil.DateTime; } }

        /// <summary>
        /// NHibernate decimal type
        /// </summary>
        public static NullableType Decimal { get { return NHibernateUtil.Decimal; } }

        /// <summary>
        /// NHibernate double type
        /// </summary>
        public static NullableType Double { get { return NHibernateUtil.Double; } }

        /// <summary>
        /// NHibernate Guid type.
        /// </summary>
        public static NullableType Guid { get { return NHibernateUtil.Guid; } }

        /// <summary>
        /// NHibernate System.Int16 (short in C#) type
        /// </summary>
        public static NullableType Int16 { get { return NHibernateUtil.Int16; } }

        /// <summary>
        /// NHibernate System.Int32 (int in C#) type
        /// </summary>
        public static NullableType Int32 { get { return NHibernateUtil.Int32; } }

        /// <summary>
        /// NHibernate System.Int64 (long in C#) type
        /// </summary>
        public static NullableType Int64 { get { return NHibernateUtil.Int64; } }

        /// <summary>
        /// NHibernate System.UInt16 (ushort in C#) type
        /// </summary>
        public static NullableType UInt16 { get { return NHibernateUtil.UInt16; } }

        /// <summary>
        /// NHibernate System.UInt32 (uint in C#) type
        /// </summary>
        public static NullableType UInt32 { get { return NHibernateUtil.UInt32; } }

        /// <summary>
        /// NHibernate System.UInt64 (ulong in C#) type
        /// </summary>
        public static NullableType UInt64 { get { return NHibernateUtil.UInt64; } }

        /// <summary>
        /// NHibernate String type
        /// </summary>
        public static NullableType String { get { return NHibernateUtil.String; } }

        /// <summary>
        /// NHibernate Ticks type
        /// </summary>
        public static NullableType TimeSpan { get { return NHibernateUtil.TimeSpan; } }
    }
}