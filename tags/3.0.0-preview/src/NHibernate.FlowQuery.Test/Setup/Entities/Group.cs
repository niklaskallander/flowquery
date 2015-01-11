namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    using System;
    using System.Collections.Generic;

    public class GroupEntity
    {
        public GroupEntity(string name, DateTime createdStamp)
            : this()
        {
            Name = name;
            CreatedStamp = createdStamp;
        }

        public GroupEntity()
        {
            Customers = new List<CustomerGroupLinkEntity>();
            Users = new List<UserGroupLinkEntity>();
        }

        public virtual DateTime CreatedStamp { get; set; }

        public virtual IList<CustomerGroupLinkEntity> Customers { get; set; }

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<UserGroupLinkEntity> Users { get; set; }
    }
}