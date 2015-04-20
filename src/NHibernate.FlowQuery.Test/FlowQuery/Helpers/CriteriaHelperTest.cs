namespace NHibernate.FlowQuery.Test.FlowQuery.Helpers
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Dtos;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    using NUnit.Framework;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    [TestFixture]
    public class CriteriaHelperTest : BaseTest
    {
        internal interface IDummyQuery2 : IFlowQuery<UserEntity, IDummyQuery2>
        {
        }

        internal interface IDummyQuery3 : IFlowQuery<UserEntity, IDummyQuery3>
        {
        }

        [Test]
        public void BuildCriteriaThrowsIfQuerySelectionIsNull()
        {
            Assert
                .That
                (
                    () => new CriteriaBuilder().Build<UserEntity, UserEntity>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void BuildDetachedCriteriaThrowsIfQueryIsNull()
        {
            Assert
                .That
                (
                    () => new CriteriaBuilder().Build<UserEntity>(null),
                    Throws.InstanceOf<ArgumentNullException>()
                );
        }

        [Test]
        public void FlowQueryImplementorCreatesNewReferencesForUsedCollectionsWhenMorphing()
        {
            var query = new DummyQuery2(Session.CreateCriteria);

            query.XProject(x => new UserDto
            {
                Fullname = x.Firstname + " " + x.Lastname,
                Id = x.Id
            });

            Assert.That(query.Mappings, Is.Not.Null);
            Assert.That(query.Mappings.Count, Is.EqualTo(2));

            IImmediateFlowQuery<UserEntity> immediate = query.Immediate();

            var morphable = immediate as IMorphableFlowQuery;

            Assert.That(morphable, Is.Not.Null);

            // ReSharper disable once PossibleNullReferenceException
            Assert.That(morphable.Mappings, Is.Not.Null);

            Assert.That(!ReferenceEquals(morphable.Mappings, query.Mappings));
            Assert.That(!ReferenceEquals(morphable.Orders, query.Orders));
            Assert.That(!ReferenceEquals(morphable.GroupBys, query.GroupBys));
            Assert.That(!ReferenceEquals(morphable.Joins, query.Joins));
            Assert.That(!ReferenceEquals(morphable.Locks, query.Locks));
            Assert.That(!ReferenceEquals(morphable.Aliases, query.Aliases));
        }

        [Test]
        public void FlowQueryImplementorThrowsIfCriteriaFactoryIsNull()
        {
            Assert.That(() => new DummyQuery2(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void FlowQueryImplementorThrowsIfQueryTypeMismatch()
        {
            Assert
                .That
                (
                    () => new DummyQuery3(Session.CreateCriteria),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void MorphableQueryImplementorPopulatesMappingsIfSet()
        {
            var query = new DummyQuery2(Session.CreateCriteria);

            query.XProject(x => new UserDto
            {
                Fullname = x.Firstname + " " + x.Lastname,
                Id = x.Id
            });

            Assert.That(query.Mappings, Is.Not.Null);
            Assert.That(query.Mappings.Count, Is.EqualTo(2));

            IImmediateFlowQuery<UserEntity> immediate = query.Immediate();

            var morphable = immediate as IMorphableFlowQuery;

            Assert.That(morphable, Is.Not.Null);

            // ReSharper disable once PossibleNullReferenceException
            Assert.That(morphable.Mappings, Is.Not.Null);

            Assert.That(morphable.Mappings.Count, Is.EqualTo(2));
        }

        [Test]
        public void QuerySelectionThrowsIfQueryIsNull()
        {
            Assert.That(() => QuerySelection.Create(null), Throws.InstanceOf<ArgumentNullException>());
        }

        internal class DummyQuery : IDetachedFlowQuery<UserEntity>
        {
            public DetachedCriteria Criteria
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IJoinBuilder<UserEntity, IDetachedFlowQuery<UserEntity>> Full
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IJoinBuilder<UserEntity, IDetachedFlowQuery<UserEntity>> Inner
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IJoinBuilder<UserEntity, IDetachedFlowQuery<UserEntity>> LeftOuter
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IJoinBuilder<UserEntity, IDetachedFlowQuery<UserEntity>> RightOuter
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IDetachedFlowQuery<UserEntity> And(params ICriterion[] criterions)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And(string property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And(Expression<Func<UserEntity, bool>> expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And
                (Expression<Func<UserEntity, object>> property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And(Expression<Func<UserEntity, WhereDelegate, bool>> expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And(IDetachedImmutableFlowQuery subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> And(DetachedCriteria subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Cacheable(bool b = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Cacheable(string region)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Cacheable(string region, CacheMode cacheMode)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Cacheable(CacheMode cacheMode)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearFetches()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearGroupBys()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearJoins()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearLimit()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearLocks()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearOrders()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearRestrictions()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> ClearTimeout()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Copy()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Count()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Count(string property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Count(IProjection projection)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Count(Expression<Func<UserEntity, object>> property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> CountLong()
            {
                throw new NotImplementedException();
            }

            public IDelayedFlowQuery<UserEntity> Delayed()
            {
                throw new NotImplementedException();
            }

            public IDelayedFlowQuery<UserEntity> Delayed(ISession session)
            {
                throw new NotImplementedException();
            }

            public IDelayedFlowQuery<UserEntity> Delayed(IStatelessSession session)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Distinct()
            {
                throw new NotImplementedException();
            }

            public IFetchBuilder<IDetachedFlowQuery<UserEntity>> Fetch(string path)
            {
                throw new NotImplementedException();
            }

            public IFetchBuilder<IDetachedFlowQuery<UserEntity>> Fetch
                (
                Expression<Func<UserEntity, object>> expression,
                Expression<Func<object>> alias = null,
                IRevealConvention revealConvention = null)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> GroupBy(Expression<Func<UserEntity, object>> property)
            {
                throw new NotImplementedException();
            }

            public IImmediateFlowQuery<UserEntity> Immediate()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a 
            ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
            /// </summary>
            /// <param name="session">
            ///     The session.
            /// </param>
            /// <returns>
            ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a 
            ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="session" /> is null.
            /// </exception>
            public IImmediateFlowQuery<UserEntity> Immediate(ISession session)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a 
            ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
            /// </summary>
            /// <param name="session">
            ///     The session.
            /// </param>
            /// <returns>
            ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a 
            ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="session" /> is null.
            /// </exception>
            public IImmediateFlowQuery<UserEntity> Immediate(IStatelessSession session)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Indistinct()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Limit(int limit)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Limit(int limit, int offset)
            {
                throw new NotImplementedException();
            }

            public ILockBuilder<IDetachedFlowQuery<UserEntity>> Lock(Expression<Func<object>> alias)
            {
                throw new NotImplementedException();
            }

            public ILockBuilder<IDetachedFlowQuery<UserEntity>> Lock(string alias)
            {
                throw new NotImplementedException();
            }

            public ILockBuilder<IDetachedFlowQuery<UserEntity>> Lock()
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderBy(string property, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderBy(IProjection projection, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderBy
                (Expression<Func<UserEntity, object>> property, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderBy<TProjection>
                (Expression<Func<TProjection, object>> property, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderBy<TProjection>(string property, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderByDescending(string property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderByDescending(IProjection projection)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderByDescending(Expression<Func<UserEntity, object>> property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderByDescending<TProjection>
                (Expression<Func<TProjection, object>> property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> OrderByDescending<TProjection>(string property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> RestrictByExample
                (UserEntity exampleInstance, Action<IExampleWrapper<UserEntity>> example)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Select(string property)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Select(IProjection projection)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Select(Expression<Func<UserEntity, object>> expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> SetRootAlias<TAlias>(Expression<Func<TAlias>> alias)
                where TAlias : class
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </summary>
            /// <returns>
            ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </returns>
            public IStreamedFlowQuery<UserEntity> Streamed()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </summary>
            /// <param name="session">
            ///     The session.
            /// </param>
            /// <returns>
            ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="session" /> is null.
            /// </exception>
            public IStreamedFlowQuery<UserEntity> Streamed(ISession session)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </summary>
            /// <param name="session">
            ///     The session.
            /// </param>
            /// <returns>
            ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
            ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
            /// </returns>
            /// <exception cref="ArgumentNullException">
            ///     <paramref name="session" /> is null.
            /// </exception>
            public IStreamedFlowQuery<UserEntity> Streamed(IStatelessSession session)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Skip(int skip)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Take(int take)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Timeout(int seconds)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where(params ICriterion[] criterions)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where(string property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where(Expression<Func<UserEntity, bool>> expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where
                (Expression<Func<UserEntity, object>> property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where(Expression<Func<UserEntity, WhereDelegate, bool>> expression)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where
                (IDetachedImmutableFlowQuery subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public IDetachedFlowQuery<UserEntity> Where(DetachedCriteria subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }
        }

        internal class DummyQuery2 : MorphableFlowQueryBase<UserEntity, IDummyQuery2>, IDummyQuery2
        {
            protected internal DummyQuery2
                (
                Func<Type, string, ICriteria> criteriaFactory,
                string alias = null,
                FlowQueryOptions options = null,
                IMorphableFlowQuery query = null)
                : base(criteriaFactory, alias, options, query)
            {
            }

            internal virtual DummyQuery2 XProject<TDestination>(Expression<Func<UserEntity, TDestination>> expression)
            {
                Project(expression);

                return this;
            }
        }

        internal class DummyQuery3 : MorphableFlowQueryBase<UserEntity, IDummyQuery2>, IDummyQuery3
        {
            protected internal DummyQuery3
                (
                Func<Type, string, ICriteria> criteriaFactory,
                string alias = null,
                FlowQueryOptions options = null,
                IMorphableFlowQuery query = null)
                : base(criteriaFactory, alias, options, query)
            {
            }

            public new IJoinBuilder<UserEntity, IDummyQuery3> Full
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public new IJoinBuilder<UserEntity, IDummyQuery3> Inner
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public new IJoinBuilder<UserEntity, IDummyQuery3> LeftOuter
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public new IJoinBuilder<UserEntity, IDummyQuery3> RightOuter
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public new IDummyQuery3 And(params ICriterion[] criterions)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 And(IDetachedImmutableFlowQuery subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 And(DetachedCriteria subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Cacheable(bool b = true)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Cacheable(string region)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Cacheable(string region, CacheMode cacheMode)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Cacheable(CacheMode cacheMode)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearFetches()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearGroupBys()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearJoins()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearLimit()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearLocks()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearOrders()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ClearRestrictions()
            {
                throw new NotImplementedException();
            }

            public new IFetchBuilder<IDummyQuery3> Fetch(string path)
            {
                throw new NotImplementedException();
            }

            public new IFetchBuilder<IDummyQuery3> Fetch
                (
                Expression<Func<UserEntity, object>> expression,
                Expression<Func<object>> alias = null,
                IRevealConvention revealConvention = null)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 GroupBy(Expression<Func<UserEntity, object>> property)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Limit(int limit)
            {
                throw new NotImplementedException();
            }

            public new ILockBuilder<IDummyQuery3> Lock(Expression<Func<object>> alias)
            {
                throw new NotImplementedException();
            }

            public new ILockBuilder<IDummyQuery3> Lock(string alias)
            {
                throw new NotImplementedException();
            }

            public new ILockBuilder<IDummyQuery3> Lock()
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 OrderBy(string property, bool ascending = true)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 OrderBy<TProjection>
                (Expression<Func<TProjection, object>> property, bool ascending)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 OrderByDescending(string property)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 OrderByDescending<TProjection>
                (Expression<Func<TProjection, object>> property)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 RestrictByExample
                (UserEntity exampleInstance, Action<IExampleWrapper<UserEntity>> example)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Skip(int skip)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Take(int take)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Where(params ICriterion[] criterions)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Where(IDetachedImmutableFlowQuery subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 Where(DetachedCriteria subquery, IsEmptyExpression expresson)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.And(string property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.And(Expression<Func<UserEntity, bool>> expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.And
                (Expression<Func<UserEntity, object>> property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.And
                (Expression<Func<UserEntity, WhereDelegate, bool>> expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.Limit(int limit, int offset)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderBy(IProjection projection, bool ascending)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderBy
                (Expression<Func<UserEntity, object>> property, bool ascending)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderBy<TProjection>(string property, bool ascending)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderByDescending(IProjection projection)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderByDescending
                (Expression<Func<UserEntity, object>> property)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFlowQuery<UserEntity, IDummyQuery3>.OrderByDescending<TProjection>(string property)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.Where(string property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.Where(Expression<Func<UserEntity, bool>> expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.Where
                (Expression<Func<UserEntity, object>> property, IsExpression expression)
            {
                throw new NotImplementedException();
            }

            IDummyQuery3 IFilterableQuery<UserEntity, IDummyQuery3>.Where
                (Expression<Func<UserEntity, WhereDelegate, bool>> expression)
            {
                throw new NotImplementedException();
            }
        }
    }
}