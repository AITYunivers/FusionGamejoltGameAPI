#if NETFX_CORE
#define SIMPLE_JSON_TYPEINFO
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using RuntimeXNA.Extensions;
using RuntimeXNA.Services;
using RuntimeXNA.RunLoop;
using RuntimeXNA.Sprites;
using RuntimeXNA.Conditions;
using RuntimeXNA.Actions;
using RuntimeXNA.Expressions;
using RuntimeXNA.Objects;
using RuntimeXNA.Params;
using RuntimeXNA.Frame;
using RuntimeXNA.OI;
using RuntimeXNA.Movements;
using RuntimeXNA.Application;
using System.Reflection;
using System.Security.Cryptography;
using System.Net;
using System.IO.IsolatedStorage;

using System.CodeDom.Compiler;
using System.Collections;
#if !SIMPLE_JSON_NO_LINQ_EXPRESSION
using System.Linq.Expressions;
#endif
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
#if SIMPLE_JSON_DYNAMIC
using System.Dynamic;
#endif
using System.Globalization;
using System.Runtime.Serialization;
using SimpleJson.Reflection;
using System.Text.RegularExpressions;

namespace RuntimeXNA.Extensions
{
    class CRunGamejoltGameAPI : CRunExtension
    {
        public const int ExtensionVersion = 7;
        public const int SDKVersion = 20;
        
		public const string JoltBase = "https://api.gamejolt.com";
		private const int ACT_AUTH = 0;
		private const int ACT_AUTHCREDS = 1;
		private const int ACT_SETGUESTNAME = 2;
		private const int ACT_FETCHUSERNAME = 3;
		private const int ACT_FETCHUSERID = 4;
		private const int ACT_OPENSESSION = 5;
		private const int ACT_PINGSESSION = 6;
		private const int ACT_PINGSTATUSSESSION = 7;
		private const int ACT_CHECKSESSION = 8;
		private const int ACT_CLOSESESSION = 9;
		private const int ACT_ADDUSERSCORE = 10;
		private const int ACT_ADDGUESTSCORE = 11;
		private const int ACT_GETSCORERANKING = 12;
		private const int ACT_FETCHSCORES = 13;
		private const int ACT_GETTABLES = 14;
		private const int ACT_GETTROPHY = 15;
		private const int ACT_GETTROPHIES = 16;
		private const int ACT_GETUNLOCKEDTROPHIES = 17;
		private const int ACT_UNLOCKTROPHY = 18;
		private const int ACT_LOCKTROPHY = 19;
		private const int ACT_GLOBALSTORAGEGETDATA = 20;
		private const int ACT_GLOBALSTORAGEGETKEYS = 21;
		private const int ACT_GLOBALSTORAGEDELETEKEY = 22;
		private const int ACT_GLOBALSTORAGESETKEY = 23;
		private const int ACT_GLOBALSTORAGEUPDATEKEY = 24;
		private const int ACT_USERSTORAGEGETDATA = 25;
		private const int ACT_USERSTORAGEGETKEYS = 26;
		private const int ACT_USERSTORAGEDELETEKEY = 27;
		private const int ACT_USERSTORAGESETKEY = 28;
		private const int ACT_USERSTORAGEUPDATEKEY = 29;
		private const int ACT_GLOBALFILESTORAGESAVEDATA = 30;
		private const int ACT_GLOBALFILESTORAGESETKEY = 31;
		private const int ACT_GLOBALFILESTORAGEUPDATEKEY = 32;
		private const int ACT_USERFILESTORAGESAVEDATA = 33;
		private const int ACT_USERFILESTORAGESETKEY = 34;
		private const int ACT_USERFILESTORAGEUPDATEKEY = 35;
		private const int ACT_GETFRIENDSLIST = 36;
		private const int ACT_GETCURRENTTIME = 37;
		private const int ACT_SETGAMEID = 38;
		private const int ACT_SETPRIVATEKEY = 39;
		private const int ACT_FETCHUSERSCORES = 40;
		private const int CND_AUTHFINISHED = 0;
		private const int CND_FETCHFINISHED = 1;
		private const int CND_OPENFINISHED = 2;
		private const int CND_PINGFINISHED = 3;
		private const int CND_CHECKFINISHED = 4;
		private const int CND_CLOSEFINISHED = 5;
		private const int CND_SCOREADDED = 6;
		private const int CND_RANKINGRETRIEVED = 7;
		private const int CND_SCORESFETCHED = 8;
		private const int CND_TABLESRETRIEVED = 9;
		private const int CND_TROPHIESRETRIEVED = 10;
		private const int CND_TROPHYADDED = 11;
		private const int CND_TROPHYREMOVED = 12;
		private const int CND_GSGETDATA = 13;
		private const int CND_GSGETKEYS = 14;
		private const int CND_GSDELETEKEY = 15;
		private const int CND_GSSETKEY = 16;
		private const int CND_GSUPDATEKEY = 17;
		private const int CND_USGETDATA = 18;
		private const int CND_USGETKEYS = 19;
		private const int CND_USDELETEKEY = 20;
		private const int CND_USSETKEY = 21;
		private const int CND_USUPDATEKEY = 22;
		private const int CND_FGSSAVEDATA = 23;
		private const int CND_FGSSETKEY = 24;
		private const int CND_FGSUPDATEKEY = 25;
		private const int CND_UGSSAVEDATA = 26;
		private const int CND_UGSSETKEY = 27;
		private const int CND_UGSUPDATEKEY = 28;
		private const int CND_GETFRIENDSLIST = 29;
		private const int CND_GETCURRENTTIME = 30;
		private const int CND_ANYCALLFINISHED = 31;
		private const int CND_ONERROR = 32;
		private const int EXP_GETJSONRESPONSE = 0;
		private const int EXP_GETRESPONSETYPE = 1;
		private const int EXP_GETRESPONSESTATUS = 2;
		private const int EXP_GETRESPONSEMESSAGE = 3;
		private const int EXP_GETUSERNAME = 4;
		private const int EXP_GETUSERTOKEN = 5;
		private const int EXP_GETGUESTNAME = 6;
		private const int EXP_FETCHEDUSERCOUNT = 7;
		private const int EXP_FETCHEDUSERDISPLAYNAME = 8;
		private const int EXP_FETCHEDUSERNAME = 9;
		private const int EXP_FETCHEDUSERID = 10;
		private const int EXP_FETCHEDUSERDESCRIPTION = 11;
		private const int EXP_FETCHEDUSERAVATAR = 12;
		private const int EXP_FETCHEDUSERWEBSITE = 13;
		private const int EXP_FETCHEDUSERSTATUS = 14;
		private const int EXP_FETCHEDUSERTYPE = 15;
		private const int EXP_FETCHEDUSERLASTLOGGEDIN = 16;
		private const int EXP_FETCHEDUSERLASTLOGGEDINTIMESTAMP = 17;
		private const int EXP_FETCHEDUSERSIGNEDUP = 18;
		private const int EXP_FETCHEDUSERSIGNEDUPTIMESTAMP = 19;
		private const int EXP_SCORERANKING = 20;
		private const int EXP_FETCHEDSCORECOUNT = 21;
		private const int EXP_FETCHEDSCOREUSERNAME = 22;
		private const int EXP_FETCHEDSCOREUSERID = 23;
		private const int EXP_FETCHEDSCOREGUESTNAME = 24;
		private const int EXP_FETCHEDSCORESCORE = 25;
		private const int EXP_FETCHEDSCORESORT = 26;
		private const int EXP_FETCHEDSCOREEXTRADATA = 27;
		private const int EXP_FETCHEDSCORESUBMIT = 28;
		private const int EXP_FETCHEDSCORESUBMITTIMESTAMP = 29;
		private const int EXP_FETCHEDTABLECOUNT = 30;
		private const int EXP_FETCHEDTABLENAME = 31;
		private const int EXP_FETCHEDTABLEID = 32;
		private const int EXP_FETCHEDTABLEDESCRIPTION = 33;
		private const int EXP_FETCHEDTABLEISPRIMARY = 34;
		private const int EXP_FETCHEDTROPHYCOUNT = 35;
		private const int EXP_FETCHEDTROPHYTITLE = 36;
		private const int EXP_FETCHEDTROPHYID = 37;
		private const int EXP_FETCHEDTROPHYDESCRIPTION = 38;
		private const int EXP_FETCHEDTROPHYDIFFICULTY = 39;
		private const int EXP_FETCHEDTROPHYIMAGEURL = 40;
		private const int EXP_FETCHEDTROPHYACHIEVED = 41;
		private const int EXP_RETRIEVEDKEYDATA = 42;
		private const int EXP_FETCHEDKEYCOUNT = 43;
		private const int EXP_FETCHEDKEY = 44;
		private const int EXP_UPDATEDKEYDATA = 45;
		private const int EXP_FETCHEDFRIENDCOUNT = 46;
		private const int EXP_FETCHEDFRIEND = 47;
		private const int EXP_TIMEYEAR = 48;
		private const int EXP_TIMEMONTH = 49;
		private const int EXP_TIMEDAY = 50;
		private const int EXP_TIMEHOUR = 51;
		private const int EXP_TIMEMINUTE = 52;
		private const int EXP_TIMESECOND = 53;
		private const int EXP_TIMETIMESTAMP = 54;
		private const int EXP_TIMETIMEZONE = 55;
		private const int EXP_GETGAMEID = 56;
		private const int EXP_GETPRIVATEKEY = 57;
		private const int EXP_GETREQUESTURL = 58;
		private const int EXP_FETCHEDSCORETABLEID = 59;
		private const int EXP_GETERRORMESSAGE = 60;
        
        private string gameID = string.Empty;
        private string privateKey = string.Empty;
        private GameAuth gameAuthData = null;
		private bool hasFilesystem = false;
        
		private List<ResponseTicket> triggerBuffer = new List<ResponseTicket>();
		private ResponseTicket latestResponse = null;

        public CRunGamejoltGameAPI()
        {
            DarkEdif.checkSupportsSDKVersion(SDKVersion);
        }

        public override bool createRunObject(CFile file, CCreateObjectInfo cob, int version)
        {
            if (ho == null)
            {
                throw new Exception("HeaderObject not defined when needed to be.");
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

        public override int handleRunObject()
        {
            while (triggerBuffer.Count > 0)
            {
                latestResponse = triggerBuffer[0];
                triggerBuffer.RemoveAt(0);

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

        private string md5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

		private string serializeUrl(string url)
		{
            return url + "&signature=" + md5(JoltBase + url + privateKey);
		}

        private void httpGet(string url, string responseType, int trigger = -1)
        {
            ResponseTicket responseTicket = new ResponseTicket(url, responseType);
            if (trigger != -1)
            {
                responseTicket.HasTrigger = true;
                responseTicket.Trigger = trigger;
            }

            WebClient client = new WebClient();

            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    responseTicket.HasError = true;
                    responseTicket.Error = e.Error.Message;
                }
                else
                {
                    try
                    {
                        responseTicket.Data = (SimpleJson.JsonObject)SimpleJson.SimpleJson.DeserializeObject(e.Result);
                    }
                    catch (Exception ex)
                    {
                        responseTicket.HasError = true;
                        responseTicket.Error = ex.Message;
                    }
                }
                triggerBuffer.Add(responseTicket);
            };

            client.DownloadStringAsync(new Uri(JoltBase + serializeUrl(url)));
        }

        // Actions
        // -------------------------------------------------
        public override void action(int num, CActExtension act)
        {
            switch(num)
			{
				case ACT_AUTH:
					string actAuthUserName = act.getParamExpString(rh, 0);
					string actAuthUserToken = act.getParamExpString(rh, 1);
					actAuth(actAuthUserName, actAuthUserToken);
					break;
				case ACT_AUTHCREDS:
					actAuthCreds();
					break;
				case ACT_SETGUESTNAME:
					string actSetGuestNameGuestName = act.getParamExpString(rh, 0);
					actSetGuestName(actSetGuestNameGuestName);
					break;
				case ACT_FETCHUSERNAME:
					string actFetchUsernameUserName = act.getParamExpString(rh, 0);
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
					string actPingStatusSessionSession = act.getParamExpString(rh, 0);
					actPingStatusSession(actPingStatusSessionSession);
					break;
				case ACT_CHECKSESSION:
					actCheckSession();
					break;
				case ACT_CLOSESESSION:
					actCloseSession();
					break;
				case ACT_ADDUSERSCORE:
					string actAddUserScoreDisplayScore = act.getParamExpString(rh, 0);
					int actAddUserScoreSortScore = act.getParamExpression(rh, 1);
					int actAddUserScoreTable = act.getParamExpression(rh, 2);
					string actAddUserScoreExtraData = act.getParamExpString(rh, 3);
					actAddUserScore(actAddUserScoreDisplayScore, actAddUserScoreSortScore, actAddUserScoreTable, actAddUserScoreExtraData);
					break;
				case ACT_ADDGUESTSCORE:
					string actAddGuestScoreDisplayScore = act.getParamExpString(rh, 0);
					int actAddGuestScoreSortScore = act.getParamExpression(rh, 1);
					int actAddGuestScoreTable = act.getParamExpression(rh, 2);
					string actAddGuestScoreExtraData = act.getParamExpString(rh, 3);
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
					string actGlobalStorageGetDataKey = act.getParamExpString(rh, 0);
					actGlobalStorageGetData(actGlobalStorageGetDataKey);
					break;
				case ACT_GLOBALSTORAGEGETKEYS:
					string actGlobalStorageGetKeysPattern = act.getParamExpString(rh, 0);
					actGlobalStorageGetKeys(actGlobalStorageGetKeysPattern);
					break;
				case ACT_GLOBALSTORAGEDELETEKEY:
					string actGlobalStorageDeleteKeyKey = act.getParamExpString(rh, 0);
					actGlobalStorageDeleteKey(actGlobalStorageDeleteKeyKey);
					break;
				case ACT_GLOBALSTORAGESETKEY:
					string actGlobalStorageSetKeyKey = act.getParamExpString(rh, 0);
					string actGlobalStorageSetKeyData = act.getParamExpString(rh, 1);
					actGlobalStorageSetKey(actGlobalStorageSetKeyKey, actGlobalStorageSetKeyData);
					break;
				case ACT_GLOBALSTORAGEUPDATEKEY:
					string actGlobalStorageUpdateKeyKey = act.getParamExpString(rh, 0);
					string actGlobalStorageUpdateKeyData = act.getParamExpString(rh, 1);
					string actGlobalStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
					actGlobalStorageUpdateKey(actGlobalStorageUpdateKeyKey, actGlobalStorageUpdateKeyData, actGlobalStorageUpdateKeyOperation);
					break;
				case ACT_USERSTORAGEGETDATA:
					string actUserStorageGetDataKey = act.getParamExpString(rh, 0);
					actUserStorageGetData(actUserStorageGetDataKey);
					break;
				case ACT_USERSTORAGEGETKEYS:
					string actUserStorageGetKeysPattern = act.getParamExpString(rh, 0);
					actUserStorageGetKeys(actUserStorageGetKeysPattern);
					break;
				case ACT_USERSTORAGEDELETEKEY:
					string actUserStorageDeleteKeyKey = act.getParamExpString(rh, 0);
					actUserStorageDeleteKey(actUserStorageDeleteKeyKey);
					break;
				case ACT_USERSTORAGESETKEY:
					string actUserStorageSetKeyKey = act.getParamExpString(rh, 0);
					string actUserStorageSetKeyData = act.getParamExpString(rh, 1);
					actUserStorageSetKey(actUserStorageSetKeyKey, actUserStorageSetKeyData);
					break;
				case ACT_USERSTORAGEUPDATEKEY:
					string actUserStorageUpdateKeyKey = act.getParamExpString(rh, 0);
					string actUserStorageUpdateKeyData = act.getParamExpString(rh, 1);
					string actUserStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
					actUserStorageUpdateKey(actUserStorageUpdateKeyKey, actUserStorageUpdateKeyData, actUserStorageUpdateKeyOperation);
					break;
				case ACT_GLOBALFILESTORAGESAVEDATA:
					string actGlobalFileStorageSaveDataKey = act.getParamExpString(rh, 0);
					string actGlobalFileStorageSaveDataFilePath = act.getParamExpString(rh, 1);
					actGlobalFileStorageSaveData(actGlobalFileStorageSaveDataKey, actGlobalFileStorageSaveDataFilePath);
					break;
				case ACT_GLOBALFILESTORAGESETKEY:
					string actGlobalFileStorageSetKeyKey = act.getParamExpString(rh, 0);
					string actGlobalFileStorageSetKeyFilePath = act.getParamExpString(rh, 1);
					actGlobalFileStorageSetKey(actGlobalFileStorageSetKeyKey, actGlobalFileStorageSetKeyFilePath);
					break;
				case ACT_GLOBALFILESTORAGEUPDATEKEY:
					string actGlobalFileStorageUpdateKeyKey = act.getParamExpString(rh, 0);
					string actGlobalFileStorageUpdateKeyFilePath = act.getParamExpString(rh, 1);
					string actGlobalFileStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
					actGlobalFileStorageUpdateKey(actGlobalFileStorageUpdateKeyKey, actGlobalFileStorageUpdateKeyFilePath, actGlobalFileStorageUpdateKeyOperation);
					break;
				case ACT_USERFILESTORAGESAVEDATA:
					string actUserFileStorageSaveDataKey = act.getParamExpString(rh, 0);
					string actUserFileStorageSaveDataFilePath = act.getParamExpString(rh, 1);
					actUserFileStorageSaveData(actUserFileStorageSaveDataKey, actUserFileStorageSaveDataFilePath);
					break;
				case ACT_USERFILESTORAGESETKEY:
					string actUserFileStorageSetKeyKey = act.getParamExpString(rh, 0);
					string actUserFileStorageSetKeyFilePath = act.getParamExpString(rh, 1);
					actUserFileStorageSetKey(actUserFileStorageSetKeyKey, actUserFileStorageSetKeyFilePath);
					break;
				case ACT_USERFILESTORAGEUPDATEKEY:
					string actUserFileStorageUpdateKeyKey = act.getParamExpString(rh, 0);
					string actUserFileStorageUpdateKeyFilePath = act.getParamExpString(rh, 1);
					string actUserFileStorageUpdateKeyOperation = act.getParamExpString(rh, 2);
					actUserFileStorageUpdateKey(actUserFileStorageUpdateKeyKey, actUserFileStorageUpdateKeyFilePath, actUserFileStorageUpdateKeyOperation);
					break;
				case ACT_GETFRIENDSLIST:
					actGetFriendsList();
					break;
				case ACT_GETCURRENTTIME:
					actGetCurrentTime();
					break;
				case ACT_SETGAMEID:
					string actSetGameIDGameID = act.getParamExpString(rh, 0);
					actSetGameID(actSetGameIDGameID);
					break;
				case ACT_SETPRIVATEKEY:
					string actSetPrivateKeyPrivateKey = act.getParamExpString(rh, 0);
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

        private void actAuth(string userName, string userToken)
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
            string contents = null;

#if WINDOWS
            string path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                ".gj-credentials"
            );
            if (File.Exists(path))
                contents = File.ReadAllText(path, Encoding.UTF8);
#else
    using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
    {
        if (storage.FileExists(".gj-credentials"))
        {
            using (IsolatedStorageFileStream stream = storage.OpenFile(".gj-credentials", FileMode.Open))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                contents = reader.ReadToEnd();
        }
    }
#endif

            if (contents == null) return;

            string[] lines = contents.Split('\n');
            if (lines.Length >= 3)
            {
                gameAuthData.UserName = lines[1].Replace("\r", "");
                gameAuthData.UserToken = lines[2].Replace("\r", "");

                httpGet(
                    "/api/game/v1_2/users/auth/?game_id=" +
                    gameID +
                    "&username=" + gameAuthData.UserName +
                    "&user_token=" + gameAuthData.UserToken,
                    "Auth",
                    CND_AUTHFINISHED
                );
            }
        }

		private void actSetGameID(string gameID)
		{
			this.gameID = gameID;
		}

		private void actSetPrivateKey(string privateKey)
		{
			this.privateKey = privateKey;
		}

		private void actSetGuestName(string guestName)
		{
			gameAuthData.GuestName = guestName;
		}

		private void actFetchUsername(string userName)
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

		private void actPingStatusSession(string status)
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

		private void actAddUserScore(string displayScore, int sortScore, int table, string extraData)
		{
			string url =
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
			if (extraData.Length > 0)
				url += "&extra_data=" + extraData;

			httpGet(
				url,
				"AddScore",
				CND_SCOREADDED
			);
		}

		private void actAddGuestScore(string displayScore, int sortScore, int table, string extraData)
		{
			string url =
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
			if (extraData.Length > 0)
				url += "&extra_data=" + extraData;

			httpGet(
				url,
				"AddScore",
				CND_SCOREADDED
			);
		}

		private void actGetScoreRanking(int score, int table)
		{
			string url =
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
			string url = "/api/game/v1_2/scores/?game_id=" + gameID;
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
			string url = "/api/game/v1_2/scores/?game_id=" + gameID;
			if (gameAuthData.UserName.Length > 0 && gameAuthData.UserToken.Length > 0)
				url += "&username=" + gameAuthData.UserName + "&user_token=" + gameAuthData.UserToken;
			else if (gameAuthData.GuestName.Length > 0)
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

		private void actGlobalStorageGetData(string key)
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

		private void actGlobalStorageGetKeys(string pattern)
		{
			string url = "/api/game/v1_2/data-store/get-keys/?game_id=" + gameID;
			if (pattern.Length > 0)
				url += "&pattern=" + pattern;

			httpGet(
				url,
				"GetDataKeys",
				CND_GSGETKEYS
			);
		}

		private void actGlobalStorageDeleteKey(string key)
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

		private void actGlobalStorageSetKey(string key, string data)
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

		private void actGlobalStorageUpdateKey(string key, string data, string operation)
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

		private void actUserStorageGetData(string key)
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

		private void actUserStorageGetKeys(string pattern)
		{
			string url =
				"/api/game/v1_2/data-store/get-keys/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken;
			if (pattern.Length > 0)
				url += "&pattern=" + pattern;

			httpGet(
				url,
				"GetDataKeys",
				CND_USGETKEYS
			);
		}

		private void actUserStorageDeleteKey(string key)
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

		private void actUserStorageSetKey(string key, string data)
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

		private void actUserStorageUpdateKey(string key, string data, string operation)
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

		private void actGlobalFileStorageSaveData(string key, string filePath)
		{
			// No implementation
		}

		private void actGlobalFileStorageSetKey(string key, string filePath)
		{
			// No implementation
		}

		private void actGlobalFileStorageUpdateKey(string key, string filePath, string operation)
		{
			// No implementation
		}

		private void actUserFileStorageSaveData(string key, string filePath)
		{
			// No implementation
		}

		private void actUserFileStorageSetKey(string key, string filePath)
		{
			// No implementation
		}

		private void actUserFileStorageUpdateKey(string key, string filePath, string operation)
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
        // -------------------------------------------------
        public override int getNumberOfConditions()
        {
            return 33;
        }

        public override bool condition(int num, CCndExtension cnd)
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

		private bool cndAnyCallTriggered()
		{
			return true;
		}

		private bool cndCallTriggered()
		{
			return true;
		}

        // Expressions
        // --------------------------------------------
        public override CValue expression(int num)
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

		private string expGetJsonResponse()
		{
			if (latestResponse.HasError)
				return "";
            return SimpleJson.SimpleJson.SerializeObject(latestResponse.Data);
		}

		private string expGetResponseType()
		{
			return latestResponse.Type;
		}

		private string expGetResponseStatus()
		{
			if (latestResponse.HasError)
				return "";

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("success"))
                return "";

            return (string)response["success"];
		}

		private string expGetResponseMessage()
		{
			if (latestResponse.HasError)
				return "";

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("message"))
                return "";

            return (string)response["message"];
		}

		private string expGetErrorMessage()
		{
			if (!latestResponse.HasError)
				return "";
			return latestResponse.Error;
		}

		private string expGetGameID()
		{
			return gameID;
		}

		private string expGetPrivateKey()
		{
			return privateKey;
		}

		private string expGetRequestURL()
		{
			return JoltBase + latestResponse.URL;
		}

		private string expGetUserName()
		{
			return gameAuthData.UserName;
		}

		private string expGetUserToken()
		{
			return gameAuthData.UserToken;
		}

		private string expGetGuestName()
		{
			return gameAuthData.GuestName;
		}

		private int expFetchedUserCount()
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["users"]).Count;
		}

		private string expFetchedUserDisplayName(int index)
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("developer_name"))
                return "";

            return (string)user["developer_name"];
		}

        private string expFetchedUsername(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("username"))
                return "";

            return (string)user["username"];
        }

        private int expFetchedUserID(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return 0;

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("id"))
                return 0;

            return int.Parse((string)user["id"]); 
        }

        private string expFetchedUserDescription(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("developer_description"))
                return "";

            return (string)user["developer_description"];
        }

        private string expFetchedUserAvatar(int index, int resolution)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("avatar_url"))
                return "";

            string avatar_url = (string)user["avatar_url"];
            return "https://m.gjcdn.net/user-avatar/" + resolution + avatar_url.Substring(34, avatar_url.LastIndexOf(".")) + ".png";
        }

        private string expFetchedUserWebsite(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("developer_website"))
                return "";

            return (string)user["developer_website"];
        }

        private string expFetchedUserStatus(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("status"))
                return "";

            return (string)user["status"];
        }

        private string expFetchedUserType(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("type"))
                return "";

            return (string)user["type"];
        }

        private string expFetchedUserLastLoggedIn(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("last_logged_in"))
                return "";

            return (string)user["last_logged_in"];
        }

        private int expFetchedUserLastLoggedInTimestamp(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return 0;

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("last_logged_in_timestamp"))
                return 0;

            return (int)user["last_logged_in_timestamp"];
        }

        private string expFetchedUserSignedUp(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return "";

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("signed_up"))
                return "";

            return (string)user["signed_up"];
        }

        private int expFetchedUserSignedUpTimestamp(int index)
        {
            if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("users") || !(response["users"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray users = (SimpleJson.JsonArray)response["users"];
            if (users.Count <= index)
                return 0;

            SimpleJson.JsonObject user = (SimpleJson.JsonObject)users[index];
            if (!user.ContainsKey("signed_up_timestamp"))
                return 0;

            return (int)user["signed_up_timestamp"];
        }

        private int expScoreRanking()
        {
            if (latestResponse.Type != "GetRank" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("rank"))
                return 0;

            return (int)response["rank"];
        }

        private int expFetchedScoreCount()
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["scores"]).Count;
        }

        private string expFetchedScoreUsername(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return "";

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("user"))
                return "";

            return (string)score["user"];
        }

        private int expFetchedScoreUserID(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return 0;

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("user_id"))
                return 0;

            return int.Parse((string)score["user_id"]);
        }

        private string expFetchedScoreGuestName(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return "";

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("guest"))
                return "";

            return (string)score["guest"];
        }

        private string expFetchedScoreScore(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return "";

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("score"))
                return "";

            return (string)score["score"];
        }

        private int expFetchedScoreSort(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return 0;

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("sort"))
                return 0;

            return int.Parse((string)score["sort"]);
        }

        private string expFetchedScoreExtraData(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return "";

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("extra_data"))
                return "";

            return (string)score["extra_data"];
        }

        private string expFetchedScoreSubmit(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return "";

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("stored"))
                return "";

            return (string)score["stored"];
        }

        private int expFetchedScoreSubmitTimestamp(int index)
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("scores") || !(response["scores"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray scores = (SimpleJson.JsonArray)response["scores"];
            if (scores.Count <= index)
                return 0;

            SimpleJson.JsonObject score = (SimpleJson.JsonObject)scores[index];
            if (!score.ContainsKey("stored_timestamp"))
                return 0;

            return (int)score["stored_timestamp"];
        }

        private int expFetchedScoreTableID()
        {
            if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
                return 0;

            Match match = Regex.Match(latestResponse.URL, @"table_id=([0-9]+)");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        private int expFetchedTableCount()
        {
            if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("tables") || !(response["tables"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["tables"]).Count;
        }

        private string expFetchedTableName(int index)
        {
            if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("tables") || !(response["tables"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray tables = (SimpleJson.JsonArray)response["tables"];
            if (tables.Count <= index)
                return "";

            SimpleJson.JsonObject table = (SimpleJson.JsonObject)tables[index];
            if (!table.ContainsKey("name"))
                return "";

            return (string)table["name"];
        }

        private int expFetchedTableID(int index)
        {
            if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("tables") || !(response["tables"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray tables = (SimpleJson.JsonArray)response["tables"];
            if (tables.Count <= index)
                return 0;

            SimpleJson.JsonObject table = (SimpleJson.JsonObject)tables[index];
            if (!table.ContainsKey("id"))
                return 0;

            return int.Parse((string)table["id"]);
        }

        private string expFetchedTableDescription(int index)
        {
            if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("tables") || !(response["tables"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray tables = (SimpleJson.JsonArray)response["tables"];
            if (tables.Count <= index)
                return "";

            SimpleJson.JsonObject table = (SimpleJson.JsonObject)tables[index];
            if (!table.ContainsKey("description"))
                return "";

            return (string)table["description"];
        }

        private int expFetchedTableIsPrimary(int index)
        {
            if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("tables") || !(response["tables"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray tables = (SimpleJson.JsonArray)response["tables"];
            if (tables.Count <= index)
                return 0;

            SimpleJson.JsonObject table = (SimpleJson.JsonObject)tables[index];
            if (!table.ContainsKey("primary"))
                return 0;

            return (int)table["primary"];
        }

        private int expFetchedTrophyCount()
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["trophies"]).Count;
        }

        private string expFetchedTrophyTitle(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return "";

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("title"))
                return "";

            return (string)trophy["title"];
        }

        private int expFetchedTrophyID(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return 0;

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("id"))
                return 0;

            return int.Parse((string)trophy["id"]);
        }

        private string expFetchedTrophyDescription(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return "";

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("description"))
                return "";

            return (string)trophy["description"];
        }

        private string expFetchedTrophyDifficulty(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return "";

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("difficulty"))
                return "";

            return (string)trophy["difficulty"];
        }

        private string expFetchedTrophyImageURL(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return "";

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("image_url"))
                return "";

            return (string)trophy["image_url"];
        }

        private string expFetchedTrophyAchieved(int index)
        {
            if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("trophies") || !(response["trophies"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray trophies = (SimpleJson.JsonArray)response["trophies"];
            if (trophies.Count <= index)
                return "";

            SimpleJson.JsonObject trophy = (SimpleJson.JsonObject)trophies[index];
            if (!trophy.ContainsKey("achieved"))
                return "";

            return (string)trophy["achieved"];
        }

        private string expRetrievedKeyData()
        {
            if (latestResponse.Type != "FetchData" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("data"))
                return "";

            return (string)response["data"];
        }

        private int expFetchedKeyCount()
        {
            if (latestResponse.Type != "GetDataKeys" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("keys") || !(response["keys"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["keys"]).Count;
        }

        private string expFetchedKey(int index)
        {
            if (latestResponse.Type != "GetDataKeys" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("keys") || !(response["keys"] is SimpleJson.JsonArray))
                return "";

            SimpleJson.JsonArray keys = (SimpleJson.JsonArray)response["keys"];
            if (keys.Count <= index)
                return "";

            SimpleJson.JsonObject key = (SimpleJson.JsonObject)keys[index];
            if (!key.ContainsKey("key"))
                return "";

            return (string)key["key"];
        }

        private string expUpdatedKeyData()
        {
            if (latestResponse.Type != "UpdateData" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("data"))
                return "";

            return (string)response["data"];
        }

        private int expFetchedFriendCount()
        {
            if (latestResponse.Type != "Friends" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("friends") || !(response["friends"] is SimpleJson.JsonArray))
                return 0;

            return ((SimpleJson.JsonArray)response["friends"]).Count;
        }

        private int expFetchedFriend(int index)
        {
            if (latestResponse.Type != "Friends" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = (SimpleJson.JsonObject)latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("friends") || !(response["friends"] is SimpleJson.JsonArray))
                return 0;

            SimpleJson.JsonArray friends = (SimpleJson.JsonArray)response["friends"];
            if (friends.Count <= index)
                return 0;

            SimpleJson.JsonObject friend = (SimpleJson.JsonObject)friends[index];
            if (!friend.ContainsKey("friend_id"))
                return 0;

            return int.Parse((string)friend["friend_id"]);
        }

        private int expTimeYear()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("year"))
                return 0;

            return int.Parse((string)response["year"]);
        }

        private int expTimeMonth()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("month"))
                return 0;

            return int.Parse((string)response["month"]);
        }

        private int expTimeDay()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("day"))
                return 0;

            return int.Parse((string)response["day"]);
        }

        private int expTimeHour()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("hour"))
                return 0;

            return int.Parse((string)response["hour"]);
        }

        private int expTimeMinute()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("minute"))
                return 0;

            return int.Parse((string)response["minute"]);
        }

        private int expTimeSecond()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("second"))
                return 0;

            return int.Parse((string)response["second"]);
        }

        private int expTimeTimestamp()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return 0;

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return 0;

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("timestamp"))
                return 0;

            return (int)response["timestamp"];
        }

        private string expTimeTimezone()
        {
            if (latestResponse.Type != "Time" || latestResponse.HasError)
                return "";

            SimpleJson.JsonObject j = latestResponse.Data;
            if (!j.ContainsKey("response"))
                return "";

            SimpleJson.JsonObject response = (SimpleJson.JsonObject)j["response"];
            if (!response.ContainsKey("timezone"))
                return "";

            return (string)response["timezone"];
        }

        public class GameAuth
	    {
            public string UserName;
            public string UserToken;
            public string GuestName;

            public GameAuth(string userName, string userToken, string guestName)
            {
                UserName = userName;
                UserToken = userToken;
                GuestName = guestName;
            }
	    }

        public class ResponseTicket
	    {
            public string URL;
            public string Type;
            public SimpleJson.JsonObject Data = null;
            public int Trigger = 0;
            public bool HasTrigger = false;

            public bool HasError = false;
            public string Error = string.Empty;

            public ResponseTicket(string url, string type)
            {
                URL = url;
                Type = type;
            }
	    }

        public class DarkEdifProperty
        {
            public uint index;
            public uint propTypeID;
            public uint propJSONIndex;
            public string propName;
            public BinaryReader propData;

            public DarkEdifProperty(uint index, uint propTypeID, uint propJSONIndex, string propName, BinaryReader propData)
            {
                this.index = index;
                this.propTypeID = propTypeID;
                this.propJSONIndex = propJSONIndex;
                this.propName = propName;
                this.propData = propData;
            }
        }

        public class DarkEdifPropSet
        {
            public string setIndicator;
            public uint numRepeats;
            public uint lastSetJSONPropIndex;
            public uint firstSetJSONPropIndex;
            public uint setNameJSONPropIndex;
            public string setName;
        
            private BinaryReader rsDV;

            public DarkEdifPropSet(BinaryReader rsDV)
            {
                // Always 'S', compared with 'L' for non-set list.
                setIndicator = ((char)rsDV.ReadByte()).ToString();
                // Number of repeats of this set, 1 is minimum and means one of this set
                numRepeats = rsDV.ReadUInt16();
                // Property that ends this set's data, as a JSON index, inclusive
                lastSetJSONPropIndex = rsDV.ReadUInt16();
                // First property that begins this set's data, as a JSON index
                firstSetJSONPropIndex = rsDV.ReadUInt16();
                // Name property JSON index that will appear in list when switching set entry
                setNameJSONPropIndex = rsDV.ReadUInt16();
                // Set name, as specified in JSON. Don't confuse with user-specified set name.
                long bytesAvailable = rsDV.BaseStream.Length - rsDV.BaseStream.Position;
                setName = Encoding.UTF8.GetString(rsDV.ReadBytes((int)bytesAvailable));

                this.rsDV = rsDV;
            }

            public uint getIndexSelected()
            {
                rsDV.BaseStream.Position = 1 + (2 * 4);
                return rsDV.ReadUInt16();
            }

            public void setIndexSelected(uint i)
            {
                using (BinaryWriter writer = new BinaryWriter(rsDV.BaseStream))
                {
                    writer.BaseStream.Position = 1 + (2 * 4);
                    writer.Write((ushort)i);
                }
            }
        }

        public class DarkEdifProperties
        {
            private uint numProps = 0;
            private uint sizeBytes = 0;
            private int propVer = 0;
            private byte[] chkboxes;
            private List<DarkEdifProperty> props;

            private static int GetFileLength(CFile file)
            {
                var data = (byte[])typeof(CFile)
                    .GetField("data", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(file);

                return data.Length;
            }

            public DarkEdifProperties(CRunExtension ext, CFile edPtrFile, int extVersion)
            {
                // DarkEdif SDK stores offset of DarkEdif props away from start of EDITDATA inside private data.
                // eHeader is 20 bytes, so this should be 20+ bytes.
                if (ext.ho.privateData < 20)
                {
                    throw new Exception("Not smart properties - eHeader missing?");
                }
                // DarkEdif SDK header read:
                // header uint32, hash uint32, hashtypes uint32, numprops uint16, pad uint16, sizeBytes uint32 (includes whole EDITDATA)
                // if prop set v2, then uint64 editor checkbox ptr
                // then checkbox list, one bit per checkbox, including non-checkbox properties
                // so skip numProps / 8 bytes
                // then moving to Data list:
                // size uint32 (includes whole Data), propType uint16, propNameSize uint8, propname u8 (lowercased), then data bytes

                int oldPos = edPtrFile.pointer;
                byte[] bytes = new byte[GetFileLength(edPtrFile)];
                edPtrFile.pointer = 0;
                edPtrFile.read(bytes);
                edPtrFile.pointer = oldPos;

                edPtrFile.skipBytes(ext.ho.privateData - 20); // sub size of eHeader; edPtrFile won't start with eHeader
                byte[] verBuff = edPtrFile.readArray(4);
		        string verStr = "";
                for (int i = verBuff.Length - 1; i >= 0; i--)
                {
                    verStr += ((char)verBuff[i]).ToString();
                }
                if (verStr == "DAR2")
                {
                    propVer = 2;
                }
                else if (verStr == "DAR1")
                {
                    propVer = 1;
                }
                else
                {
                    throw new Exception("Version string " + verStr + " unknown. Did you restore the file offset?");
                }
                // Pull out hash, hashTypes, numProps, pad, sizeBytes, visibleEditorProps
		        BinaryReader header = new BinaryReader(new MemoryStream(edPtrFile.readArray(4 + 4 + 2 + 2 + 4 + (propVer > 1 ? 8 : 0))));
                header.BaseStream.Position = 4 + 4; // Skip past hash and hashTypes
                numProps = header.ReadUInt16();
                header.BaseStream.Position = 4 + 4 + 4; // skip past numProps and pad
                sizeBytes = header.ReadUInt32();

                BinaryReader editData = new BinaryReader(new MemoryStream(edPtrFile.readArray(
                    (int)sizeBytes -
                    // skip eHeader
                    ext.ho.privateData -
                    // cursor offset
                    4 -
                    // Skip DarkEdif header
                    (int)header.BaseStream.Length
                )));
                editData.BaseStream.Position = 0;
		        chkboxes = editData.ReadBytes((int)Math.Ceiling(numProps / 8.0));

                props = new List<DarkEdifProperty>();
                editData.BaseStream.Position = chkboxes.Length;
                long bytesAvailable = editData.BaseStream.Length - editData.BaseStream.Position;
                BinaryReader data = new BinaryReader(new MemoryStream(editData.ReadBytes((int)bytesAvailable)));

                // Dont need TextDecoder

                uint propSize = 0;
                uint propEnd = 0;
                data.BaseStream.Position = 0; // pt
                for (uint i = 0; i < numProps; ++i)
                {
                    propSize = data.ReadUInt32();
                    propEnd = (uint)data.BaseStream.Position - 4 + propSize;
                    uint propTypeID = data.ReadUInt16();
                    // propJSONIndex does not exist in Data in DarkEdif smart props ver 1, so JSON index is same as prop index
			        uint propJSONIndex = i;
                    if (propVer == 2)
                    {
				        propJSONIndex = data.ReadUInt16();
                    }
                    int propNameLength = data.ReadByte();

                    string propName = Encoding.UTF8.GetString(data.ReadBytes(propNameLength));
                    BinaryReader propData = new BinaryReader(new MemoryStream(data.ReadBytes((int)(propEnd - data.BaseStream.Position))));

                    props.Add(new DarkEdifProperty(i, propTypeID, propJSONIndex, propName, propData));
                    data.BaseStream.Position = propEnd;
                }
            }

            public bool IsComboBoxProp(int propTypeID)
            {
			    // PROPTYPE_COMBOBOX, PROPTYPE_COMBOBOXBTN, PROPTYPE_ICONCOMBOBOX
			    return propTypeID == 7 || propTypeID == 20 || propTypeID == 24;
            }

            public DarkEdifPropSet RuntimePropSet(DarkEdifProperty data)
            {
                DarkEdifPropSet rs = new DarkEdifPropSet(data.propData);
			    if (rs.setIndicator != "S")
				    throw new Exception("Not a prop set!");
			    return rs;
            }

            public int GetPropertyIndex(object chkIDOrName)
            {
                if (propVer > 1)
                {
                    int jsonIdx = -1;
                    DarkEdifProperty p = null;
                    if (chkIDOrName is int || chkIDOrName is uint || chkIDOrName is float || chkIDOrName is double)
                    {
                        foreach (DarkEdifProperty prop in props)
                        {
                            if (prop.index == Convert.ToUInt32(chkIDOrName))
                            {
                                p = prop;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (DarkEdifProperty prop2 in props)
                        {
                            if (prop2.propName == chkIDOrName.ToString())
                            {
                                p = prop2;
                                break;
                            }
                        }
                    }
                    if (p == null)
                    {
                        throw new Exception("Invalid property name \"" + chkIDOrName + "\"");
                    }
                    jsonIdx = (int)p.propJSONIndex;

                    // Look up prop index from JSON index - DarkEdif::Properties::PropIdxFromJSONIdx
				    DarkEdifProperty data = props[0];
                    int i = 0;
                    while (data.propJSONIndex != jsonIdx)
                    {
                        if (i >= numProps)
                        {
						    throw new Exception("Couldn't find property of JSON ID " + jsonIdx + ", hit property " + i + " of " + numProps + " stored.\n");
					    }

                        long oldPos = data.propData.BaseStream.Position;
                        data.propData.BaseStream.Position = 0;
                        char propDataIdentifier = '\0';
                        if (data.propData.BaseStream.Length > 0)
                            propDataIdentifier = (char)data.propData.ReadByte();
                        data.propData.BaseStream.Position = oldPos;
                        if (IsComboBoxProp((int)data.propTypeID) && propDataIdentifier == 'S')
                        {
                            DarkEdifPropSet rs = RuntimePropSet(data);
                            DarkEdifProperty rsContainer = data;
                            if (jsonIdx > rs.lastSetJSONPropIndex)
                            {
                                while (data.propJSONIndex != rs.lastSetJSONPropIndex)
                                {
                                    data = props[i++];
                                }
                                rs = null;
                                rsContainer = null;
                            }
                            // It's within this set's range
                            else if (jsonIdx >= rs.firstSetJSONPropIndex && jsonIdx <= rs.lastSetJSONPropIndex)
                            {
                                if (rs.getIndexSelected() > 0)
                                {
                                    int j = 0;
                                    while (true)
                                    {
                                        data = props[++i];

                                        // Skip until end of this entry, then move to next prop
									    if (data.propJSONIndex == rs.lastSetJSONPropIndex)
                                        {
                                            if (++j == rs.getIndexSelected())
                                            {
                                                data = props[++i];
											    break;
                                            }
                                        }
                                    }
								    continue;
                                }
                                else
                                {
								    data = props[++i];
								    continue;
                                }
                            }
						    // else it's not in this set: continue to standard loop
						    else
                            {
							    rs = null;
                                rsContainer = null;
						    }
                        }
					
					    data = props[++i];
                    }
				    return (int)data.index;
                }
                if (chkIDOrName is int || chkIDOrName is uint || chkIDOrName is float || chkIDOrName is double)
                {
				    if (numProps <= Convert.ToUInt32(chkIDOrName))
                    {
					    throw new Exception("Invalid property ID " + chkIDOrName + ", max ID is " + (numProps - 1) + ".");
				    }
				    return (int)chkIDOrName;
			    }
                DarkEdifProperty p2 = null;
                foreach (DarkEdifProperty prop3 in props)
                {
                    if (prop3.propName == chkIDOrName.ToString())
                    {
                        p2 = prop3;
                        break;
                    }
                }
                if (p2 == null)
                {
                    throw new Exception("Invalid property name \"" + chkIDOrName + "\"");
                }
			    return (int)p2.index;
            }

            public bool IsPropChecked(object chkIDOrName)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
			    if (idx == -1)
                {
				    return false;
			    }
			    return (chkboxes[(int)Math.Floor(idx / 8.0)] & (1 << idx % 8)) != 0;
            }

            static readonly List<int> textPropIDs = new List<int>
            {
                5,  // PROPTYPE_EDIT_STRING
                22, // PROPTYPE_EDIT_MULTILINE
                16, // PROPTYPE_FILENAME
                19, // PROPTYPE_PICTUREFILENAME
                26, // PROPTYPE_DIRECTORYNAME
            };

            public string GetPropertyStr(object chkIDOrName)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
			    if (idx == -1)
                {
				    return null;
			    }
			    DarkEdifProperty prop = props[idx];
                if (textPropIDs.IndexOf((int)prop.propTypeID) != -1 || IsComboBoxProp((int)prop.propTypeID))
                {
                    // Prop ver 2 added repeating prop sets
				    if (propVer == 2 && IsComboBoxProp((int)prop.propTypeID))
                    {
                        long oldPos = prop.propData.BaseStream.Position;
                        prop.propData.BaseStream.Position = 0;
						char setIndicator = '\0';
						if (prop.propData.BaseStream.Length > 0)
							setIndicator = (char)prop.propData.ReadByte();
						prop.propData.BaseStream.Position = oldPos;
                        if (setIndicator == 'L')
                        {
                            prop.propData.BaseStream.Position = 1;
                            long bytesAvailable = prop.propData.BaseStream.Length - prop.propData.BaseStream.Position;
                            return Encoding.UTF8.GetString(prop.propData.ReadBytes((int)bytesAvailable));
                        }
                        else if (setIndicator == 'S')
                        {
                            throw new Exception("Property " + prop.propName + " is not textual.");
                        }
                        throw new Exception("Property " + prop.propName + " is not a valid list property.");
                    }
                    prop.propData.BaseStream.Position = 0;
                    long bytesAvailable2 = prop.propData.BaseStream.Length - prop.propData.BaseStream.Position;
                    string t = Encoding.UTF8.GetString(prop.propData.ReadBytes((int)bytesAvailable2));
                    if (prop.propTypeID == 22) //PROPTYPE_EDIT_MULTILINE
                    {
					    t = t.Replace("\r", ""); // CRLF to LF
				    }
				    return t;
                }
			    throw new Exception("Property " + prop.propName + " is not textual.");
            }

            static readonly List<int> numPropIDsInteger = new List<int>
            {
				6, // PROPTYPE_EDIT_NUMBER
				9, // PROPTYPE_COLOR
				11, // PROPTYPE_SLIDEREDIT
				12, // PROPTYPE_SPINEDIT
				13 // PROPTYPE_DIRCTRL
            };

            static readonly List<int> numPropIDsFloat = new List<int>
            {
				21, // PROPTYPE_EDIT_FLOAT
				27 // PROPTYPE_SPINEDITFLOAT
            };

            public double GetPropertyNum(object chkIDOrName)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
                if (idx == -1)
                {
				    return 0.0;
			    }
                DarkEdifProperty prop = props[idx];
			    if (numPropIDsInteger.IndexOf((int)prop.propTypeID) != -1)
                {
                    prop.propData.BaseStream.Position = 0;
				    return prop.propData.ReadUInt32();
			    }
			    if (numPropIDsFloat.IndexOf((int)prop.propTypeID) != -1)
                {
                    prop.propData.BaseStream.Position = 0;
				    return prop.propData.ReadSingle();
			    }
			    throw new Exception("Property " + prop.propName + " is not numeric.");
            }

            public int GetPropertyImageID(object chkIDOrName, int imgID)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
                if (idx == -1)
                {
				    return -1;
			    }
                DarkEdifProperty prop = props[idx];
			    if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
                {
				    throw new Exception("Property " + prop.propName + " is not an image list.");
			    }
			
			    if (imgID < 0)
                {
				    throw new Exception("Image index " + imgID + " is invalid.");
			    }
                prop.propData.BaseStream.Position = 0;
			    if (imgID >= prop.propData.ReadUInt16())
                {
				    return -1;
			    }
			
                prop.propData.BaseStream.Position = 2 * (1 + idx);
			    return prop.propData.ReadUInt16();
            }

            public int GetPropertyNumImages(object chkIDOrName, int imgID)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
                if (idx == -1)
                {
				    return -1;
			    }
                DarkEdifProperty prop = props[idx];
			    if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
                {
				    throw new Exception("Property " + prop.propName + " is not an image list.");
			    }
			
                prop.propData.BaseStream.Position = 0;
			    return prop.propData.ReadUInt16();
            }

            public Point? GetSizeProperty(object chkIDOrName)
            {
			    int idx = GetPropertyIndex(chkIDOrName);
                if (idx == -1)
                {
				    return null;
			    }
                DarkEdifProperty prop = props[idx];
			    if (prop.propTypeID != 8) // PROPTYPE_SIZE
                {
				    throw new Exception("Property " + prop.propName + " is not an size property.");
			    }

                Point size = new Point();
                prop.propData.BaseStream.Position = 0;
                size.X = prop.propData.ReadInt32();
                prop.propData.BaseStream.Position = 4;
                size.Y = prop.propData.ReadInt32();
                return size;
            }

            // TODO: PropSetIterator and LoopPropSet
        }

        public static class DarkEdif
        {
            private static Dictionary<string, object> data = new Dictionary<string, object>();
            private static int sdkVersion = 20;

            public static object getGlobalData(string key)
            {
                key = key.ToLower();
                if (data.ContainsKey(key))
                {
                    return data[key];
                }
                return null;
            }

            public static void setGlobalData(string key, object value)
            {
                key = key.ToLower();
                data[key] = value;
            }
        
            // Could not implement getCurrentFusionEventNumber: evgLine is not in Flash

            public static void checkSupportsSDKVersion(int sdkVer)
            {
                if (sdkVer < 16 || sdkVer > 20)
                {
                    throw new Exception("Flash DarkEdif SDK does not support SDK version " + sdkVersion);
                }
            }

            public static DarkEdifProperties getProperties(CRunExtension ext, CFile edPtrFile, int extVersion)
            {
                return new DarkEdifProperties(ext, edPtrFile, extVersion);
            }
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="SimpleJson.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation.
//
//    Licensed under the MIT License (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.opensource.org/licenses/mit-license.php
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/simple-json</website>
//-----------------------------------------------------------------------

// VERSION:

// NOTE: uncomment the following line to make SimpleJson class internal.
//#define SIMPLE_JSON_INTERNAL

// NOTE: uncomment the following line to make JsonArray and JsonObject class internal.
//#define SIMPLE_JSON_OBJARRAYINTERNAL

// NOTE: uncomment the following line to enable dynamic support.
//#define SIMPLE_JSON_DYNAMIC

// NOTE: uncomment the following line to enable DataContract support.
//#define SIMPLE_JSON_DATACONTRACT

// NOTE: uncomment the following line to enable IReadOnlyCollection<T> and IReadOnlyList<T> support.
//#define SIMPLE_JSON_READONLY_COLLECTIONS

// NOTE: uncomment the following line to disable linq expressions/compiled lambda (better performance) instead of method.invoke().
// define if you are using .net framework <= 3.0 or < WP7.5
//#define SIMPLE_JSON_NO_LINQ_EXPRESSION

// NOTE: uncomment the following line if you are compiling under Window Metro style application/library.
// usually already defined in properties
//#define NETFX_CORE;

// If you are targetting WinStore, WP8 and NET4.5+ PCL make sure to #define SIMPLE_JSON_TYPEINFO;

// original json parsing code from http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable RedundantExplicitArrayCreation
// ReSharper disable SuggestUseVarKeywordEvident
namespace SimpleJson
{
    /// <summary>
    /// Represents the json array.
    /// </summary>
    [GeneratedCode("simple-json", "1.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
#if SIMPLE_JSON_OBJARRAYINTERNAL
    internal
#else
    public
#endif
 class JsonArray : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArray"/> class. 
        /// </summary>
        public JsonArray() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonArray"/> class. 
        /// </summary>
        /// <param name="capacity">The capacity of the json array.</param>
        public JsonArray(int capacity) : base(capacity) { }

        /// <summary>
        /// The json representation of the array.
        /// </summary>
        /// <returns>The json representation of the array.</returns>
        public override string ToString()
        {
            return SimpleJson.SerializeObject(this) ?? string.Empty;
        }
    }

    /// <summary>
    /// Represents the json object.
    /// </summary>
    [GeneratedCode("simple-json", "1.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
#if SIMPLE_JSON_OBJARRAYINTERNAL
    internal
#else
    public
#endif
 class JsonObject :
#if SIMPLE_JSON_DYNAMIC
 DynamicObject,
#endif
 IDictionary<string, object>
    {
        /// <summary>
        /// The internal member dictionary.
        /// </summary>
        private readonly Dictionary<string, object> _members;

        /// <summary>
        /// Initializes a new instance of <see cref="JsonObject"/>.
        /// </summary>
        public JsonObject()
        {
            _members = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> for the type of the key.</param>
        public JsonObject(IEqualityComparer<string> comparer)
        {
            _members = new Dictionary<string, object>(comparer);
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get { return GetAtIndex(_members, index); }
        }

        internal static object GetAtIndex(IDictionary<string, object> obj, int index)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (index >= obj.Count)
                throw new ArgumentOutOfRangeException("index");
            int i = 0;
            foreach (KeyValuePair<string, object> o in obj)
                if (i++ == index) return o.Value;
            return null;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            _members.Add(key, value);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return _members.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get { return _members.Keys; }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _members.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return _members.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<object> Values
        {
            get { return _members.Values; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        public object this[string key]
        {
            get { return _members[key]; }
            set { _members[key] = value; }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            _members.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _members.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _members.ContainsKey(item.Key) && _members[item.Key] == item.Value;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            int num = Count;
            foreach (KeyValuePair<string, object> kvp in this)
            {
                array[arrayIndex++] = kvp;
                if (--num <= 0)
                    return;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _members.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return _members.Remove(item.Key);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// Returns a json <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A json <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return SimpleJson.SerializeObject(this);
        }

#if SIMPLE_JSON_DYNAMIC
        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            Type targetType = binder.Type;

            if ((targetType == typeof(IEnumerable)) ||
                (targetType == typeof(IEnumerable<KeyValuePair<string, object>>)) ||
                (targetType == typeof(IDictionary<string, object>)) ||
                (targetType == typeof(IDictionary)))
            {
                result = this;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that delete an object member. This method is not intended for use in C# or Visual Basic.
        /// </summary>
        /// <param name="binder">Provides information about the deletion.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            return _members.Remove(binder.Name);
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes"/> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null) throw new ArgumentNullException("indexes");
            if (indexes.Length == 1)
            {
                result = ((IDictionary<string, object>)this)[(string)indexes[0]];
                return true;
            }
            result = null;
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
        /// <returns>
        /// Alwasy returns true.
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object value;
            if (_members.TryGetValue(binder.Name, out value))
            {
                result = value;
                return true;
            }
            result = null;
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="indexes"/> is equal to 3.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, <paramref name="value"/> is equal to 10.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
        /// </returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null) throw new ArgumentNullException("indexes");
            if (indexes.Length == 1)
            {
                ((IDictionary<string, object>)this)[(string)indexes[0]] = value;
                return true;
            }
            return base.TrySetIndex(binder, indexes, value);
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // <pex>
            if (binder == null)
                throw new ArgumentNullException("binder");
            // </pex>
            _members[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        /// A sequence that contains dynamic member names.
        /// </returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var key in Keys)
                yield return key;
        }
#endif
    }
}

namespace SimpleJson
{
    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    /// 
    /// JSON uses Arrays and Objects. These correspond here to the datatypes JsonArray(IList&lt;object>) and JsonObject(IDictionary&lt;string,object>).
    /// All numbers are parsed to doubles.
    /// </summary>
    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
 static class SimpleJson
    {
        private const int TOKEN_NONE = 0;
        private const int TOKEN_CURLY_OPEN = 1;
        private const int TOKEN_CURLY_CLOSE = 2;
        private const int TOKEN_SQUARED_OPEN = 3;
        private const int TOKEN_SQUARED_CLOSE = 4;
        private const int TOKEN_COLON = 5;
        private const int TOKEN_COMMA = 6;
        private const int TOKEN_STRING = 7;
        private const int TOKEN_NUMBER = 8;
        private const int TOKEN_TRUE = 9;
        private const int TOKEN_FALSE = 10;
        private const int TOKEN_NULL = 11;
        private const int BUILDER_CAPACITY = 2000;

        private static readonly char[] EscapeTable;
        private static readonly char[] EscapeCharacters = new char[] { '"', '\\', '\b', '\f', '\n', '\r', '\t' };
        private static readonly string EscapeCharactersString = new string(EscapeCharacters);

        static SimpleJson()
        {
            EscapeTable = new char[93];
            EscapeTable['"'] = '"';
            EscapeTable['\\'] = '\\';
            EscapeTable['\b'] = 'b';
            EscapeTable['\f'] = 'f';
            EscapeTable['\n'] = 'n';
            EscapeTable['\r'] = 'r';
            EscapeTable['\t'] = 't';
        }

        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An IList&lt;object>, a IDictionary&lt;string,object>, a double, a string, null, true, or false</returns>
        public static object DeserializeObject(string json)
        {
            object obj;
            if (TryDeserializeObject(json, out obj))
                return obj;
            throw new SerializationException("Invalid JSON string");
        }

        /// <summary>
        /// Try parsing the json string into a value.
        /// </summary>
        /// <param name="json">
        /// A JSON string.
        /// </param>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// Returns true if successfull otherwise false.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        public static bool TryDeserializeObject(string json, out object obj)
        {
            bool success = true;
            if (json != null)
            {
                char[] charArray = json.ToCharArray();
                int index = 0;
                obj = ParseValue(charArray, ref index, ref success);
            }
            else
                obj = null;

            return success;
        }

        public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            object jsonObject = DeserializeObject(json);
            return type == null || jsonObject != null && ReflectionUtils.IsAssignableFrom(jsonObject.GetType(), type)
                       ? jsonObject
                       : (jsonSerializerStrategy ?? CurrentJsonSerializerStrategy).DeserializeObject(jsonObject, type);
        }

        public static object DeserializeObject(string json, Type type)
        {
            return DeserializeObject(json, type, null);
        }

        public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            return (T)DeserializeObject(json, typeof(T), jsonSerializerStrategy);
        }

        public static T DeserializeObject<T>(string json)
        {
            return (T)DeserializeObject(json, typeof(T), null);
        }

        /// <summary>
        /// Converts a IDictionary&lt;string,object> / IList&lt;object> object into a JSON string
        /// </summary>
        /// <param name="json">A IDictionary&lt;string,object> / IList&lt;object></param>
        /// <param name="jsonSerializerStrategy">Serializer strategy to use</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy)
        {
            StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
            bool success = SerializeValue(jsonSerializerStrategy, json, builder);
            return (success ? builder.ToString() : null);
        }

        public static string SerializeObject(object json)
        {
            return SerializeObject(json, CurrentJsonSerializerStrategy);
        }

        public static string EscapeToJavascriptString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return jsonString;

            StringBuilder sb = new StringBuilder();
            char c;

            for (int i = 0; i < jsonString.Length; )
            {
                c = jsonString[i++];

                if (c == '\\')
                {
                    int remainingLength = jsonString.Length - i;
                    if (remainingLength >= 2)
                    {
                        char lookahead = jsonString[i];
                        if (lookahead == '\\')
                        {
                            sb.Append('\\');
                            ++i;
                        }
                        else if (lookahead == '"')
                        {
                            sb.Append("\"");
                            ++i;
                        }
                        else if (lookahead == 't')
                        {
                            sb.Append('\t');
                            ++i;
                        }
                        else if (lookahead == 'b')
                        {
                            sb.Append('\b');
                            ++i;
                        }
                        else if (lookahead == 'n')
                        {
                            sb.Append('\n');
                            ++i;
                        }
                        else if (lookahead == 'r')
                        {
                            sb.Append('\r');
                            ++i;
                        }
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        static IDictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
        {
            IDictionary<string, object> table = new JsonObject();
            int token;

            // {
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                token = LookAhead(json, index);
                if (token == TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == TOKEN_COMMA)
                    NextToken(json, ref index);
                else if (token == TOKEN_CURLY_CLOSE)
                {
                    NextToken(json, ref index);
                    return table;
                }
                else
                {
                    // name
                    string name = ParseString(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }
                    // :
                    token = NextToken(json, ref index);
                    if (token != TOKEN_COLON)
                    {
                        success = false;
                        return null;
                    }
                    // value
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        success = false;
                        return null;
                    }
                    table[name] = value;
                }
            }
            return table;
        }

        static JsonArray ParseArray(char[] json, ref int index, ref bool success)
        {
            JsonArray array = new JsonArray();

            // [
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                int token = LookAhead(json, index);
                if (token == TOKEN_NONE)
                {
                    success = false;
                    return null;
                }
                else if (token == TOKEN_COMMA)
                    NextToken(json, ref index);
                else if (token == TOKEN_SQUARED_CLOSE)
                {
                    NextToken(json, ref index);
                    break;
                }
                else
                {
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                        return null;
                    array.Add(value);
                }
            }
            return array;
        }

        static object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (LookAhead(json, index))
            {
                case TOKEN_STRING:
                    return ParseString(json, ref index, ref success);
                case TOKEN_NUMBER:
                    return ParseNumber(json, ref index, ref success);
                case TOKEN_CURLY_OPEN:
                    return ParseObject(json, ref index, ref success);
                case TOKEN_SQUARED_OPEN:
                    return ParseArray(json, ref index, ref success);
                case TOKEN_TRUE:
                    NextToken(json, ref index);
                    return true;
                case TOKEN_FALSE:
                    NextToken(json, ref index);
                    return false;
                case TOKEN_NULL:
                    NextToken(json, ref index);
                    return null;
                case TOKEN_NONE:
                    break;
            }
            success = false;
            return null;
        }

        static string ParseString(char[] json, ref int index, ref bool success)
        {
            StringBuilder s = new StringBuilder(BUILDER_CAPACITY);
            char c;

            EatWhitespace(json, ref index);

            // "
            c = json[index++];
            bool complete = false;
            while (!complete)
            {
                if (index == json.Length)
                    break;

                c = json[index++];
                if (c == '"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == json.Length)
                        break;
                    c = json[index++];
                    if (c == '"')
                        s.Append('"');
                    else if (c == '\\')
                        s.Append('\\');
                    else if (c == '/')
                        s.Append('/');
                    else if (c == 'b')
                        s.Append('\b');
                    else if (c == 'f')
                        s.Append('\f');
                    else if (c == 'n')
                        s.Append('\n');
                    else if (c == 'r')
                        s.Append('\r');
                    else if (c == 't')
                        s.Append('\t');
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint;
                            if (!(success = UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out codePoint)))
                                return "";

                            // convert the integer codepoint to a unicode char and add to string
                            if (0xD800 <= codePoint && codePoint <= 0xDBFF)  // if high surrogate
                            {
                                index += 4; // skip 4 chars
                                remainingLength = json.Length - index;
                                if (remainingLength >= 6)
                                {
                                    uint lowCodePoint;
                                    if (new string(json, index, 2) == "\\u" && UInt32.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out lowCodePoint))
                                    {
                                        if (0xDC00 <= lowCodePoint && lowCodePoint <= 0xDFFF)    // if low surrogate
                                        {
                                            s.Append((char)codePoint);
                                            s.Append((char)lowCodePoint);
                                            index += 6; // skip 6 chars
                                            continue;
                                        }
                                    }
                                }
                                success = false;    // invalid surrogate pair
                                return "";
                            }
                            s.Append(ConvertFromUtf32((int)codePoint));
                            // skip 4 chars
                            index += 4;
                        }
                        else
                            break;
                    }
                }
                else
                    s.Append(c);
            }
            if (!complete)
            {
                success = false;
                return null;
            }
            return s.ToString();
        }

        private static string ConvertFromUtf32(int utf32)
        {
            // http://www.java2s.com/Open-Source/CSharp/2.6.4-mono-.net-core/System/System/Char.cs.htm
            if (utf32 < 0 || utf32 > 0x10FFFF)
                throw new ArgumentOutOfRangeException("utf32", "The argument must be from 0 to 0x10FFFF.");
            if (0xD800 <= utf32 && utf32 <= 0xDFFF)
                throw new ArgumentOutOfRangeException("utf32", "The argument must not be in surrogate pair range.");
            if (utf32 < 0x10000)
                return new string((char)utf32, 1);
            utf32 -= 0x10000;
            return new string(new char[] { (char)((utf32 >> 10) + 0xD800), (char)(utf32 % 0x0400 + 0xDC00) });
        }

        static object ParseNumber(char[] json, ref int index, ref bool success)
        {
            EatWhitespace(json, ref index);
            int lastIndex = GetLastIndexOfNumber(json, index);
            int charLength = (lastIndex - index) + 1;
            object returnNumber;
            string str = new string(json, index, charLength);
            if (str.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || str.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
            {
                double number;
                success = double.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
                returnNumber = number;
            }
            else
            {
                long number;
                success = long.TryParse(new string(json, index, charLength), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
                returnNumber = number;
            }
            index = lastIndex + 1;
            return returnNumber;
        }

        static int GetLastIndexOfNumber(char[] json, int index)
        {
            int lastIndex;
            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1) break;
            return lastIndex - 1;
        }

        static void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
                if (" \t\n\r\b\f".IndexOf(json[index]) == -1) break;
        }

        static int LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return NextToken(json, ref saveIndex);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        static int NextToken(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);
            if (index == json.Length)
                return TOKEN_NONE;
            char c = json[index];
            index++;
            switch (c)
            {
                case '{':
                    return TOKEN_CURLY_OPEN;
                case '}':
                    return TOKEN_CURLY_CLOSE;
                case '[':
                    return TOKEN_SQUARED_OPEN;
                case ']':
                    return TOKEN_SQUARED_CLOSE;
                case ',':
                    return TOKEN_COMMA;
                case '"':
                    return TOKEN_STRING;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                    return TOKEN_NUMBER;
                case ':':
                    return TOKEN_COLON;
            }
            index--;
            int remainingLength = json.Length - index;
            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
                {
                    index += 5;
                    return TOKEN_FALSE;
                }
            }
            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
                {
                    index += 4;
                    return TOKEN_TRUE;
                }
            }
            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
                {
                    index += 4;
                    return TOKEN_NULL;
                }
            }
            return TOKEN_NONE;
        }

        static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
        {
            bool success = true;
            string stringValue = value as string;
            if (stringValue != null)
                success = SerializeString(stringValue, builder);
            else
            {
                IDictionary<string, object> dict = value as IDictionary<string, object>;
                if (dict != null)
                {
                    success = SerializeObject(jsonSerializerStrategy, dict.Keys, dict.Values, builder);
                }
                else
                {
                    IDictionary<string, string> stringDictionary = value as IDictionary<string, string>;
                    if (stringDictionary != null)
                    {
                        success = SerializeObject(jsonSerializerStrategy, stringDictionary.Keys, stringDictionary.Values, builder);
                    }
                    else
                    {
                        IEnumerable enumerableValue = value as IEnumerable;
                        if (enumerableValue != null)
                            success = SerializeArray(jsonSerializerStrategy, enumerableValue, builder);
                        else if (IsNumeric(value))
                            success = SerializeNumber(value, builder);
                        else if (value is bool)
                            builder.Append((bool)value ? "true" : "false");
                        else if (value == null)
                            builder.Append("null");
                        else
                        {
                            object serializedObject;
                            success = jsonSerializerStrategy.TrySerializeNonPrimitiveObject(value, out serializedObject);
                            if (success)
                                SerializeValue(jsonSerializerStrategy, serializedObject, builder);
                        }
                    }
                }
            }
            return success;
        }

        static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
        {
            builder.Append("{");
            IEnumerator ke = keys.GetEnumerator();
            IEnumerator ve = values.GetEnumerator();
            bool first = true;
            while (ke.MoveNext() && ve.MoveNext())
            {
                object key = ke.Current;
                object value = ve.Current;
                if (!first)
                    builder.Append(",");
                string stringKey = key as string;
                if (stringKey != null)
                    SerializeString(stringKey, builder);
                else
                    if (!SerializeValue(jsonSerializerStrategy, value, builder)) return false;
                builder.Append(":");
                if (!SerializeValue(jsonSerializerStrategy, value, builder))
                    return false;
                first = false;
            }
            builder.Append("}");
            return true;
        }

        static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
        {
            builder.Append("[");
            bool first = true;
            foreach (object value in anArray)
            {
                if (!first)
                    builder.Append(",");
                if (!SerializeValue(jsonSerializerStrategy, value, builder))
                    return false;
                first = false;
            }
            builder.Append("]");
            return true;
        }

        static bool SerializeString(string aString, StringBuilder builder)
        {
            // Happy path if there's nothing to be escaped. IndexOfAny is highly optimized (and unmanaged)
            if (aString.IndexOfAny(EscapeCharacters) == -1)
            {
                builder.Append('"');
                builder.Append(aString);
                builder.Append('"');

                return true;
            }

            builder.Append('"');
            int safeCharacterCount = 0;
            char[] charArray = aString.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];

                // Non ascii characters are fine, buffer them up and send them to the builder
                // in larger chunks if possible. The escape table is a 1:1 translation table
                // with \0 [default(char)] denoting a safe character.
                if (c >= EscapeTable.Length || EscapeTable[c] == default(char))
                {
                    safeCharacterCount++;
                }
                else
                {
                    if (safeCharacterCount > 0)
                    {
                        builder.Append(charArray, i - safeCharacterCount, safeCharacterCount);
                        safeCharacterCount = 0;
                    }

                    builder.Append('\\');
                    builder.Append(EscapeTable[c]);
                }
            }

            if (safeCharacterCount > 0)
            {
                builder.Append(charArray, charArray.Length - safeCharacterCount, safeCharacterCount);
            }

            builder.Append('"');
            return true;
        }

        static bool SerializeNumber(object number, StringBuilder builder)
        {
            if (number is long)
                builder.Append(((long)number).ToString(CultureInfo.InvariantCulture));
            else if (number is ulong)
                builder.Append(((ulong)number).ToString(CultureInfo.InvariantCulture));
            else if (number is int)
                builder.Append(((int)number).ToString(CultureInfo.InvariantCulture));
            else if (number is uint)
                builder.Append(((uint)number).ToString(CultureInfo.InvariantCulture));
            else if (number is decimal)
                builder.Append(((decimal)number).ToString(CultureInfo.InvariantCulture));
            else if (number is float)
                builder.Append(((float)number).ToString(CultureInfo.InvariantCulture));
            else
                builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
            return true;
        }

        /// <summary>
        /// Determines if a given object is numeric in any way
        /// (can be integer, double, null, etc).
        /// </summary>
        static bool IsNumeric(object value)
        {
            if (value is sbyte) return true;
            if (value is byte) return true;
            if (value is short) return true;
            if (value is ushort) return true;
            if (value is int) return true;
            if (value is uint) return true;
            if (value is long) return true;
            if (value is ulong) return true;
            if (value is float) return true;
            if (value is double) return true;
            if (value is decimal) return true;
            return false;
        }

        private static IJsonSerializerStrategy _currentJsonSerializerStrategy;
        public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
        {
            get
            {
                return _currentJsonSerializerStrategy ??
                    (_currentJsonSerializerStrategy =
#if SIMPLE_JSON_DATACONTRACT
 DataContractJsonSerializerStrategy
#else
 PocoJsonSerializerStrategy
#endif
);
            }
            set
            {
                _currentJsonSerializerStrategy = value;
            }
        }

        private static PocoJsonSerializerStrategy _pocoJsonSerializerStrategy;
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy
        {
            get
            {
                return _pocoJsonSerializerStrategy ?? (_pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());
            }
        }

#if SIMPLE_JSON_DATACONTRACT

        private static DataContractJsonSerializerStrategy _dataContractJsonSerializerStrategy;
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Advanced)]
        public static DataContractJsonSerializerStrategy DataContractJsonSerializerStrategy
        {
            get
            {
                return _dataContractJsonSerializerStrategy ?? (_dataContractJsonSerializerStrategy = new DataContractJsonSerializerStrategy());
            }
        }

#endif
    }

    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
 interface IJsonSerializerStrategy
    {
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        bool TrySerializeNonPrimitiveObject(object input, out object output);
        object DeserializeObject(object value, Type type);
    }

    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
 class PocoJsonSerializerStrategy : IJsonSerializerStrategy
    {
        internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;
        internal IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>> GetCache;
        internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

        internal static readonly Type[] EmptyTypes = new Type[0];
        internal static readonly Type[] ArrayConstructorParameterTypes = new Type[] { typeof(int) };

        private static readonly string[] Iso8601Format = new string[]
                                                             {
                                                                 @"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z",
                                                                 @"yyyy-MM-dd\THH:mm:ss\Z",
                                                                 @"yyyy-MM-dd\THH:mm:ssK"
                                                             };

        public PocoJsonSerializerStrategy()
        {
            ConstructorCache = new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(ContructorDelegateFactory);
            GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
            SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
        }

        protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            return clrPropertyName;
        }

        internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
        {
            return ReflectionUtils.GetContructor(key, key.IsArray ? ArrayConstructorParameterTypes : EmptyTypes);
        }

        internal virtual IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
        {
            IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanRead)
                {
                    MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                    if (getMethod.IsStatic || !getMethod.IsPublic)
                        continue;
                    result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = ReflectionUtils.GetGetMethod(propertyInfo);
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (fieldInfo.IsStatic || !fieldInfo.IsPublic)
                    continue;
                result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = ReflectionUtils.GetGetMethod(fieldInfo);
            }
            return result;
        }

        internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
        {
            IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanWrite)
                {
                    MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                    if (setMethod.IsStatic || !setMethod.IsPublic)
                        continue;
                    result[MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (fieldInfo.IsInitOnly || fieldInfo.IsStatic || !fieldInfo.IsPublic)
                    continue;
                result[MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
            }
            return result;
        }

        public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
        {
            return TrySerializeKnownTypes(input, out output) || TrySerializeUnknownTypes(input, out output);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public virtual object DeserializeObject(object value, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            string str = value as string;

            if (type == typeof(Guid) && string.IsNullOrEmpty(str))
                return default(Guid);

            if (value == null)
                return null;

            object obj = null;

            if (str != null)
            {
                if (str.Length != 0) // We know it can't be null now.
                {
                    if (type == typeof(DateTime) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime)))
                        return DateTime.ParseExact(str, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    if (type == typeof(DateTimeOffset) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset)))
                        return DateTimeOffset.ParseExact(str, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    if (type == typeof(Guid) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid)))
                        return new Guid(str);
                    if (type == typeof(Uri))
                    {
                        bool isValid = Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute);

                        Uri result;
                        if (isValid && Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out result))
                            return result;

                        return null;
                    }

                    if (type == typeof(string))
                        return str;

                    return Convert.ChangeType(str, type, CultureInfo.InvariantCulture);
                }
                else
                {
                    if (type == typeof(Guid))
                        obj = default(Guid);
                    else if (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                        obj = null;
                    else
                        obj = str;
                }
                // Empty string case
                if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
                    return str;
            }
            else if (value is bool)
                return value;

            bool valueIsLong = value is long;
            bool valueIsDouble = value is double;
            if ((valueIsLong && type == typeof(long)) || (valueIsDouble && type == typeof(double)))
                return value;
            if ((valueIsDouble && type != typeof(double)) || (valueIsLong && type != typeof(long)))
            {
                obj = type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(bool) || type == typeof(decimal) || type == typeof(byte) || type == typeof(short)
                            ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture)
                            : value;
            }
            else
            {
                IDictionary<string, object> objects = value as IDictionary<string, object>;
                if (objects != null)
                {
                    IDictionary<string, object> jsonObject = objects;

                    if (ReflectionUtils.IsTypeDictionary(type))
                    {
                        // if dictionary then
                        Type[] types = ReflectionUtils.GetGenericTypeArguments(type);
                        Type keyType = types[0];
                        Type valueType = types[1];

                        Type genericType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

                        IDictionary dict = (IDictionary)ConstructorCache[genericType]();

                        foreach (KeyValuePair<string, object> kvp in jsonObject)
                            dict.Add(kvp.Key, DeserializeObject(kvp.Value, valueType));

                        obj = dict;
                    }
                    else
                    {
                        if (type == typeof(object))
                            obj = value;
                        else
                        {
                            obj = ConstructorCache[type]();
                            foreach (KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> setter in SetCache[type])
                            {
                                object jsonValue;
                                if (jsonObject.TryGetValue(setter.Key, out jsonValue))
                                {
                                    jsonValue = DeserializeObject(jsonValue, setter.Value.Key);
                                    setter.Value.Value(obj, jsonValue);
                                }
                            }
                        }
                    }
                }
                else
                {
                    IList<object> valueAsList = value as IList<object>;
                    if (valueAsList != null)
                    {
                        IList<object> jsonObject = valueAsList;
                        IList list = null;

                        if (type.IsArray)
                        {
                            list = (IList)ConstructorCache[type](jsonObject.Count);
                            int i = 0;
                            foreach (object o in jsonObject)
                                list[i++] = DeserializeObject(o, type.GetElementType());
                        }
                        else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof(IList), type))
                        {
                            Type innerType = ReflectionUtils.GetGenericListElementType(type);
                            list = (IList)(ConstructorCache[type] ?? ConstructorCache[typeof(List<>).MakeGenericType(innerType)])(jsonObject.Count);
                            foreach (object o in jsonObject)
                                list.Add(DeserializeObject(o, innerType));
                        }
                        obj = list;
                    }
                }
                return obj;
            }
            if (ReflectionUtils.IsNullableType(type))
                return ReflectionUtils.ToNullableType(obj, type);
            return obj;
        }

        protected virtual object SerializeEnum(Enum p)
        {
            return Convert.ToDouble(p, CultureInfo.InvariantCulture);
        }

        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        protected virtual bool TrySerializeKnownTypes(object input, out object output)
        {
            bool returnValue = true;
            if (input is DateTime)
                output = ((DateTime)input).ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
            else if (input is DateTimeOffset)
                output = ((DateTimeOffset)input).ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
            else if (input is Guid)
                output = ((Guid)input).ToString("D");
            else if (input is Uri)
                output = input.ToString();
            else
            {
                Enum inputEnum = input as Enum;
                if (inputEnum != null)
                    output = SerializeEnum(inputEnum);
                else
                {
                    returnValue = false;
                    output = null;
                }
            }
            return returnValue;
        }
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Need to support .NET 2")]
        protected virtual bool TrySerializeUnknownTypes(object input, out object output)
        {
            if (input == null) throw new ArgumentNullException("input");
            output = null;
            Type type = input.GetType();
            if (type.FullName == null)
                return false;
            IDictionary<string, object> obj = new JsonObject();
            IDictionary<string, ReflectionUtils.GetDelegate> getters = GetCache[type];
            foreach (KeyValuePair<string, ReflectionUtils.GetDelegate> getter in getters)
            {
                if (getter.Value != null)
                    obj.Add(MapClrMemberNameToJsonFieldName(getter.Key), getter.Value(input));
            }
            output = obj;
            return true;
        }
    }

#if SIMPLE_JSON_DATACONTRACT
    [GeneratedCode("simple-json", "1.0.0")]
#if SIMPLE_JSON_INTERNAL
    internal
#else
    public
#endif
 class DataContractJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        public DataContractJsonSerializerStrategy()
        {
            GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(GetterValueFactory);
            SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(SetterValueFactory);
        }

        internal override IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
        {
            bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
            if (!hasDataContract)
                return base.GetterValueFactory(type);
            string jsonKey;
            IDictionary<string, ReflectionUtils.GetDelegate> result = new Dictionary<string, ReflectionUtils.GetDelegate>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanRead)
                {
                    MethodInfo getMethod = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
                    if (!getMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                        result[jsonKey] = ReflectionUtils.GetGetMethod(propertyInfo);
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (!fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                    result[jsonKey] = ReflectionUtils.GetGetMethod(fieldInfo);
            }
            return result;
        }

        internal override IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
        {
            bool hasDataContract = ReflectionUtils.GetAttribute(type, typeof(DataContractAttribute)) != null;
            if (!hasDataContract)
                return base.SetterValueFactory(type);
            string jsonKey;
            IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> result = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
            foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
            {
                if (propertyInfo.CanWrite)
                {
                    MethodInfo setMethod = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
                    if (!setMethod.IsStatic && CanAdd(propertyInfo, out jsonKey))
                        result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
                }
            }
            foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
            {
                if (!fieldInfo.IsInitOnly && !fieldInfo.IsStatic && CanAdd(fieldInfo, out jsonKey))
                    result[jsonKey] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
            }
            // todo implement sorting for DATACONTRACT.
            return result;
        }

        private static bool CanAdd(MemberInfo info, out string jsonKey)
        {
            jsonKey = null;
            if (ReflectionUtils.GetAttribute(info, typeof(IgnoreDataMemberAttribute)) != null)
                return false;
            DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)ReflectionUtils.GetAttribute(info, typeof(DataMemberAttribute));
            if (dataMemberAttribute == null)
                return false;
            jsonKey = string.IsNullOrEmpty(dataMemberAttribute.Name) ? info.Name : dataMemberAttribute.Name;
            return true;
        }
    }

#endif

    namespace Reflection
    {
        // This class is meant to be copied into other libraries. So we want to exclude it from Code Analysis rules
        // that might be in place in the target project.
        [GeneratedCode("reflection-utils", "1.0.0")]
#if SIMPLE_JSON_REFLECTION_UTILS_PUBLIC
        public
#else
        internal
#endif
 class ReflectionUtils
        {
            private static readonly object[] EmptyObjects = new object[] { };

            public delegate object GetDelegate(object source);
            public delegate void SetDelegate(object source, object value);
            public delegate object ConstructorDelegate(params object[] args);

            public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

#if SIMPLE_JSON_TYPEINFO
            public static TypeInfo GetTypeInfo(Type type)
            {
                return type.GetTypeInfo();
            }
#else
            public static Type GetTypeInfo(Type type)
            {
                return type;
            }
#endif

            public static Attribute GetAttribute(MemberInfo info, Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                if (info == null || type == null || !info.IsDefined(type))
                    return null;
                return info.GetCustomAttribute(type);
#else
                if (info == null || type == null || !Attribute.IsDefined(info, type))
                    return null;
                return Attribute.GetCustomAttribute(info, type);
#endif
            }

            public static Type GetGenericListElementType(Type type)
            {
                IEnumerable<Type> interfaces;
#if SIMPLE_JSON_TYPEINFO
                interfaces = type.GetTypeInfo().ImplementedInterfaces;
#else
                interfaces = type.GetInterfaces();
#endif
                foreach (Type implementedInterface in interfaces)
                {
                    if (IsTypeGeneric(implementedInterface) &&
                        implementedInterface.GetGenericTypeDefinition() == typeof(IList<>))
                    {
                        return GetGenericTypeArguments(implementedInterface)[0];
                    }
                }
                return GetGenericTypeArguments(type)[0];
            }

            public static Attribute GetAttribute(Type objectType, Type attributeType)
            {

#if SIMPLE_JSON_TYPEINFO
                if (objectType == null || attributeType == null || !objectType.GetTypeInfo().IsDefined(attributeType))
                    return null;
                return objectType.GetTypeInfo().GetCustomAttribute(attributeType);
#else
                if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
                    return null;
                return Attribute.GetCustomAttribute(objectType, attributeType);
#endif
            }

            public static Type[] GetGenericTypeArguments(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetTypeInfo().GenericTypeArguments;
#else
                return type.GetGenericArguments();
#endif
            }

            public static bool IsTypeGeneric(Type type)
            {
                return GetTypeInfo(type).IsGenericType;
            }

            public static bool IsTypeGenericeCollectionInterface(Type type)
            {
                if (!IsTypeGeneric(type))
                    return false;

                Type genericDefinition = type.GetGenericTypeDefinition();

                return (genericDefinition == typeof(IList<>)
                    || genericDefinition == typeof(ICollection<>)
                    || genericDefinition == typeof(IEnumerable<>)
#if SIMPLE_JSON_READONLY_COLLECTIONS
                    || genericDefinition == typeof(IReadOnlyCollection<>)
                    || genericDefinition == typeof(IReadOnlyList<>)
#endif
);
            }

            public static bool IsAssignableFrom(Type type1, Type type2)
            {
                return GetTypeInfo(type1).IsAssignableFrom(GetTypeInfo(type2));
            }

            public static bool IsTypeDictionary(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                if (typeof(IDictionary<,>).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    return true;
#else
                if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
                    return true;
#endif
                if (!GetTypeInfo(type).IsGenericType)
                    return false;

                Type genericDefinition = type.GetGenericTypeDefinition();
                return genericDefinition == typeof(IDictionary<,>);
            }

            public static bool IsNullableType(Type type)
            {
                return GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            public static object ToNullableType(object obj, Type nullableType)
            {
                return obj == null ? null : Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture);
            }

            public static bool IsValueType(Type type)
            {
                return GetTypeInfo(type).IsValueType;
            }

            public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetTypeInfo().DeclaredConstructors;
#else
                return type.GetConstructors();
#endif
            }

            public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
            {
                IEnumerable<ConstructorInfo> constructorInfos = GetConstructors(type);
                int i;
                bool matches;
                foreach (ConstructorInfo constructorInfo in constructorInfos)
                {
                    ParameterInfo[] parameters = constructorInfo.GetParameters();
                    if (argsType.Length != parameters.Length)
                        continue;

                    i = 0;
                    matches = true;
                    foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                    {
                        if (parameterInfo.ParameterType != argsType[i])
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (matches)
                        return constructorInfo;
                }

                return null;
            }

            public static IEnumerable<PropertyInfo> GetProperties(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetRuntimeProperties();
#else
                return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#endif
            }

            public static IEnumerable<FieldInfo> GetFields(Type type)
            {
#if SIMPLE_JSON_TYPEINFO
                return type.GetRuntimeFields();
#else
                return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#endif
            }

            public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_TYPEINFO
                return propertyInfo.GetMethod;
#else
                return propertyInfo.GetGetMethod(true);
#endif
            }

            public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_TYPEINFO
                return propertyInfo.SetMethod;
#else
                return propertyInfo.GetSetMethod(true);
#endif
            }

            public static ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetConstructorByReflection(constructorInfo);
#else
                return GetConstructorByExpression(constructorInfo);
#endif
            }

            public static ConstructorDelegate GetContructor(Type type, params Type[] argsType)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetConstructorByReflection(type, argsType);
#else
                return GetConstructorByExpression(type, argsType);
#endif
            }

            public static ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
            {
                return delegate(object[] args) { return constructorInfo.Invoke(args); };
            }

            public static ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
            {
                ConstructorInfo constructorInfo = GetConstructorInfo(type, argsType);
                return constructorInfo == null ? null : GetConstructorByReflection(constructorInfo);
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION

            public static ConstructorDelegate GetConstructorByExpression(ConstructorInfo constructorInfo)
            {
                ParameterInfo[] paramsInfo = constructorInfo.GetParameters();
                ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
                Expression[] argsExp = new Expression[paramsInfo.Length];
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Expression index = Expression.Constant(i);
                    Type paramType = paramsInfo[i].ParameterType;
                    Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                    Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                    argsExp[i] = paramCastExp;
                }
                NewExpression newExp = Expression.New(constructorInfo, argsExp);
                Expression<Func<object[], object>> lambda = Expression.Lambda<Func<object[], object>>(newExp, param);
                Func<object[], object> compiledLambda = lambda.Compile();
                return delegate(object[] args) { return compiledLambda(args); };
            }

            public static ConstructorDelegate GetConstructorByExpression(Type type, params Type[] argsType)
            {
                ConstructorInfo constructorInfo = GetConstructorInfo(type, argsType);
                return constructorInfo == null ? null : GetConstructorByExpression(constructorInfo);
            }

#endif

            public static GetDelegate GetGetMethod(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetGetMethodByReflection(propertyInfo);
#else
                return GetGetMethodByExpression(propertyInfo);
#endif
            }

            public static GetDelegate GetGetMethod(FieldInfo fieldInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetGetMethodByReflection(fieldInfo);
#else
                return GetGetMethodByExpression(fieldInfo);
#endif
            }

            public static GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
            {
                MethodInfo methodInfo = GetGetterMethodInfo(propertyInfo);
                return delegate(object source) { return methodInfo.Invoke(source, EmptyObjects); };
            }

            public static GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
            {
                return delegate(object source) { return fieldInfo.GetValue(source); };
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION

            public static GetDelegate GetGetMethodByExpression(PropertyInfo propertyInfo)
            {
                MethodInfo getMethodInfo = GetGetterMethodInfo(propertyInfo);
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                UnaryExpression instanceCast = (!IsValueType(propertyInfo.DeclaringType)) ? Expression.TypeAs(instance, propertyInfo.DeclaringType) : Expression.Convert(instance, propertyInfo.DeclaringType);
                Func<object, object> compiled = Expression.Lambda<Func<object, object>>(Expression.TypeAs(Expression.Call(instanceCast, getMethodInfo), typeof(object)), instance).Compile();
                return delegate(object source) { return compiled(source); };
            }

            public static GetDelegate GetGetMethodByExpression(FieldInfo fieldInfo)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                MemberExpression member = Expression.Field(Expression.Convert(instance, fieldInfo.DeclaringType), fieldInfo);
                GetDelegate compiled = Expression.Lambda<GetDelegate>(Expression.Convert(member, typeof(object)), instance).Compile();
                return delegate(object source) { return compiled(source); };
            }

#endif

            public static SetDelegate GetSetMethod(PropertyInfo propertyInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetSetMethodByReflection(propertyInfo);
#else
                return GetSetMethodByExpression(propertyInfo);
#endif
            }

            public static SetDelegate GetSetMethod(FieldInfo fieldInfo)
            {
#if SIMPLE_JSON_NO_LINQ_EXPRESSION
                return GetSetMethodByReflection(fieldInfo);
#else
                return GetSetMethodByExpression(fieldInfo);
#endif
            }

            public static SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
            {
                MethodInfo methodInfo = GetSetterMethodInfo(propertyInfo);
                return delegate(object source, object value) { methodInfo.Invoke(source, new object[] { value }); };
            }

            public static SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
            {
                return delegate(object source, object value) { fieldInfo.SetValue(source, value); };
            }

#if !SIMPLE_JSON_NO_LINQ_EXPRESSION

            public static SetDelegate GetSetMethodByExpression(PropertyInfo propertyInfo)
            {
                MethodInfo setMethodInfo = GetSetterMethodInfo(propertyInfo);
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                UnaryExpression instanceCast = (!IsValueType(propertyInfo.DeclaringType)) ? Expression.TypeAs(instance, propertyInfo.DeclaringType) : Expression.Convert(instance, propertyInfo.DeclaringType);
                UnaryExpression valueCast = (!IsValueType(propertyInfo.PropertyType)) ? Expression.TypeAs(value, propertyInfo.PropertyType) : Expression.Convert(value, propertyInfo.PropertyType);
                Action<object, object> compiled = Expression.Lambda<Action<object, object>>(Expression.Call(instanceCast, setMethodInfo, valueCast), new ParameterExpression[] { instance, value }).Compile();
                return delegate(object source, object val) { compiled(source, val); };
            }

            public static SetDelegate GetSetMethodByExpression(FieldInfo fieldInfo)
            {
                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                Action<object, object> compiled = Expression.Lambda<Action<object, object>>(
                    Assign(Expression.Field(Expression.Convert(instance, fieldInfo.DeclaringType), fieldInfo), Expression.Convert(value, fieldInfo.FieldType)), instance, value).Compile();
                return delegate(object source, object val) { compiled(source, val); };
            }

            public static BinaryExpression Assign(Expression left, Expression right)
            {
#if SIMPLE_JSON_TYPEINFO
                return Expression.Assign(left, right);
#else
                MethodInfo assign = typeof(Assigner<>).MakeGenericType(left.Type).GetMethod("Assign");
                BinaryExpression assignExpr = Expression.Add(left, right, assign);
                return assignExpr;
#endif
            }

            private static class Assigner<T>
            {
                public static T Assign(ref T left, T right)
                {
                    return (left = right);
                }
            }

#endif

            public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
            {
                private readonly object _lock = new object();
                private readonly ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;
                private Dictionary<TKey, TValue> _dictionary;

                public ThreadSafeDictionary(ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
                {
                    _valueFactory = valueFactory;
                }

                private TValue Get(TKey key)
                {
                    if (_dictionary == null)
                        return AddValue(key);
                    TValue value;
                    if (!_dictionary.TryGetValue(key, out value))
                        return AddValue(key);
                    return value;
                }

                private TValue AddValue(TKey key)
                {
                    TValue value = _valueFactory(key);
                    lock (_lock)
                    {
                        if (_dictionary == null)
                        {
                            _dictionary = new Dictionary<TKey, TValue>();
                            _dictionary[key] = value;
                        }
                        else
                        {
                            TValue val;
                            if (_dictionary.TryGetValue(key, out val))
                                return val;
                            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>(_dictionary);
                            dict[key] = value;
                            _dictionary = dict;
                        }
                    }
                    return value;
                }

                public void Add(TKey key, TValue value)
                {
                    throw new NotImplementedException();
                }

                public bool ContainsKey(TKey key)
                {
                    return _dictionary.ContainsKey(key);
                }

                public ICollection<TKey> Keys
                {
                    get { return _dictionary.Keys; }
                }

                public bool Remove(TKey key)
                {
                    throw new NotImplementedException();
                }

                public bool TryGetValue(TKey key, out TValue value)
                {
                    value = this[key];
                    return true;
                }

                public ICollection<TValue> Values
                {
                    get { return _dictionary.Values; }
                }

                public TValue this[TKey key]
                {
                    get { return Get(key); }
                    set { throw new NotImplementedException(); }
                }

                public void Add(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                public void Clear()
                {
                    throw new NotImplementedException();
                }

                public bool Contains(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
                {
                    throw new NotImplementedException();
                }

                public int Count
                {
                    get { return _dictionary.Count; }
                }

                public bool IsReadOnly
                {
                    get { throw new NotImplementedException(); }
                }

                public bool Remove(KeyValuePair<TKey, TValue> item)
                {
                    throw new NotImplementedException();
                }

                public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
                {
                    return _dictionary.GetEnumerator();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return _dictionary.GetEnumerator();
                }
            }

        }
    }
}
// ReSharper restore LoopCanBeConvertedToQuery
// ReSharper restore RedundantExplicitArrayCreation
// ReSharper restore SuggestUseVarKeywordEvident