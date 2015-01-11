namespace NHibernate.FlowQuery.Test.Setup.Dtos
{
    public class UserDto
    {
        public UserDto(string fullname)
        {
            Fullname = fullname;
        }

        public UserDto()
        {
        }

        public string Fullname { get; set; }

        public long Id { get; set; }

        public bool IsOnline { get; set; }

        public long SettingId { get; set; }

        public string SomeValue { get; set; }

        public string Username { get; set; }
    }
}