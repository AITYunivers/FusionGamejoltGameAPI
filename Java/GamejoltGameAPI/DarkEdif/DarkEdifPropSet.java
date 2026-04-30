package GamejoltGameAPI.DarkEdif;

import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Yunivers and Phi
 */
public class DarkEdifPropSet {
    public String setIndicator;
    public long numRepeats;
    public long lastSetJSONPropIndex;
    public long firstSetJSONPropIndex;
    public long setNameJSONPropIndex;
    public String setName;

    private ByteBuffer rsDV;

    public DarkEdifPropSet(ByteBuffer rsDV)
    {
        // Always 'S', compared with 'L' for non-set list.
        setIndicator = String.valueOf((char)(rsDV.get() & 0xFF));
        // Number of repeats of this set, 1 is minimum and means one of this set
        numRepeats = rsDV.getShort() & 0xFFFF;
        // Property that ends this set's data, as a JSON index, inclusive
        lastSetJSONPropIndex = rsDV.getShort() & 0xFFFF;
        // First property that begins this set's data, as a JSON index
        firstSetJSONPropIndex = rsDV.getShort() & 0xFFFF;
        // Name property JSON index that will appear in list when switching set entry
        setNameJSONPropIndex = rsDV.getShort() & 0xFFFF;
        // Set name, as specified in JSON. Don't confuse with user-specified set name.
        long bytesAvailable = rsDV.limit() - rsDV.position();
        byte[] bytes = new byte[(int)bytesAvailable];
        rsDV.get(bytes);
        try {
            setName = new String(bytes, "UTF-8");
        } catch (UnsupportedEncodingException ex) {
            Logger.getLogger(DarkEdifPropSet.class.getName()).log(Level.SEVERE, null, ex);
        }

        this.rsDV = rsDV;
    }

    public long getIndexSelected()
    {
        rsDV.position(1 + (2 * 4));
        return rsDV.getShort() & 0xFFFF;
    }

    public void setIndexSelected(long i)
    {
        rsDV.position(1 + (2 * 4));
        rsDV.putShort((short)i);
    }
}
