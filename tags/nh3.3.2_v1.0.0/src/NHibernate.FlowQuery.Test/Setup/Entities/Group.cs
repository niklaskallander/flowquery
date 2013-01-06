using System;
using System.Collections.Generic;

namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class GroupEntity
    {
		#region Constructors (2) 

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

		#endregion Constructors 

		#region Properties (5) 

        public virtual DateTime CreatedStamp { get; set; }

        public virtual IList<CustomerGroupLinkEntity> Customers { get; set; }

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual IList<UserGroupLinkEntity> Users { get; set; }

		#endregion Properties 
    }
}