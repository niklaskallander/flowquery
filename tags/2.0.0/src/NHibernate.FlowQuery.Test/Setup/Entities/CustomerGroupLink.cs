namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class CustomerGroupLinkEntity
    {
		#region Constructors (2) 

        public CustomerGroupLinkEntity(CustomerEntity customer, GroupEntity group)
        {
            Customer = customer;
            Group = group;
        }

        public CustomerGroupLinkEntity() { }

		#endregion Constructors 

		#region Properties (3) 

        public virtual CustomerEntity Customer { get; set; }

        public virtual GroupEntity Group { get; set; }

        public virtual long Id { get; set; }

		#endregion Properties 
    }
}