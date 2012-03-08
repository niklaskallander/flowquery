using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace NHibernate.FlowQuery.Test
{
    public class Dummy : DomainObject<long>
    {
		#region Properties (2) 

        public bool Test { get; set; }

        public string Value { get; set; }

		#endregion Properties 
    }

    public class DomainObject<T>
    {
		#region Properties (1) 

        public virtual T PK { get; set; }

		#endregion Properties 
    }

    public class ClassConvention : IClassConvention
    {
		#region Methods (1) 

        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType + "Tbl");
        }

		#endregion Methods 
    }

    public class IdConvention : IIdConvention
    {


        #region IConvention<IIdentityInspector,IIdentityInstance> Members

        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "PK");
        }

        #endregion
    }

}