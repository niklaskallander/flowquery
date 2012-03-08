namespace NHibernate.FlowQuery.Test.Setup.Dtos
{
    public class UserDto
    {
        #region Constructors (2)

        public UserDto(string fullname)
        {
            Fullname = fullname;
        }

        public UserDto() { }

        #endregion Constructors

        #region Properties (4)

        public string SomeValue { get; set; }

        public string Fullname { get; set; }

        public long Id { get; set; }

        public long SettingId { get; set; }

        public bool IsOnline { get; set; }

        public string Username { get; set; }

        #endregion Properties
    }
}