package Extensions
{
	import Actions.*;
	
	import Conditions.*;
	
	import Expressions.*;
	
	import Objects.*;
	
	import RunLoop.*;
	
	import Services.*;
	
	import Sprites.*;

	import Extensions.DarkEdif.*;
	
	import flash.events.*;
	import flash.net.*;
	import flash.filesystem.*;
	import flash.utils.*;

	public class CRunGamejoltGameAPI extends CRunExtension
	{
        public const ExtensionVersion:int = 6;
        public const SDKVersion:int = 20;

		public static const JoltBase:String = "https://api.gamejolt.com";
		private static const ACT_AUTH:int = 0;
		private static const ACT_AUTHCREDS:int = 1;
		private static const ACT_SETGUESTNAME:int = 2;
		private static const ACT_FETCHUSERNAME:int = 3;
		private static const ACT_FETCHUSERID:int = 4;
		private static const ACT_OPENSESSION:int = 5;
		private static const ACT_PINGSESSION:int = 6;
		private static const ACT_PINGSTATUSSESSION:int = 7;
		private static const ACT_CHECKSESSION:int = 8;
		private static const ACT_CLOSESESSION:int = 9;
		private static const ACT_ADDUSERSCORE:int = 10;
		private static const ACT_ADDGUESTSCORE:int = 11;
		private static const ACT_GETSCORERANKING:int = 12;
		private static const ACT_FETCHSCORES:int = 13;
		private static const ACT_GETTABLES:int = 14;
		private static const ACT_GETTROPHY:int = 15;
		private static const ACT_GETTROPHIES:int = 16;
		private static const ACT_GETUNLOCKEDTROPHIES:int = 17;
		private static const ACT_UNLOCKTROPHY:int = 18;
		private static const ACT_LOCKTROPHY:int = 19;
		private static const ACT_GLOBALSTORAGEGETDATA:int = 20;
		private static const ACT_GLOBALSTORAGEGETKEYS:int = 21;
		private static const ACT_GLOBALSTORAGEDELETEKEY:int = 22;
		private static const ACT_GLOBALSTORAGESETKEY:int = 23;
		private static const ACT_GLOBALSTORAGEUPDATEKEY:int = 24;
		private static const ACT_USERSTORAGEGETDATA:int = 25;
		private static const ACT_USERSTORAGEGETKEYS:int = 26;
		private static const ACT_USERSTORAGEDELETEKEY:int = 27;
		private static const ACT_USERSTORAGESETKEY:int = 28;
		private static const ACT_USERSTORAGEUPDATEKEY:int = 29;
		private static const ACT_GLOBALFILESTORAGESAVEDATA:int = 30;
		private static const ACT_GLOBALFILESTORAGESETKEY:int = 31;
		private static const ACT_GLOBALFILESTORAGEUPDATEKEY:int = 32;
		private static const ACT_USERFILESTORAGESAVEDATA:int = 33;
		private static const ACT_USERFILESTORAGESETKEY:int = 34;
		private static const ACT_USERFILESTORAGEUPDATEKEY:int = 35;
		private static const ACT_GETFRIENDSLIST:int = 36;
		private static const ACT_GETCURRENTTIME:int = 37;
		private static const ACT_SETGAMEID:int = 38;
		private static const ACT_SETPRIVATEKEY:int = 39;
		private static const ACT_FETCHUSERSCORES:int = 40;
		private static const CND_AUTHFINISHED:int = 0;
		private static const CND_FETCHFINISHED:int = 1;
		private static const CND_OPENFINISHED:int = 2;
		private static const CND_PINGFINISHED:int = 3;
		private static const CND_CHECKFINISHED:int = 4;
		private static const CND_CLOSEFINISHED:int = 5;
		private static const CND_SCOREADDED:int = 6;
		private static const CND_RANKINGRETRIEVED:int = 7;
		private static const CND_SCORESFETCHED:int = 8;
		private static const CND_TABLESRETRIEVED:int = 9;
		private static const CND_TROPHIESRETRIEVED:int = 10;
		private static const CND_TROPHYADDED:int = 11;
		private static const CND_TROPHYREMOVED:int = 12;
		private static const CND_GSGETDATA:int = 13;
		private static const CND_GSGETKEYS:int = 14;
		private static const CND_GSDELETEKEY:int = 15;
		private static const CND_GSSETKEY:int = 16;
		private static const CND_GSUPDATEKEY:int = 17;
		private static const CND_USGETDATA:int = 18;
		private static const CND_USGETKEYS:int = 19;
		private static const CND_USDELETEKEY:int = 20;
		private static const CND_USSETKEY:int = 21;
		private static const CND_USUPDATEKEY:int = 22;
		private static const CND_FGSSAVEDATA:int = 23;
		private static const CND_FGSSETKEY:int = 24;
		private static const CND_FGSUPDATEKEY:int = 25;
		private static const CND_UGSSAVEDATA:int = 26;
		private static const CND_UGSSETKEY:int = 27;
		private static const CND_UGSUPDATEKEY:int = 28;
		private static const CND_GETFRIENDSLIST:int = 29;
		private static const CND_GETCURRENTTIME:int = 30;
		private static const CND_ANYCALLFINISHED:int = 31;
		private static const CND_ONERROR:int = 32;
		private static const EXP_GETJSONRESPONSE:int = 0;
		private static const EXP_GETRESPONSETYPE:int = 1;
		private static const EXP_GETRESPONSESTATUS:int = 2;
		private static const EXP_GETRESPONSEMESSAGE:int = 3;
		private static const EXP_GETUSERNAME:int = 4;
		private static const EXP_GETUSERTOKEN:int = 5;
		private static const EXP_GETGUESTNAME:int = 6;
		private static const EXP_FETCHEDUSERCOUNT:int = 7;
		private static const EXP_FETCHEDUSERDISPLAYNAME:int = 8;
		private static const EXP_FETCHEDUSERNAME:int = 9;
		private static const EXP_FETCHEDUSERID:int = 10;
		private static const EXP_FETCHEDUSERDESCRIPTION:int = 11;
		private static const EXP_FETCHEDUSERAVATAR:int = 12;
		private static const EXP_FETCHEDUSERWEBSITE:int = 13;
		private static const EXP_FETCHEDUSERSTATUS:int = 14;
		private static const EXP_FETCHEDUSERTYPE:int = 15;
		private static const EXP_FETCHEDUSERLASTLOGGEDIN:int = 16;
		private static const EXP_FETCHEDUSERLASTLOGGEDINTIMESTAMP:int = 17;
		private static const EXP_FETCHEDUSERSIGNEDUP:int = 18;
		private static const EXP_FETCHEDUSERSIGNEDUPTIMESTAMP:int = 19;
		private static const EXP_SCORERANKING:int = 20;
		private static const EXP_FETCHEDSCORECOUNT:int = 21;
		private static const EXP_FETCHEDSCOREUSERNAME:int = 22;
		private static const EXP_FETCHEDSCOREUSERID:int = 23;
		private static const EXP_FETCHEDSCOREGUESTNAME:int = 24;
		private static const EXP_FETCHEDSCORESCORE:int = 25;
		private static const EXP_FETCHEDSCORESORT:int = 26;
		private static const EXP_FETCHEDSCOREEXTRADATA:int = 27;
		private static const EXP_FETCHEDSCORESUBMIT:int = 28;
		private static const EXP_FETCHEDSCORESUBMITTIMESTAMP:int = 29;
		private static const EXP_FETCHEDTABLECOUNT:int = 30;
		private static const EXP_FETCHEDTABLENAME:int = 31;
		private static const EXP_FETCHEDTABLEID:int = 32;
		private static const EXP_FETCHEDTABLEDESCRIPTION:int = 33;
		private static const EXP_FETCHEDTABLEISPRIMARY:int = 34;
		private static const EXP_FETCHEDTROPHYCOUNT:int = 35;
		private static const EXP_FETCHEDTROPHYTITLE:int = 36;
		private static const EXP_FETCHEDTROPHYID:int = 37;
		private static const EXP_FETCHEDTROPHYDESCRIPTION:int = 38;
		private static const EXP_FETCHEDTROPHYDIFFICULTY:int = 39;
		private static const EXP_FETCHEDTROPHYIMAGEURL:int = 40;
		private static const EXP_FETCHEDTROPHYACHIEVED:int = 41;
		private static const EXP_RETRIEVEDKEYDATA:int = 42;
		private static const EXP_FETCHEDKEYCOUNT:int = 43;
		private static const EXP_FETCHEDKEY:int = 44;
		private static const EXP_UPDATEDKEYDATA:int = 45;
		private static const EXP_FETCHEDFRIENDCOUNT:int = 46;
		private static const EXP_FETCHEDFRIEND:int = 47;
		private static const EXP_TIMEYEAR:int = 48;
		private static const EXP_TIMEMONTH:int = 49;
		private static const EXP_TIMEDAY:int = 50;
		private static const EXP_TIMEHOUR:int = 51;
		private static const EXP_TIMEMINUTE:int = 52;
		private static const EXP_TIMESECOND:int = 53;
		private static const EXP_TIMETIMESTAMP:int = 54;
		private static const EXP_TIMETIMEZONE:int = 55;
		private static const EXP_GETGAMEID:int = 56;
		private static const EXP_GETPRIVATEKEY:int = 57;
		private static const EXP_GETREQUESTURL:int = 58;
		private static const EXP_FETCHEDSCORETABLEID:int = 59;
		private static const EXP_GETERRORMESSAGE:int = 60;

        private var gameID:String = "";
        private var privateKey:String = "";
        private var gameAuthData:CRunGamejoltGameAPIGameAuth = null;
		private var hasFilesystem:Boolean = false;
		
		private var triggerBuffer:Vector.<CRunGamejoltGameAPIResponseTicket> = null;
		private var latestResponse:CRunGamejoltGameAPIResponseTicket = null;

        public function CRunGamejoltGameAPI()
        {
            DarkEdif.checkSupportsSDKVersion(SDKVersion);
			triggerBuffer = new Vector.<CRunGamejoltGameAPIResponseTicket>();
			
			try
			{
				getDefinitionByName("flash.filesystem.File");
				hasFilesystem = true;
			}
			catch (e:Error) {}
        }
		
	    public override function createRunObject(file:CBinaryFile, cob:CCreateObjectInfo, version:int):Boolean
	    {
            if (ho == null)
            {
                throw new Error("HeaderObject not defined when needed to be.");
            }

            // DarkEdif properties are accessible as on other platforms: IsPropChecked(), GetPropertyStr(), GetPropertyNum()
            var props:DarkEdifProperties = DarkEdif.getProperties(this, file, version);
            gameID = props.GetPropertyStr(0);
		    privateKey = props.GetPropertyStr(1);
		    gameAuthData = DarkEdif.getGlobalData("AuthInfo");
            if (gameAuthData == null)
            {
                gameAuthData = new CRunGamejoltGameAPIGameAuth("", "", "");
                DarkEdif.setGlobalData("AuthInfo", gameAuthData);
            }
            
            // The return value is not used in this version of the runtime: always return false.
            return false;
	    }

		public override function handleRunObject():int
	    {
			while (triggerBuffer.length > 0)
			{
				latestResponse = triggerBuffer.shift();

				// Error Handling!
				if (latestResponse.HasError)
				{
					ho.generateEvent(CND_ONERROR, 0);
					continue;
				}

				if (latestResponse.HasTrigger)
					ho.generateEvent(latestResponse.Trigger, 0);
				ho.generateEvent(CND_ANYCALLFINISHED, 0); // Cnd_AnyCallFinished
			}
			return 0;
	    }

		private function serializeUrl(url:String):String
		{
			return url + "&signature=" + CRunGamejoltGameAPIMD5.hash(JoltBase + url + privateKey);
		}

		private function httpGet(url:String, responseType:String, trigger:int = -1):void
		{
			var responseTicket:CRunGamejoltGameAPIResponseTicket = new CRunGamejoltGameAPIResponseTicket(url, responseType);
			if (trigger != -1)
			{
				responseTicket.HasTrigger = true;
				responseTicket.Trigger = trigger;
			}

			var request:URLRequest = new URLRequest(JoltBase + serializeUrl(url));
			var loader:URLLoader = new URLLoader();
			loader.dataFormat = URLLoaderDataFormat.TEXT;

			loader.addEventListener(Event.COMPLETE, function(e:Event):void
			{
				try
				{
					responseTicket.Data = JSON.parse(loader.data);
				}
				catch (error:Error)
				{
					responseTicket.HasError = true;
					responseTicket.Error = error.message;
				}
				triggerBuffer.push(responseTicket);
			});

			loader.addEventListener(IOErrorEvent.IO_ERROR, function(e:IOErrorEvent):void
			{
				responseTicket.HasError = true;
				responseTicket.Error = e.text;
				triggerBuffer.push(responseTicket);
			});

			loader.load(request);
		}

 	    // Actions
	    // -------------------------------------------------
	    public override function action(num:int, act:CActExtension):void
	    {
			switch(num)
			{
				case ACT_AUTH:
					var actAuthUserName:String = act.getParamExpString(rh, 0);
					var actAuthUserToken:String = act.getParamExpString(rh, 1);
					actAuth(actAuthUserName, actAuthUserToken);
					break;
				case ACT_AUTHCREDS:
					actAuthCreds();
					break;
				case ACT_SETGUESTNAME:
					var actSetGuestNameGuestName:String = act.getParamExpString(rh, 0);
					actSetGuestName(actSetGuestNameGuestName);
					break;
				case ACT_FETCHUSERNAME:
					var actFetchUsernameUserName:String = act.getParamExpString(rh, 0);
					actFetchUsername(actFetchUsernameUserName);
					break;
				case ACT_FETCHUSERID:
					var actFetchUserIDUserID:int = act.getParamExpression(rh, 0);
					actFetchUserID(actFetchUserIDUserID);
					break;
				case ACT_OPENSESSION:
					actOpenSession();
					break;
				case ACT_PINGSESSION:
					actPingSession();
					break;
				case ACT_PINGSTATUSSESSION:
					var actPingStatusSessionSession:String = act.getParamExpString(rh, 0);
					actPingStatusSession(actPingStatusSessionSession);
					break;
				case ACT_CHECKSESSION:
					actCheckSession();
					break;
				case ACT_CLOSESESSION:
					actCloseSession();
					break;
				case ACT_ADDUSERSCORE:
					var actAddUserScoreDisplayScore:String = act.getParamExpString(rh, 0);
					var actAddUserScoreSortScore:int = act.getParamExpression(rh, 1);
					var actAddUserScoreTable:int = act.getParamExpression(rh, 2);
					var actAddUserScoreExtraData:String = act.getParamExpString(rh, 3);
					actAddUserScore(actAddUserScoreDisplayScore, actAddUserScoreSortScore, actAddUserScoreTable, actAddUserScoreExtraData);
					break;
				case ACT_ADDGUESTSCORE:
					var actAddGuestScoreDisplayScore:String = act.getParamExpString(rh, 0);
					var actAddGuestScoreSortScore:int = act.getParamExpression(rh, 1);
					var actAddGuestScoreTable:int = act.getParamExpression(rh, 2);
					var actAddGuestScoreExtraData:String = act.getParamExpString(rh, 3);
					actAddGuestScore(actAddGuestScoreDisplayScore, actAddGuestScoreSortScore, actAddGuestScoreTable, actAddGuestScoreExtraData);
					break;
				case ACT_GETSCORERANKING:
					var actGetScoreRankingScore:int = act.getParamExpression(rh, 0);
					var actGetScoreRankingTable:int = act.getParamExpression(rh, 1);
					actGetScoreRanking(actGetScoreRankingScore, actGetScoreRankingTable);
					break;
				case ACT_FETCHSCORES:
					var actFetchScoresTable:int = act.getParamExpression(rh, 0);
					var actFetchScoresLimit:int = act.getParamExpression(rh, 1);
					var actFetchScoresBetterThan:int = act.getParamExpression(rh, 2);
					var actFetchScoresWorseThan:int = act.getParamExpression(rh, 3);
					actFetchScores(actFetchScoresTable, actFetchScoresLimit, actFetchScoresBetterThan, actFetchScoresWorseThan);
					break;
				case ACT_GETTABLES:
					actGetTables();
					break;
				case ACT_GETTROPHY:
					var actGetTrophyTrophy:int = act.getParamExpression(rh, 0);
					actGetTrophy(actGetTrophyTrophy);
					break;
				case ACT_GETTROPHIES:
					actGetTrophies();
					break;
				case ACT_GETUNLOCKEDTROPHIES:
					actGetUnlockedTrophies();
					break;
				case ACT_UNLOCKTROPHY:
					var actUnlockTrophyTrophy:int = act.getParamExpression(rh, 0);
					actUnlockTrophy(actUnlockTrophyTrophy);
					break;
				case ACT_LOCKTROPHY:
					var actLockTrophyTrophy:int = act.getParamExpression(rh, 0);
					actLockTrophy(actLockTrophyTrophy);
					break;
				case ACT_GLOBALSTORAGEGETDATA:
					var actGlobalStorageGetDataKey:String = act.getParamExpString(rh, 0);
					actGlobalStorageGetData(actGlobalStorageGetDataKey);
					break;
				case ACT_GLOBALSTORAGEGETKEYS:
					var actGlobalStorageGetKeysPattern:String = act.getParamExpString(rh, 0);
					actGlobalStorageGetKeys(actGlobalStorageGetKeysPattern);
					break;
				case ACT_GLOBALSTORAGEDELETEKEY:
					var actGlobalStorageDeleteKeyKey:String = act.getParamExpString(rh, 0);
					actGlobalStorageDeleteKey(actGlobalStorageDeleteKeyKey);
					break;
				case ACT_GLOBALSTORAGESETKEY:
					var actGlobalStorageSetKeyKey:String = act.getParamExpString(rh, 0);
					var actGlobalStorageSetKeyData:String = act.getParamExpString(rh, 1);
					actGlobalStorageSetKey(actGlobalStorageSetKeyKey, actGlobalStorageSetKeyData);
					break;
				case ACT_GLOBALSTORAGEUPDATEKEY:
					var actGlobalStorageUpdateKeyKey:String = act.getParamExpString(rh, 0);
					var actGlobalStorageUpdateKeyData:String = act.getParamExpString(rh, 1);
					var actGlobalStorageUpdateKeyOperation:String = act.getParamExpString(rh, 2);
					actGlobalStorageUpdateKey(actGlobalStorageUpdateKeyKey, actGlobalStorageUpdateKeyData, actGlobalStorageUpdateKeyOperation);
					break;
				case ACT_USERSTORAGEGETDATA:
					var actUserStorageGetDataKey:String = act.getParamExpString(rh, 0);
					actUserStorageGetData(actUserStorageGetDataKey);
					break;
				case ACT_USERSTORAGEGETKEYS:
					var actUserStorageGetKeysPattern:String = act.getParamExpString(rh, 0);
					actUserStorageGetKeys(actUserStorageGetKeysPattern);
					break;
				case ACT_USERSTORAGEDELETEKEY:
					var actUserStorageDeleteKeyKey:String = act.getParamExpString(rh, 0);
					actUserStorageDeleteKey(actUserStorageDeleteKeyKey);
					break;
				case ACT_USERSTORAGESETKEY:
					var actUserStorageSetKeyKey:String = act.getParamExpString(rh, 0);
					var actUserStorageSetKeyData:String = act.getParamExpString(rh, 1);
					actUserStorageSetKey(actUserStorageSetKeyKey, actUserStorageSetKeyData);
					break;
				case ACT_USERSTORAGEUPDATEKEY:
					var actUserStorageUpdateKeyKey:String = act.getParamExpString(rh, 0);
					var actUserStorageUpdateKeyData:String = act.getParamExpString(rh, 1);
					var actUserStorageUpdateKeyOperation:String = act.getParamExpString(rh, 2);
					actUserStorageUpdateKey(actUserStorageUpdateKeyKey, actUserStorageUpdateKeyData, actUserStorageUpdateKeyOperation);
					break;
				case ACT_GLOBALFILESTORAGESAVEDATA:
					var actGlobalFileStorageSaveDataKey:String = act.getParamExpString(rh, 0);
					var actGlobalFileStorageSaveDataFilePath:String = act.getParamExpString(rh, 1);
					actGlobalFileStorageSaveData(actGlobalFileStorageSaveDataKey, actGlobalFileStorageSaveDataFilePath);
					break;
				case ACT_GLOBALFILESTORAGESETKEY:
					var actGlobalFileStorageSetKeyKey:String = act.getParamExpString(rh, 0);
					var actGlobalFileStorageSetKeyFilePath:String = act.getParamExpString(rh, 1);
					actGlobalFileStorageSetKey(actGlobalFileStorageSetKeyKey, actGlobalFileStorageSetKeyFilePath);
					break;
				case ACT_GLOBALFILESTORAGEUPDATEKEY:
					var actGlobalFileStorageUpdateKeyKey:String = act.getParamExpString(rh, 0);
					var actGlobalFileStorageUpdateKeyFilePath:String = act.getParamExpString(rh, 1);
					var actGlobalFileStorageUpdateKeyOperation:String = act.getParamExpString(rh, 2);
					actGlobalFileStorageUpdateKey(actGlobalFileStorageUpdateKeyKey, actGlobalFileStorageUpdateKeyFilePath, actGlobalFileStorageUpdateKeyOperation);
					break;
				case ACT_USERFILESTORAGESAVEDATA:
					var actUserFileStorageSaveDataKey:String = act.getParamExpString(rh, 0);
					var actUserFileStorageSaveDataFilePath:String = act.getParamExpString(rh, 1);
					actUserFileStorageSaveData(actUserFileStorageSaveDataKey, actUserFileStorageSaveDataFilePath);
					break;
				case ACT_USERFILESTORAGESETKEY:
					var actUserFileStorageSetKeyKey:String = act.getParamExpString(rh, 0);
					var actUserFileStorageSetKeyFilePath:String = act.getParamExpString(rh, 1);
					actUserFileStorageSetKey(actUserFileStorageSetKeyKey, actUserFileStorageSetKeyFilePath);
					break;
				case ACT_USERFILESTORAGEUPDATEKEY:
					var actUserFileStorageUpdateKeyKey:String = act.getParamExpString(rh, 0);
					var actUserFileStorageUpdateKeyFilePath:String = act.getParamExpString(rh, 1);
					var actUserFileStorageUpdateKeyOperation:String = act.getParamExpString(rh, 2);
					actUserFileStorageUpdateKey(actUserFileStorageUpdateKeyKey, actUserFileStorageUpdateKeyFilePath, actUserFileStorageUpdateKeyOperation);
					break;
				case ACT_GETFRIENDSLIST:
					actGetFriendsList();
					break;
				case ACT_GETCURRENTTIME:
					actGetCurrentTime();
					break;
				case ACT_SETGAMEID:
					var actSetGameIDGameID:String = act.getParamExpString(rh, 0);
					actSetGameID(actSetGameIDGameID);
					break;
				case ACT_SETPRIVATEKEY:
					var actSetPrivateKeyPrivateKey:String = act.getParamExpString(rh, 0);
					actSetPrivateKey(actSetPrivateKeyPrivateKey);
					break;
				case ACT_FETCHUSERSCORES:
					var actFetchUserScoresTable:int = act.getParamExpression(rh, 0);
					var actFetchUserScoresLimit:int = act.getParamExpression(rh, 1);
					var actFetchUserScoresBetterThan:int = act.getParamExpression(rh, 2);
					var actFetchUserScoresWorseThan:int = act.getParamExpression(rh, 3);
					actFetchUserScores(actFetchUserScoresTable, actFetchUserScoresLimit, actFetchUserScoresBetterThan, actFetchUserScoresWorseThan);
					break;
			}
	    }

		private function actAuth(userName:String, userToken:String):void
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

				'Auth',
				CND_AUTHFINISHED
			)
		}

		private function actAuthCreds():void
		{
			// Not available, possibly in a browser
			if (!hasFilesystem)
			{
				var response:CRunGamejoltGameAPIResponseTicket = new CRunGamejoltGameAPIResponseTicket('', '');
				response.HasError = true;
				response.Error = "Cannot use 'Authorize user via .gj-credentials' in a browser";
				triggerBuffer.push(response);
				return;
			}

			var stream:FileStream = new FileStream();
			var file:File = File.applicationDirectory.resolvePath(".gj-credentials");

			if (file.exists)
			{
				stream.open(file, FileMode.READ);
				var contents:String = stream.readUTFBytes(stream.bytesAvailable);
				stream.close();

				var lines:Array = contents.split("\n");
				if (lines.length >= 3)
				{
					gameAuthData.UserName = lines[1].replace(/\r/g, "");
					gameAuthData.UserToken = lines[2].replace(/\r/g, "");
				}
				
				httpGet(
					"/api/game/v1_2/users/auth/?game_id=" +
					gameID +
					"&username=" +
					gameAuthData.UserName +
					"&user_token=" +
					gameAuthData.UserToken,

					'Auth',
					CND_AUTHFINISHED
				)
			}
		}

		private function actSetGameID(gameID:String):void
		{
			this.gameID = gameID;
		}

		private function actSetPrivateKey(privateKey:String):void
		{
			this.privateKey = privateKey;
		}

		private function actSetGuestName(guestName:String):void
		{
			gameAuthData.GuestName = guestName;
		}

		private function actFetchUsername(userName:String):void
		{
			httpGet(
				"/api/game/v1_2/users/?game_id=" +
				gameID +
				"&username=" +
				userName,

				'FetchUsers',
				CND_FETCHFINISHED
			)
		}

		private function actFetchUserID(userID:int):void
		{
			httpGet(
				"/api/game/v1_2/users/?game_id=" +
				gameID +
				"&user_id=" +
				userID,

				'FetchUsers',
				CND_FETCHFINISHED
			)
		}

		private function actOpenSession():void
		{
			httpGet(
				"/api/game/v1_2/sessions/open/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'OpenSession',
				CND_OPENFINISHED
			)
		}

		private function actPingSession():void
		{
			httpGet(
				"/api/game/v1_2/sessions/ping/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'PingSession',
				CND_PINGFINISHED
			)
		}

		private function actPingStatusSession(status:String):void
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

				'PingSession',
				CND_PINGFINISHED
			)
		}

		private function actCheckSession():void
		{
			httpGet(
				"/api/game/v1_2/sessions/check/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'CheckSession',
				CND_CHECKFINISHED
			)
		}

		private function actCloseSession():void
		{
			httpGet(
				"/api/game/v1_2/sessions/close/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'CloseSession',
				CND_CLOSEFINISHED
			)
		}

		private function actAddUserScore(displayScore:String, sortScore:int, table:int, extraData:String):void
		{
			var url:String =
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
			if (extraData.length > 0)
				url += "&extra_data=" + extraData;

			httpGet(
				url,
				'AddScore',
				CND_SCOREADDED
			)
		}

		private function actAddGuestScore(displayScore:String, sortScore:int, table:int, extraData:String):void
		{
			var url:String =
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
			if (extraData.length > 0)
				url += "&extra_data=" + extraData;

			httpGet(
				url,
				'AddScore',
				CND_SCOREADDED
			)
		}

		private function actGetScoreRanking(score:int, table:int):void
		{
			var url:String =
				"/api/game/v1_2/scores/get-rank/?game_id=" + 
				gameID +
				"&sort=" + 
				score;
			if (table != -1)
				url += "&table_id=" + table;

			httpGet(
				url,
				'GetRank',
				CND_RANKINGRETRIEVED
			)
		}

		private function actFetchScores(table:int, limit:int, betterThan:int, worseThan:int):void
		{
			var url:String = "/api/game/v1_2/scores/?game_id=" + gameID;
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
				'FetchScores',
				CND_SCORESFETCHED
			)
		}

		private function actFetchUserScores(table:int, limit:int, betterThan:int, worseThan:int):void
		{
			var url:String = "/api/game/v1_2/scores/?game_id=" + gameID;
			if (gameAuthData.UserName.length > 0 && gameAuthData.UserToken.length > 0)
				url += "&username=" + gameAuthData.UserName + "&user_token=" + gameAuthData.UserToken;
			else if (gameAuthData.GuestName.length > 0)
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
				'FetchScores',
				CND_SCORESFETCHED
			)
		}

		private function actGetTables():void
		{
			httpGet(
				"/api/game/v1_2/scores/tables/?game_id=" +
				gameID,

				'ScoreTables',
				CND_TABLESRETRIEVED
			)
		}

		private function actGetTrophy(trophy:int):void
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

				'FetchTrophies',
				CND_TROPHIESRETRIEVED
			)
		}

		private function actGetTrophies():void
		{
			httpGet(
				"/api/game/v1_2/trophies/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'FetchTrophies',
				CND_TROPHIESRETRIEVED
			)
		}

		private function actGetUnlockedTrophies():void
		{
			httpGet(
				"/api/game/v1_2/trophies/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken +
				"&achieved=true",

				'FetchTrophies',
				CND_TROPHIESRETRIEVED
			)
		}

		private function actUnlockTrophy(trophy:int):void
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

				'AchieveTrophy',
				CND_TROPHYADDED
			)
		}

		private function actLockTrophy(trophy:int):void
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

				'RevokeTrophy',
				CND_TROPHYREMOVED
			)
		}

		private function actGlobalStorageGetData(key:String):void
		{
			httpGet(
				"/api/game/v1_2/data-store/?game_id=" +
				gameID +
				"&key=" +
				key,

				'FetchData',
				CND_GSGETDATA
			)
		}

		private function actGlobalStorageGetKeys(pattern:String):void
		{
			var url:String = "/api/game/v1_2/data-store/get-keys/?game_id=" + gameID;
			if (pattern.length > 0)
				url += "&pattern=" + pattern;

			httpGet(
				url,
				'GetDataKeys',
				CND_GSGETKEYS
			)
		}

		private function actGlobalStorageDeleteKey(key:String):void
		{
			httpGet(
				"/api/game/v1_2/data-store/remove/?game_id=" +
				gameID +
				"&key=" +
				key,

				'RemoveData',
				CND_GSDELETEKEY
			)
		}

		private function actGlobalStorageSetKey(key:String, data:String):void
		{
			httpGet(
				"/api/game/v1_2/data-store/set/?game_id=" +
				gameID +
				"&key=" +
				key +
				"&data=" +
				data,

				'SetData',
				CND_GSSETKEY
			)
		}

		private function actGlobalStorageUpdateKey(key:String, data:String, operation:String):void
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

				'UpdateData',
				CND_GSUPDATEKEY
			)
		}

		private function actUserStorageGetData(key:String):void
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

				'FetchData',
				CND_USGETDATA
			)
		}

		private function actUserStorageGetKeys(pattern:String):void
		{
			var url:String =
				"/api/game/v1_2/data-store/get-keys/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken;
			if (pattern.length > 0)
				url += "&pattern=" + pattern;

			httpGet(
				url,
				'GetDataKeys',
				CND_USGETKEYS
			)
		}

		private function actUserStorageDeleteKey(key:String):void
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

				'RemoveData',
				CND_USDELETEKEY
			)
		}

		private function actUserStorageSetKey(key:String, data:String):void
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

				'SetData',
				CND_USSETKEY
			)
		}

		private function actUserStorageUpdateKey(key:String, data:String, operation:String):void
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

				'UpdateData',
				CND_USUPDATEKEY
			)
		}

		private function actGlobalFileStorageSaveData(key:String, filePath:String):void
		{
			// No implementation
		}

		private function actGlobalFileStorageSetKey(key:String, filePath:String):void
		{
			// No implementation
		}

		private function actGlobalFileStorageUpdateKey(key:String, filePath:String, operation:String):void
		{
			// No implementation
		}

		private function actUserFileStorageSaveData(key:String, filePath:String):void
		{
			// No implementation
		}

		private function actUserFileStorageSetKey(key:String, filePath:String):void
		{
			// No implementation
		}

		private function actUserFileStorageUpdateKey(key:String, filePath:String, operation:String):void
		{
			// No implementation
		}

		private function actGetFriendsList():void
		{
			httpGet(
				"/api/game/v1_2/friends/?game_id=" +
				gameID +
				"&username=" +
				gameAuthData.UserName +
				"&user_token=" +
				gameAuthData.UserToken,

				'Friends',
				CND_GETFRIENDSLIST
			)
		}

		private function actGetCurrentTime():void
		{
			httpGet(
				"/api/game/v1_2/time/?game_id=" +
				gameID,

				'Time',
				CND_GETCURRENTTIME
			)
		}

 	    // Conditions
	    // -------------------------------------------------
	    public override function getNumberOfConditions():int
	    {
	        return 33;
	    }

	    public override function condition(num:int, cnd:CCndExtension):Boolean
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

		private function cndAnyCallTriggered():Boolean
		{
			return true;
		}

		private function cndCallTriggered():Boolean
		{
			return true;
		}
		
		// Expressions
		// -------------------------------------------------
	    public override function expression(num:int):CValue
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
					var expFetchedUserDisplayNameIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserDisplayName(expFetchedUserDisplayNameIndex));
				case EXP_FETCHEDUSERNAME:
					var expFetchedUsernameIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUsername(expFetchedUsernameIndex));
				case EXP_FETCHEDUSERID:
					var expFetchedUserIDIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserID(expFetchedUserIDIndex));
				case EXP_FETCHEDUSERDESCRIPTION:
					var expFetchedUserDescriptionIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserDescription(expFetchedUserDescriptionIndex));
				case EXP_FETCHEDUSERAVATAR:
					var expFetchedUserAvatarIndex:int = ho.getExpParam().getInt();
					var expFetchedUserAvatarResolution:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserAvatar(expFetchedUserAvatarIndex, expFetchedUserAvatarResolution));
				case EXP_FETCHEDUSERWEBSITE:
					var expFetchedUserWebsiteIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserWebsite(expFetchedUserWebsiteIndex));
				case EXP_FETCHEDUSERSTATUS:
					var expFetchedUserStatusIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserStatus(expFetchedUserStatusIndex));
				case EXP_FETCHEDUSERTYPE:
					var expFetchedUserTypeIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserType(expFetchedUserTypeIndex));
				case EXP_FETCHEDUSERLASTLOGGEDIN:
					var expFetchedUserLastLoggedInIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserLastLoggedIn(expFetchedUserLastLoggedInIndex));
				case EXP_FETCHEDUSERLASTLOGGEDINTIMESTAMP:
					var expFetchedUserLastLoggedInTimestampIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserLastLoggedInTimestamp(expFetchedUserLastLoggedInTimestampIndex));
				case EXP_FETCHEDUSERSIGNEDUP:
					var expFetchedUserSignedUpIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserSignedUp(expFetchedUserSignedUpIndex));
				case EXP_FETCHEDUSERSIGNEDUPTIMESTAMP:
					var expFetchedUserSignedUpTimestampIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedUserSignedUpTimestamp(expFetchedUserSignedUpTimestampIndex));
				case EXP_SCORERANKING:
					return new CValue(expScoreRanking());
				case EXP_FETCHEDSCORECOUNT:
					return new CValue(expFetchedScoreCount());
				case EXP_FETCHEDSCOREUSERNAME:
					var expFetchedScoreUsernameIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreUsername(expFetchedScoreUsernameIndex));
				case EXP_FETCHEDSCOREUSERID:
					var expFetchedScoreUserIDIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreUserID(expFetchedScoreUserIDIndex));
				case EXP_FETCHEDSCOREGUESTNAME:
					var expFetchedScoreGuestNameIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreGuestName(expFetchedScoreGuestNameIndex));
				case EXP_FETCHEDSCORESCORE:
					var expFetchedScoreScoreIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreScore(expFetchedScoreScoreIndex));
				case EXP_FETCHEDSCORESORT:
					var expFetchedScoreSortIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreSort(expFetchedScoreSortIndex));
				case EXP_FETCHEDSCOREEXTRADATA:
					var expFetchedScoreExtraDataIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreExtraData(expFetchedScoreExtraDataIndex));
				case EXP_FETCHEDSCORESUBMIT:
					var expFetchedScoreSubmitIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreSubmit(expFetchedScoreSubmitIndex));
				case EXP_FETCHEDSCORESUBMITTIMESTAMP:
					var expFetchedScoreSubmitTimestampIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedScoreSubmitTimestamp(expFetchedScoreSubmitTimestampIndex));
				case EXP_FETCHEDTABLECOUNT:
					return new CValue(expFetchedTableCount());
				case EXP_FETCHEDTABLENAME:
					var expFetchedTableNameIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTableName(expFetchedTableNameIndex));
				case EXP_FETCHEDTABLEID:
					var expFetchedTableIDIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTableID(expFetchedTableIDIndex));
				case EXP_FETCHEDTABLEDESCRIPTION:
					var expFetchedTableDescriptionIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTableDescription(expFetchedTableDescriptionIndex));
				case EXP_FETCHEDTABLEISPRIMARY:
					var expFetchedTableIsPrimaryIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTableIsPrimary(expFetchedTableIsPrimaryIndex));
				case EXP_FETCHEDTROPHYCOUNT:
					return new CValue(expFetchedTrophyCount());
				case EXP_FETCHEDTROPHYTITLE:
					var expFetchedTrophyTitleIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyTitle(expFetchedTrophyTitleIndex));
				case EXP_FETCHEDTROPHYID:
					var expFetchedTrophyIDIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyID(expFetchedTrophyIDIndex));
				case EXP_FETCHEDTROPHYDESCRIPTION:
					var expFetchedTrophyDescriptionIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyDescription(expFetchedTrophyDescriptionIndex));
				case EXP_FETCHEDTROPHYDIFFICULTY:
					var expFetchedTrophyDifficultyIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyDifficulty(expFetchedTrophyDifficultyIndex));
				case EXP_FETCHEDTROPHYIMAGEURL:
					var expFetchedTrophyImageURLIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyImageURL(expFetchedTrophyImageURLIndex));
				case EXP_FETCHEDTROPHYACHIEVED:
					var expFetchedTrophyAchievedIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedTrophyAchieved(expFetchedTrophyAchievedIndex));
				case EXP_RETRIEVEDKEYDATA:
					return new CValue(expRetrievedKeyData());
				case EXP_FETCHEDKEYCOUNT:
					return new CValue(expFetchedKeyCount());
				case EXP_FETCHEDKEY:
					var expFetchedKeyIndex:int = ho.getExpParam().getInt();
					return new CValue(expFetchedKey(expFetchedKeyIndex));
				case EXP_UPDATEDKEYDATA:
					return new CValue(expUpdatedKeyData());
				case EXP_FETCHEDFRIENDCOUNT:
					return new CValue(expFetchedFriendCount());
				case EXP_FETCHEDFRIEND:
					var expFetchedFriendIndex:int = ho.getExpParam().getInt();
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

		private function expGetJsonResponse():String
		{
			if (latestResponse.HasError)
				return "";
			return JSON.stringify(latestResponse.Data);
		}

		private function expGetResponseType():String
		{
			return latestResponse.Type;
		}

		private function expGetResponseStatus():String
		{
			if (latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.success == null)
				return "";
				
			return j.response.success;
		}

		private function expGetResponseMessage():String
		{
			if (latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.message == null)
				return "";
				
			return j.response.message;
		}

		private function expGetErrorMessage():String
		{
			if (!latestResponse.HasError)
				return "";
			return latestResponse.Error;
		}

		private function expGetGameID():String
		{
			return gameID;
		}

		private function expGetPrivateKey():String
		{
			return privateKey;
		}

		private function expGetRequestURL():String
		{
			return JoltBase + latestResponse.URL;
		}

		private function expGetUserName():String
		{
			return gameAuthData.UserName;
		}

		private function expGetUserToken():String
		{
			return gameAuthData.UserToken;
		}

		private function expGetGuestName():String
		{
			return gameAuthData.GuestName;
		}

		private function expFetchedUserCount():int
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array))
				return 0;
				
			return j.response.users.length;
		}

		private function expFetchedUserDisplayName(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.developer_name == null)
				return "";

			return user.developer_name;
		}

		private function expFetchedUsername(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.username == null)
				return "";

			return user.username;
		}

		private function expFetchedUserID(index:int):int
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return 0;
			var user:Object = j.response.users[index];
			if (user.id == null)
				return 0;

			return parseInt(user.id);
		}

		private function expFetchedUserDescription(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.developer_description == null)
				return "";

			return user.developer_description;
		}

		private function expFetchedUserAvatar(index:int, resolution:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.avatar_url == null)
				return "";

			return "https://m.gjcdn.net/user-avatar/" + resolution + user.avatar_url.substring(34, user.avatar_url.lastIndexOf(".")) + ".png";
		}

		private function expFetchedUserWebsite(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.developer_website == null)
				return "";

			return user.developer_website;
		}

		private function expFetchedUserStatus(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.status == null)
				return "";

			return user.status;
		}

		private function expFetchedUserType(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.type == null)
				return "";

			return user.type;
		}

		private function expFetchedUserLastLoggedIn(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.last_logged_in == null)
				return "";

			return user.last_logged_in;
		}

		private function expFetchedUserLastLoggedInTimestamp(index:int):int
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return 0;
			var user:Object = j.response.users[index];
			if (user.last_logged_in_timestamp == null)
				return 0;

			return user.last_logged_in_timestamp;
		}

		private function expFetchedUserSignedUp(index:int):String
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return "";
			var user:Object = j.response.users[index];
			if (user.signed_up == null)
				return "";

			return user.signed_up;
		}

		private function expFetchedUserSignedUpTimestamp(index:int):int
		{
			if (latestResponse.Type != "FetchUsers" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.users == null || !(j.response.users is Array) || j.response.users.length <= index)
				return 0;
			var user:Object = j.response.users[index];
			if (user.signed_up_timestamp == null)
				return 0;

			return user.signed_up_timestamp;
		}

		private function expScoreRanking():int
		{
			if (latestResponse.Type != "GetRank" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.rank == null)
				return 0;

			return j.response.rank;
		}

		private function expFetchedScoreCount():int
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array))
				return 0;

			return j.response.scores.length;
		}

		private function expFetchedScoreUsername(index:int):String
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return "";
			var score:Object = j.response.scores[index];
			if (score.user == null)
				return "";

			return score.user;
		}

		private function expFetchedScoreUserID(index:int):int
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return 0;
			var score:Object = j.response.scores[index];
			if (score.user_id == null)
				return 0;

			return parseInt(score.user_id);
		}

		private function expFetchedScoreGuestName(index:int):String
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return "";
			var score:Object = j.response.scores[index];
			if (score.guest == null)
				return "";

			return score.guest;
		}

		private function expFetchedScoreScore(index:int):String
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return "";
			var score:Object = j.response.scores[index];
			if (score.score == null)
				return "";

			return score.score;
		}

		private function expFetchedScoreSort(index:int):int
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return 0;
			var score:Object = j.response.scores[index];
			if (score.sort == null)
				return 0;

			return parseInt(score.sort);
		}

		private function expFetchedScoreExtraData(index:int):String
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return "";
			var score:Object = j.response.scores[index];
			if (score.extra_data == null)
				return "";

			return score.extra_data;
		}

		private function expFetchedScoreSubmit(index:int):String
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return "";
			var score:Object = j.response.scores[index];
			if (score.stored == null)
				return "";

			return score.stored;
		}

		private function expFetchedScoreSubmitTimestamp(index:int):int
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.scores == null || !(j.response.scores is Array) || j.response.scores.length <= index)
				return 0;
			var score:Object = j.response.scores[index];
			if (score.stored_timestamp == null)
				return 0;

			return score.stored_timestamp;
		}

		private function expFetchedScoreTableID():int
		{
			if (latestResponse.Type != "FetchScores" || latestResponse.HasError)
				return 0;

			var match:Array = latestResponse.URL.match(/table_id=([0-9]+)/);
			return match ? parseInt(match[1]) : 0;
		}

		private function expFetchedTableCount():int
		{
			if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.tables == null || !(j.response.tables is Array))
				return 0;

			return j.response.tables.length;
		}

		private function expFetchedTableName(index:int):String
		{
			if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.tables == null || !(j.response.tables is Array) || j.response.tables.length <= index)
				return "";
			var table:Object = j.response.tables[index];
			if (table.name == null)
				return "";

			return table.name;
		}

		private function expFetchedTableID(index:int):int
		{
			if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.tables == null || !(j.response.tables is Array) || j.response.tables.length <= index)
				return 0;
			var table:Object = j.response.tables[index];
			if (table.id == null)
				return 0;

			return parseInt(table.id);
		}

		private function expFetchedTableDescription(index:int):String
		{
			if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.tables == null || !(j.response.tables is Array) || j.response.tables.length <= index)
				return "";
			var table:Object = j.response.tables[index];
			if (table.description == null)
				return "";

			return table.description;
		}

		private function expFetchedTableIsPrimary(index:int):int
		{
			if (latestResponse.Type != "ScoreTables" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.tables == null || !(j.response.tables is Array) || j.response.tables.length <= index)
				return 0;
			var table:Object = j.response.tables[index];
			if (table.primary == null)
				return 0;

			return table.primary;
		}

		private function expFetchedTrophyCount():int
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array))
				return 0;

			return j.response.trophies.length;
		}

		private function expFetchedTrophyTitle(index:int):String
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return "";
			var trophy:Object = j.response.trophies[index];
			if (trophy.title == null)
				return "";

			return trophy.title;
		}

		private function expFetchedTrophyID(index:int):int
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return 0;
			var trophy:Object = j.response.trophies[index];
			if (trophy.id == null)
				return 0;

			return parseInt(trophy.id);
		}

		private function expFetchedTrophyDescription(index:int):String
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return "";
			var trophy:Object = j.response.trophies[index];
			if (trophy.description == null)
				return "";

			return trophy.description;
		}

		private function expFetchedTrophyDifficulty(index:int):String
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return "";
			var trophy:Object = j.response.trophies[index];
			if (trophy.difficulty == null)
				return "";

			return trophy.difficulty;
		}

		private function expFetchedTrophyImageURL(index:int):String
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return "";
			var trophy:Object = j.response.trophies[index];
			if (trophy.image_url == null)
				return "";

			return trophy.image_url;
		}

		private function expFetchedTrophyAchieved(index:int):String
		{
			if (latestResponse.Type != "FetchTrophies" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.trophies == null || !(j.response.trophies is Array) || j.response.trophies.length <= index)
				return "";
			var trophy:Object = j.response.trophies[index];
			if (trophy.achieved == null)
				return "";

			return trophy.achieved;
		}

		private function expRetrievedKeyData():String
		{
			if (latestResponse.Type != "FetchData" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.data == null)
				return "";

			return j.response.data;
		}

		private function expFetchedKeyCount():int
		{
			if (latestResponse.Type != "GetDataKeys" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.keys == null || !(j.response.keys is Array))
				return 0;

			return j.response.keys.length;
		}

		private function expFetchedKey(index:int):String
		{
			if (latestResponse.Type != "GetDataKeys" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.keys == null || !(j.response.keys is Array) || j.response.keys.length <= index)
				return "";
			var key:Object = j.response.keys[index];
			if (key.key == null)
				return "";

			return key.key;
		}

		private function expUpdatedKeyData():String
		{
			if (latestResponse.Type != "UpdateData" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.data == null)
				return "";

			return j.response.data;
		}

		private function expFetchedFriendCount():int
		{
			if (latestResponse.Type != "Friends" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.friends == null || !(j.response.friends is Array))
				return 0;

			return j.response.friends.length;
		}

		private function expFetchedFriend(index:int):int
		{
			if (latestResponse.Type != "Friends" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.friends == null || !(j.response.friends is Array) || j.response.friends.length <= index)
				return 0;
			var friend:Object = j.response.friends[index];
			if (friend.friend_id == null)
				return 0;

			return parseInt(friend.friend_id);
		}

		private function expTimeYear():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.year == null)
				return 0;

			return parseInt(j.response.year);
		}

		private function expTimeMonth():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.month == null)
				return 0;

			return parseInt(j.response.month);
		}

		private function expTimeDay():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.day == null)
				return 0;

			return parseInt(j.response.day);
		}

		private function expTimeHour():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.hour == null)
				return 0;

			return parseInt(j.response.hour);
		}

		private function expTimeMinute():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.minute == null)
				return 0;

			return parseInt(j.response.minute);
		}

		private function expTimeSecond():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.second == null)
				return 0;

			return parseInt(j.response.second);
		}

		private function expTimeTimestamp():int
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return 0;

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.timestamp == null)
				return 0;

			return j.response.timestamp;
		}

		private function expTimeTimezone():String
		{
			if (latestResponse.Type != "Time" || latestResponse.HasError)
				return "";

			var j:Object = latestResponse.Data;
			if (j.response == null || j.response.timezone == null)
				return "";

			return j.response.timezone;
		}
	}
}