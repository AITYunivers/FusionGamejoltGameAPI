#pragma once

enum ResponseType
{
	Unknown,
	Auth,
	FetchUsers,
	OpenSession,
	PingSession,
	CheckSession,
	CloseSession,
	FetchScores,
	ScoreTables,
	AddScore,
	GetRank,
	FetchTrophies,
	AchieveTrophy,
	RevokeTrophy,
	SetData,
	UpdateData,
	RemoveData,
	FetchData,
	GetDataKeys,
	Friends,
	Time
};
