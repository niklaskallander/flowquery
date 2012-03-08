using System;
using System.Collections.Generic;

namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    public interface IUserEntity
    {
        #region Data Members (9)

        DateTime CreatedStamp { get; set; }

        string Firstname { get; set; }

        long Id { get; set; }

        bool IsOnline { get; set; }

        DateTime? LastLoggedInStamp { get; set; }

        string Lastname { get; set; }

        string Password { get; set; }

        RoleEnum Role { get; set; }

        string Username { get; set; }

        #endregion Data Members
    }

    public class UserEntity : IUserEntity
    {
        #region Constructors (2)

        public UserEntity(string username, string password, string firstname, string lastname, DateTime createdStamp, RoleEnum role, string testvalue)
            : this()
        {
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            CreatedStamp = createdStamp;
            Role = role;
            m_TestValue = testvalue;
        }

        public UserEntity()
        {
            Groups = new List<UserGroupLinkEntity>();
        }

        #endregion Constructors

        #region Properties (11)

        public virtual DateTime CreatedStamp { get; set; }

        public virtual string Firstname { get; set; }

        public virtual IList<UserGroupLinkEntity> Groups { get; set; }

        public virtual long Id { get; set; }

        public virtual bool IsOnline { get; set; }

        public virtual DateTime? LastLoggedInStamp { get; set; }

        public virtual string Lastname { get; set; }

        public virtual string Password { get; set; }

        public virtual RoleEnum Role { get; set; }

        public virtual Setting Setting { get; set; }

        public virtual string Username { get; set; }

        protected string m_TestValue;

        #endregion Properties
    }
}