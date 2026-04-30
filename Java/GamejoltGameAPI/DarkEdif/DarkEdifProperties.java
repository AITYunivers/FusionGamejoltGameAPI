package GamejoltGameAPI.DarkEdif;

import Extensions.*;
import Services.*;
import java.awt.Point;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.*;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Yunivers and Phi
 */
public class DarkEdifProperties {
    private long numProps = 0;
    private long sizeBytes = 0;
    private int propVer = 0;
    private byte[] chkboxes;
    private List props;

    private static int GetFileLength(CBinaryFile file)
    {
        try {
            java.lang.reflect.Field field = CBinaryFile.class.getDeclaredField("data");
            field.setAccessible(true);
            byte[] data = (byte[]) field.get(file);
            return data.length;
        } catch (IllegalArgumentException ex) {
            Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
        } catch (NoSuchFieldException ex) {
            Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
        } catch (SecurityException ex) {
            Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
        }
        return 0;
    }

    public DarkEdifProperties(CRunExtension ext, CBinaryFile edPtrFile, int extVersion)
    {
        // DarkEdif SDK stores offset of DarkEdif props away from start of EDITDATA inside private data.
        // eHeader is 20 bytes, so this should be 20+ bytes.
        if (ext.ho.privateData < 20)
        {
            throw new RuntimeException("Not smart properties - eHeader missing?");
        }
        // DarkEdif SDK header read:
        // header uint32, hash uint32, hashtypes uint32, numprops uint16, pad uint16, sizeBytes uint32 (includes whole EDITDATA)
        // if prop set v2, then uint64 editor checkbox ptr
        // then checkbox list, one bit per checkbox, including non-checkbox properties
        // so skip numProps / 8 bytes
        // then moving to Data list:
        // size uint32 (includes whole Data), propType uint16, propNameSize uint8, propname u8 (lowercased), then data bytes

        int oldPos = edPtrFile.getFilePointer();
        byte[] bytes = new byte[GetFileLength(edPtrFile)];
        edPtrFile.seek(0);
        edPtrFile.read(bytes);
        edPtrFile.seek(oldPos);
        byte[] verBuff = new byte[4];
        edPtrFile.skipBytes(ext.ho.privateData - 20); // sub size of eHeader; edPtrFile won't start with eHeader
        edPtrFile.read(verBuff);
        String verStr = "";
        for (int i = verBuff.length - 1; i >= 0; i--)
        {
            verStr += (char)verBuff[i];
        }
        if (verStr.equals("DAR2"))
        {
            propVer = 2;
        }
        else if (verStr.equals("DAR1"))
        {
            propVer = 1;
        }
        else
        {
            throw new RuntimeException("Version string " + verStr + " unknown. Did you restore the file offset?");
        }
        // Pull out hash, hashTypes, numProps, pad, sizeBytes, visibleEditorProps
        byte[] headerBytes = new byte[4 + 4 + 2 + 2 + 4 + (propVer > 1 ? 8 : 0)];
        edPtrFile.read(headerBytes);
        ByteBuffer header = ByteBuffer.wrap(headerBytes);
        header.order(ByteOrder.LITTLE_ENDIAN);
        header.position(4 + 4); // Skip past hash and hashTypes
        numProps = header.getShort() & 0xFFFF;
        header.position(4 + 4 + 4); // skip past numProps and pad
        sizeBytes = header.getInt() & 0xFFFFFFFFL;

        byte[] editDataBytes = new byte[
            (int)sizeBytes -
            // skip eHeader
            ext.ho.privateData -
            // cursor offset
            4 -
            // Skip DarkEdif header
            (int)header.limit()
        ];
        edPtrFile.read(editDataBytes);
        ByteBuffer editData = ByteBuffer.wrap(editDataBytes);
        editData.order(ByteOrder.LITTLE_ENDIAN);
        editData.position(0);
        chkboxes = new byte[(int)Math.ceil(numProps / 8.0)];
        editData.get(chkboxes, 0, chkboxes.length);

        props = new ArrayList();
        editData.position(chkboxes.length);
        ByteBuffer data = editData.slice();
        data.order(ByteOrder.LITTLE_ENDIAN);

        // Dont need TextDecoder

        long propSize = 0;
        long propEnd = 0;
        data.position(0); // pt
        for (long i = 0; i < numProps; ++i)
        {
            propSize = data.getInt() & 0xFFFFFFFFL;
            propEnd = data.position() - 4 + propSize;
            long propTypeID = data.getShort() & 0xFFFF;
            // propJSONIndex does not exist in Data in DarkEdif smart props ver 1, so JSON index is same as prop index
            long propJSONIndex = i;
            if (propVer == 2)
            {
                propJSONIndex = data.getShort() & 0xFFFF;
            }
            int propNameLength = data.get() & 0xFF;
            byte[] propNameBytes = new byte[propNameLength];
            data.get(propNameBytes);
            String propName = "";
            try {
                propName = new String(propNameBytes, "UTF-8");
            } catch (UnsupportedEncodingException ex) {
                Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
            }

            byte[] propDataBytes = new byte[(int)(propEnd - data.position())];
            data.get(propDataBytes);
            ByteBuffer propData = ByteBuffer.wrap(propDataBytes);
            propData.order(ByteOrder.LITTLE_ENDIAN);

            props.add(new DarkEdifProperty(i, propTypeID, propJSONIndex, propName, propData));
            data.position((int)propEnd);
        }
    }

    public boolean IsComboBoxProp(int propTypeID)
    {
        // PROPTYPE_COMBOBOX, PROPTYPE_COMBOBOXBTN, PROPTYPE_ICONCOMBOBOX
        return propTypeID == 7 || propTypeID == 20 || propTypeID == 24;
    }

    public DarkEdifPropSet RuntimePropSet(DarkEdifProperty data)
    {
        DarkEdifPropSet rs = new DarkEdifPropSet(data.propData);
        if (!rs.setIndicator.equals("S"))
            throw new RuntimeException("Not a prop set!");
        return rs;
    }

    public int GetPropertyIndex(Object chkIDOrName) 
    {
        if (propVer > 1)
        {
            int jsonIdx = -1;
            DarkEdifProperty p = null;
            if (chkIDOrName instanceof Integer || chkIDOrName instanceof Long || chkIDOrName instanceof Float || chkIDOrName instanceof Double)
            {
                long id = ((Number)chkIDOrName).longValue();
                for (int i = 0; i < props.size(); i++)
                {
                    DarkEdifProperty prop = (DarkEdifProperty)props.get(i);
                    if (prop.index == id)
                    {
                        p = prop;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < props.size(); i++)
                {
                    DarkEdifProperty prop2 = (DarkEdifProperty)props.get(i);
                    if (prop2.propName.equals(chkIDOrName.toString()))
                    {
                        p = prop2;
                        break;
                    }
                }
            }
            if (p == null)
            {
                throw new RuntimeException("Invalid property name \"" + chkIDOrName + "\"");
            }
            jsonIdx = (int)p.propJSONIndex;

            // Look up prop index from JSON index - DarkEdif::Properties::PropIdxFromJSONIdx
            DarkEdifProperty data = (DarkEdifProperty)props.get(0);
            int i = 0;
            while (data.propJSONIndex != jsonIdx)
            {
                if (i >= numProps)
                {
                    throw new RuntimeException("Couldn't find property of JSON ID " + jsonIdx + ", hit property " + i + " of " + numProps + " stored.\n");
                }

                char propDataIdentifier = (char)data.propData.get(0);
                if (IsComboBoxProp((int)data.propTypeID) && propDataIdentifier == 'S')
                {
                    DarkEdifPropSet rs = RuntimePropSet(data);
                    DarkEdifProperty rsContainer = data;
                    if (jsonIdx > rs.lastSetJSONPropIndex)
                    {
                        while (data.propJSONIndex != rs.lastSetJSONPropIndex)
                        {
                            data = (DarkEdifProperty)props.get(i++);
                        }
                        rs = null;
                        rsContainer = null;
                    }
                    // It's within this set's range
                    else if (jsonIdx >= rs.firstSetJSONPropIndex && jsonIdx <= rs.lastSetJSONPropIndex)
                    {
                        if (rs.getIndexSelected() > 0)
                        {
                            int j = 0;
                            while (true)
                            {
                                data = (DarkEdifProperty)props.get(++i);

                                // Skip until end of this entry, then move to next prop
                                if (data.propJSONIndex == rs.lastSetJSONPropIndex)
                                {
                                    if (++j == rs.getIndexSelected())
                                    {
                                        data = (DarkEdifProperty)props.get(++i);
                                        break;
                                    }
                                }
                            }
                            continue;
                        }
                        else
                        {
                            data = (DarkEdifProperty)props.get(++i);
                            continue;
                        }
                    }
                    // else it's not in this set: continue to standard loop
                    else
                    {
                        rs = null;
                        rsContainer = null;
                    }
                }

                data = (DarkEdifProperty)props.get(++i);
            }
            return (int)data.index;
        }
        if (chkIDOrName instanceof Integer || chkIDOrName instanceof Long || chkIDOrName instanceof Float || chkIDOrName instanceof Double)
        {
            long id = ((Number)chkIDOrName).longValue();
            if (numProps <= id)
            {
                throw new RuntimeException("Invalid property ID " + chkIDOrName + ", max ID is " + (numProps - 1) + ".");
            }
            return (int)id;
        }
        DarkEdifProperty p2 = null;
        for (int i = 0; i < props.size(); i++)
        {
            DarkEdifProperty prop3 = (DarkEdifProperty)props.get(i);
            if (prop3.propName.equals(chkIDOrName.toString()))
            {
                p2 = prop3;
                break;
            }
        }
        if (p2 == null)
        {
            throw new RuntimeException("Invalid property name \"" + chkIDOrName + "\"");
        }
        return (int)p2.index;
    }

    public boolean IsPropChecked(Object chkIDOrName)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return false;
        }
        return (chkboxes[(int)Math.floor(idx / 8.0)] & (1 << idx % 8)) != 0;
    }

    static final List textPropIDs = Arrays.asList(new Integer[]
    {
        5,  // PROPTYPE_EDIT_STRING
        22, // PROPTYPE_EDIT_MULTILINE
        16, // PROPTYPE_FILENAME
        19, // PROPTYPE_PICTUREFILENAME
        26, // PROPTYPE_DIRECTORYNAME
    });

    public String GetPropertyStr(Object chkIDOrName)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return null;
        }
        DarkEdifProperty prop = (DarkEdifProperty)props.get(idx);
        if (textPropIDs.indexOf((int)prop.propTypeID) != -1 || IsComboBoxProp((int)prop.propTypeID))
        {
            // Prop ver 2 added repeating prop sets
            if (propVer == 2 && IsComboBoxProp((int)prop.propTypeID))
            {
                char setIndicator = (char)prop.propData.get(0);
                if (setIndicator == 'L')
                {
                    prop.propData.position(1);
                    byte[] propStrBytes = new byte[prop.propData.remaining()];
                    prop.propData.get(propStrBytes);
                    try {
                        return new String(propStrBytes, "UTF-8");
                    } catch (UnsupportedEncodingException ex) {
                        Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
                    }
                    return "";
                }
                else if (setIndicator == 'S')
                {
                    throw new RuntimeException("Property " + prop.propName + " is not textual.");
                }
                throw new RuntimeException("Property " + prop.propName + " is not a valid list property.");
            }
            prop.propData.position(0);
            byte[] tBytes = new byte[prop.propData.remaining()];
            prop.propData.get(tBytes);
            String t = "";
            try {
                t = new String(tBytes, "UTF-8");
            } catch (UnsupportedEncodingException ex) {
                Logger.getLogger(DarkEdifProperties.class.getName()).log(Level.SEVERE, null, ex);
            }
            if (prop.propTypeID == 22) //PROPTYPE_EDIT_MULTILINE
            {
                t = t.replace("\r", ""); // CRLF to LF
            }
            return t;
        }
        throw new RuntimeException("Property " + prop.propName + " is not textual.");
    }

    static final List numPropIDsInteger = Arrays.asList(new Integer[]
    {
        6, // PROPTYPE_EDIT_NUMBER
        9, // PROPTYPE_COLOR
        11, // PROPTYPE_SLIDEREDIT
        12, // PROPTYPE_SPINEDIT
        13 // PROPTYPE_DIRCTRL
    });

    static final List numPropIDsFloat = Arrays.asList(new Integer[]
    {
        21, // PROPTYPE_EDIT_FLOAT
        27 // PROPTYPE_SPINEDITFLOAT
    });

    public double GetPropertyNum(Object chkIDOrName)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return 0.0;
        }
        DarkEdifProperty prop = (DarkEdifProperty)props.get(idx);
        if (numPropIDsInteger.indexOf((int)prop.propTypeID) != -1)
        {
            return prop.propData.getInt(0) & 0xFFFFFFFFL;
        }
        if (numPropIDsFloat.indexOf((int)prop.propTypeID) != -1)
        {
            return prop.propData.getFloat(0);
        }
        throw new RuntimeException("Property " + prop.propName + " is not numeric.");
    }

    public int GetPropertyImageID(Object chkIDOrName, int imgID)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return -1;
        }
        DarkEdifProperty prop = (DarkEdifProperty)props.get(idx);
        if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
        {
            throw new RuntimeException("Property " + prop.propName + " is not an image list.");
        }

        if (imgID < 0)
        {
            throw new RuntimeException("Image index " + imgID + " is invalid.");
        }
        if (imgID >= (prop.propData.getShort(0) & 0xFFFF))
        {
            return -1;
        }

        return prop.propData.getShort(2 * (1 + idx)) & 0xFFFF;
    }

    public int GetPropertyNumImages(Object chkIDOrName, int imgID)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return -1;
        }
        DarkEdifProperty prop = (DarkEdifProperty)props.get(idx);
        if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
        {
            throw new RuntimeException("Property " + prop.propName + " is not an image list.");
        }

        return prop.propData.getShort(0) & 0xFFFF;
    }

    public Point GetSizeProperty(Object chkIDOrName)
    {
        int idx = GetPropertyIndex(chkIDOrName);
        if (idx == -1)
        {
            return null;
        }
        DarkEdifProperty prop = (DarkEdifProperty)props.get(idx);
        if (prop.propTypeID != 8) // PROPTYPE_SIZE
        {
            throw new RuntimeException("Property " + prop.propName + " is not an size property.");
        }

        Point size = new Point();
        size.x = prop.propData.getInt(0);
        size.y = prop.propData.getInt(4);
        return size;
    }
}
