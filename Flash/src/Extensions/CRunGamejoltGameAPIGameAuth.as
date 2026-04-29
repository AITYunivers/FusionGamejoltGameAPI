package Extensions
{
	public class CRunGamejoltGameAPIGameAuth
	{
        public var UserName:String;
        public var UserToken:String;
        public var GuestName:String;

        public function CRunGamejoltGameAPIGameAuth(userName:String, userToken:String, guestName:String)
        {
            UserName = userName;
            UserToken = userToken;
            GuestName = guestName;
        }
	}
}