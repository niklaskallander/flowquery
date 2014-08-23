namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using NHibernate.Type;

    /// <summary>
    ///     A static utility class providing methods to resolve <see cref="NHibernate.Type.IType" />
    ///     representations from <see cref="System.Type" /> instances.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        ///     Type cache.
        /// </summary>
        private static readonly Dictionary<Type, IType> ClrTypeToNHibernateType = new Dictionary<Type, IType>();

        /// <summary>
        ///     Initializes static members of the <see cref="TypeHelper" /> class.
        /// </summary>
        static TypeHelper()
        {
            PropertyInfo[] properties = typeof(TypeHelper).GetProperties();

            foreach (PropertyInfo info in properties)
            {
                bool isAssignable = typeof(IType)
                    .IsAssignableFrom(info.PropertyType);

                if (isAssignable)
                {
                    var type = (IType)info.GetValue(null, null);

                    ClrTypeToNHibernateType[type.ReturnedClass] = type;
                }
            }
        }

        /// <summary>
        ///     Gets the NHibernate boolean type.
        /// </summary>
        /// <value>
        ///     The NHibernate boolean type.
        /// </value>
        public static NullableType Boolean
        {
            get
            {
                return NHibernateUtil.Boolean;
            }
        }

        /// <summary>
        ///     Gets the NHibernate date + time type.
        /// </summary>
        /// <value>
        ///     The NHibernate date + time type.
        /// </value>
        public static NullableType DateTime
        {
            get
            {
                return NHibernateUtil.DateTime;
            }
        }

        /// <summary>
        ///     Gets the NHibernate decimal type.
        /// </summary>
        /// <value>
        ///     The NHibernate decimal type.
        /// </value>
        public static NullableType Decimal
        {
            get
            {
                return NHibernateUtil.Decimal;
            }
        }

        /// <summary>
        ///     Gets the NHibernate double type.
        /// </summary>
        /// <value>
        ///     The NHibernate double type.
        /// </value>
        public static NullableType Double
        {
            get
            {
                return NHibernateUtil.Double;
            }
        }

        /// <summary>
        ///     Gets the NHibernate UUID type.
        /// </summary>
        /// <value>
        ///     The NHibernate UUID type.
        /// </value>
        public static NullableType Guid
        {
            get
            {
                return NHibernateUtil.Guid;
            }
        }

        /// <summary>
        ///     Gets the NHibernate short type.
        /// </summary>
        /// <value>
        ///     The NHibernate short type.
        /// </value>
        public static NullableType Int16
        {
            get
            {
                return NHibernateUtil.Int16;
            }
        }

        /// <summary>
        ///     Gets the NHibernate integer type.
        /// </summary>
        /// <value>
        ///     The NHibernate integer type.
        /// </value>
        public static NullableType Int32
        {
            get
            {
                return NHibernateUtil.Int32;
            }
        }

        /// <summary>
        ///     Gets the NHibernate long type.
        /// </summary>
        /// <value>
        ///     The NHibernate long type.
        /// </value>
        public static NullableType Int64
        {
            get
            {
                return NHibernateUtil.Int64;
            }
        }

        /// <summary>
        /// Gets the NHibernate string type.
        /// </summary>
        /// <value>
        /// The NHibernate string type.
        /// </value>
        public static NullableType String
        {
            get
            {
                return NHibernateUtil.String;
            }
        }

        /// <summary>
        /// Gets the NHibernate time type.
        /// </summary>
        /// <value>
        /// The NHibernate time type.
        /// </value>
        public static NullableType TimeSpan
        {
            get
            {
                return NHibernateUtil.TimeSpan;
            }
        }

        /// <summary>
        /// Gets the NHibernate unsigned short type.
        /// </summary>
        /// <value>
        /// The NHibernate unsigned short type.
        /// </value>
        public static NullableType UInt16
        {
            get
            {
                return NHibernateUtil.UInt16;
            }
        }

        /// <summary>
        /// Gets the NHibernate unsigned integer type.
        /// </summary>
        /// <value>
        /// The NHibernate unsigned integer type.
        /// </value>
        public static NullableType UInt32
        {
            get
            {
                return NHibernateUtil.UInt32;
            }
        }

        /// <summary>
        /// Gets the NHibernate unsigned long type.
        /// </summary>
        /// <value>
        /// The NHibernate unsigned long type.
        /// </value>
        public static NullableType UInt64
        {
            get
            {
                return NHibernateUtil.UInt64;
            }
        }

        /// <summary>
        ///     Attempts to resolve the <see cref="IType" /> representation for the provided <see cref="System.Type" />
        ///     value.
        /// </summary>
        /// <param name="clrType">
        ///     The <see cref="System.Type" /> value to resolve.
        /// </param>
        /// <param name="isCastType">
        ///     A value indicating whether the provided <see cref="System.Type" /> value is used in a casting operation.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IType"/> representation of the provided <see cref="System.Type" /> value.
        /// </returns>
        public static IType GuessType(Type clrType, bool isCastType = false)
        {
            if (clrType.IsGenericType && typeof(Nullable<>) == clrType.GetGenericTypeDefinition())
            {
                clrType = clrType.GetGenericArguments()[0];
            }

            IType nhibernateType;

            bool flag = ClrTypeToNHibernateType
                .TryGetValue(clrType, out nhibernateType);

            if (flag)
            {
                return nhibernateType;
            }

            if (isCastType)
            {
                return null;
            }

            return NHibernateUtil.GuessType(clrType);
        }
    }
}