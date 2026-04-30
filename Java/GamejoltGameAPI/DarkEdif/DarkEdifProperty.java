package GamejoltGameAPI.DarkEdif;

import java.nio.ByteBuffer;

/**
 *
 * @author Yunivers and Phi
 */
public class DarkEdifProperty {
    public long index;
    public long propTypeID;
    public long propJSONIndex;
    public String propName;
    public ByteBuffer propData;

    public DarkEdifProperty(long index, long propTypeID, long propJSONIndex, String propName, ByteBuffer propData)
    {
        this.index = index;
        this.propTypeID = propTypeID;
        this.propJSONIndex = propJSONIndex;
        this.propName = propName;
        this.propData = propData;
    }
}
