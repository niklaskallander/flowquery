namespace NHibernate.FlowQuery.Revealing.Conventions
{
    public interface IRevealConvention
    {
		#region Operations (1) 

        string RevealFrom(string property);

		#endregion Operations 
    }
}