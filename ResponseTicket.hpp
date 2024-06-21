#pragma once
#include "httplib.h"
#include <ResponseType.hpp>

struct ResponseTicket
{
	std::string URL;
	ResponseType Type;
	httplib::Result Response;
	int Trigger = 0;
	bool HasTrigger = false;
};
