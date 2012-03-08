namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class UserGroupLinkEntity
    {
		#region Constructors (2) 

        public UserGroupLinkEntity(UserEntity user, GroupEntity group)
        {
            User = user;
            Group = group;
        }

        public UserGroupLinkEntity() { }

		#endregion Constructors 

		#region Properties (3) 

        public virtual GroupEntity Group { get; set; }

        public virtual long Id { get; set; }

        public virtual UserEntity User { get; set; }

		#endregion Properties 
    }
}