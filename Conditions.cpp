#include "Common.hpp"

bool Extension::Cnd_AnyCallTriggered()
{
	return true;
}

bool Extension::Cnd_CallTriggered()
{
	Runtime.GenerateEvent(Cnd_AnyCallFinished);
	return true;
}
