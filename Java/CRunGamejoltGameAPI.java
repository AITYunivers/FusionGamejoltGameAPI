

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
import Extensions.*;
import java.net.*;
import java.security.MessageDigest;
import java.util.ArrayList;
import java.util.List;
import GamejoltGameAPI.MinimalJson.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

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
            case ACT_AUTHCREDS:
                actAuthCreds();
                break;
            case ACT_SETGUESTNAME:
                String actSetGuestNameGuestName = act.getParamExpString(rh, 0);
                actSetGuestName(actSetGuestNameGuestName);
                break;
            case ACT_FETCHUSERNAME:
                String actFetchUsernameUserName = act.getParamExpString(rh, 0);
                actFetchUsername(actFetchUsernameUserName);
                break;
            case ACT_FETCHUSERID:
                int actFetchUserIDUserID = act.getParamExpression(rh, 0);
                actFetchUserID(actFetchUserIDUserID);
                break;
            case ACT_OPENSESSION:
                actOpenSession();
                break;
            case ACT_PINGSESSION:
                actPingSession();
                break;
            case ACT_PINGSTATUSSESSION:
                String actPingStatusSessionSession = act.getParamExpString(rh, 0);
                actPingStatusSession(actPingStatusSessionSession);
                break;
            case ACT_CHECKSESSION:
                actCheckSession();
                break;
            case ACT_CLOSESESSION:
                actCloseSession();
                break;
            case ACT_ADDUSERSCORE:
                String actAddUserScoreDisplayScore = act.getParamExpString(rh, 0);
                int actAddUserScoreSortScore = act.getParamExpression(rh, 1);
                int actAddUserScoreTable = act.getParamExpression(rh, 2);
                String actAddUserScoreExtraData = act.getParamExpString(rh, 3);
                actAddUserScore(actAddUserScoreDisplayScore, actAddUserScoreSortScore, actAddUserScoreTable, actAddUserScoreExtraData);
                break;
            case ACT_ADDGUESTSCORE:
                String actAddGuestScoreDisplayScore = act.getParamExpString(rh, 0);
                int actAddGuestScoreSortScore = act.getParamExpression(rh, 1);
                int actAddGuestScoreTable = act.getParamExpression(rh, 2);
                String actAddGuestScoreExtraData = act.getParamExpString(rh, 3);
                actAddGuestScore(actAddGuestScoreDisplayScore, actAddGuestScoreSortScore, actAddGuestScoreTable, actAddGuestScoreExtraData);
                break;
            case ACT_GETSCORERANKING:
                int actGetScoreRankingScore = act.getParamExpression(rh, 0);
                int actGetScoreRankingTable = act.getParamExpression(rh, 1);
                actGetScoreRanking(actGetScoreRankingScore, actGetScoreRankingTable);
                break;
            case ACT_FETCHSCORES:
                int actFetchScoresTable = act.getParamExpression(rh, 0);
                int actFetchScoresLimit = act.getParamExpression(rh, 1);
                int actFetchScoresBetterThan = act.getParamExpression(rh, 2);
                int actFetchScoresWorseThan = act.getParamExpression(rh, 3);
                actFetchScores(actFetchScoresTable, actFetchScoresLimit, actFetchScoresBetterThan, actFetchScoresWorseThan);
                break;
            case ACT_GETTABLES:
                actGetTables();
                break;
            case ACT_GETTROPHY:
                int actGetTrophyTrophy = act.getParamExpression(rh, 0);
                actGetTrophy(actGetTrophyTrophy);
                break;
            case ACT_GETTROPHIES:
                actGetTrophies();
                break;
            case ACT_GETUNLOCKEDTROPHIES:
                actGetUnlockedTrophies();
                break;
            case ACT_UNLOCKTROPHY:
                int actUnlockTrophyTrophy = act.getParamExpression(rh, 0);
                actUnlockTrophy(actUnlockTrophyTrophy);
                break;
            case ACT_LOCKTROPHY:
                int actLockTrophyTrophy = act.getParamExpression(rh, 0);
                actLockTrophy(actLockTrophyTrophy);
                break;
            case ACT_GLOBALSTORAGEGETDATA:
                String actGlobalStorageGetDataKey = act.getParamExpString(rh, 0);
                actGlobalStorageGetData(actGlobalStorageGetDataKey);
                break;
            case ACT_GLOBALSTORAGEGETKEYS:
                String actGlobalStorageGetKeysPattern = act.getParamExpString(rh, 0);
                actGlobalStorageGetKeys(actGlobalStorageGetKeysPattern);
                break;
            case ACT_GLOBALSTORAGEDELETEKEY:
                String actGlobalStorageDeleteKeyKey = act.getParamExpString(rh, 0);
                actGlobalStorageDeleteKey(actGlobalStorageDeleteKeyKey);
                break;
            case ACT_GLOBALSTORAGESETKEY:
                String actGlobalStorageSetKeyKey = act.getParamExpString(rh, 0);
                String actGlobalStorageSetKeyData = act.getParamExpString(rh, 1);
                actGlobalStorageSetKey(actGlobalStorageSetKeyKey, actGlobalStorageSetKeyData);
                break;
            case ACT_GLOBALSTORAGEUPDATEKEY:
                String actGlobalStorageUpdateKeyKey = act.getParamExpString(rh, 0);
                String actGlobalStorageUpdateKeyData = act.getParamExpString(rh, 1);
                String actGlobalStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
                actGlobalStorageUpdateKey(actGlobalStorageUpdateKeyKey, actGlobalStorageUpdateKeyData, actGlobalStorageUpdateKeyOperation);
                break;
            case ACT_USERSTORAGEGETDATA:
                String actUserStorageGetDataKey = act.getParamExpString(rh, 0);
                actUserStorageGetData(actUserStorageGetDataKey);
                break;
            case ACT_USERSTORAGEGETKEYS:
                String actUserStorageGetKeysPattern = act.getParamExpString(rh, 0);
                actUserStorageGetKeys(actUserStorageGetKeysPattern);
                break;
            case ACT_USERSTORAGEDELETEKEY:
                String actUserStorageDeleteKeyKey = act.getParamExpString(rh, 0);
                actUserStorageDeleteKey(actUserStorageDeleteKeyKey);
                break;
            case ACT_USERSTORAGESETKEY:
                String actUserStorageSetKeyKey = act.getParamExpString(rh, 0);
                String actUserStorageSetKeyData = act.getParamExpString(rh, 1);
                actUserStorageSetKey(actUserStorageSetKeyKey, actUserStorageSetKeyData);
                break;
            case ACT_USERSTORAGEUPDATEKEY:
                String actUserStorageUpdateKeyKey = act.getParamExpString(rh, 0);
                String actUserStorageUpdateKeyData = act.getParamExpString(rh, 1);
                String actUserStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
                actUserStorageUpdateKey(actUserStorageUpdateKeyKey, actUserStorageUpdateKeyData, actUserStorageUpdateKeyOperation);
                break;
            case ACT_GLOBALFILESTORAGESAVEDATA:
                String actGlobalFileStorageSaveDataKey = act.getParamExpString(rh, 0);
                String actGlobalFileStorageSaveDataFilePath = act.getParamExpString(rh, 1);
                actGlobalFileStorageSaveData(actGlobalFileStorageSaveDataKey, actGlobalFileStorageSaveDataFilePath);
                break;
            case ACT_GLOBALFILESTORAGESETKEY:
                String actGlobalFileStorageSetKeyKey = act.getParamExpString(rh, 0);
                String actGlobalFileStorageSetKeyFilePath = act.getParamExpString(rh, 1);
                actGlobalFileStorageSetKey(actGlobalFileStorageSetKeyKey, actGlobalFileStorageSetKeyFilePath);
                break;
            case ACT_GLOBALFILESTORAGEUPDATEKEY:
                String actGlobalFileStorageUpdateKeyKey = act.getParamExpString(rh, 0);
                String actGlobalFileStorageUpdateKeyFilePath = act.getParamExpString(rh, 1);
                String actGlobalFileStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
                actGlobalFileStorageUpdateKey(actGlobalFileStorageUpdateKeyKey, actGlobalFileStorageUpdateKeyFilePath, actGlobalFileStorageUpdateKeyOperation);
                break;
            case ACT_USERFILESTORAGESAVEDATA:
                String actUserFileStorageSaveDataKey = act.getParamExpString(rh, 0);
                String actUserFileStorageSaveDataFilePath = act.getParamExpString(rh, 1);
                actUserFileStorageSaveData(actUserFileStorageSaveDataKey, actUserFileStorageSaveDataFilePath);
                break;
            case ACT_USERFILESTORAGESETKEY:
                String actUserFileStorageSetKeyKey = act.getParamExpString(rh, 0);
                String actUserFileStorageSetKeyFilePath = act.getParamExpString(rh, 1);
                actUserFileStorageSetKey(actUserFileStorageSetKeyKey, actUserFileStorageSetKeyFilePath);
                break;
            case ACT_USERFILESTORAGEUPDATEKEY:
                String actUserFileStorageUpdateKeyKey = act.getParamExpString(rh, 0);
                String actUserFileStorageUpdateKeyFilePath = act.getParamExpString(rh, 1);
                String actUserFileStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
                actUserFileStorageUpdateKey(actUserFileStorageUpdateKeyKey, actUserFileStorageUpdateKeyFilePath, actUserFileStorageUpdateKeyOperation);
                break;
            case ACT_GETFRIENDSLIST:
                actGetFriendsList();
                break;
            case ACT_GETCURRENTTIME:
                actGetCurrentTime();
                break;
            case ACT_SETGAMEID:
                String actSetGameIDGameID = act.getParamExpString(rh, 0);
                actSetGameID(actSetGameIDGameID);
                break;
            case ACT_SETPRIVATEKEY:
                String actSetPrivateKeyPrivateKey = act.getParamExpString(rh, 0);
                actSetPrivateKey(actSetPrivateKeyPrivateKey);
                break;
            case ACT_FETCHUSERSCORES:
                int actFetchUserScoresTable = act.getParamExpression(rh, 0);
                int actFetchUserScoresLimit = act.getParamExpression(rh, 1);
                int actFetchUserScoresBetterThan = act.getParamExpression(rh, 2);
                int actFetchUserScoresWorseThan = act.getParamExpression(rh, 3);
                actFetchUserScores(actFetchUserScoresTable, actFetchUserScoresLimit, actFetchUserScoresBetterThan, actFetchUserScoresWorseThan);
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

    private void actAuthCreds()
    {
        String contents = null;
        try
        {
            File file = new File(".gj-credentials");
            if (file.exists())
            {
                BufferedReader reader = new BufferedReader(new InputStreamReader(new FileInputStream(file), "UTF-8"));
                StringBuilder sb = new StringBuilder();
                String line;
                while ((line = reader.readLine()) != null)
                    sb.append(line).append("\n");
                reader.close();
                contents = sb.toString();
            }
        }
        catch (Exception e)
        {
            triggerBuffer.add(ResponseTicket.MakeError(e.getMessage(), "Auth"));
            return;
        }

        if (contents == null || contents.length() == 0)
        {
            triggerBuffer.add(ResponseTicket.MakeError(".gj-credentials was empty!", "Auth"));
            return;
        }

        String[] lines = contents.split("\n");
        if (lines.length >= 3)
        {
            gameAuthData.UserName = lines[1].replace("\r", "");
            gameAuthData.UserToken = lines[2].replace("\r", "");

            httpGet(
                "/api/game/v1_2/users/auth/?game_id=" +
                gameID +
                "&username=" + gameAuthData.UserName +
                "&user_token=" + gameAuthData.UserToken,

                "Auth",
                CND_AUTHFINISHED
            );
        }
        else
        {
            triggerBuffer.add(ResponseTicket.MakeError(".gj-credentials was in an invalid format!", "Auth"));
        }
    }

    private void actSetGameID(String gameID)
    {
        this.gameID = gameID;
    }

    private void actSetPrivateKey(String privateKey)
    {
        this.privateKey = privateKey;
    }

    private void actSetGuestName(String guestName)
    {
        gameAuthData.GuestName = guestName;
    }

    private void actFetchUsername(String userName)
    {
        httpGet(
            "/api/game/v1_2/users/?game_id=" +
            gameID +
            "&username=" +
            userName,

            "FetchUsers",
            CND_FETCHFINISHED
        );
    }

    private void actFetchUserID(int userID)
    {
        httpGet(
            "/api/game/v1_2/users/?game_id=" +
            gameID +
            "&user_id=" +
            userID,

            "FetchUsers",
            CND_FETCHFINISHED
        );
    }

    private void actOpenSession()
    {
        httpGet(
            "/api/game/v1_2/sessions/open/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "OpenSession",
            CND_OPENFINISHED
        );
    }

    private void actPingSession()
    {
        httpGet(
            "/api/game/v1_2/sessions/ping/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "PingSession",
            CND_PINGFINISHED
        );
    }

    private void actPingStatusSession(String status)
    {
        httpGet(
            "/api/game/v1_2/sessions/ping/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&status=" +
            status,

            "PingSession",
            CND_PINGFINISHED
        );
    }

    private void actCheckSession()
    {
        httpGet(
            "/api/game/v1_2/sessions/check/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "CheckSession",
            CND_CHECKFINISHED
        );
    }

    private void actCloseSession()
    {
        httpGet(
            "/api/game/v1_2/sessions/close/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "CloseSession",
            CND_CLOSEFINISHED
        );
    }

    private void actAddUserScore(String displayScore, int sortScore, int table, String extraData)
    {
        String url =
            "/api/game/v1_2/scores/add/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&score=" +
            displayScore +
            "&sort=" +
            sortScore;
        if (table != -1)
            url += "&table_id=" + table;
        if (extraData.length() > 0)
            url += "&extra_data=" + extraData;

        httpGet(
            url,
            "AddScore",
            CND_SCOREADDED
        );
    }

    private void actAddGuestScore(String displayScore, int sortScore, int table, String extraData)
    {
        String url =
            "/api/game/v1_2/scores/add/?game_id=" +
            gameID +
            "&guest=" +
            gameAuthData.GuestName +
            "&score=" +
            displayScore +
            "&sort=" +
            sortScore;
        if (table != -1)
            url += "&table_id=" + table;
        if (extraData.length() > 0)
            url += "&extra_data=" + extraData;

        httpGet(
            url,
            "AddScore",
            CND_SCOREADDED
        );
    }

    private void actGetScoreRanking(int score, int table)
    {
        String url =
            "/api/game/v1_2/scores/get-rank/?game_id=" +
            gameID +
            "&sort=" +
            score;
        if (table != -1)
            url += "&table_id=" + table;

        httpGet(
            url,
            "GetRank",
            CND_RANKINGRETRIEVED
        );
    }

    private void actFetchScores(int table, int limit, int betterThan, int worseThan)
    {
        String url = "/api/game/v1_2/scores/?game_id=" + gameID;
        if (table != -1)
            url += "&table_id=" + table;
        if (limit != -1)
            url += "&limit=" + limit;
        if (betterThan != -1)
            url += "&better_than=" + betterThan;
        if (worseThan != -1)
            url += "&worse_than=" + worseThan;

        httpGet(
            url,
            "FetchScores",
            CND_SCORESFETCHED
        );
    }

    private void actFetchUserScores(int table, int limit, int betterThan, int worseThan)
    {
        String url = "/api/game/v1_2/scores/?game_id=" + gameID;
        if (gameAuthData.UserName.length() > 0 && gameAuthData.UserToken.length() > 0)
            url += "&username=" + gameAuthData.UserName + "&user_token=" + gameAuthData.UserToken;
        else if (gameAuthData.GuestName.length() > 0)
            url += "&guest=" + gameAuthData.GuestName;

        if (table != -1)
            url += "&table_id=" + table;
        if (limit != -1)
            url += "&limit=" + limit;
        if (betterThan != -1)
            url += "&better_than=" + betterThan;
        if (worseThan != -1)
            url += "&worse_than=" + worseThan;

        httpGet(
            url,
            "FetchScores",
            CND_SCORESFETCHED
        );
    }

    private void actGetTables()
    {
        httpGet(
            "/api/game/v1_2/scores/tables/?game_id=" +
            gameID,

            "ScoreTables",
            CND_TABLESRETRIEVED
        );
    }

    private void actGetTrophy(int trophy)
    {
        httpGet(
            "/api/game/v1_2/trophies/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&trophy_id=" +
            trophy,

            "FetchTrophies",
            CND_TROPHIESRETRIEVED
        );
    }

    private void actGetTrophies()
    {
        httpGet(
            "/api/game/v1_2/trophies/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "FetchTrophies",
            CND_TROPHIESRETRIEVED
        );
    }

    private void actGetUnlockedTrophies()
    {
        httpGet(
            "/api/game/v1_2/trophies/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&achieved=true",

            "FetchTrophies",
            CND_TROPHIESRETRIEVED
        );
    }

    private void actUnlockTrophy(int trophy)
    {
        httpGet(
            "/api/game/v1_2/trophies/add-achieved/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&trophy_id=" +
            trophy,

            "AchieveTrophy",
            CND_TROPHYADDED
        );
    }

    private void actLockTrophy(int trophy)
    {
        httpGet(
            "/api/game/v1_2/trophies/remove-achieved/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&trophy_id=" +
            trophy,

            "RevokeTrophy",
            CND_TROPHYREMOVED
        );
    }

    private void actGlobalStorageGetData(String key)
    {
        httpGet(
            "/api/game/v1_2/data-store/?game_id=" +
            gameID +
            "&key=" +
            key,

            "FetchData",
            CND_GSGETDATA
        );
    }

    private void actGlobalStorageGetKeys(String pattern)
    {
        String url = "/api/game/v1_2/data-store/get-keys/?game_id=" + gameID;
        if (pattern.length() > 0)
            url += "&pattern=" + pattern;

        httpGet(
            url,
            "GetDataKeys",
            CND_GSGETKEYS
        );
    }

    private void actGlobalStorageDeleteKey(String key)
    {
        httpGet(
            "/api/game/v1_2/data-store/remove/?game_id=" +
            gameID +
            "&key=" +
            key,

            "RemoveData",
            CND_GSDELETEKEY
        );
    }

    private void actGlobalStorageSetKey(String key, String data)
    {
        httpGet(
            "/api/game/v1_2/data-store/set/?game_id=" +
            gameID +
            "&key=" +
            key +
            "&data=" +
            data,

            "SetData",
            CND_GSSETKEY
        );
    }

    private void actGlobalStorageUpdateKey(String key, String data, String operation)
    {
        httpGet(
            "/api/game/v1_2/data-store/update/?game_id=" +
            gameID +
            "&key=" +
            key +
            "&value=" +
            data +
            "&operation=" +
            operation,

            "UpdateData",
            CND_GSUPDATEKEY
        );
    }

    private void actUserStorageGetData(String key)
    {
        httpGet(
            "/api/game/v1_2/data-store/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&key=" +
            key,

            "FetchData",
            CND_USGETDATA
        );
    }

    private void actUserStorageGetKeys(String pattern)
    {
        String url =
            "/api/game/v1_2/data-store/get-keys/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken;
        if (pattern.length() > 0)
            url += "&pattern=" + pattern;

        httpGet(
            url,
            "GetDataKeys",
            CND_USGETKEYS
        );
    }

    private void actUserStorageDeleteKey(String key)
    {
        httpGet(
            "/api/game/v1_2/data-store/remove/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&key=" +
            key,

            "RemoveData",
            CND_USDELETEKEY
        );
    }

    private void actUserStorageSetKey(String key, String data)
    {
        httpGet(
            "/api/game/v1_2/data-store/set/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&key=" +
            key +
            "&data=" +
            data,

            "SetData",
            CND_USSETKEY
        );
    }

    private void actUserStorageUpdateKey(String key, String data, String operation)
    {
        httpGet(
            "/api/game/v1_2/data-store/update/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken +
            "&key=" +
            key +
            "&value=" +
            data +
            "&operation=" +
            operation,

            "UpdateData",
            CND_USUPDATEKEY
        );
    }

    private void actGlobalFileStorageSaveData(String key, String filePath)
    {
        // No implementation
    }

    private void actGlobalFileStorageSetKey(String key, String filePath)
    {
        // No implementation
    }

    private void actGlobalFileStorageUpdateKey(String key, String filePath, String operation)
    {
        // No implementation
    }

    private void actUserFileStorageSaveData(String key, String filePath)
    {
        // No implementation
    }

    private void actUserFileStorageSetKey(String key, String filePath)
    {
        // No implementation
    }

    private void actUserFileStorageUpdateKey(String key, String filePath, String operation)
    {
        // No implementation
    }

    private void actGetFriendsList()
    {
        httpGet(
            "/api/game/v1_2/friends/?game_id=" +
            gameID +
            "&username=" +
            gameAuthData.UserName +
            "&user_token=" +
            gameAuthData.UserToken,

            "Friends",
            CND_GETFRIENDSLIST
        );
    }

    private void actGetCurrentTime()
    {
        httpGet(
            "/api/game/v1_2/time/?game_id=" +
            gameID,

            "Time",
            CND_GETCURRENTTIME
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
            case EXP_GETRESPONSETYPE:
                return new CValue(expGetResponseType());
            case EXP_GETRESPONSESTATUS:
                return new CValue(expGetResponseStatus());
            case EXP_GETRESPONSEMESSAGE:
                return new CValue(expGetResponseMessage());
            case EXP_GETUSERNAME:
                return new CValue(expGetUserName());
            case EXP_GETUSERTOKEN:
                return new CValue(expGetUserToken());
            case EXP_GETGUESTNAME:
                return new CValue(expGetGuestName());
            case EXP_FETCHEDUSERCOUNT:
                return new CValue(expFetchedUserCount());
            case EXP_FETCHEDUSERDISPLAYNAME:
                int expFetchedUserDisplayNameIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserDisplayName(expFetchedUserDisplayNameIndex));
            case EXP_FETCHEDUSERNAME:
                int expFetchedUsernameIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUsername(expFetchedUsernameIndex));
            case EXP_FETCHEDUSERID:
                int expFetchedUserIDIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserID(expFetchedUserIDIndex));
            case EXP_FETCHEDUSERDESCRIPTION:
                int expFetchedUserDescriptionIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserDescription(expFetchedUserDescriptionIndex));
            case EXP_FETCHEDUSERAVATAR:
                int expFetchedUserAvatarIndex = ho.getExpParam().getInt();
                int expFetchedUserAvatarResolution = ho.getExpParam().getInt();
                return new CValue(expFetchedUserAvatar(expFetchedUserAvatarIndex, expFetchedUserAvatarResolution));
            case EXP_FETCHEDUSERWEBSITE:
                int expFetchedUserWebsiteIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserWebsite(expFetchedUserWebsiteIndex));
            case EXP_FETCHEDUSERSTATUS:
                int expFetchedUserStatusIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserStatus(expFetchedUserStatusIndex));
            case EXP_FETCHEDUSERTYPE:
                int expFetchedUserTypeIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserType(expFetchedUserTypeIndex));
            case EXP_FETCHEDUSERLASTLOGGEDIN:
                int expFetchedUserLastLoggedInIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserLastLoggedIn(expFetchedUserLastLoggedInIndex));
            case EXP_FETCHEDUSERLASTLOGGEDINTIMESTAMP:
                int expFetchedUserLastLoggedInTimestampIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserLastLoggedInTimestamp(expFetchedUserLastLoggedInTimestampIndex));
            case EXP_FETCHEDUSERSIGNEDUP:
                int expFetchedUserSignedUpIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserSignedUp(expFetchedUserSignedUpIndex));
            case EXP_FETCHEDUSERSIGNEDUPTIMESTAMP:
                int expFetchedUserSignedUpTimestampIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedUserSignedUpTimestamp(expFetchedUserSignedUpTimestampIndex));
            case EXP_SCORERANKING:
                return new CValue(expScoreRanking());
            case EXP_FETCHEDSCORECOUNT:
                return new CValue(expFetchedScoreCount());
            case EXP_FETCHEDSCOREUSERNAME:
                int expFetchedScoreUsernameIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreUsername(expFetchedScoreUsernameIndex));
            case EXP_FETCHEDSCOREUSERID:
                int expFetchedScoreUserIDIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreUserID(expFetchedScoreUserIDIndex));
            case EXP_FETCHEDSCOREGUESTNAME:
                int expFetchedScoreGuestNameIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreGuestName(expFetchedScoreGuestNameIndex));
            case EXP_FETCHEDSCORESCORE:
                int expFetchedScoreScoreIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreScore(expFetchedScoreScoreIndex));
            case EXP_FETCHEDSCORESORT:
                int expFetchedScoreSortIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreSort(expFetchedScoreSortIndex));
            case EXP_FETCHEDSCOREEXTRADATA:
                int expFetchedScoreExtraDataIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreExtraData(expFetchedScoreExtraDataIndex));
            case EXP_FETCHEDSCORESUBMIT:
                int expFetchedScoreSubmitIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreSubmit(expFetchedScoreSubmitIndex));
            case EXP_FETCHEDSCORESUBMITTIMESTAMP:
                int expFetchedScoreSubmitTimestampIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedScoreSubmitTimestamp(expFetchedScoreSubmitTimestampIndex));
            case EXP_FETCHEDTABLECOUNT:
                return new CValue(expFetchedTableCount());
            case EXP_FETCHEDTABLENAME:
                int expFetchedTableNameIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTableName(expFetchedTableNameIndex));
            case EXP_FETCHEDTABLEID:
                int expFetchedTableIDIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTableID(expFetchedTableIDIndex));
            case EXP_FETCHEDTABLEDESCRIPTION:
                int expFetchedTableDescriptionIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTableDescription(expFetchedTableDescriptionIndex));
            case EXP_FETCHEDTABLEISPRIMARY:
                int expFetchedTableIsPrimaryIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTableIsPrimary(expFetchedTableIsPrimaryIndex));
            case EXP_FETCHEDTROPHYCOUNT:
                return new CValue(expFetchedTrophyCount());
            case EXP_FETCHEDTROPHYTITLE:
                int expFetchedTrophyTitleIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyTitle(expFetchedTrophyTitleIndex));
            case EXP_FETCHEDTROPHYID:
                int expFetchedTrophyIDIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyID(expFetchedTrophyIDIndex));
            case EXP_FETCHEDTROPHYDESCRIPTION:
                int expFetchedTrophyDescriptionIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyDescription(expFetchedTrophyDescriptionIndex));
            case EXP_FETCHEDTROPHYDIFFICULTY:
                int expFetchedTrophyDifficultyIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyDifficulty(expFetchedTrophyDifficultyIndex));
            case EXP_FETCHEDTROPHYIMAGEURL:
                int expFetchedTrophyImageURLIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyImageURL(expFetchedTrophyImageURLIndex));
            case EXP_FETCHEDTROPHYACHIEVED:
                int expFetchedTrophyAchievedIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedTrophyAchieved(expFetchedTrophyAchievedIndex));
            case EXP_RETRIEVEDKEYDATA:
                return new CValue(expRetrievedKeyData());
            case EXP_FETCHEDKEYCOUNT:
                return new CValue(expFetchedKeyCount());
            case EXP_FETCHEDKEY:
                int expFetchedKeyIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedKey(expFetchedKeyIndex));
            case EXP_UPDATEDKEYDATA:
                return new CValue(expUpdatedKeyData());
            case EXP_FETCHEDFRIENDCOUNT:
                return new CValue(expFetchedFriendCount());
            case EXP_FETCHEDFRIEND:
                int expFetchedFriendIndex = ho.getExpParam().getInt();
                return new CValue(expFetchedFriend(expFetchedFriendIndex));
            case EXP_TIMEYEAR:
                return new CValue(expTimeYear());
            case EXP_TIMEMONTH:
                return new CValue(expTimeMonth());
            case EXP_TIMEDAY:
                return new CValue(expTimeDay());
            case EXP_TIMEHOUR:
                return new CValue(expTimeHour());
            case EXP_TIMEMINUTE:
                return new CValue(expTimeMinute());
            case EXP_TIMESECOND:
                return new CValue(expTimeSecond());
            case EXP_TIMETIMESTAMP:
                return new CValue(expTimeTimestamp());
            case EXP_TIMETIMEZONE:
                return new CValue(expTimeTimezone());
            case EXP_GETGAMEID:
                return new CValue(expGetGameID());
            case EXP_GETPRIVATEKEY:
                return new CValue(expGetPrivateKey());
            case EXP_GETREQUESTURL:
                return new CValue(expGetRequestURL());
            case EXP_FETCHEDSCORETABLEID:
                return new CValue(expFetchedScoreTableID());
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

    private String expGetResponseType()
    {
        return latestResponse.Type;
    }

    private String expGetResponseStatus()
    {
        if (latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        return response.getString("success", "");
    }

    private String expGetResponseMessage()
    {
        if (latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        return response.getString("message", "");
    }

    private String expGetErrorMessage()
    {
        if (!latestResponse.HasError)
            return "";
        return latestResponse.Error;
    }

    private String expGetGameID()
    {
        return gameID;
    }

    private String expGetPrivateKey()
    {
        return privateKey;
    }

    private String expGetRequestURL()
    {
        return JoltBase + latestResponse.URL;
    }

    private String expGetUserName()
    {
        return gameAuthData.UserName;
    }

    private String expGetUserToken()
    {
        return gameAuthData.UserToken;
    }

    private String expGetGuestName()
    {
        return gameAuthData.GuestName;
    }

    private int expFetchedUserCount()
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return 0;

        return response.get("users").asArray().size();
    }

    private String expFetchedUserDisplayName(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("developer_name", "");
    }

    private String expFetchedUsername(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("username", "");
    }

    private int expFetchedUserID(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return 0;

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return 0;

        JsonObject user = users.get(index).asObject();
        return Integer.parseInt(user.getString("id", "0"));
    }

    private String expFetchedUserDescription(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("developer_description", "");
    }

    private String expFetchedUserAvatar(int index, int resolution)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        String avatar_url = user.getString("avatar_url", "");
        return "https://m.gjcdn.net/user-avatar/" + resolution + avatar_url.substring(34, avatar_url.lastIndexOf(".")) + ".png";
    }

    private String expFetchedUserWebsite(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("developer_website", "");
    }

    private String expFetchedUserStatus(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("status", "");
    }

    private String expFetchedUserType(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("type", "");
    }

    private String expFetchedUserLastLoggedIn(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("last_logged_in", "");
    }

    private int expFetchedUserLastLoggedInTimestamp(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return 0;

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return 0;

        JsonObject user = users.get(index).asObject();
        return user.getInt("last_logged_in_timestamp", 0);
    }

    private String expFetchedUserSignedUp(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return "";

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return "";

        JsonObject user = users.get(index).asObject();
        return user.getString("signed_up", "");
    }

    private int expFetchedUserSignedUpTimestamp(int index)
    {
        if (!latestResponse.Type.equals("FetchUsers") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("users") || !response.get("users").isArray())
            return 0;

        JsonArray users = response.get("users").asArray();
        if (users.size() <= index)
            return 0;

        JsonObject user = users.get(index).asObject();
        return user.getInt("signed_up_timestamp", 0);
    }

    private int expScoreRanking()
    {
        if (!latestResponse.Type.equals("GetRank") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return response.getInt("rank", 0);
    }

    private int expFetchedScoreCount()
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return 0;

        return response.get("scores").asArray().size();
    }

    private String expFetchedScoreUsername(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return "";

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return "";

        JsonObject score = scores.get(index).asObject();
        return score.getString("user", "");
    }

    private int expFetchedScoreUserID(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return 0;

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return 0;

        JsonObject score = scores.get(index).asObject();
        return Integer.parseInt(score.getString("user_id", "0"));
    }

    private String expFetchedScoreGuestName(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return "";

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return "";

        JsonObject score = scores.get(index).asObject();
        return score.getString("guest", "");
    }

    private String expFetchedScoreScore(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return "";

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return "";

        JsonObject score = scores.get(index).asObject();
        return score.getString("score", "");
    }

    private int expFetchedScoreSort(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return 0;

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return 0;

        JsonObject score = scores.get(index).asObject();
        return Integer.parseInt(score.getString("sort", "0"));
    }

    private String expFetchedScoreExtraData(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return "";

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return "";

        JsonObject score = scores.get(index).asObject();
        return score.getString("extra_data", "");
    }

    private String expFetchedScoreSubmit(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return "";

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return "";

        JsonObject score = scores.get(index).asObject();
        return score.getString("stored", "");
    }

    private int expFetchedScoreSubmitTimestamp(int index)
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("scores") || !response.get("scores").isArray())
            return 0;

        JsonArray scores = response.get("scores").asArray();
        if (scores.size() <= index)
            return 0;

        JsonObject score = scores.get(index).asObject();
        return score.getInt("stored_timestamp", 0);
    }

    private int expFetchedScoreTableID()
    {
        if (!latestResponse.Type.equals("FetchScores") || latestResponse.HasError)
            return 0;

        Matcher match = Pattern.compile("table_id=([0-9]+)").matcher(latestResponse.URL);
        return match.find() ? Integer.parseInt(match.group(1)) : 0;
    }

    private int expFetchedTableCount()
    {
        if (!latestResponse.Type.equals("ScoreTables") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("tables") || !response.get("tables").isArray())
            return 0;

        return response.get("tables").asArray().size();
    }

    private String expFetchedTableName(int index)
    {
        if (!latestResponse.Type.equals("ScoreTables") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("tables") || !response.get("tables").isArray())
            return "";

        JsonArray tables = response.get("tables").asArray();
        if (tables.size() <= index)
            return "";

        JsonObject table = tables.get(index).asObject();
        return table.getString("name", "");
    }

    private int expFetchedTableID(int index)
    {
        if (!latestResponse.Type.equals("ScoreTables") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("tables") || !response.get("tables").isArray())
            return 0;

        JsonArray tables = response.get("tables").asArray();
        if (tables.size() <= index)
            return 0;

        JsonObject table = tables.get(index).asObject();
        return Integer.parseInt(table.getString("id", "0"));
    }

    private String expFetchedTableDescription(int index)
    {
        if (!latestResponse.Type.equals("ScoreTables") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("tables") || !response.get("tables").isArray())
            return "";

        JsonArray tables = response.get("tables").asArray();
        if (tables.size() <= index)
            return "";

        JsonObject table = tables.get(index).asObject();
        return table.getString("description", "");
    }

    private String expFetchedTableIsPrimary(int index)
    {
        if (!latestResponse.Type.equals("ScoreTables") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("tables") || !response.get("tables").isArray())
            return "";

        JsonArray tables = response.get("tables").asArray();
        if (tables.size() <= index)
            return "";

        JsonObject table = tables.get(index).asObject();
        return table.getString("primary", "");
    }

    private int expFetchedTrophyCount()
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return 0;

        return response.get("trophies").asArray().size();
    }

    private String expFetchedTrophyTitle(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return "";

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return "";

        JsonObject trophy = trophies.get(index).asObject();
        return trophy.getString("title", "");
    }
    
    private int expFetchedTrophyID(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return 0;

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return 0;

        JsonObject trophy = trophies.get(index).asObject();
        return Integer.parseInt(trophy.getString("id", "0"));
    }

    private String expFetchedTrophyDescription(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return "";

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return "";

        JsonObject trophy = trophies.get(index).asObject();
        return trophy.getString("description", "");
    }

    private String expFetchedTrophyDifficulty(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return "";

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return "";

        JsonObject trophy = trophies.get(index).asObject();
        return trophy.getString("difficulty", "");
    }

    private String expFetchedTrophyImageURL(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return "";

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return "";

        JsonObject trophy = trophies.get(index).asObject();
        return trophy.getString("image_url", "");
    }

    private String expFetchedTrophyAchieved(int index)
    {
        if (!latestResponse.Type.equals("FetchTrophies") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("trophies") || !response.get("trophies").isArray())
            return "";

        JsonArray trophies = response.get("trophies").asArray();
        if (trophies.size() <= index)
            return "";

        JsonObject trophy = trophies.get(index).asObject();
        return trophy.getString("achieved", "");
    }

    private String expRetrievedKeyData()
    {
        if (!latestResponse.Type.equals("FetchData") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        return response.getString("data", "");
    }

    private int expFetchedKeyCount()
    {
        if (!latestResponse.Type.equals("GetDataKeys") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("keys") || !response.get("keys").isArray())
            return 0;

        return response.get("keys").asArray().size();
    }

    private String expFetchedKey(int index)
    {
        if (!latestResponse.Type.equals("GetDataKeys") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        if (!response.contains("keys") || !response.get("keys").isArray())
            return "";

        JsonArray keys = response.get("keys").asArray();
        if (keys.size() <= index)
            return "";

        JsonObject key = keys.get(index).asObject();
        return key.getString("key", "");
    }

    private String expUpdatedKeyData()
    {
        if (!latestResponse.Type.equals("UpdateData") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        return response.getString("data", "");
    }

    private int expFetchedFriendCount()
    {
        if (!latestResponse.Type.equals("Friends") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("friends") || !response.get("friends").isArray())
            return 0;

        return response.get("friends").asArray().size();
    }

    private int expFetchedFriend(int index)
    {
        if (!latestResponse.Type.equals("Friends") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        if (!response.contains("friends") || !response.get("friends").isArray())
            return 0;

        JsonArray friends = response.get("friends").asArray();
        if (friends.size() <= index)
            return 0;

        JsonObject friend = friends.get(index).asObject();
        return Integer.parseInt(friend.getString("friend_id", "0"));
    }

    private int expTimeYear()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("year", "0"));
    }

    private int expTimeMonth()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("month", "0"));
    }

    private int expTimeDay()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("day", "0"));
    }

    private int expTimeHour()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("hour", "0"));
    }

    private int expTimeMinute()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("minute", "0"));
    }

    private int expTimeSecond()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return Integer.parseInt(response.getString("second", "0"));
    }

    private int expTimeTimestamp()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return 0;

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return 0;

        JsonObject response = j.get("response").asObject();
        return response.getInt("timestamp", 0);
    }

    private String expTimeTimezone()
    {
        if (!latestResponse.Type.equals("Time") || latestResponse.HasError)
            return "";

        JsonObject j = latestResponse.Data;
        if (!j.contains("response"))
            return "";

        JsonObject response = j.get("response").asObject();
        return response.getString("timezone", "");
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
