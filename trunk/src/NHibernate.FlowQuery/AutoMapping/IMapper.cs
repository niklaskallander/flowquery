namespace NHibernate.FlowQuery.AutoMapping
{
    /// <summary>
    ///     An interface defining a utility to map instances of one type to another.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        ///     Maps the given <see cref="T:TSource" /> object into a <see cref="T:TDestination" /> instance.
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
        TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();
    }
}