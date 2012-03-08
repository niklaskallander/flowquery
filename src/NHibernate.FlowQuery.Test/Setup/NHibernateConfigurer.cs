using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
//using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Cfg;
using NHibernate.FlowQuery.Test.Setup.Entities;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.FlowQuery.Test.Setup
{
    public static class NHibernateConfigurer
    {
        #region Fields (2)

        private static ISessionFactory m_Factory;

        #endregion Fields

        #region Methods (4)

        public static void AddData()
        {
            using (var session = m_Factory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var c1 = new CustomerEntity("Pelle Svensson", new DateTime(2001, 9, 11));
                var c2 = new CustomerEntity("Arne Vaise", new DateTime(2005, 3, 15));
                var c3 = new CustomerEntity("Kalle Palle", DateTime.Now);
                var c4 = new CustomerEntity("Lisa Oscarsson", new DateTime(2007, 12, 29));

                session.Save(c1);
                session.Save(c2);
                session.Save(c3);
                session.Save(c4);

                var s1 = new Setting();
                var s2 = new Setting();
                var s3 = new Setting();
                var s4 = new Setting();
                var s5 = new Setting();
                var s6 = new Setting();

                session.Save(s1);
                session.Save(s2);
                session.Save(s3);
                session.Save(s4);
                session.Save(s5);
                session.Save(s6);

                var u1 = new UserEntity("Wimpy", "Cool01", "Niklas", "Källander", new DateTime(2001, 9, 11), RoleEnum.Administrator, "1") { LastLoggedInStamp = DateTime.Now, IsOnline = true, Setting = s6 };
                var u2 = new UserEntity("Izmid", "Cool02", "Lars", "Wilk", new DateTime(2001, 4, 22), RoleEnum.Webmaster, "2") { LastLoggedInStamp = DateTime.Now, IsOnline = true, Setting = s6 };
                var u3 = new UserEntity("Empor", "Cool03", "Kossan", "Muu", new DateTime(2001, 5, 3), RoleEnum.Administrator, "3") { LastLoggedInStamp = DateTime.Now, IsOnline = true, Setting = s6 };
                var u4 = new UserEntity("Lajsa", "Cool04", "Lotta", "Bråk", DateTime.Now, RoleEnum.Standard, "4") { Setting = s6 };

                session.Save(u1);
                session.Save(u2);
                session.Save(u3);
                session.Save(u4);

                var g1 = new GroupEntity("A1", DateTime.Now);
                var g2 = new GroupEntity("B2", new DateTime(2009, 11, 27));

                session.Save(g1);
                session.Save(g2);

                var cgl1 = new CustomerGroupLinkEntity(c1, g1);
                var cgl2 = new CustomerGroupLinkEntity(c2, g1);
                var cgl3 = new CustomerGroupLinkEntity(c3, g1);
                var cgl4 = new CustomerGroupLinkEntity(c4, g1);
                var cgl5 = new CustomerGroupLinkEntity(c1, g2);
                var cgl6 = new CustomerGroupLinkEntity(c3, g2);

                session.Save(cgl1);
                session.Save(cgl2);
                session.Save(cgl3);
                session.Save(cgl4);
                session.Save(cgl5);
                session.Save(cgl6);

                g1.Customers.Add(cgl1);
                g1.Customers.Add(cgl2);
                g1.Customers.Add(cgl3);
                g1.Customers.Add(cgl4);
                g2.Customers.Add(cgl5);
                g2.Customers.Add(cgl6);

                c1.Groups.Add(cgl1);
                c1.Groups.Add(cgl5);
                c2.Groups.Add(cgl2);
                c3.Groups.Add(cgl3);
                c3.Groups.Add(cgl6);
                c4.Groups.Add(cgl4);

                var ugl1 = new UserGroupLinkEntity(u1, g1);
                var ugl2 = new UserGroupLinkEntity(u1, g2);
                var ugl3 = new UserGroupLinkEntity(u2, g2);
                var ugl4 = new UserGroupLinkEntity(u3, g1);
                var ugl5 = new UserGroupLinkEntity(u3, g2);

                session.Save(ugl1);
                session.Save(ugl2);
                session.Save(ugl3);
                session.Save(ugl4);
                session.Save(ugl5);

                g1.Users.Add(ugl1);
                g1.Users.Add(ugl4);
                g2.Users.Add(ugl2);
                g2.Users.Add(ugl3);
                g2.Users.Add(ugl5);

                u1.Groups.Add(ugl1);
                u1.Groups.Add(ugl2);
                u2.Groups.Add(ugl3);
                u3.Groups.Add(ugl4);
                u3.Groups.Add(ugl5);

                transaction.Commit();
            }
        }

        private static void BuildSchema(Configuration configuration)
        {
            SchemaExport schemaExport = new SchemaExport(configuration);
            schemaExport.Create(false, true);
        }

        public static void Configure()
        {
            //NHibernateProfiler.Initialize();

            m_Factory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Data Source=.\sqlexpress; Initial Catalog=FlowQueryTestDB; Integrated Security=SSPI;"))
                .Mappings
                (
                    m => m.AutoMappings
                        .Add
                        (
                            AutoMap
                                .AssemblyOf<UserEntity>()
                                .Where
                                (
                                    (t) => t.Namespace == typeof(UserEntity).Namespace
                                )
                                .Override<UserEntity>(x => x.Map(FluentNHibernate.Reveal.Member<UserEntity>("m_TestValue")).Access.Field())
                        )
                )
                .ExposeConfiguration(c =>
                {
                    BuildSchema(c);
                })
                .BuildSessionFactory();
        }

        public static ISessionFactory GetSessionFactory()
        {
            return m_Factory;
        }

        #endregion Methods
    }
}