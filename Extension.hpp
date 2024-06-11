#pragma once
#include "DarkEdif.hpp"
#ifndef __ANDROID__
#include <windows.h>
#include <tchar.h>
#endif
#include "httplib.h"
#include <fstream>
#include "json.hpp"
#include <cctype>
#include <iomanip>
#include <sstream>
#include <codecvt>
#include <string>
#include <cstdio>
#include <deque>
#include <iostream>
#include <ResponseType.hpp>
#include <ResponseTicket.hpp>
#include <nlohmann/json.hpp>
#include <vector>
#include <algorithm>
using json = nlohmann::json;

class Extension
{
public:

	RunHeader* rhPtr;
	RunObjectMultiPlatPtr rdPtr; // you should not need to access this
#ifdef __ANDROID__
	global<jobject> javaExtPtr;
#elif defined(__APPLE__)
	void* const objCExtPtr;
#endif

	Edif::Runtime Runtime;

	static const int MinimumBuild = 254;
	static const int Version = 1;

	// If you change OEFLAGS, make sure you modify RUNDATA so the data is available, or you'll get crashes!
	// For example, OEFLAGS::VALUES makes use of the AltVals rv struct.
	static const OEFLAGS OEFLAGS = OEFLAGS::NONE;
	static const OEPREFS OEPREFS = OEPREFS::NONE;

	static const int WindowProcPriority = 100;

#ifdef _WIN32
	Extension(RunObject* const rdPtr, const EDITDATA* const edPtr, const CreateObjectInfo* const cobPtr);
#elif defined(__ANDROID__)
	Extension(const EDITDATA* const edPtr, const jobject javaExtPtr);
#else
	Extension(const EDITDATA* const edPtr, void* const objCExtPtr);
#endif
	~Extension();

#pragma region JoltData
	std::tstring JoltBase = _T("http://api.gamejolt.com");
	std::tstring GameID = _T("");
	std::tstring PrivateKey = _T("");
	std::tstring UserName = _T("");
	std::tstring UserToken = _T("");
	std::tstring GuestName = _T("");
#pragma endregion

#pragma region HTTP
	std::unique_ptr<ResponseTicket> LatestResponse;
	std::tstring SerializeUrl(std::tstring url);
	std::unique_ptr<ResponseTicket> HttpGet(std::tstring url, ResponseType type);
	void SetTrigger(std::unique_ptr<ResponseTicket> ticket, int trigger);
	std::tstring URLEncode(std::tstring str);
	std::tstring URLDecode(std::tstring str);
	json GetJson();
	std::vector<char> EncodingChars;
#pragma endregion

#pragma region Action Thead Runners
	std::deque<std::unique_ptr<std::thread>> Threads;

	void Auth(const TCHAR* userName, const TCHAR* userToken);
	void AuthCreds();
	void FetchUsername(const TCHAR* name);
	void FetchUserID(int userId);

	void OpenSession();
	void PingSession();
	void PingStatusSession(const TCHAR* status);
	void CheckSession();
	void CloseSession();

	void AddUserScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData);
	void AddGuestScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData);
	void GetScoreRanking(int score, int table);
	void FetchScores(int table, int limit, int betterThan, int worseThan);
	void FetchUserScores(int table, int limit, int betterThan, int worseThan);
	void GetTables();

	void GetTrophy(int trophy);
	void GetTrophies();
	void GetUnlockedTrophies();
	void UnlockTrophy(int trophy);
	void LockTrophy(int trophy);

	void GlobalStorageGetData(const TCHAR* key);
	void GlobalStorageGetKeys(const TCHAR* pattern);
	void GlobalStorageDeleteKey(const TCHAR* key);
	void GlobalStorageSetKey(const TCHAR* key, const TCHAR* data);
	void GlobalStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation);

	void UserStorageGetData(const TCHAR* key);
	void UserStorageGetKeys(const TCHAR* pattern);
	void UserStorageDeleteKey(const TCHAR* key);
	void UserStorageSetKey(const TCHAR* key, const TCHAR* data);
	void UserStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation);

	void GlobalFileStorageSaveData(const TCHAR* key, const TCHAR* filePath);
	void GlobalFileStorageSetKey(const TCHAR* key, const TCHAR* filePath);
	void GlobalFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation);

	void UserFileStorageSaveData(const TCHAR* key, const TCHAR* filePath);
	void UserFileStorageSetKey(const TCHAR* key, const TCHAR* filePath);
	void UserFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation);

	void GetFriendsList();
	void GetCurrentTime();
#pragma endregion

#pragma region Actions
	void Act_Auth(const TCHAR* userName, const TCHAR* userToken);
	void Act_AuthCreds();
	void Act_SetGameID(const TCHAR* gameId);
	void Act_SetPrivateKey(const TCHAR* privateKey);
	void Act_SetGuestName(const TCHAR* userName);
	void Act_FetchUsername(const TCHAR* userName);
	void Act_FetchUserID(int userId);

	void Act_OpenSession();
	void Act_PingSession();
	void Act_PingStatusSession(const TCHAR* status);
	void Act_CheckSession();
	void Act_CloseSession();

	void Act_AddUserScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData);
	void Act_AddGuestScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData);
	void Act_GetScoreRanking(int score, int table);
	void Act_FetchScores(int table, int limit, int betterThan, int worseThan);
	void Act_FetchUserScores(int table, int limit, int betterThan, int worseThan);
	void Act_GetTables();

	void Act_GetTrophy(int trophy);
	void Act_GetTrophies();
	void Act_GetUnlockedTrophies();
	void Act_UnlockTrophy(int trophy);
	void Act_LockTrophy(int trophy);

	void Act_GlobalStorageGetData(const TCHAR* key);
	void Act_GlobalStorageGetKeys(const TCHAR* pattern);
	void Act_GlobalStorageDeleteKey(const TCHAR* key);
	void Act_GlobalStorageSetKey(const TCHAR* key, const TCHAR* data);
	void Act_GlobalStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation);

	void Act_UserStorageGetData(const TCHAR* key);
	void Act_UserStorageGetKeys(const TCHAR* pattern);
	void Act_UserStorageDeleteKey(const TCHAR* key);
	void Act_UserStorageSetKey(const TCHAR* key, const TCHAR* data);
	void Act_UserStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation);

	void Act_GlobalFileStorageSaveData(const TCHAR* key, const TCHAR* filePath);
	void Act_GlobalFileStorageSetKey(const TCHAR* key, const TCHAR* filePath);
	void Act_GlobalFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation);

	void Act_UserFileStorageSaveData(const TCHAR* key, const TCHAR* filePath);
	void Act_UserFileStorageSetKey(const TCHAR* key, const TCHAR* filePath);
	void Act_UserFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation);

	void Act_GetFriendsList();
	void Act_GetCurrentTime();
#pragma endregion

#pragma region Conditions
	bool Cnd_AnyCallTriggered();
	bool Cnd_CallTriggered();

// Call by adding to this list
	Edif::recursive_mutex TriggerLock;
	std::deque<std::unique_ptr<ResponseTicket>> TriggerBuffer;


#define Cnd_AnyCallFinished		31

#define Cnd_AuthFinished		0
#define Cnd_FetchFinished		1

#define Cnd_OpenFinished		2
#define Cnd_PingFinished		3
#define Cnd_CheckFinished		4
#define Cnd_CloseFinished		5

#define Cnd_ScoreAdded			6
#define Cnd_RankingRetrieved	7
#define Cnd_ScoresFetched		8
#define Cnd_TablesRetrieved		9

#define Cnd_TrophiesRetrieved	10
#define Cnd_TrophyAdded			11
#define Cnd_TrophyRemoved		12

#define Cnd_GSGetData			13
#define Cnd_GSGetKeys			14
#define Cnd_GSDeleteKey			15
#define Cnd_GSSetKey			16
#define Cnd_GSUpdateKey			17

#define Cnd_USGetData			18
#define Cnd_USGetKeys			19
#define Cnd_USDeleteKey			20
#define Cnd_USSetKey			21
#define Cnd_USUpdateKey			22

#define Cnd_FGSSaveData			23
#define Cnd_FGSSetKey			24
#define Cnd_FGSUpdateKey		25

#define Cnd_FUSSaveData			26
#define Cnd_FUSSetKey			27
#define Cnd_FUSUpdateKey		28

#define Cnd_GetFriendsList		29
#define Cnd_GetCurrentTime		30
#pragma endregion

#pragma region Expressions
	const TCHAR* Exp_GetJsonResponse();
	const TCHAR* Exp_GetResponseType();
	const TCHAR* Exp_GetResponseStatus();
	const TCHAR* Exp_GetResponseMessage();

	const TCHAR* Exp_GetGameID();
	const TCHAR* Exp_GetPrivateKey();
	const TCHAR* Exp_GetUserName();
	const TCHAR* Exp_GetUserToken();
	const TCHAR* Exp_GetGuestName();

	int	  Exp_FetchedUserCount();
	const TCHAR* Exp_FetchedUserDisplayName(int index);
	const TCHAR* Exp_FetchedUsername(int index);
	int  Exp_FetchedUserID(int index);
	const TCHAR* Exp_FetchedUserDescription(int index);
	const TCHAR* Exp_FetchedUserAvatar(int index, int resolution);
	const TCHAR* Exp_FetchedUserWebsite(int index);
	const TCHAR* Exp_FetchedUserStatus(int index);
	const TCHAR* Exp_FetchedUserType(int index);
	const TCHAR* Exp_FetchedUserLastLoggedIn(int index);
	int	 Exp_FetchedUserLastLoggedInTimestamp(int index);
	const TCHAR* Exp_FetchedUserSignedUp(int index);
	int	 Exp_FetchedUserSignedUpTimestamp(int index);

	int	 Exp_ScoreRanking();
	int	 Exp_FetchedScoreCount();
	const TCHAR* Exp_FetchedScoreUsername(int index);
	int  Exp_FetchedScoreUserID(int index);
	const TCHAR* Exp_FetchedScoreGuestName(int index);
	const TCHAR* Exp_FetchedScoreScore(int index);
	int  Exp_FetchedScoreSort(int index);
	const TCHAR* Exp_FetchedScoreExtraData(int index);
	const TCHAR* Exp_FetchedScoreSubmit(int index);
	int	 Exp_FetchedScoreSubmitTimestamp(int index);
	int	 Exp_FetchedTableCount();
	const TCHAR* Exp_FetchedTableName(int index);
	int Exp_FetchedTableID(int index);
	const TCHAR* Exp_FetchedTableDescription(int index);
	int	 Exp_FetchedTableIsPrimary(int index);

	int	 Exp_FetchedTrophyCount();
	const TCHAR* Exp_FetchedTrophyTitle(int index);
	int  Exp_FetchedTrophyID(int index);
	const TCHAR* Exp_FetchedTrophyDescription(int index);
	const TCHAR* Exp_FetchedTrophyDifficulty(int index);
	const TCHAR* Exp_FetchedTrophyImageURL(int index);
	const TCHAR* Exp_FetchedTrophyAchieved(int index);

	const TCHAR* Exp_RetrievedKeyData();
	int	 Exp_FetchedKeyCount();
	const TCHAR* Exp_FetchedKey(int index);
	const TCHAR* Exp_UpdatedKeyData();

	int	 Exp_FetchedFriendCount();
	int  Exp_FetchedFriend(int index);

	int  Exp_TimeYear();
	int  Exp_TimeMonth();
	int  Exp_TimeDay();
	int  Exp_TimeHour();
	int  Exp_TimeMinute();
	int  Exp_TimeSecond();
	int	 Exp_TimeTimestamp();
	const TCHAR* Exp_TimeTimezone();
#pragma endregion

#pragma region Fusion
	void UnlinkedAction(int ID);
	long UnlinkedCondition(int ID);
	long UnlinkedExpression(int ID);

	REFLAG Handle();
	REFLAG Display();

	short FusionRuntimePaused();
	short FusionRuntimeContinued();
#pragma endregion
};
