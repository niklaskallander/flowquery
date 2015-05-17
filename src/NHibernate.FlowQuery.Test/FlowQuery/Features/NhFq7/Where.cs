namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    public static class Where
    {
        public static IQueryFilter<Setting> Id_Of_Setting_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.And(x => x.Id == id));
        }

        public static IQueryFilter<Setting> Id_SpecifiedByMagicString_Of_Setting_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.And("Id", Is.EqualTo(id)));
        }

        public static void Id_Of_JoinedSetting_FilteredInJoin_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting, () => setting.Id == id);
        }

        public static void Id_Of_JoinedSetting_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, Id_Of_Setting_Is(id));
        }

        public static void Id_Of_JoinedSetting_UsingIsHelper_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, Id_Of_Setting_UsingIsHelper_Is(id));
        }

        public static void Id_Of_JoinedSetting_UsingWhereHelper_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting)
                .ApplyFilterOn(() => setting, Id_Of_Setting_UsingWhereHelper_Is(id));
        }

        public static void Id_Of_JoinedSetting_FilteredInJoin_UsingRevealer_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting, () => setting.Id == id, NoopRevealConvention());
        }

        public static void Id_Of_JoinedSetting_UsingRevealer_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join(x => x.Setting, () => setting, NoopRevealConvention())
                .ApplyFilterOn(() => setting, Id_Of_Setting_Is(id));
        }

        public static void Id_Of_JoinedSetting_AliasedByMagicString_FilteredInJoin_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join("Setting", () => setting, () => setting.Id == id);
        }

        public static void Id_Of_JoinedSetting_AliasedByMagicString_Is
            (
            IFilterableQuery<UserEntity> query,
            int id
            )
        {
            Setting setting = null;

            query.Inner.Join("Setting", () => setting)
                .ApplyFilterOn(() => setting, Id_Of_Setting_Is(id));
        }

        public static IQueryFilter<UserEntity> Id_Of_User_Is(int between, int and)
        {
            return new QueryFilter<UserEntity>(query => query.And(x => x.Id, Is.Between(between, and)));
        }

        public static IQueryFilter<UserEntity> Id_SpecifedByMagicString_Of_User_Is(int between, int and)
        {
            return new QueryFilter<UserEntity>(query => query.And("Id", Is.Between(between, and)));
        }

        public static IQueryFilter<UserEntity> Id_Of_User_UsingWhereHelper_Is(int between, int and)
        {
            return new QueryFilter<UserEntity>(query => query.And((x, y) => y(x.Id, Is.Between(between, and))));
        }

        public static IQueryFilter<T> Subquery_Returns_Nothing<T>(IDetachedImmutableFlowQuery subquery)
        {
            return new QueryFilter<T>(query => query.And(subquery, Is.Empty()));
        }

        public static IQueryFilter<T> Subquery_Returns_Nothing<T>(DetachedCriteria subquery)
        {
            return new QueryFilter<T>(query => query.And(subquery, Is.Empty()));
        }

        private static IQueryFilter<Setting> Id_Of_Setting_UsingIsHelper_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.And(x => x.Id, Is.EqualTo(id)));
        }

        private static IQueryFilter<Setting> Id_Of_Setting_UsingWhereHelper_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.And((x, where) => where(x.Id, Is.EqualTo(id))));
        }

        private static IRevealConvention NoopRevealConvention()
        {
            return new CustomConvention(x => x);
        }
    }
}
