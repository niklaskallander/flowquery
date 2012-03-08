namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    /// Adds "_" to the beginning of the provided string.
    /// </summary>
    public class UnderscoreConvention : IRevealConvention
    {
		#region Methods (1) 

        protected virtual string RevealFrom(string property)
        {
            return string.Format("_{0}", property);
        }

		#endregion Methods 



        #region IRevealConvention Members

        string IRevealConvention.RevealFrom(string property)
        {
            return RevealFrom(property);
        }

        #endregion
    }
}