#pragma once
#include "httplib.h"
#include <ResponseType.hpp>

struct ResponseTicket
{
	ResponseType Type;
	httplib::Result Response;
	int Trigger = 0;
	bool HasTrigger = false;
};
