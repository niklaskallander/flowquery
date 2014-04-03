namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public class CacheableUser
    {
        public CacheableUser()
        {
            
        }

        public CacheableUser(string name)
        {
            Name = name;
        }

        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}