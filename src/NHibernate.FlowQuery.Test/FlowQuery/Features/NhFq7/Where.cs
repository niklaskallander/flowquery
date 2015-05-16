namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.FlowQuery.Test.Setup.Entities;

    public static class Where
    {
        public static IQueryFilter<Setting> Id_Of_Setting_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.Where(x => x.Id == id));
        }

        public static IQueryFilter<Setting> Id_SpecifiedByMagicString_Of_Setting_Is(int id)
        {
            return new QueryFilter<Setting>(query => query.Where("Id", Is.EqualTo(id)));
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
            return new QueryFilter<UserEntity>(query => query.Where(x => x.Id, Is.Between(between, and)));
        }

        private static IRevealConvention NoopRevealConvention()
        {
            return new CustomConvention(x => x);
        }
    }
}
