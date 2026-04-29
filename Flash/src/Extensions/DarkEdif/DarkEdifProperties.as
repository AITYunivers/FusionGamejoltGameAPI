package Extensions.DarkEdif
{
	import Actions.*;
	
	import Conditions.*;
	
	import Expressions.*;
	
	import Objects.*;
	
	import RunLoop.*;
	
	import Services.*;
	import Extensions.*;
    import flash.utils.ByteArray;
    import flash.geom.Point;

    public class DarkEdifProperties
    {
        private var numProps:uint = 0;
        private var sizeBytes:uint = 0;
        private var propVer:int = 0;
        private var chkboxes:ByteArray;
        private var props:Vector.<DarkEdifProperty>;

        public function DarkEdifProperties(ext:CRunExtension, edPtrFile:CBinaryFile, extVersion:int)
        {
            // DarkEdif SDK stores offset of DarkEdif props away from start of EDITDATA inside private data.
            // eHeader is 20 bytes, so this should be 20+ bytes.
            if (ext.ho.privateData < 20)
            {
                throw new Error("Not smart properties - eHeader missing?");
            }
            // DarkEdif SDK header read:
            // header uint32, hash uint32, hashtypes uint32, numprops uint16, pad uint16, sizeBytes uint32 (includes whole EDITDATA)
            // if prop set v2, then uint64 editor checkbox ptr
            // then checkbox list, one bit per checkbox, including non-checkbox properties
            // so skip numProps / 8 bytes
            // then moving to Data list:
            // size uint32 (includes whole Data), propType uint16, propNameSize uint8, propname u8 (lowercased), then data bytes

            var bytes:ByteArray = new ByteArray();
            var oldPos:uint = edPtrFile.data.position;
            edPtrFile.data.readBytes(bytes, 0, edPtrFile.data.bytesAvailable);
            edPtrFile.data.position = oldPos;

            edPtrFile.skipBytes(ext.ho.privateData - 20); // sub size of eHeader; edPtrFile won't start with eHeader
            var verBuff:ByteArray = edPtrFile.readByteArray(4);
		    var verStr:String = "";
            for (var i:int = verBuff.length - 1; i >= 0; i--)
            {
                verStr += String.fromCharCode(verBuff[i]);
            }
            var propVer:int;
            if (verStr == 'DAR2')
            {
                propVer = 2;
            }
            else if (verStr == 'DAR1')
            {
                propVer = 1;
            }
            else
            {
                throw new Error("Version string " + verStr + " unknown. Did you restore the file offset?");
            }
            // Pull out hash, hashTypes, numProps, pad, sizeBytes, visibleEditorProps
		    var header:ByteArray = edPtrFile.readByteArray(4 + 4 + 2 + 2 + 4 + (propVer > 1 ? 8 : 0));
            header.endian = Endian.LITTLE_ENDIAN;
            header.position = 4 + 4; // Skip past hash and hashTypes
            numProps = header.readUnsignedShort();
            header.position = 4 + 4 + 4; // skip past numProps and pad
            sizeBytes = header.readUnsignedInt();

            var editData:ByteArray = edPtrFile.readByteArray(
                sizeBytes -
                // skip area between eHeader -> Props
                (ext.ho.privateData - 20) -
                // Skip DarkEdif header
                header.length
            );
		    chkboxes = new ByteArray();
            editData.position = 0;
            editData.readBytes(chkboxes, 0, Math.ceil(numProps / 8));

            props = new Vector.<DarkEdifProperty>();
            var data:ByteArray = new ByteArray();
            editData.position = chkboxes.length;
            editData.readBytes(data, 0, editData.bytesAvailable);

            // Dont need TextDecoder

            var propSize:int = 0;
            var propEnd:int = 0;
            data.position = 0; // pt
            data.endian = Endian.LITTLE_ENDIAN;
            for (var i:int = 0; i < numProps; ++i)
            {
                propSize = data.readUnsignedInt();
                propEnd = data.position - 4 + propSize;
                var propTypeID:uint = data.readUnsignedShort();
                // propJSONIndex does not exist in Data in DarkEdif smart props ver 1, so JSON index is same as prop index
			    var propJSONIndex:int = i;
                if (propVer == 2)
                {
				    propJSONIndex = data.readUnsignedShort();
                }
                var propNameLength:int = data.readUnsignedByte();
                var propName:String = data.readUTFBytes(propNameLength);
                var propData:ByteArray = new ByteArray();
                data.readBytes(propData, 0, propEnd - data.position);

                props.push(new DarkEdifProperty(i, propTypeID, propJSONIndex, propName, propData));
                data.position = propEnd;
            }
        }

        public function IsComboBoxProp(propTypeID:int):Boolean
        {
			// PROPTYPE_COMBOBOX, PROPTYPE_COMBOBOXBTN, PROPTYPE_ICONCOMBOBOX
			return propTypeID == 7 || propTypeID == 20 || propTypeID == 24;
        }

        public function RuntimePropSet(data:DarkEdifProperty):DarkEdifPropSet
        {
            var rs:DarkEdifPropSet = new DarkEdifPropSet(data.propData);
			if (rs.setIndicator != 'S')
				throw new Error("Not a prop set!");
			return rs;
        }

        public function GetPropertyIndex(chkIDOrName:Object):int
        {
            if (propVer > 1)
            {
                var jsonIdx:int = -1;
                var p:DarkEdifProperty = null;
                if (chkIDOrName is int || chkIDOrName is uint || chkIDOrName is Number)
                {
                    for each (var prop:DarkEdifProperty in props)
                    {
                        if (prop.index == chkIDOrName)
                        {
                            p = prop;
                            break;
                        }
                    }
                }
                else
                {
                    for each (var prop2:DarkEdifProperty in props)
                    {
                        if (prop2.propName == chkIDOrName)
                        {
                            p = prop2;
                            break;
                        }
                    }
                }
                if (p == null)
                {
                    throw new Error("Invalid property name \"" + chkIDOrName + "\"");
                }
                jsonIdx = p.propJSONIndex;

                // Look up prop index from JSON index - DarkEdif::Properties::PropIdxFromJSONIdx
				var data:DarkEdifProperty = props[0];
                var i:int = 0;
                while (data.propJSONIndex != jsonIdx)
                {
                    if (i >= numProps)
                    {
						throw new Error("Couldn't find property of JSON ID " + jsonIdx + ", hit property " + i + " of " + numProps + " stored.\n");
					}

                    if (IsComboBoxProp(data.propTypeID) && String.fromCharCode(data.propData[0]) == 'S')
                    {
                        var rs:DarkEdifPropSet = RuntimePropSet(data);
                        var rsContainer:DarkEdifProperty = data;
                        if (jsonIdx > rs.lastSetJSONPropIndex)
                        {
                            while (data.propJSONIndex != rs.lastSetJSONPropIndex)
                            {
                                data = props[i++];
                            }
                            rs = rsContainer = null;
                        }
                        // It's within this set's range
                        else if (jsonIdx >= rs.firstSetJSONPropIndex && jsonIdx <= rs.lastSetJSONPropIndex)
                        {
                            if (rs.getIndexSelected() > 0)
                            {
                                var j:int = 0
                                while (true)
                                {
                                    data = props[++i];

                                    // Skip until end of this entry, then move to next prop
									if (data.propJSONIndex == rs.lastSetJSONPropIndex)
                                    {
                                        if (++j == rs.getIndexSelected())
                                        {
                                            data = props[++i];
											break;
                                        }
                                    }
                                }
								continue;
                            }
                            else
                            {
								data = props[++i];
								continue;
                            }
                        }
						// else it's not in this set: continue to standard loop
						else
                        {
							rs = rsContainer = null;
						}
                    }
					
					data = props[++i];
                }
				return data.index;
            }
            if (chkIDOrName is int || chkIDOrName is uint || chkIDOrName is Number)
            {
				if (numProps <= chkIDOrName)
                {
					throw new Error("Invalid property ID " + chkIDOrName + ", max ID is " + (numProps - 1) + ".");
				}
				return chkIDOrName;
			}
            var p:DarkEdifProperty = null;
            for each (var prop3:DarkEdifProperty in props)
            {
                if (prop3.propName == chkIDOrName)
                {
                    p = prop3;
                    break;
                }
            }
            if (p == null)
            {
                throw new Error("Invalid property name \"" + chkIDOrName + "\"");
            }
			return p.index;
        }

        public function IsPropChecked(chkIDOrName:Object):Boolean
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
			if (idx == -1)
            {
				return false;
			}
			return (chkboxes[Math.floor(idx / 8)] & (1 << idx % 8)) != 0;
        }

        public function GetPropertyStr(chkIDOrName:Object):String
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
			if (idx == -1)
            {
				return null;
			}
			var prop:DarkEdifProperty = props[idx];
            const textPropIDs:Vector.<int> = new <int>[
                5,  // PROPTYPE_EDIT_STRING
                22, // PROPTYPE_EDIT_MULTILINE
                16, // PROPTYPE_FILENAME
                19, // PROPTYPE_PICTUREFILENAME
                26, // PROPTYPE_DIRECTORYNAME
            ];
            if (textPropIDs.indexOf(prop.propTypeID) != -1 || IsComboBoxProp(prop.propTypeID))
            {
                // Prop ver 2 added repeating prop sets
				if (propVer == 2 && IsComboBoxProp(prop.propTypeID))
                {
                    var setIndicator:String = String.fromCharCode(prop.propData[0]);
                    if (setIndicator == 'L')
                    {
                        prop.propData.position = 1;
                        return prop.propData.readUTFBytes(prop.propData.bytesAvailable);
                    }
                    else if (setIndicator == 'S')
                    {
                        throw new Error("Property " + prop.propName + " is not textual.");
                    }
                    throw new Error("Property " + prop.propName + " is not a valid list property.");
                }
                prop.propData.position = 0;
                var t:String = prop.propData.readUTFBytes(prop.propData.bytesAvailable);
                if (prop.propTypeID == 22) //PROPTYPE_EDIT_MULTILINE
                {
					t = t.replace(/\r/g, ""); // CRLF to LF
				}
				return t;
            }
			throw new Error("Property " + prop.propName + " is not textual.");
        }

        public function GetPropertyNum(chkIDOrName:Object):Number
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
            if (idx == -1)
            {
				return 0.0;
			}
            var prop:DarkEdifProperty = props[idx];
            const numPropIDsInteger:Vector.<int> = new <int>[
				6, // PROPTYPE_EDIT_NUMBER
				9, // PROPTYPE_COLOR
				11, // PROPTYPE_SLIDEREDIT
				12, // PROPTYPE_SPINEDIT
				13 // PROPTYPE_DIRCTRL
            ];
            const numPropIDsFloat:Vector.<int> = new <int>[
				21, // PROPTYPE_EDIT_FLOAT
				27 // PROPTYPE_SPINEDITFLOAT
            ];
			if (numPropIDsInteger.indexOf(prop.propTypeID) != -1)
            {
                prop.propData.position = 0;
				return prop.propData.readUnsignedInt();
			}
			if (numPropIDsFloat.indexOf(prop.propTypeID) != -1)
            {
                prop.propData.position = 0;
				return prop.propData.readFloat();
			}
			throw new Error("Property " + prop.propName + " is not numeric.");
        }

        public function GetPropertyImageID(chkIDOrName:Object, imgID:int):int
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
            if (idx == -1)
            {
				return -1;
			}
            var prop:DarkEdifProperty = props[idx];
			if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
            {
				throw new Error("Property " + prop.propName + " is not an image list.");
			}
			
			if ((int(imgID) != imgID) || imgID < 0)
            {
				throw new Error("Image index " + imgID + " is invalid.");
			}
            prop.propData.position = 0;
			if (imgID >= prop.propData.readUnsignedShort())
            {
				return -1;
			}
			
            prop.propData.position = 2 * (1 + idx);
			return prop.propData.readUnsignedShort();
        }

        public function GetPropertyNumImages(chkIDOrName:Object, imgID:int):int
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
            if (idx == -1)
            {
				return -1;
			}
            var prop:DarkEdifProperty = props[idx];
			if (prop.propTypeID != 23) // PROPTYPE_IMAGELIST
            {
				throw new Error("Property " + prop.propName + " is not an image list.");
			}
			
            prop.propData.position = 0;
			return prop.propData.readUnsignedShort();
        }

        public function GetSizeProperty(chkIDOrName:Object):Point
        {
			var idx:int = GetPropertyIndex(chkIDOrName);
            if (idx == -1)
            {
				return null;
			}
            var prop:DarkEdifProperty = props[idx];
			if (prop.propTypeID != 8) // PROPTYPE_SIZE
            {
				throw new Error("Property " + prop.propName + " is not an size property.");
			}

            var size:Point = new Point();
            prop.propData.position = 0;
            size.x = prop.propData.readInt();
            prop.propData.position = 4;
            size.y = prop.propData.readInt();
            return size;
        }

        // TODO: PropSetIterator and LoopPropSet
    }
}