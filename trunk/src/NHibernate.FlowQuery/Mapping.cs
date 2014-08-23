namespace NHibernate.FlowQuery
{
    using System;

    using NHibernate.FlowQuery.AutoMapping;

    /// <summary>
    ///     A static utility class providing default mapping functionality as defined by <see cref="IMapper" />.
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        ///     The default <see cref="IMapper" /> instance to use.
        /// </summary>
        private static IMapper _mapper;

        /// <summary>
        ///     Initializes static members of the <see cref="Mapping" /> class.
        /// </summary>
        static Mapping()
        {
            _mapper = new DefaultMapper();
        }

        /// <summary>
        ///     Maps the provided <see cref="T:TSource" /> instance into a <see cref="T:TDestination" /> instance.
        /// </summary>
        /// <param name="source">
        ///     The source instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source instance.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the destination instance.
        /// </typeparam>
        /// <returns>
        ///     The mapped <see cref="T:TDestination" /> instance.
        /// </returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        ///     Sets the default <see cref="IMapper" /> instance used by this static utility class.
        /// </summary>
        /// <param name="mapper">
        ///     The mapper.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="mapper" /> is null.
        /// </exception>
        public static void SetMapper(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            _mapper = mapper;
        }
    }
}