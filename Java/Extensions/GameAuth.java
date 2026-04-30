package Extensions;

/**
 *
 * @author Yunivers
 */
public class GameAuth {
    public String UserName;
    public String UserToken;
    public String GuestName;

    public GameAuth(String userName, String userToken, String guestName)
    {
        UserName = userName;
        UserToken = userToken;
        GuestName = guestName;
    }
}
