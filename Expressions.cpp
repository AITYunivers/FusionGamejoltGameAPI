#include "Common.hpp"

#pragma region Response
const TCHAR * Extension::Exp_GetJsonResponse()
{
	if (LatestResponse->Type == Unknown)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(j.dump(4)).c_str());
}

const TCHAR * Extension::Exp_GetResponseType()
{
	switch (LatestResponse->Type)
	{
		default:
		case ResponseType::Unknown:
			return _T("Unknown");
		case ResponseType::Auth:
			return _T("Auth");
		case ResponseType::FetchUsers:
			return _T("FetchUsers");
		case ResponseType::OpenSession:
			return _T("OpenSession");
		case ResponseType::PingSession:
			return _T("PingSession");
		case ResponseType::CheckSession:
			return _T("CheckSession");
		case ResponseType::CloseSession:
			return _T("CloseSession");
		case ResponseType::FetchScores:
			return _T("FetchScores");
		case ResponseType::ScoreTables:
			return _T("ScoreTables");
		case ResponseType::AddScore:
			return _T("AddScore");
		case ResponseType::GetRank:
			return _T("GetRank");
		case ResponseType::FetchTrophies:
			return _T("FetchTrophies");
		case ResponseType::AchieveTrophy:
			return _T("AchieveTrophy");
		case ResponseType::RevokeTrophy:
			return _T("RevokeTrophy");
		case ResponseType::SetData:
			return _T("SetData");
		case ResponseType::UpdateData:
			return _T("UpdateData");
		case ResponseType::RemoveData:
			return _T("RemoveData");
		case ResponseType::FetchData:
			return _T("FetchData");
		case ResponseType::GetDataKeys:
			return _T("GetDataKeys");
		case ResponseType::Friends:
			return _T("Friends");
		case ResponseType::Time:
			return _T("Time");
	}
}

const TCHAR* Extension::Exp_GetResponseStatus()
{
	if (LatestResponse->Type == Unknown)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string status;
	j["response"]["success"].get_to(status);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(status).c_str());
}

const TCHAR* Extension::Exp_GetResponseMessage()
{
	if (LatestResponse->Type == Unknown)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"].contains("message"))
		return Runtime.CopyString(_T(""));
	std::string message;
	j["response"]["message"].get_to(message);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(message).c_str());
}
#pragma endregion

#pragma region Internal Data
const TCHAR * Extension::Exp_GetGameID()
{
	return Runtime.CopyString(GameID.c_str());
}

const TCHAR * Extension::Exp_GetPrivateKey()
{
	return Runtime.CopyString(PrivateKey.c_str());
}

const TCHAR * Extension::Exp_GetRequestURL()
{
	return Runtime.CopyString((JoltBase + DarkEdif::UTF8ToTString(LatestResponse->URL)).c_str());
}

const TCHAR * Extension::Exp_GetUserName()
{
	return Runtime.CopyString(GameAuthData->UserName.c_str());
}

const TCHAR * Extension::Exp_GetUserToken()
{
	return Runtime.CopyString(GameAuthData->UserToken.c_str());
}


const TCHAR * Extension::Exp_GetGuestName()
{
	return Runtime.CopyString(GameAuthData->GuestName.c_str());
}
#pragma endregion

#pragma region Fetched Users
int Extension::Exp_FetchedUserCount()
{
	if (LatestResponse->Type != FetchUsers)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array())
		return 0;
	return j["response"]["users"].size();
}

const TCHAR* Extension::Exp_FetchedUserDisplayName(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string displayName;
	j["response"]["users"][index]["developer_name"].get_to(displayName);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(displayName).c_str());
}

const TCHAR* Extension::Exp_FetchedUsername(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string username;
	j["response"]["users"][index]["username"].get_to(username);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(username).c_str());
}

int Extension::Exp_FetchedUserID(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return 0;
	std::string id;
	j["response"]["users"][index]["id"].get_to(id);
	return stoi(id);
}

const TCHAR* Extension::Exp_FetchedUserDescription(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string desc;
	j["response"]["users"][index]["developer_description"].get_to(desc);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(desc).c_str());
}

const TCHAR* Extension::Exp_FetchedUserAvatar(int index, int resolution)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string avatar;
	j["response"]["users"][index]["avatar_url"].get_to(avatar);
	std::tstring avatarOut = _T("https://m.gjcdn.net/user-avatar/") + std::to_tstring(resolution) + DarkEdif::UTF8ToTString(avatar.substr(34, avatar.length() - 38)) + _T(".png");
	return Runtime.CopyString(avatarOut.c_str());
}

const TCHAR* Extension::Exp_FetchedUserWebsite(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string website;
	j["response"]["users"][index]["developer_website"].get_to(website);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(website).c_str());
}

const TCHAR* Extension::Exp_FetchedUserStatus(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string status;
	j["response"]["users"][index]["status"].get_to(status);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(status).c_str());
}

const TCHAR* Extension::Exp_FetchedUserType(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string type;
	j["response"]["users"][index]["type"].get_to(type);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(type).c_str());
}

const TCHAR* Extension::Exp_FetchedUserLastLoggedIn(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string login;
	j["response"]["users"][index]["last_logged_in"].get_to(login);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(login).c_str());
}

int Extension::Exp_FetchedUserLastLoggedInTimestamp(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return 0;
	int loginStamp;
	j["response"]["users"][index]["last_logged_in_timestamp"].get_to(loginStamp);
	return loginStamp;
}

const TCHAR* Extension::Exp_FetchedUserSignedUp(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string signUp;
	j["response"]["users"][index]["signed_up"].get_to(signUp);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(signUp).c_str());
}

int Extension::Exp_FetchedUserSignedUpTimestamp(int index)
{
	if (LatestResponse->Type != FetchUsers)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["users"].is_array() || j["response"]["users"].size() == 0)
		return 0;
	int signUpStamp;
	j["response"]["users"][index]["signed_up_timestamp"].get_to(signUpStamp);
	return signUpStamp;
}
#pragma endregion

#pragma region Scores -> Get Ranking
int Extension::Exp_ScoreRanking()
{
	if (LatestResponse->Type != GetRank)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	int ranking;
	j["response"]["rank"].get_to(ranking);
	return ranking;
}
#pragma endregion

#pragma region Scores -> Table Scores
int Extension::Exp_FetchedScoreCount()
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array())
		return 0;
	return j["response"]["scores"].size();
}

const TCHAR* Extension::Exp_FetchedScoreUsername(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string username;
	j["response"]["scores"][index]["user"].get_to(username);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(username).c_str());
}

int Extension::Exp_FetchedScoreUserID(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return 0;
	std::string userId;
	j["response"]["scores"][index]["user_id"].get_to(userId);
	return stoi(userId);
}

const TCHAR* Extension::Exp_FetchedScoreGuestName(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string guestName;
	j["response"]["scores"][index]["guest"].get_to(guestName);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(guestName).c_str());
}

const TCHAR* Extension::Exp_FetchedScoreScore(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string score;
	j["response"]["scores"][index]["score"].get_to(score);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(score).c_str());
}

int Extension::Exp_FetchedScoreSort(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return 0;
	std::string sort;
	j["response"]["scores"][index]["sort"].get_to(sort);
	return stoi(sort);
}

const TCHAR* Extension::Exp_FetchedScoreExtraData(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string data;
	j["response"]["scores"][index]["extra_data"].get_to(data);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(data).c_str());
}

const TCHAR* Extension::Exp_FetchedScoreSubmit(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string submitTime;
	j["response"]["scores"][index]["stored"].get_to(submitTime);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(submitTime).c_str());
}

int Extension::Exp_FetchedScoreSubmitTimestamp(int index)
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["scores"].is_array() || j["response"]["scores"].size() == 0)
		return 0;
	int submitStamp;
	j["response"]["scores"][index]["stored_timestamp"].get_to(submitStamp);
	return submitStamp;
}

int Extension::Exp_FetchedScoreTableID()
{
	if (LatestResponse->Type != ResponseType::FetchScores)
		return 0;

	std::regex re("table_id=([0-9]+)");
	std::smatch match;

	if (std::regex_search(LatestResponse->URL, match, re) && match.size() > 1)
		return stoi(match.str(1));
	else return 0;
}
#pragma endregion

#pragma region Scores -> Fetched Tables
int Extension::Exp_FetchedTableCount()
{
	if (LatestResponse->Type != ScoreTables)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["tables"].is_array())
		return 0;
	return j["response"]["tables"].size();
}

const TCHAR* Extension::Exp_FetchedTableName(int index)
{
	if (LatestResponse->Type != ScoreTables)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["tables"].is_array() || j["response"]["tables"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string name;
	j["response"]["tables"][index]["name"].get_to(name);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(name).c_str());
}

int Extension::Exp_FetchedTableID(int index)
{
	if (LatestResponse->Type != ScoreTables)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["tables"].is_array() || j["response"]["tables"].size() == 0)
		return 0;
	std::string id;
	j["response"]["tables"][index]["id"].get_to(id);
	return stoi(id);
}

const TCHAR* Extension::Exp_FetchedTableDescription(int index)
{
	if (LatestResponse->Type != ScoreTables)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["tables"].is_array() || j["response"]["tables"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string desc;
	j["response"]["tables"][index]["description"].get_to(desc);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(desc).c_str());
}

int Extension::Exp_FetchedTableIsPrimary(int index)
{
	if (LatestResponse->Type != ScoreTables)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["tables"].is_array() || j["response"]["tables"].size() == 0)
		return 0;
	std::string prim;
	j["response"]["tables"][index]["primary"].get_to(prim);
	return prim == "1" ? 1 : 0;
}
#pragma endregion

#pragma region Fetched Trophies
int Extension::Exp_FetchedTrophyCount()
{
	if (LatestResponse->Type != FetchTrophies)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array())
		return 0;
	return j["response"]["trophies"].size();
}

const TCHAR* Extension::Exp_FetchedTrophyTitle(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string name;
	j["response"]["trophies"][index]["title"].get_to(name);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(name).c_str());
}

int Extension::Exp_FetchedTrophyID(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return 0;
	std::string id;
	j["response"]["trophies"][index]["id"].get_to(id);
	return stoi(id);
}

const TCHAR* Extension::Exp_FetchedTrophyDescription(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string desc;
	j["response"]["trophies"][index]["description"].get_to(desc);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(desc).c_str());
}

const TCHAR* Extension::Exp_FetchedTrophyDifficulty(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string diff;
	j["response"]["trophies"][index]["difficulty"].get_to(diff);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(diff).c_str());
}

const TCHAR* Extension::Exp_FetchedTrophyImageURL(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string img;
	j["response"]["trophies"][index]["image_url"].get_to(img);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(img).c_str());
}

const TCHAR* Extension::Exp_FetchedTrophyAchieved(int index)
{
	if (LatestResponse->Type != FetchTrophies)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["trophies"].is_array() || j["response"]["trophies"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string achieved;
	j["response"]["trophies"][index]["achieved"].get_to(achieved);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(achieved).c_str());
}
#pragma endregion

#pragma region Data Storage -> Retrieved Key Data
const TCHAR* Extension::Exp_RetrievedKeyData()
{
	if (LatestResponse->Type != FetchData)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string data;
	j["response"]["data"].get_to(data);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(data).c_str());
}
#pragma endregion

#pragma region Data Storage -> Fetched Keys
int Extension::Exp_FetchedKeyCount()
{
	if (LatestResponse->Type != GetDataKeys)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["keys"].is_array())
		return 0;
	return j["response"]["keys"].size();
}

const TCHAR* Extension::Exp_FetchedKey(int index)
{
	if (LatestResponse->Type != GetDataKeys)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["keys"].is_array() || j["response"]["keys"].size() == 0)
		return Runtime.CopyString(_T(""));
	std::string key;
	j["response"]["keys"][index]["key"].get_to(key);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(key).c_str());
}
#pragma endregion

#pragma region Data Storage -> Updated Key Data
const TCHAR* Extension::Exp_UpdatedKeyData()
{
	if (LatestResponse->Type != UpdateData)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string data;
	j["response"]["data"].get_to(data);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(data).c_str());
}
#pragma endregion

#pragma region Extra -> Fetched Friends
int Extension::Exp_FetchedFriendCount()
{
	if (LatestResponse->Type != Friends)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["friends"].is_array())
		return 0;
	return j["response"]["friends"].size();
}

int Extension::Exp_FetchedFriend(int index)
{
	if (LatestResponse->Type != Friends)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	if (!j["response"]["friends"].is_array() || j["response"]["friends"].size() == 0)
		return 0;
	std::string id;
	j["response"]["friends"][index]["friend_id"].get_to(id);
	return stoi(id);
}
#pragma endregion

#pragma region Extra -> Time
int Extension::Exp_TimeYear()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string year;
	j["response"]["year"].get_to(year);
	return stoi(year);
}

int Extension::Exp_TimeMonth()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string month;
	j["response"]["month"].get_to(month);
	return stoi(month);
}

int Extension::Exp_TimeDay()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string day;
	j["response"]["day"].get_to(day);
	return stoi(day);
}

int Extension::Exp_TimeHour()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string hour;
	j["response"]["hour"].get_to(hour);
	return stoi(hour);
}

int Extension::Exp_TimeMinute()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string minute;
	j["response"]["minute"].get_to(minute);
	return stoi(minute);
}

int Extension::Exp_TimeSecond()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string second;
	j["response"]["second"].get_to(second);
	return stoi(second);
}

int Extension::Exp_TimeTimestamp()
{
	if (LatestResponse->Type != Time)
		return 0;
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	int timeStamp;
	j["response"]["timestamp"].get_to(timeStamp);
	return timeStamp;
}

const TCHAR* Extension::Exp_TimeTimezone()
{
	if (LatestResponse->Type != Time)
		return Runtime.CopyString(_T(""));
	nlohmann::json j = LatestResponse->GetResponseJson(this);
	std::string timeZone;
	j["response"]["timezone"].get_to(timeZone);
	return Runtime.CopyString(DarkEdif::UTF8ToTString(timeZone).c_str());
}
#pragma endregion
