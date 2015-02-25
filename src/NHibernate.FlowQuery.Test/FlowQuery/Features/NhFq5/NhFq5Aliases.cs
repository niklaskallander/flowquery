namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq5
{
    using NHibernate.FlowQuery.Test.Setup.Entities;

    public class NhFq5Aliases
    {
        public static readonly GroupEntity StaticGroupField = null;

        public readonly GroupEntity _groupField = null;

        public static GroupEntity StaticGroupProperty
        {
            get
            {
                return StaticGroupField;
            }
        }

        public GroupEntity GroupProperty
        {
            get
            {
                return _groupField;
            }
        }
    }
}