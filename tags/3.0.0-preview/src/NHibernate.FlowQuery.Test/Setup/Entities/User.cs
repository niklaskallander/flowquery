namespace NHibernate.FlowQuery.Test.Setup.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IUserEntity
    {
        DateTime CreatedStamp { get; set; }

        string Firstname { get; set; }

        long Id { get; set; }

        bool IsOnline { get; set; }

        DateTime? LastLoggedInStamp { get; set; }

        string Lastname { get; set; }

        string Password { get; set; }

        RoleEnum Role { get; set; }

        string Username { get; set; }
    }

    public class UserEntity : IUserEntity
    {
        // ReSharper disable once InconsistentNaming
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", 
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", 
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1308:VariableNamesMustNotBePrefixed", 
            Justification = "Reviewed. Suppression is OK here.")]
        protected string m_TestValue;

        public UserEntity
            (
            string username, 
            string password, 
            string firstname, 
            string lastname, 
            DateTime createdStamp, 
            RoleEnum role, 
            string testvalue
            )
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

        public virtual DateTime CreatedStamp { get; set; }

        public virtual string Firstname { get; set; }

        public virtual IList<UserGroupLinkEntity> Groups { get; set; }

        public virtual long Id { get; set; }

        public virtual bool IsOnline { get; set; }

        public virtual DateTime? LastLoggedInStamp { get; set; }

        public virtual string Lastname { get; set; }

        public virtual int NumberOfLogOns { get; set; }

        public virtual string Password { get; set; }

        public virtual RoleEnum Role { get; set; }

        public virtual Setting Setting { get; set; }

        public virtual string Username { get; set; }
    }
}