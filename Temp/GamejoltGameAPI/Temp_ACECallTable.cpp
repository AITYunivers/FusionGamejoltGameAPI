// DarkEdifPreBuildTool: This file was generated for a Extension.cpp/h that was modified at 2024-06-11T02:30:00.2141831Z.
// This file is used by the DarkEdifPreBuildTool as an input template for the calltable generator.
// Do not modify or delete this file.
#if DARKEDIF_ACE_CALL_TABLE_INDEX==0
			case 31:
				return ext->Cnd_AnyCallTriggered();
			case 0:
				return ext->Cnd_CallTriggered();
			case 1:
				return ext->Cnd_CallTriggered();
			case 2:
				return ext->Cnd_CallTriggered();
			case 3:
				return ext->Cnd_CallTriggered();
			case 4:
				return ext->Cnd_CallTriggered();
			case 5:
				return ext->Cnd_CallTriggered();
			case 6:
				return ext->Cnd_CallTriggered();
			case 7:
				return ext->Cnd_CallTriggered();
			case 8:
				return ext->Cnd_CallTriggered();
			case 9:
				return ext->Cnd_CallTriggered();
			case 10:
				return ext->Cnd_CallTriggered();
			case 11:
				return ext->Cnd_CallTriggered();
			case 12:
				return ext->Cnd_CallTriggered();
			case 13:
				return ext->Cnd_CallTriggered();
			case 14:
				return ext->Cnd_CallTriggered();
			case 15:
				return ext->Cnd_CallTriggered();
			case 16:
				return ext->Cnd_CallTriggered();
			case 17:
				return ext->Cnd_CallTriggered();
			case 18:
				return ext->Cnd_CallTriggered();
			case 19:
				return ext->Cnd_CallTriggered();
			case 20:
				return ext->Cnd_CallTriggered();
			case 21:
				return ext->Cnd_CallTriggered();
			case 22:
				return ext->Cnd_CallTriggered();
			case 23:
				return ext->Cnd_CallTriggered();
			case 24:
				return ext->Cnd_CallTriggered();
			case 25:
				return ext->Cnd_CallTriggered();
			case 26:
				return ext->Cnd_CallTriggered();
			case 27:
				return ext->Cnd_CallTriggered();
			case 28:
				return ext->Cnd_CallTriggered();
			case 29:
				return ext->Cnd_CallTriggered();
			case 30:
				return ext->Cnd_CallTriggered();

#elif DARKEDIF_ACE_CALL_TABLE_INDEX==1
			case 0:
				return ext->Act_Auth(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 1:
				return ext->Act_AuthCreds(), 0;
			case 38:
				return ext->Act_SetGameID(*(const TCHAR* *)&Parameters[0]), 0;
			case 39:
				return ext->Act_SetPrivateKey(*(const TCHAR* *)&Parameters[0]), 0;
			case 2:
				return ext->Act_SetGuestName(*(const TCHAR* *)&Parameters[0]), 0;
			case 3:
				return ext->Act_FetchUsername(*(const TCHAR* *)&Parameters[0]), 0;
			case 4:
				return ext->Act_FetchUserID(*(int *)&Parameters[0]), 0;
			case 5:
				return ext->Act_OpenSession(), 0;
			case 6:
				return ext->Act_PingSession(), 0;
			case 7:
				return ext->Act_PingStatusSession(*(const TCHAR* *)&Parameters[0]), 0;
			case 8:
				return ext->Act_CheckSession(), 0;
			case 9:
				return ext->Act_CloseSession(), 0;
			case 10:
				return ext->Act_AddUserScore(*(const TCHAR* *)&Parameters[0], *(int *)&Parameters[1], *(int *)&Parameters[2], *(const TCHAR* *)&Parameters[3]), 0;
			case 11:
				return ext->Act_AddGuestScore(*(const TCHAR* *)&Parameters[0], *(int *)&Parameters[1], *(int *)&Parameters[2], *(const TCHAR* *)&Parameters[3]), 0;
			case 12:
				return ext->Act_GetScoreRanking(*(int *)&Parameters[0], *(int *)&Parameters[1]), 0;
			case 13:
				return ext->Act_FetchScores(*(int *)&Parameters[0], *(int *)&Parameters[1], *(int *)&Parameters[2], *(int *)&Parameters[3]), 0;
			case 40:
				return ext->Act_FetchUserScores(*(int *)&Parameters[0], *(int *)&Parameters[1], *(int *)&Parameters[2], *(int *)&Parameters[3]), 0;
			case 14:
				return ext->Act_GetTables(), 0;
			case 15:
				return ext->Act_GetTrophy(*(int *)&Parameters[0]), 0;
			case 16:
				return ext->Act_GetTrophies(), 0;
			case 17:
				return ext->Act_GetUnlockedTrophies(), 0;
			case 18:
				return ext->Act_UnlockTrophy(*(int *)&Parameters[0]), 0;
			case 19:
				return ext->Act_LockTrophy(*(int *)&Parameters[0]), 0;
			case 20:
				return ext->Act_GlobalStorageGetData(*(const TCHAR* *)&Parameters[0]), 0;
			case 21:
				return ext->Act_GlobalStorageGetKeys(*(const TCHAR* *)&Parameters[0]), 0;
			case 22:
				return ext->Act_GlobalStorageDeleteKey(*(const TCHAR* *)&Parameters[0]), 0;
			case 23:
				return ext->Act_GlobalStorageSetKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 24:
				return ext->Act_GlobalStorageUpdateKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1], *(const TCHAR* *)&Parameters[2]), 0;
			case 25:
				return ext->Act_UserStorageGetData(*(const TCHAR* *)&Parameters[0]), 0;
			case 26:
				return ext->Act_UserStorageGetKeys(*(const TCHAR* *)&Parameters[0]), 0;
			case 27:
				return ext->Act_UserStorageDeleteKey(*(const TCHAR* *)&Parameters[0]), 0;
			case 28:
				return ext->Act_UserStorageSetKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 29:
				return ext->Act_UserStorageUpdateKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1], *(const TCHAR* *)&Parameters[2]), 0;
			case 30:
				return ext->Act_GlobalFileStorageSaveData(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 31:
				return ext->Act_GlobalFileStorageSetKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 32:
				return ext->Act_GlobalFileStorageUpdateKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1], *(const TCHAR* *)&Parameters[2]), 0;
			case 33:
				return ext->Act_UserFileStorageSaveData(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 34:
				return ext->Act_UserFileStorageSetKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1]), 0;
			case 35:
				return ext->Act_UserFileStorageUpdateKey(*(const TCHAR* *)&Parameters[0], *(const TCHAR* *)&Parameters[1], *(const TCHAR* *)&Parameters[2]), 0;
			case 36:
				return ext->Act_GetFriendsList(), 0;
			case 37:
				return ext->Act_GetCurrentTime(), 0;

#elif DARKEDIF_ACE_CALL_TABLE_INDEX==2
		case 0:
			*((const TCHAR* *)&Result) = ext->Exp_GetJsonResponse();
			break;
		case 1:
			*((const TCHAR* *)&Result) = ext->Exp_GetResponseType();
			break;
		case 2:
			*((const TCHAR* *)&Result) = ext->Exp_GetResponseStatus();
			break;
		case 3:
			*((const TCHAR* *)&Result) = ext->Exp_GetResponseMessage();
			break;
		case 56:
			*((const TCHAR* *)&Result) = ext->Exp_GetGameID();
			break;
		case 57:
			*((const TCHAR* *)&Result) = ext->Exp_GetPrivateKey();
			break;
		case 4:
			*((const TCHAR* *)&Result) = ext->Exp_GetUserName();
			break;
		case 5:
			*((const TCHAR* *)&Result) = ext->Exp_GetUserToken();
			break;
		case 6:
			*((const TCHAR* *)&Result) = ext->Exp_GetGuestName();
			break;
		case 7:
			*((int *)&Result) = ext->Exp_FetchedUserCount();
			break;
		case 8:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserDisplayName(*(int *)&Parameters[0]);
			break;
		case 9:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUsername(*(int *)&Parameters[0]);
			break;
		case 10:
			*((int *)&Result) = ext->Exp_FetchedUserID(*(int *)&Parameters[0]);
			break;
		case 11:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserDescription(*(int *)&Parameters[0]);
			break;
		case 12:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserAvatar(*(int *)&Parameters[0], *(int *)&Parameters[1]);
			break;
		case 13:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserWebsite(*(int *)&Parameters[0]);
			break;
		case 14:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserStatus(*(int *)&Parameters[0]);
			break;
		case 15:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserType(*(int *)&Parameters[0]);
			break;
		case 16:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserLastLoggedIn(*(int *)&Parameters[0]);
			break;
		case 17:
			*((int *)&Result) = ext->Exp_FetchedUserLastLoggedInTimestamp(*(int *)&Parameters[0]);
			break;
		case 18:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedUserSignedUp(*(int *)&Parameters[0]);
			break;
		case 19:
			*((int *)&Result) = ext->Exp_FetchedUserSignedUpTimestamp(*(int *)&Parameters[0]);
			break;
		case 20:
			*((int *)&Result) = ext->Exp_ScoreRanking();
			break;
		case 21:
			*((int *)&Result) = ext->Exp_FetchedScoreCount();
			break;
		case 22:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedScoreUsername(*(int *)&Parameters[0]);
			break;
		case 23:
			*((int *)&Result) = ext->Exp_FetchedScoreUserID(*(int *)&Parameters[0]);
			break;
		case 24:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedScoreGuestName(*(int *)&Parameters[0]);
			break;
		case 25:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedScoreScore(*(int *)&Parameters[0]);
			break;
		case 26:
			*((int *)&Result) = ext->Exp_FetchedScoreSort(*(int *)&Parameters[0]);
			break;
		case 27:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedScoreExtraData(*(int *)&Parameters[0]);
			break;
		case 28:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedScoreSubmit(*(int *)&Parameters[0]);
			break;
		case 29:
			*((int *)&Result) = ext->Exp_FetchedScoreSubmitTimestamp(*(int *)&Parameters[0]);
			break;
		case 30:
			*((int *)&Result) = ext->Exp_FetchedTableCount();
			break;
		case 31:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTableName(*(int *)&Parameters[0]);
			break;
		case 32:
			*((int *)&Result) = ext->Exp_FetchedTableID(*(int *)&Parameters[0]);
			break;
		case 33:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTableDescription(*(int *)&Parameters[0]);
			break;
		case 34:
			*((int *)&Result) = ext->Exp_FetchedTableIsPrimary(*(int *)&Parameters[0]);
			break;
		case 35:
			*((int *)&Result) = ext->Exp_FetchedTrophyCount();
			break;
		case 36:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTrophyTitle(*(int *)&Parameters[0]);
			break;
		case 37:
			*((int *)&Result) = ext->Exp_FetchedTrophyID(*(int *)&Parameters[0]);
			break;
		case 38:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTrophyDescription(*(int *)&Parameters[0]);
			break;
		case 39:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTrophyDifficulty(*(int *)&Parameters[0]);
			break;
		case 40:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTrophyImageURL(*(int *)&Parameters[0]);
			break;
		case 41:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedTrophyAchieved(*(int *)&Parameters[0]);
			break;
		case 42:
			*((const TCHAR* *)&Result) = ext->Exp_RetrievedKeyData();
			break;
		case 43:
			*((int *)&Result) = ext->Exp_FetchedKeyCount();
			break;
		case 44:
			*((const TCHAR* *)&Result) = ext->Exp_FetchedKey(*(int *)&Parameters[0]);
			break;
		case 45:
			*((const TCHAR* *)&Result) = ext->Exp_UpdatedKeyData();
			break;
		case 46:
			*((int *)&Result) = ext->Exp_FetchedFriendCount();
			break;
		case 47:
			*((int *)&Result) = ext->Exp_FetchedFriend(*(int *)&Parameters[0]);
			break;
		case 48:
			*((int *)&Result) = ext->Exp_TimeYear();
			break;
		case 49:
			*((int *)&Result) = ext->Exp_TimeMonth();
			break;
		case 50:
			*((int *)&Result) = ext->Exp_TimeDay();
			break;
		case 51:
			*((int *)&Result) = ext->Exp_TimeHour();
			break;
		case 52:
			*((int *)&Result) = ext->Exp_TimeMinute();
			break;
		case 53:
			*((int *)&Result) = ext->Exp_TimeSecond();
			break;
		case 54:
			*((int *)&Result) = ext->Exp_TimeTimestamp();
			break;
		case 55:
			*((const TCHAR* *)&Result) = ext->Exp_TimeTimezone();
			break;

#else
	#error Incorrect DarkEdif ACE Call Table index
#endif
