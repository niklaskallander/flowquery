using System;
using System.Linq.Expressions;
using System.Transactions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Test.Entities;

namespace NHibernate.FlowQuery.Test
{
    class Program
    {
        #region Methods (3)

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration
                      .MsSql2005
                        .ConnectionString(@"Data Source=.\localsql;Initial Catalog=FlowQueryTest;Integrated Security=SSPI;")
                )
                .Mappings
                (
                    m => m.AutoMappings.Add
                    (
                        AutoMap.AssemblyOf<User>()
                            .Where(type => type.Namespace.EndsWith("Entities"))
                            .Override<User>(mapper => mapper.Map(u => u.Role).CustomType(typeof(Role)))
                            .Conventions
                                .Add
                                (
                                    Table.Is(ci => ci.EntityType.Name + "Tbl"),
                                    PrimaryKey.Name.Is(i => i.EntityType.Name + "PK"),
                                    ForeignKey.EndsWith("PK"),
                                    DefaultLazy.Always(),
                                    DefaultCascade.All(),
                                    DefaultAccess.Property()
                                )
                    )
                )
                .BuildSessionFactory();

        }

        static void Main(string[] args)
        {
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            Test(null);

            Expression<Func<A, object>> lambda = a => a.B.C.D.Name.Length;

            Console.WriteLine(lambda.Body.ToString());


            Console.ReadKey();
        }

        static void Test(Setting s)
        {
            using (ISessionFactory factory = CreateSessionFactory())
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            using (ISession session = factory.OpenSession())
            {


            }
        }

        #endregion Methods

        #region Nested Classes (4)


        class A
        {
            #region Properties (1)

            public B B { get; set; }

            #endregion Properties
        }
        class B
        {
            #region Properties (1)

            public C C { get; set; }

            #endregion Properties
        }
        class C
        {
            #region Properties (1)

            public D D { get; set; }

            #endregion Properties
        }
        class D
        {
            #region Properties (1)

            public string Name { get; set; }

            #endregion Properties
        }
        #endregion Nested Classes
    }
}