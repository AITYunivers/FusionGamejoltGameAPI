package Extensions;

import GamejoltGameAPI.MinimalJson.JsonObject;

/**
 *
 * @author Yunivers
 */
public class ResponseTicket {
    public String URL;
    public String Type;
    public JsonObject Data = null;
    public int Trigger = 0;
    public Boolean HasTrigger = false;

    public Boolean HasError = false;
    public String Error = "";

    public ResponseTicket(String url, String type)
    {
        URL = url;
        Type = type;
    }
}
