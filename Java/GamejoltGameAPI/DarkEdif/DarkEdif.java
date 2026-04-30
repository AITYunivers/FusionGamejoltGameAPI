package GamejoltGameAPI.DarkEdif;

import Extensions.CRunExtension;
import Services.CBinaryFile;
import java.util.*;

/**
 *
 * @author Yunivers and Phi
 */
public class DarkEdif
{
    private static Map data = new HashMap();
    private static int sdkVersion = 20;

    private DarkEdif()
    {
        throw new RuntimeException("DarkEdif is a static class, you cannot initialize it!");
    }

    public static Object getGlobalData(String key)
    {
        key = key.toLowerCase();
        if (data.containsKey(key))
        {
            return data.get(key);
        }
        return null;
    }

    public static void setGlobalData(String key, Object value)
    {
        key = key.toLowerCase();
        data.put(key, value);
    }

    // Could not implement getCurrentFusionEventNumber: evgLine is not in Flash (haha i am not gonna check java :D)

    public static void checkSupportsSDKVersion(int sdkVer)
    {
        if (sdkVer < 16 || sdkVer > 20)
        {
            throw new RuntimeException("Flash DarkEdif SDK does not support SDK version " + sdkVersion);
        }
    }

    public static DarkEdifProperties getProperties(CRunExtension ext, CBinaryFile edPtrFile, int extVersion)
    {
        return new DarkEdifProperties(ext, edPtrFile, extVersion);
    }
}
