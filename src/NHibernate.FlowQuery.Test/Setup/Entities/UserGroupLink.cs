namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class UserGroupLinkEntity
    {
        public UserGroupLinkEntity(UserEntity user, GroupEntity group)
        {
            User = user;
            Group = group;
        }

        public UserGroupLinkEntity()
        {
        }

        public virtual GroupEntity Group { get; set; }

        public virtual long Id { get; set; }

        public virtual UserEntity User { get; set; }
    }
}