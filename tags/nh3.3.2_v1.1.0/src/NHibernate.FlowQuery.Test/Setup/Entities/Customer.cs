using System;
using System.Collections.Generic;

namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class CustomerEntity
    {
		#region Constructors (2) 

        public CustomerEntity(string name, DateTime createdStamp)
            : this()
        {
            Name = name;
            CreatedStamp = createdStamp;
        }

        public CustomerEntity()
        {
            Groups = new List<CustomerGroupLinkEntity>();
        }

		#endregion Constructors 

		#region Properties (4) 

        public virtual DateTime CreatedStamp { get; set; }

        public virtual IList<CustomerGroupLinkEntity> Groups { get; set; }

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

		#endregion Properties 
    }
}