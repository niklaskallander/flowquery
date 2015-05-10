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
            var query = new DummyQuery1(Session.CreateCriteria);

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
            Assert.That(() => new DummyQuery1(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void FlowQueryImplementorThrowsIfQueryTypeMismatch()
        {
            Assert
                .That
                (
                    () => new DummyQuery2(Session.CreateCriteria),
                    Throws.ArgumentException
                );
        }

        [Test]
        public void MorphableQueryImplementorPopulatesMappingsIfSet()
        {
            var query = new DummyQuery1(Session.CreateCriteria);

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

        private sealed class DummyQuery1 : MorphableFlowQueryBase<UserEntity, IDummyQuery2>, IDummyQuery2
        {
            internal DummyQuery1
                (
                Func<Type, string, ICriteria> criteriaFactory,
                string alias = null,
                FlowQueryOptions options = null,
                IMorphableFlowQuery query = null
                )
                : base(criteriaFactory, alias, options, query)
            {
            }

            internal void XProject<TDestination>(Expression<Func<UserEntity, TDestination>> expression)
            {
                Project(expression);
            }
        }

        private class DummyQuery2 : MorphableFlowQueryBase<UserEntity, IDummyQuery2>, IDummyQuery3
        {
            internal DummyQuery2
                (
                Func<Type, string, ICriteria> criteriaFactory,
                string alias = null,
                FlowQueryOptions options = null,
                IMorphableFlowQuery query = null
                )
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

            public new IDummyQuery3 ApplyFilter(IQueryFilter<UserEntity> filter)
            {
                throw new NotImplementedException();
            }

            public new IDummyQuery3 ApplyFilterOn<TAlias>
                (
                Expression<Func<TAlias>> alias,
                IQueryFilter<TAlias> filter
                )
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
                IRevealConvention revealConvention = null
                )
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