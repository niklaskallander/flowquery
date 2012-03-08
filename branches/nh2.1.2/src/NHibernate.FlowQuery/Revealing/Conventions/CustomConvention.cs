using System;

namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    /// Lets you provide a custom convention delegate.
    /// </summary>
    public class CustomConvention : IRevealConvention
    {
		#region Fields (1) 

        private Func<string, string> m_CustomConvention;

		#endregion Fields 

		#region Constructors (1) 

        public CustomConvention(Func<string, string> customConvention)
        {
            if (customConvention == null)
            {
                throw new ArgumentNullException("customConvention");
            }

            m_CustomConvention = customConvention;
        }

		#endregion Constructors 

		#region Methods (1) 

        protected virtual string RevealFrom(string property)
        {
            return m_CustomConvention(property);
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