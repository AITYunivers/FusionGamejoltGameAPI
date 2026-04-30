package Extensions;

//----------------------------------------------------------------------------------
//
// CRUNGAMEJOLTGAMEAPI: extension object
//
//----------------------------------------------------------------------------------
import java.awt.Graphics2D;
import java.io.*;
import Services.*;
import RunLoop.*;
import Conditions.*;
import Actions.*;
import GamejoltGameAPI.DarkEdif.*;
import Expressions.*;
import java.net.*;
import java.security.MessageDigest;
import java.util.ArrayList;
import java.util.List;
import GamejoltGameAPI.MinimalJson.*;

public class CRunGamejoltGameAPI extends CRunExtension
{
    public static final int ExtensionVersion = 6;
    public static final int SDKVersion = 20;

    public static final String JoltBase = "http://api.gamejolt.com";
    private static final int ACT_AUTH = 0;
    private static final int ACT_AUTHCREDS = 1;
    private static final int ACT_SETGUESTNAME = 2;
    private static final int ACT_FETCHUSERNAME = 3;
    private static final int ACT_FETCHUSERID = 4;
    private static final int ACT_OPENSESSION = 5;
    private static final int ACT_PINGSESSION = 6;
    private static final int ACT_PINGSTATUSSESSION = 7;
    private static final int ACT_CHECKSESSION = 8;
    private static final int ACT_CLOSESESSION = 9;
    private static final int ACT_ADDUSERSCORE = 10;
    private static final int ACT_ADDGUESTSCORE = 11;
    private static final int ACT_GETSCORERANKING = 12;
    private static final int ACT_FETCHSCORES = 13;
    private static final int ACT_GETTABLES = 14;
    private static final int ACT_GETTROPHY = 15;
    private static final int ACT_GETTROPHIES = 16;
    private static final int ACT_GETUNLOCKEDTROPHIES = 17;
    private static final int ACT_UNLOCKTROPHY = 18;
    private static final int ACT_LOCKTROPHY = 19;
    private static final int ACT_GLOBALSTORAGEGETDATA = 20;
    private static final int ACT_GLOBALSTORAGEGETKEYS = 21;
    private static final int ACT_GLOBALSTORAGEDELETEKEY = 22;
    private static final int ACT_GLOBALSTORAGESETKEY = 23;
    private static final int ACT_GLOBALSTORAGEUPDATEKEY = 24;
    private static final int ACT_USERSTORAGEGETDATA = 25;
    private static final int ACT_USERSTORAGEGETKEYS = 26;
    private static final int ACT_USERSTORAGEDELETEKEY = 27;
    private static final int ACT_USERSTORAGESETKEY = 28;
    private static final int ACT_USERSTORAGEUPDATEKEY = 29;
    private static final int ACT_GLOBALFILESTORAGESAVEDATA = 30;
    private static final int ACT_GLOBALFILESTORAGESETKEY = 31;
    private static final int ACT_GLOBALFILESTORAGEUPDATEKEY = 32;
    private static final int ACT_USERFILESTORAGESAVEDATA = 33;
    private static final int ACT_USERFILESTORAGESETKEY = 34;
    private static final int ACT_USERFILESTORAGEUPDATEKEY = 35;
    private static final int ACT_GETFRIENDSLIST = 36;
    private static final int ACT_GETCURRENTTIME = 37;
    private static final int ACT_SETGAMEID = 38;
    private static final int ACT_SETPRIVATEKEY = 39;
    private static final int ACT_FETCHUSERSCORES = 40;
    private static final int CND_AUTHFINISHED = 0;
    private static final int CND_FETCHFINISHED = 1;
    private static final int CND_OPENFINISHED = 2;
    private static final int CND_PINGFINISHED = 3;
    private static final int CND_CHECKFINISHED = 4;
    private static final int CND_CLOSEFINISHED = 5;
    private static final int CND_SCOREADDED = 6;
    private static final int CND_RANKINGRETRIEVED = 7;
    private static final int CND_SCORESFETCHED = 8;
    private static final int CND_TABLESRETRIEVED = 9;
    private static final int CND_TROPHIESRETRIEVED = 10;
    private static final int CND_TROPHYADDED = 11;
    private static final int CND_TROPHYREMOVED = 12;
    private static final int CND_GSGETDATA = 13;
    private static final int CND_GSGETKEYS = 14;
    private static final int CND_GSDELETEKEY = 15;
    private static final int CND_GSSETKEY = 16;
    private static final int CND_GSUPDATEKEY = 17;
    private static final int CND_USGETDATA = 18;
    private static final int CND_USGETKEYS = 19;
    private static final int CND_USDELETEKEY = 20;
    private static final int CND_USSETKEY = 21;
    private static final int CND_USUPDATEKEY = 22;
    private static final int CND_FGSSAVEDATA = 23;
    private static final int CND_FGSSETKEY = 24;
    private static final int CND_FGSUPDATEKEY = 25;
    private static final int CND_UGSSAVEDATA = 26;
    private static final int CND_UGSSETKEY = 27;
    private static final int CND_UGSUPDATEKEY = 28;
    private static final int CND_GETFRIENDSLIST = 29;
    private static final int CND_GETCURRENTTIME = 30;
    private static final int CND_ANYCALLFINISHED = 31;
    private static final int CND_ONERROR = 32;
    private static final int EXP_GETJSONRESPONSE = 0;
    private static final int EXP_GETRESPONSETYPE = 1;
    private static final int EXP_GETRESPONSESTATUS = 2;
    private static final int EXP_GETRESPONSEMESSAGE = 3;
    private static final int EXP_GETUSERNAME = 4;
    private static final int EXP_GETUSERTOKEN = 5;
    private static final int EXP_GETGUESTNAME = 6;
    private static final int EXP_FETCHEDUSERCOUNT = 7;
    private static final int EXP_FETCHEDUSERDISPLAYNAME = 8;
    private static final int EXP_FETCHEDUSERNAME = 9;
    private static final int EXP_FETCHEDUSERID = 10;
    private static final int EXP_FETCHEDUSERDESCRIPTION = 11;
    private static final int EXP_FETCHEDUSERAVATAR = 12;
    private static final int EXP_FETCHEDUSERWEBSITE = 13;
    private static final int EXP_FETCHEDUSERSTATUS = 14;
    private static final int EXP_FETCHEDUSERTYPE = 15;
    private static final int EXP_FETCHEDUSERLASTLOGGEDIN = 16;
    private static final int EXP_FETCHEDUSERLASTLOGGEDINTIMESTAMP = 17;
    private static final int EXP_FETCHEDUSERSIGNEDUP = 18;
    private static final int EXP_FETCHEDUSERSIGNEDUPTIMESTAMP = 19;
    private static final int EXP_SCORERANKING = 20;
    private static final int EXP_FETCHEDSCORECOUNT = 21;
    private static final int EXP_FETCHEDSCOREUSERNAME = 22;
    private static final int EXP_FETCHEDSCOREUSERID = 23;
    private static final int EXP_FETCHEDSCOREGUESTNAME = 24;
    private static final int EXP_FETCHEDSCORESCORE = 25;
    private static final int EXP_FETCHEDSCORESORT = 26;
    private static final int EXP_FETCHEDSCOREEXTRADATA = 27;
    private static final int EXP_FETCHEDSCORESUBMIT = 28;
    private static final int EXP_FETCHEDSCORESUBMITTIMESTAMP = 29;
    private static final int EXP_FETCHEDTABLECOUNT = 30;
    private static final int EXP_FETCHEDTABLENAME = 31;
    private static final int EXP_FETCHEDTABLEID = 32;
    private static final int EXP_FETCHEDTABLEDESCRIPTION = 33;
    private static final int EXP_FETCHEDTABLEISPRIMARY = 34;
    private static final int EXP_FETCHEDTROPHYCOUNT = 35;
    private static final int EXP_FETCHEDTROPHYTITLE = 36;
    private static final int EXP_FETCHEDTROPHYID = 37;
    private static final int EXP_FETCHEDTROPHYDESCRIPTION = 38;
    private static final int EXP_FETCHEDTROPHYDIFFICULTY = 39;
    private static final int EXP_FETCHEDTROPHYIMAGEURL = 40;
    private static final int EXP_FETCHEDTROPHYACHIEVED = 41;
    private static final int EXP_RETRIEVEDKEYDATA = 42;
    private static final int EXP_FETCHEDKEYCOUNT = 43;
    private static final int EXP_FETCHEDKEY = 44;
    private static final int EXP_UPDATEDKEYDATA = 45;
    private static final int EXP_FETCHEDFRIENDCOUNT = 46;
    private static final int EXP_FETCHEDFRIEND = 47;
    private static final int EXP_TIMEYEAR = 48;
    private static final int EXP_TIMEMONTH = 49;
    private static final int EXP_TIMEDAY = 50;
    private static final int EXP_TIMEHOUR = 51;
    private static final int EXP_TIMEMINUTE = 52;
    private static final int EXP_TIMESECOND = 53;
    private static final int EXP_TIMETIMESTAMP = 54;
    private static final int EXP_TIMETIMEZONE = 55;
    private static final int EXP_GETGAMEID = 56;
    private static final int EXP_GETPRIVATEKEY = 57;
    private static final int EXP_GETREQUESTURL = 58;
    private static final int EXP_FETCHEDSCORETABLEID = 59;
    private static final int EXP_GETERRORMESSAGE = 60;

    private String gameID = "";
    private String privateKey = "";
    private GameAuth gameAuthData = null;
    private Boolean hasFilesystem = false;

    private List triggerBuffer = new ArrayList();
    private ResponseTicket latestResponse = null;

    public CRunGamejoltGameAPI()
    {
        DarkEdif.checkSupportsSDKVersion(SDKVersion);
    }

    @Override
    public boolean createRunObject(CBinaryFile file, CCreateObjectInfo cob, int version)
    {
        if (ho == null)
        {
            throw new RuntimeException("HeaderObject not defined when needed to be.");
        }

        // DarkEdif properties are accessible as on other platforms: IsPropChecked(), GetPropertyStr(), GetPropertyNum()
        DarkEdifProperties props = DarkEdif.getProperties(this, file, version);
        gameID = props.GetPropertyStr(0);
        privateKey = props.GetPropertyStr(1);
        gameAuthData = (GameAuth)DarkEdif.getGlobalData("AuthInfo");
        if (gameAuthData == null)
        {
            gameAuthData = new GameAuth("", "", "");
            DarkEdif.setGlobalData("AuthInfo", gameAuthData);
        }

        // The return value is not used in this version of the runtime: always return false.
        return false;
    }

    @Override
    public int handleRunObject()
    {
        while (triggerBuffer.size() > 0)
        {
            latestResponse = (ResponseTicket)triggerBuffer.remove(0);

            // Error Handling!
            if (latestResponse.HasError)
            {
                ho.generateEvent(CND_ONERROR, 0);
                continue;
            }

            if (latestResponse.HasTrigger)
                ho.generateEvent(latestResponse.Trigger, 0);
            ho.generateEvent(CND_ANYCALLFINISHED, 0);
        }
        return 0;
    }

    private String md5(String input)
    {
        try
        {
            MessageDigest md = MessageDigest.getInstance("MD5");
            byte[] hash = md.digest(input.getBytes("UTF-8"));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.length; i++)
            {
                sb.append(String.format("%02x", hash[i] & 0xFF));
            }
            return sb.toString();
        }
        catch (Exception e)
        {
            throw new RuntimeException(e);
        }
    }

    private String serializeUrl(String url)
    {
        return url + "&signature=" + md5(JoltBase + url + privateKey);
    }

    private void httpGet(String url, String responseType)
    {
        httpGet(url, responseType, -1);
    }

    private void httpGet(String url, String responseType, int trigger)
    {
        final ResponseTicket responseTicket = new ResponseTicket(url, responseType);
        if (trigger != -1)
        {
            responseTicket.HasTrigger = true;
            responseTicket.Trigger = trigger;
        }

        final String fullUrl = JoltBase + serializeUrl(url);

        new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                try
                {
                    URL u = new URL(fullUrl);
                    HttpURLConnection conn = (HttpURLConnection) u.openConnection();
                    conn.setRequestMethod("GET");

                    BufferedReader reader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "UTF-8"));
                    StringBuilder sb = new StringBuilder();
                    String line;
                    while ((line = reader.readLine()) != null)
                    {
                        sb.append(line);
                    }
                    reader.close();

                    try
                    {
                        responseTicket.Data = Json.parse(sb.toString()).asObject();
                    }
                    catch (Exception ex)
                    {
                        responseTicket.HasError = true;
                        responseTicket.Error = ex.getMessage();
                    }
                }
                catch (Exception e)
                {
                    responseTicket.HasError = true;
                    responseTicket.Error = e.getMessage();
                }
                triggerBuffer.add(responseTicket);
            }
        }).start();
    }
    // Actions
    // -------------------------------------------------
    @Override
    public void action(int num, CActExtension act)
    {
        switch(num)
        {
            case ACT_AUTH:
                String actAuthUserName = act.getParamExpString(rh, 0);
                String actAuthUserToken = act.getParamExpString(rh, 1);
                actAuth(actAuthUserName, actAuthUserToken);
                break;
        }
    }

    private void actAuth(String userName, String userToken)
    {
        gameAuthData.UserName = userName;
        gameAuthData.UserToken = userToken;
        httpGet(
            "/api/game/v1_2/users/auth/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "Auth",
            CND_AUTHFINISHED
        );
    }
    
    // Conditions
    // --------------------------------------------------
    @Override
    public int getNumberOfConditions()
    {
        return 33;
    }

    @Override
    public boolean condition(int num, CCndExtension cnd)
    {
        switch (num)
        {
            case CND_AUTHFINISHED:
            case CND_FETCHFINISHED:
            case CND_OPENFINISHED:
            case CND_PINGFINISHED:
            case CND_CHECKFINISHED:
            case CND_CLOSEFINISHED:
            case CND_SCOREADDED:
            case CND_RANKINGRETRIEVED:
            case CND_SCORESFETCHED:
            case CND_TABLESRETRIEVED:
            case CND_TROPHIESRETRIEVED:
            case CND_TROPHYADDED:
            case CND_TROPHYREMOVED:
            case CND_GSGETDATA:
            case CND_GSGETKEYS:
            case CND_GSDELETEKEY:
            case CND_GSSETKEY:
            case CND_GSUPDATEKEY:
            case CND_USGETDATA:
            case CND_USGETKEYS:
            case CND_USDELETEKEY:
            case CND_USSETKEY:
            case CND_USUPDATEKEY:
            case CND_FGSSAVEDATA:
            case CND_FGSSETKEY:
            case CND_FGSUPDATEKEY:
            case CND_UGSSAVEDATA:
            case CND_UGSSETKEY:
            case CND_UGSUPDATEKEY:
            case CND_GETFRIENDSLIST:
            case CND_GETCURRENTTIME:
                return cndCallTriggered();
            case CND_ANYCALLFINISHED:
                return cndAnyCallTriggered();
            case CND_ONERROR:
                return cndCallTriggered();
        }
        return false;
    }

    private boolean cndAnyCallTriggered()
    {
        return true;
    }

    private boolean cndCallTriggered()
    {
        return true;
    }

    // Expressions
    // --------------------------------------------
    @Override
    public CValue expression(int num)
    {
        switch (num)
        {
            case EXP_GETJSONRESPONSE:
                return new CValue(expGetJsonResponse());
            case EXP_GETRESPONSESTATUS:
                return new CValue(expGetResponseStatus());
            case EXP_GETERRORMESSAGE:
                return new CValue(expGetErrorMessage());
        }
        return null;
    }

    private String expGetJsonResponse()
    {
        if (latestResponse.HasError)
            return "";
        return latestResponse.Data.toString();
    }

    private String expGetResponseStatus()
    {
        if (latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("success"))
            return "";

        return response.getString("success", "");
    }

    private String expGetErrorMessage()
    {
        if (!latestResponse.HasError)
            return "";
        return latestResponse.Error;
    }

    // Abstraction
    // ---------------------------------------------
    @Override
    public void displayRunObject(Graphics2D arg0)
    {

    }

    @Override
    public void destroyRunObject(boolean arg0)
    {

    }
}
