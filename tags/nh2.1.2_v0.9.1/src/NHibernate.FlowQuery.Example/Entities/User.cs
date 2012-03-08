using System.Collections.Generic;
namespace NHibernate.FlowQuery.Test.Entities
{
    public enum Role
    {
        Regular = 0,
        Premium = 1,
        Administrator = 2,
    }

    public class UserRoleLink
    {
        public virtual long Id { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }

    public class MappingTest
    {
        #region Properties (5)

        public virtual string Email { get; set; }

        public virtual long Id { get; set; }

        public virtual string NewProperty { get; set; }

        public virtual Setting Setting { get; set; }

        public virtual string Username { get; set; }

        #endregion Properties
    }

    public class User
    {
        #region Constructors (2)

        public User(string username) { Username = username; }

        public User() { }

        #endregion Constructors

        #region Properties (9)

        public virtual string Email { get; set; }

        public virtual string Firstname { get; set; }

        public virtual long Id { get; set; }

        public virtual string Lastname { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string Password { get; set; }

        public virtual Role Role { get; set; }

        public virtual IEnumerable<UserRoleLink> Roles { get; set; }

        public virtual Setting Setting { get; set; }

        public virtual string Username { get; set; }

        #endregion Properties
    }
}