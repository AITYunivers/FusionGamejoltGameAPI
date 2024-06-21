#include "Common.hpp"

void Extension::Act_Auth(const TCHAR* userName, const TCHAR* userToken)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->Auth(userName, userToken); });
	Threads.push_back(std::move(t));
}

void Extension::Auth(const TCHAR* userName, const TCHAR* userToken)
{
	GameAuthData->UserName = URLEncode(userName);
	GameAuthData->UserToken = URLEncode(userToken);
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/users/auth/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::Auth),
		Cnd_AuthFinished);
}

void Extension::Act_AuthCreds()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->AuthCreds(); });
	Threads.push_back(std::move(t));
}

void Extension::AuthCreds()
{
	std::tfstream file(".gj-credentials");
	if (file.is_open())
	{
		std::tstring version;
		std::getline(file, version);
		std::getline(file, GameAuthData->UserName);
		std::getline(file, GameAuthData->UserToken);

		GameAuthData->UserName = URLEncode(GameAuthData->UserName);
		GameAuthData->UserToken = URLEncode(GameAuthData->UserToken);
	}
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/users/auth/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::Auth),
		Cnd_AuthFinished);
}

void Extension::Act_SetGameID(const TCHAR* gameId)
{
	GameID = URLEncode(gameId);
}

void Extension::Act_SetPrivateKey(const TCHAR* privateKey)
{
	PrivateKey = URLEncode(privateKey);
}

void Extension::Act_SetGuestName(const TCHAR* name)
{
	GameAuthData->GuestName = URLEncode(name);
}

void Extension::Act_FetchUsername(const TCHAR* userName)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->FetchUsername(userName); });
	Threads.push_back(std::move(t));
}

void Extension::FetchUsername(const TCHAR* userName)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/users/?game_id=") +
			GameID +
			_T("&username=") +
			URLEncode(userName),
			ResponseType::FetchUsers),
		Cnd_FetchFinished);
}

void Extension::Act_FetchUserID(int userId)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->FetchUserID(userId); });
	Threads.push_back(std::move(t));
}

void Extension::FetchUserID(int userId)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/users/?game_id=") +
			GameID +
			_T("&user_id=") +
			std::to_tstring(userId),
			ResponseType::FetchUsers),
		Cnd_FetchFinished);
}

void Extension::Act_OpenSession()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->OpenSession(); });
	Threads.push_back(std::move(t));
}

void Extension::OpenSession()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/sessions/open/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::OpenSession),
		Cnd_OpenFinished);
}

void Extension::Act_PingSession()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->PingSession(); });
	Threads.push_back(std::move(t));
}

void Extension::PingSession()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/sessions/ping/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::PingSession),
		Cnd_PingFinished);
}

void Extension::Act_PingStatusSession(const TCHAR* status)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->PingStatusSession(status); });
	Threads.push_back(std::move(t));
}

void Extension::PingStatusSession(const TCHAR* status)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/sessions/ping/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&status=") +
			URLEncode(status),
			ResponseType::PingSession),
		Cnd_PingFinished);
}

void Extension::Act_CheckSession()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->CheckSession(); });
	Threads.push_back(std::move(t));
}

void Extension::CheckSession()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/sessions/check/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::CheckSession),
		Cnd_CheckFinished);
}

void Extension::Act_CloseSession()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->CloseSession(); });
	Threads.push_back(std::move(t));
}

void Extension::CloseSession()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/sessions/close/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::CloseSession),
		Cnd_CloseFinished);
}

void Extension::Act_AddUserScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->AddUserScore(displayScore, sortScore, table, extraData); });
	Threads.push_back(std::move(t));
}

void Extension::AddUserScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData)
{
	std::tstring url = _T("/api/game/v1_2/scores/add/?game_id=") + GameID + _T("&username=") + GameAuthData->UserName + _T("&user_token=") + GameAuthData->UserToken + _T("&score=") + URLEncode(displayScore) + _T("&sort=") + std::to_tstring(sortScore);
	if (table != -1)
		url += _T("&table_id=") + std::to_tstring(table);
	std::tstring extraDataStr = URLEncode(extraData);
	if (extraDataStr.length() > 0)
		url += _T("&extra_data=") + extraDataStr;
	SetTrigger(
		HttpGet(
			url,
			ResponseType::AddScore),
		Cnd_ScoreAdded);
}

void Extension::Act_AddGuestScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->AddGuestScore(displayScore, sortScore, table, extraData); });
	Threads.push_back(std::move(t));
}

void Extension::AddGuestScore(const TCHAR* displayScore, int sortScore, int table, const TCHAR* extraData)
{
	std::tstring url = _T("/api/game/v1_2/scores/add/?game_id=") + GameID + _T("&guest=") + GameAuthData->GuestName + _T("&score=") + URLEncode(displayScore) + _T("&sort=") + std::to_tstring(sortScore);
	if (table != -1)
		url += _T("&table_id=") + std::to_tstring(table);
	std::tstring extraDataStr = URLEncode(extraData);
	if (extraDataStr.length() > 0)
		url += _T("&extra_data=") + extraDataStr;
	SetTrigger(
		HttpGet(
			url,
			ResponseType::AddScore),
		Cnd_ScoreAdded);
}

void Extension::Act_GetScoreRanking(int score, int table)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetScoreRanking(score, table); });
	Threads.push_back(std::move(t));
}

void Extension::GetScoreRanking(int score, int table)
{
	std::tstring url = _T("/api/game/v1_2/scores/get-rank/?game_id=") + GameID + _T("&sort=") + std::to_tstring(score);
	if (table != -1)
		url += _T("&table_id=") + std::to_tstring(table);
	SetTrigger(
		HttpGet(
			url,
			ResponseType::GetRank),
		Cnd_RankingRetrieved);
}

void Extension::Act_FetchScores(int table, int limit, int betterThan, int worseThan)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->FetchScores(table, limit, betterThan, worseThan); });
	Threads.push_back(std::move(t));
}

void Extension::FetchScores(int table, int limit, int betterThan, int worseThan)
{
	std::tstring url = _T("/api/game/v1_2/scores/?game_id=") + GameID;
	if (table != -1)
		url += _T("&table_id=") + std::to_tstring(table);
	if (limit != -1)
		url += _T("&limit=") + std::to_tstring(limit);
	if (betterThan != -1)
		url += _T("&better_than=") + std::to_tstring(betterThan);
	if (worseThan != -1)
		url += _T("&worse_than=") + std::to_tstring(worseThan);
	SetTrigger(
		HttpGet(
			url,
			ResponseType::FetchScores),
		Cnd_ScoresFetched);
}

void Extension::Act_FetchUserScores(int table, int limit, int betterThan, int worseThan)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->FetchUserScores(table, limit, betterThan, worseThan); });
	Threads.push_back(std::move(t));
}

void Extension::FetchUserScores(int table, int limit, int betterThan, int worseThan)
{
	std::tstring url = _T("/api/game/v1_2/scores/?game_id=") + GameID;
	if (GameAuthData->UserName.length() > 0 && GameAuthData->UserToken.length() > 0)
		url += _T("&username=") + GameAuthData->UserName + _T("&user_token=") + GameAuthData->UserToken;
	else if (GameAuthData->GuestName.length() > 0)
		url += _T("&guest=") + GameAuthData->GuestName;
	if (table != -1)
		url += _T("&table_id=") + std::to_tstring(table);
	if (limit != -1)
		url += _T("&limit=") + std::to_tstring(limit);
	if (betterThan != -1)
		url += _T("&better_than=") + std::to_tstring(betterThan);
	if (worseThan != -1)
		url += _T("&worse_than=") + std::to_tstring(worseThan);
	SetTrigger(
		HttpGet(
			url,
			ResponseType::FetchScores),
		Cnd_ScoresFetched);
}

void Extension::Act_GetTables()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetTables(); });
	Threads.push_back(std::move(t));
}

void Extension::GetTables()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/scores/tables/?game_id=") +
			GameID,
			ResponseType::ScoreTables),
		Cnd_TablesRetrieved);
}

void Extension::Act_GetTrophy(int trophy)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetTrophy(trophy); });
	Threads.push_back(std::move(t));
}

void Extension::GetTrophy(int trophy)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/trophies/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&trophy_id=") +
			std::to_tstring(trophy),
			ResponseType::FetchTrophies),
		Cnd_TrophiesRetrieved);
}

void Extension::Act_GetTrophies()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetTrophies(); });
	Threads.push_back(std::move(t));
}

void Extension::GetTrophies()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/trophies/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::FetchTrophies),
		Cnd_TrophiesRetrieved);
}

void Extension::Act_GetUnlockedTrophies()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetUnlockedTrophies(); });
	Threads.push_back(std::move(t));
}

void Extension::GetUnlockedTrophies()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/trophies/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&achieved=true"),
			ResponseType::FetchTrophies),
		Cnd_TrophiesRetrieved);
}

void Extension::Act_UnlockTrophy(int trophy)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UnlockTrophy(trophy); });
	Threads.push_back(std::move(t));
}

void Extension::UnlockTrophy(int trophy)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/trophies/add-achieved/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&trophy_id=") +
			std::to_tstring(trophy),
			ResponseType::AchieveTrophy),
		Cnd_TrophyAdded);
}

void Extension::Act_LockTrophy(int trophy)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->LockTrophy(trophy); });
	Threads.push_back(std::move(t));
}

void Extension::LockTrophy(int trophy)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/trophies/remove-achieved/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&trophy_id=") +
			std::to_tstring(trophy),
			ResponseType::RevokeTrophy),
		Cnd_TrophyRemoved);
}

void Extension::Act_GlobalStorageGetData(const TCHAR* key)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalStorageGetData(key); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalStorageGetData(const TCHAR* key)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/?game_id=") +
			GameID +
			_T("&key=") +
			URLEncode(key),
			ResponseType::FetchData),
		Cnd_GSGetData);
}

void Extension::Act_GlobalStorageGetKeys(const TCHAR* pattern)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalStorageGetKeys(pattern); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalStorageGetKeys(const TCHAR* pattern)
{
	std::tstring url = _T("/api/game/v1_2/data-store/get-keys/?game_id=") + GameID;
	std::tstring patternStr = URLEncode(pattern);
	if (patternStr.length() > 0)
		url += _T("&pattern=") + patternStr;
	SetTrigger(
		HttpGet(
			url,
			ResponseType::GetDataKeys),
		Cnd_GSGetKeys);
}

void Extension::Act_GlobalStorageDeleteKey(const TCHAR* key)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalStorageDeleteKey(key); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalStorageDeleteKey(const TCHAR* key)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/remove/?game_id=") +
			GameID +
			_T("&key=") +
			URLEncode(key),
			ResponseType::RemoveData),
		Cnd_GSDeleteKey);
}

void Extension::Act_GlobalStorageSetKey(const TCHAR* key, const TCHAR* data)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalStorageSetKey(key, data); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalStorageSetKey(const TCHAR* key, const TCHAR* data)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/set/?game_id=") +
			GameID +
			_T("&key=") +
			URLEncode(key) +
			_T("&data=") +
			URLEncode(data),
			ResponseType::SetData),
		Cnd_GSSetKey);
}

void Extension::Act_GlobalStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalStorageUpdateKey(key, data, operation); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/update/?game_id=") +
			GameID +
			_T("&key=") +
			URLEncode(key) +
			_T("&value=") +
			URLEncode(data) +
			_T("&operation=") +
			URLEncode(operation),
			ResponseType::UpdateData),
		Cnd_GSUpdateKey);
}

void Extension::Act_UserStorageGetData(const TCHAR* key)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserStorageGetData(key); });
	Threads.push_back(std::move(t));
}

void Extension::UserStorageGetData(const TCHAR* key)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&key=") +
			URLEncode(key),
			ResponseType::FetchData),
		Cnd_USGetData);
}

void Extension::Act_UserStorageGetKeys(const TCHAR* pattern)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserStorageGetKeys(pattern); });
	Threads.push_back(std::move(t));
}

void Extension::UserStorageGetKeys(const TCHAR* pattern)
{
	std::tstring url = _T("/api/game/v1_2/data-store/get-keys/?game_id=") + GameID + _T("&username=") + GameAuthData->UserName + _T("&user_token=") + GameAuthData->UserToken;
	std::tstring patternStr = URLEncode(pattern);
	if (patternStr.length() > 0)
		url += _T("&pattern=") + patternStr;
	SetTrigger(
		HttpGet(
			url,
			ResponseType::GetDataKeys),
		Cnd_USGetKeys);
}

void Extension::Act_UserStorageDeleteKey(const TCHAR* key)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserStorageDeleteKey(key); });
	Threads.push_back(std::move(t));
}

void Extension::UserStorageDeleteKey(const TCHAR* key)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/remove/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&key=") +
			URLEncode(key),
			ResponseType::RemoveData),
		Cnd_USDeleteKey);
}

void Extension::Act_UserStorageSetKey(const TCHAR* key, const TCHAR* data)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserStorageSetKey(key, data); });
	Threads.push_back(std::move(t));
}

void Extension::UserStorageSetKey(const TCHAR* key, const TCHAR* data)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/set/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&key=") +
			URLEncode(key) +
			_T("&data=") +
			URLEncode(data),
			ResponseType::SetData),
		Cnd_USSetKey);
}

void Extension::Act_UserStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserStorageUpdateKey(key, data, operation); });
	Threads.push_back(std::move(t));
}

void Extension::UserStorageUpdateKey(const TCHAR* key, const TCHAR* data, const TCHAR* operation)
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/data-store/update/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken +
			_T("&key=") +
			URLEncode(key) +
			_T("&value=") +
			URLEncode(data) +
			_T("&operation=") +
			URLEncode(operation),
			ResponseType::UpdateData),
		Cnd_USUpdateKey);
}

void Extension::Act_GlobalFileStorageSaveData(const TCHAR* key, const TCHAR* filePath)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalFileStorageSaveData(key, filePath); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalFileStorageSaveData(const TCHAR* key, const TCHAR* filePath)
{
#ifndef __ANDROID__
	OutputDebugString(_T("I dont wanna code this :D"));
#endif
	//LatestResponseType = ResponseType::FetchData;
	//Cnd_FGSSaveData);
}

void Extension::Act_GlobalFileStorageSetKey(const TCHAR* key, const TCHAR* filePath)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalFileStorageSetKey(key, filePath); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalFileStorageSetKey(const TCHAR* key, const TCHAR* filePath)
{
	std::tstring filePathStr = filePath;
	std::ifstream file(filePathStr, std::ifstream::ate | std::ifstream::binary);
	if (!file.is_open()) {
		std::cerr << "Unable to open file: " << filePathStr.c_str() << std::endl;
#ifndef __ANDROID__
		//OutputDebugString(("Unable to open file: " + filePathStr).c_str());
#endif
		return;
	}
	file.tellg();
	std::tstring content((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
	file.close();
	content = URLEncode(content);
	if (content.length() > 500000)
	{
#ifndef __ANDROID__
		//OutputDebugString(("File size too large! Max: 500kb, Input: " + std::to_string(content.length())).c_str());
#endif
		return;
	}
	std::tstring url = _T("/api/game/v1_2/data-store/set/?game_id=") + GameID + _T("&key=") = URLEncode(key) + _T("&data=") + content;
	//HttpGet(url);
	//LatestResponseType = ResponseType::SetData;
	//Cnd_FGSSetKey);
}

void Extension::Act_GlobalFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GlobalFileStorageUpdateKey(key, filePath, operation); });
	Threads.push_back(std::move(t));
}

void Extension::GlobalFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation)
{
	std::ifstream file(filePath, std::ifstream::ate | std::ifstream::binary);
	if (!file.is_open()) {
		std::cerr << "Unable to open file: " << filePath << std::endl;
		return;
	}
	file.tellg();
	std::tstring content((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
	file.close();
	content = URLEncode(content);
	if (content.length() > 500000)
	{
#ifndef __ANDROID__
		//OutputDebugString(("File size too large! Max: 500kb, Input: " + std::to_string(content.length())).c_str());
#endif
		return;
	}
	std::tstring url = _T("/api/game/v1_2/data-store/update/?game_id=") + GameID + _T("&key=") = URLEncode(key) + _T("&operation=") + URLEncode(operation) + _T("&data=") + content;
	//HttpGet(url);
	//LatestResponseType = ResponseType::UpdateData;
	//Cnd_FGSUpdateKey);
}

void Extension::Act_UserFileStorageSaveData(const TCHAR* key, const TCHAR* filePath)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserFileStorageSaveData(key, filePath); });
	Threads.push_back(std::move(t));
}

void Extension::UserFileStorageSaveData(const TCHAR* key, const TCHAR* filePath)
{
#ifndef __ANDROID__
	OutputDebugString(_T("I dont wanna code this :D"));
#endif
	//LatestResponseType = ResponseType::FetchData;
	//Cnd_FUSSaveData);
}

void Extension::Act_UserFileStorageSetKey(const TCHAR* key, const TCHAR* filePath)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserFileStorageSetKey(key, filePath); });
	Threads.push_back(std::move(t));
}

void Extension::UserFileStorageSetKey(const TCHAR* key, const TCHAR* filePath)
{
	std::ifstream file(filePath, std::ifstream::ate | std::ifstream::binary);
	if (!file.is_open()) {
		std::cerr << "Unable to open file: " << filePath << std::endl;
		return;
	}
	file.tellg();
	std::tstring content((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
	file.close();
	content = URLEncode(content);
	if (content.length() > 500000)
	{
#ifndef __ANDROID__
		//OutputDebugString((_T("File size too large! Max: 500kb, Input: ") + std::to_string(content.length())).c_str());
#endif
		return;
	}
	std::tstring url = _T("/api/game/v1_2/data-store/set/?game_id=") + GameID + _T("&username=") + GameAuthData->UserName + _T("&user_token=") + GameAuthData->UserToken + _T("&key=") = URLEncode(key) + _T("&data=") + content;
	//HttpGet(url);
	//LatestResponseType = ResponseType::SetData;
	//Cnd_FUSSetKey);
}

void Extension::Act_UserFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation)
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->UserFileStorageUpdateKey(key, filePath, operation); });
	Threads.push_back(std::move(t));
}

void Extension::UserFileStorageUpdateKey(const TCHAR* key, const TCHAR* filePath, const TCHAR* operation)
{
	std::ifstream file(filePath, std::ifstream::ate | std::ifstream::binary);
	if (!file.is_open()) {
		std::cerr << "Unable to open file: " << filePath << std::endl;
		return;
	}
	file.tellg();
	std::tstring content((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
	file.close();
	content = URLEncode(content);
	if (content.length() > 500000)
	{
#ifndef __ANDROID__
		//OutputDebugString(("File size too large! Max: 500kb, Input: " + std::to_string(content.length())).c_str());
#endif
		return;
	}
	std::tstring url = _T("/api/game/v1_2/data-store/update/?game_id=") + GameID + _T("&username=") + GameAuthData->UserName + _T("&user_token=") + GameAuthData->UserToken + _T("&key=") = URLEncode(key) + _T("&operation=") + URLEncode(operation) + _T("&data=") + content;
	//HttpGet(url);
	//LatestResponseType = ResponseType::UpdateData;
	//Cnd_FUSUpdateKey);
}

void Extension::Act_GetFriendsList()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetFriendsList(); });
	Threads.push_back(std::move(t));
}


void Extension::GetFriendsList()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/friends/?game_id=") +
			GameID +
			_T("&username=") +
			GameAuthData->UserName +
			_T("&user_token=") +
			GameAuthData->UserToken,
			ResponseType::Friends),
		Cnd_GetFriendsList);
}

void Extension::Act_GetCurrentTime()
{
	std::unique_ptr<std::thread> t = std::make_unique<std::thread>([=]() { this->GetCurrentTime(); });
	Threads.push_back(std::move(t));
}


void Extension::GetCurrentTime()
{
	SetTrigger(
		HttpGet(
			_T("/api/game/v1_2/time/?game_id=") +
			GameID,
			ResponseType::Time),
		Cnd_GetCurrentTime);
}
