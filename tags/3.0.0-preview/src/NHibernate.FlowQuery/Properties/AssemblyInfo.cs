using System;
using System.Reflection;
using System.Security;

[assembly: AssemblyDescription("An extension for NHibernate that provides a better flow when, and a safer way of, " +
    "querying over NHibernate's ICriteria API.")]
[assembly: AssemblyCompany("Niklas Källander")]
[assembly: AssemblyCopyright("Copyright © Niklas Källander 2010 - 2014")]
[assembly: AssemblyProduct("NHibernate.FlowQuery")]
[assembly: AssemblyTitle("NHibernate.FlowQuery")]

[assembly: AssemblyVersion("3.0.*")]
[assembly: AssemblyFileVersion("3.0.0")]
[assembly: AssemblyInformationalVersion("3.0.0-preview")]

[assembly: AllowPartiallyTrustedCallers]
[assembly: CLSCompliant(true)]