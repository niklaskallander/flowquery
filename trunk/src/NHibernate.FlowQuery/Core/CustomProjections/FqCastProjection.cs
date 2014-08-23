namespace NHibernate.FlowQuery.Core.CustomProjections
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.Type;

    /// <summary>
    ///     A helper projection which fixes an issue with the <see cref="CastProjection" /> causing casted projections
    ///     not be aggregated even though the underlying <see cref="IProjection" /> object might be.
    /// </summary>
    [Serializable]
    public class FqCastProjection : CastProjection
    {
        /// <summary>
        ///     The underlying <see cref="IProjection" /> object.
        /// </summary>
        private readonly IProjection _projection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FqCastProjection"/> class.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="IType" /> type which the provided <see cref="IProjection" /> 
        ///     <paramref name="projection" /> should be casted to.
        /// </param>
        /// <param name="projection">
        ///     The <see cref="IProjection" /> object that requires casting.
        /// </param>
        public FqCastProjection(IType type, IProjection projection)
            : base(type, projection)
        {
            _projection = projection;
        }

        /// <summary>
        ///     Gets a value indicating whether the underlying <see cref="IProjection" /> object is aggregated.
        /// </summary>
        /// <value>
        ///     Indicates whether the underlying <see cref="IProjection" /> object is aggregated.
        /// </value>
        public override bool IsAggregate
        {
            get
            {
                return _projection.IsAggregate;
            }
        }
    }
}