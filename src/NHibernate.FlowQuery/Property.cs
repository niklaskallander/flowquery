using System;

namespace NHibernate.FlowQuery
{
    public static class Property
    {
		#region Methods (1) 

        public static TReturn As<TReturn>(string property)
        {
            throw new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

		#endregion Methods 
    }
}