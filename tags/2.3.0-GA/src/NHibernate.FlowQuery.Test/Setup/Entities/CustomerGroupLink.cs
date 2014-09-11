namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class CustomerGroupLinkEntity
    {
        public CustomerGroupLinkEntity(CustomerEntity customer, GroupEntity group)
        {
            Customer = customer;
            Group = group;
        }

        public CustomerGroupLinkEntity()
        {
        }

        public virtual CustomerEntity Customer { get; set; }

        public virtual GroupEntity Group { get; set; }

        public virtual long Id { get; set; }
    }
}