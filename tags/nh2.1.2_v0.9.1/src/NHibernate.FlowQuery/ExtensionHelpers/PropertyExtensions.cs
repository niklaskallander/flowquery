using System;

namespace NHibernate.FlowQuery.ExtensionHelpers
{
    public static class PropertyExtensions
    {
        #region Methods (1)

        public static TReturn As<TReturn>(this string property)
        {
            throw new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        #endregion Methods
    }
}