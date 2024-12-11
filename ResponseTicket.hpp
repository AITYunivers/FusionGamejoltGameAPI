#pragma once
#include <Common.hpp>
#include <nlohmann/json.hpp>

struct ResponseTicket
{
	~ResponseTicket();

	std::string URL;
	ResponseType Type;
	httplib::Result Response;
	int Trigger = 0;
	bool HasTrigger = false;

private:
	std::unique_ptr<nlohmann::json> cached_json;
public:
	nlohmann::json GetResponseJson(Extension* ext);
};
