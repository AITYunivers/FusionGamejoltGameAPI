#include "Common.hpp"
#include "md5.h"

///
/// EXTENSION CONSTRUCTOR/DESTRUCTOR
///

#ifdef _WIN32
Extension::Extension(RunObject* const _rdPtr, const EDITDATA* const edPtr, const CreateObjectInfo* const cobPtr) :
	rdPtr(_rdPtr), rhPtr(_rdPtr->get_rHo()->get_AdRunHeader()), Runtime(this)
#elif defined(__ANDROID__)
Extension::Extension(const EDITDATA* const edPtr, const jobject javaExtPtr) :
	javaExtPtr(javaExtPtr, "Extension::javaExtPtr from Extension ctor"),
	Runtime(this, this->javaExtPtr)
#else
Extension::Extension(const EDITDATA* const edPtr, void* const objCExtPtr) :
	objCExtPtr(objCExtPtr), Runtime(this, objCExtPtr)
#endif
{
	LinkAction(0,  Act_Auth);
	LinkAction(1,  Act_AuthCreds);
	LinkAction(38, Act_SetGameID);
	LinkAction(39, Act_SetPrivateKey);
	LinkAction(2,  Act_SetGuestName);
	LinkAction(3,  Act_FetchUsername);
	LinkAction(4,  Act_FetchUserID);

	LinkAction(5,  Act_OpenSession);
	LinkAction(6,  Act_PingSession);
	LinkAction(7,  Act_PingStatusSession);
	LinkAction(8,  Act_CheckSession);
	LinkAction(9,  Act_CloseSession);

	LinkAction(10, Act_AddUserScore);
	LinkAction(11, Act_AddGuestScore);
	LinkAction(12, Act_GetScoreRanking);
	LinkAction(13, Act_FetchScores);
	LinkAction(40, Act_FetchUserScores);
	LinkAction(14, Act_GetTables);

	LinkAction(15, Act_GetTrophy);
	LinkAction(16, Act_GetTrophies);
	LinkAction(17, Act_GetUnlockedTrophies);
	LinkAction(18, Act_UnlockTrophy);
	LinkAction(19, Act_LockTrophy);

	LinkAction(20, Act_GlobalStorageGetData);
	LinkAction(21, Act_GlobalStorageGetKeys);
	LinkAction(22, Act_GlobalStorageDeleteKey);
	LinkAction(23, Act_GlobalStorageSetKey);
	LinkAction(24, Act_GlobalStorageUpdateKey);

	LinkAction(25, Act_UserStorageGetData);
	LinkAction(26, Act_UserStorageGetKeys);
	LinkAction(27, Act_UserStorageDeleteKey);
	LinkAction(28, Act_UserStorageSetKey);
	LinkAction(29, Act_UserStorageUpdateKey);

	LinkAction(30, Act_GlobalFileStorageSaveData);
	LinkAction(31, Act_GlobalFileStorageSetKey);
	LinkAction(32, Act_GlobalFileStorageUpdateKey);

	LinkAction(33, Act_UserFileStorageSaveData);
	LinkAction(34, Act_UserFileStorageSetKey);
	LinkAction(35, Act_UserFileStorageUpdateKey);

	LinkAction(36, Act_GetFriendsList);
	LinkAction(37, Act_GetCurrentTime);


	LinkCondition(31, Cnd_AnyCallTriggered	/* Cnd_AnyCallFinished   */);

	LinkCondition(0,  Cnd_CallTriggered		/* Cnd_AuthFinished      */);
	LinkCondition(1,  Cnd_CallTriggered		/* Cnd_FetchFinished     */);

	LinkCondition(2,  Cnd_CallTriggered		/* Cnd_OpenFinished      */);
	LinkCondition(3,  Cnd_CallTriggered		/* Cnd_PingFinished      */);
	LinkCondition(4,  Cnd_CallTriggered		/* Cnd_CheckFinished     */);
	LinkCondition(5,  Cnd_CallTriggered		/* Cnd_CloseFinished     */);

	LinkCondition(6,  Cnd_CallTriggered		/* Cnd_ScoreAdded        */);
	LinkCondition(7,  Cnd_CallTriggered		/* Cnd_RankingRetrieved  */);
	LinkCondition(8,  Cnd_CallTriggered		/* Cnd_ScoresFetched     */);
	LinkCondition(9,  Cnd_CallTriggered		/* Cnd_TablesRetrieved   */);

	LinkCondition(10, Cnd_CallTriggered		/* Cnd_TrophiesRetrieved */);
	LinkCondition(11, Cnd_CallTriggered		/* Cnd_TrophyAdded       */);
	LinkCondition(12, Cnd_CallTriggered		/* Cnd_TrophyRemoved     */);

	LinkCondition(13, Cnd_CallTriggered		/* Cnd_GSGetData         */);
	LinkCondition(14, Cnd_CallTriggered		/* Cnd_GSGetKeys         */);
	LinkCondition(15, Cnd_CallTriggered		/* Cnd_GSDeleteKey       */);
	LinkCondition(16, Cnd_CallTriggered		/* Cnd_GSSetKey          */);
	LinkCondition(17, Cnd_CallTriggered		/* Cnd_GSUpdateKey       */);

	LinkCondition(18, Cnd_CallTriggered		/* Cnd_USGetData         */);
	LinkCondition(19, Cnd_CallTriggered		/* Cnd_USGetKeys         */);
	LinkCondition(20, Cnd_CallTriggered		/* Cnd_USDeleteKey       */);
	LinkCondition(21, Cnd_CallTriggered		/* Cnd_USSetKey          */);
	LinkCondition(22, Cnd_CallTriggered		/* Cnd_USUpdateKey       */);

	LinkCondition(23, Cnd_CallTriggered		/* Cnd_FGSSaveData       */);
	LinkCondition(24, Cnd_CallTriggered		/* Cnd_FGSSetKey         */);
	LinkCondition(25, Cnd_CallTriggered		/* Cnd_FGSUpdateKey      */);

	LinkCondition(26, Cnd_CallTriggered		/* Cnd_FUSSaveData       */);
	LinkCondition(27, Cnd_CallTriggered		/* Cnd_FUSSetKey         */);
	LinkCondition(28, Cnd_CallTriggered		/* Cnd_FUSUpdateKey      */);

	LinkCondition(29, Cnd_CallTriggered		/* Cnd_GetFriendsList    */);
	LinkCondition(30, Cnd_CallTriggered		/* Cnd_GetCurrentTime    */);


	LinkExpression(0, Exp_GetJsonResponse);
	LinkExpression(1, Exp_GetResponseType);
	LinkExpression(2, Exp_GetResponseStatus);
	LinkExpression(3, Exp_GetResponseMessage);

	LinkExpression(56, Exp_GetGameID);
	LinkExpression(57, Exp_GetPrivateKey);
	LinkExpression(4, Exp_GetUserName);
	LinkExpression(5, Exp_GetUserToken);
	LinkExpression(6, Exp_GetGuestName);

	LinkExpression(7, Exp_FetchedUserCount);
	LinkExpression(8, Exp_FetchedUserDisplayName);
	LinkExpression(9, Exp_FetchedUsername);
	LinkExpression(10, Exp_FetchedUserID);
	LinkExpression(11, Exp_FetchedUserDescription);
	LinkExpression(12, Exp_FetchedUserAvatar);
	LinkExpression(13, Exp_FetchedUserWebsite);
	LinkExpression(14, Exp_FetchedUserStatus);
	LinkExpression(15, Exp_FetchedUserType);
	LinkExpression(16, Exp_FetchedUserLastLoggedIn);
	LinkExpression(17, Exp_FetchedUserLastLoggedInTimestamp);
	LinkExpression(18, Exp_FetchedUserSignedUp);
	LinkExpression(19, Exp_FetchedUserSignedUpTimestamp);

	LinkExpression(20, Exp_ScoreRanking);
	LinkExpression(21, Exp_FetchedScoreCount);
	LinkExpression(22, Exp_FetchedScoreUsername);
	LinkExpression(23, Exp_FetchedScoreUserID);
	LinkExpression(24, Exp_FetchedScoreGuestName);
	LinkExpression(25, Exp_FetchedScoreScore);
	LinkExpression(26, Exp_FetchedScoreSort);
	LinkExpression(27, Exp_FetchedScoreExtraData);
	LinkExpression(28, Exp_FetchedScoreSubmit);
	LinkExpression(29, Exp_FetchedScoreSubmitTimestamp);
	LinkExpression(30, Exp_FetchedTableCount);
	LinkExpression(31, Exp_FetchedTableName);
	LinkExpression(32, Exp_FetchedTableID);
	LinkExpression(33, Exp_FetchedTableDescription);
	LinkExpression(34, Exp_FetchedTableIsPrimary);

	LinkExpression(35, Exp_FetchedTrophyCount);
	LinkExpression(36, Exp_FetchedTrophyTitle);
	LinkExpression(37, Exp_FetchedTrophyID);
	LinkExpression(38, Exp_FetchedTrophyDescription);
	LinkExpression(39, Exp_FetchedTrophyDifficulty);
	LinkExpression(40, Exp_FetchedTrophyImageURL);
	LinkExpression(41, Exp_FetchedTrophyAchieved);

	LinkExpression(42, Exp_RetrievedKeyData);
	LinkExpression(43, Exp_FetchedKeyCount);
	LinkExpression(44, Exp_FetchedKey);
	LinkExpression(45, Exp_UpdatedKeyData);

	LinkExpression(46, Exp_FetchedFriendCount);
	LinkExpression(47, Exp_FetchedFriend);

	LinkExpression(48, Exp_TimeYear);
	LinkExpression(49, Exp_TimeMonth);
	LinkExpression(50, Exp_TimeDay);
	LinkExpression(51, Exp_TimeHour);
	LinkExpression(52, Exp_TimeMinute);
	LinkExpression(53, Exp_TimeSecond);
	LinkExpression(54, Exp_TimeTimestamp);
	LinkExpression(55, Exp_TimeTimezone);

	GameID = edPtr->Props.GetPropertyStr(0);
	PrivateKey = edPtr->Props.GetPropertyStr(1);

	EncodingChars = { ':', '/', '?', '#', '[', ']', '@', '!', '$', '&', '\'', '(', ')', '*', '+', ',', ';', '=', '%' };
}

Extension::~Extension()
{
	for (auto& t : Threads)
		t->join();
}

std::tstring Extension::SerializeUrl(std::tstring url)
{
#ifdef _UNICODE
	return url + _T("&signature=") + DarkEdif::UTF8ToTString(md5(DarkEdif::TStringToANSI(JoltBase + url + PrivateKey)));
#else
	return url + "&signature=" + md5(JoltBase + url + PrivateKey);
#endif
}

std::unique_ptr<ResponseTicket> Extension::HttpGet(std::tstring url, ResponseType type)
{
	std::unique_ptr<ResponseTicket> ticket = std::make_unique<ResponseTicket>();
	ticket->Type = type;
	httplib::Client cli(DarkEdif::TStringToANSI(JoltBase));
	ticket->Response = cli.Get(DarkEdif::TStringToANSI(SerializeUrl(url)));
	return ticket;
}

void Extension::SetTrigger(std::unique_ptr<ResponseTicket> ticket, int trigger)
{
	TriggerLock.edif_lock();
	ticket->HasTrigger = true;
	ticket->Trigger = trigger;
	TriggerBuffer.push_back(std::move(ticket));
	TriggerLock.edif_unlock();
}

std::tstring Extension::URLEncode(std::tstring str) {
#ifndef _WIN32
#define _istalnum isalnum
#define _stprintf sprintf
#endif
	std::tstring new_str = _T("");
	TCHAR c;
	int ic;
	const TCHAR* chars = str.c_str();
	TCHAR bufHex[10];
	int len = _tcslen(chars);

	for (int i = 0; i < len; i++) {
		c = chars[i];
		ic = c;
		if (c == _T(' '))
			new_str += _T('+');
		else if (std::find(EncodingChars.begin(), EncodingChars.end(), c) != EncodingChars.end() ||
				 ic <= 32 || ic >= 127)
		{
			_stprintf(bufHex, _T("%X"), c);
			if (ic < 16)
				new_str += _T("%0");
			else
				new_str += _T("%");
			new_str += bufHex;
		}
		else
			new_str += c;
	}
	return new_str;
}

std::tstring Extension::URLDecode(std::tstring str) {
#ifndef _WIN32
#define _stscanf sscanf
#endif
	std::tstring ret = _T("");
	TCHAR ch;
	int i, ii, len = str.length();

	for (i = 0; i < len; i++) {
		if (str[i] != _T('%'))
		{
			if (str[i] == _T('+'))
				ret += _T(' ');
			else
				ret += str[i];
		}
		else
		{
			_stscanf(str.substr(i + 1, 2).c_str(), _T("%x"), &ii);
			ch = static_cast<TCHAR>(ii);
			ret += ch;
			i = i + 2;
		}
	}
	return ret;
}

json Extension::GetJson()
{
#ifdef _UNICODE
	const char* src = LatestResponse->Response->body.c_str();
	size_t size_needed = mbstowcs(NULL, src, 0) + 1;
	TCHAR* tstrTo = new TCHAR[size_needed];
	mbstowcs(tstrTo, src, size_needed);
	std::wstring wstr(tstrTo);
	std::u16string jDataU;
	for (wchar_t wc : wstr)
	{
		if (wc < 0x80) {
			jDataU += static_cast<char>(wc);
		}
		else
		{
			std::stringstream ss;
			ss << "\\u" << std::setfill('0') << std::setw(4) << std::hex << static_cast<int>(wc);
			std::string hex_str = ss.str();
			for (char c : hex_str) {
				jDataU += static_cast<char16_t>(c);
			}
		}
	}
	json j = json::parse(jDataU);
#else
	json j = json::parse(LatestResponse->Response->body.c_str());
#endif
	return j;
}

REFLAG Extension::Handle()
{
	if (TriggerLock.edif_try_lock())
	{
		while (!TriggerBuffer.empty())
		{
			std::swap(LatestResponse, TriggerBuffer[0]);
			TriggerBuffer.pop_front();
			if (LatestResponse->HasTrigger)
				Runtime.GenerateEvent(LatestResponse->Trigger);
		}
		TriggerLock.edif_unlock();
	}
	return REFLAG::NONE;
}


REFLAG Extension::Display()
{
	/*
		If you return REFLAG_DISPLAY in Handle() this routine will run.
	*/

	// Ok
	return REFLAG::DISPLAY;
}

short Extension::FusionRuntimePaused()
{

	// Ok
	return 0;
}

short Extension::FusionRuntimeContinued()
{

	// Ok
	return 0;
}


// These are called if there's no function linked to an ID

void Extension::UnlinkedAction(int ID)
{
	DarkEdif::MsgBox::Error(_T("Extension::UnlinkedAction() called"), _T("Running a fallback for action ID %d. Make sure you ran LinkAction()."), ID);
}

long Extension::UnlinkedCondition(int ID)
{
	DarkEdif::MsgBox::Error(_T("Extension::UnlinkedCondition() called"), _T("Running a fallback for condition ID %d. Make sure you ran LinkCondition()."), ID);
	return 0;
}

long Extension::UnlinkedExpression(int ID)
{
	DarkEdif::MsgBox::Error(_T("Extension::UnlinkedExpression() called"), _T("Running a fallback for expression ID %d. Make sure you ran LinkExpression()."), ID);
	// Unlinked A/C/E is fatal error, but try not to return null string and definitely crash it
	if ((size_t)ID < Edif::SDK->ExpressionInfos.size() && Edif::SDK->ExpressionInfos[ID]->Flags.ef == ExpReturnType::String)
		return (long)Runtime.CopyString(_T(""));
	return 0;
}
