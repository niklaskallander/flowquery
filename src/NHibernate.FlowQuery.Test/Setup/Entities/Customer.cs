namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    using System;
    using System.Collections.Generic;

    public class CustomerEntity
    {
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

        public virtual DateTime CreatedStamp { get; set; }

        public virtual IList<CustomerGroupLinkEntity> Groups { get; set; }

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}