namespace datingApp.API.Data
{
    public class User
    {
        public int Id{ get; set; }
        public string Username { get; set; }
        public byte[] Passwordhash { get; set; }
        public byte[] Passwordstyle { get; set; }
    }
}