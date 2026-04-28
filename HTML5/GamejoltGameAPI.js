/*
	This GamejoltGameAPI Fusion extension HTML5 port is under MIT license.

	Modification for purposes of tuning to your own HTML5 application is permitted, but must retain this notice and not be redistributed,
	outside of its (hopefully minified) presence inside your HTML5 website's source code.
*/

/* global console, darkEdif, globalThis, alert, CRunExtension, FinalizationRegistry, CServices */
/* jslint esversion: 6, sub: true */

// This is strict, but that can be assumed
// "use strict";

// Global data, including sub-applications, just how God intended.
// Note: This will allow newer SDK versions in later SDKs to take over.
// We need this[] and globalThis[] instead of direct because HTML5 Final Project minifies and breaks the names otherwise
globalThis['darkEdif'] = (globalThis['darkEdif'] && globalThis['darkEdif'].sdkVersion >= 20) ? globalThis['darkEdif'] :
	new (/** @constructor */ function() {
	// window variable is converted into __scope for some reason, so globalThis it is.
	this.data = {};
	this.getGlobalData = function (key) {
		key = key.toLowerCase();
		if (key in this.data) {
			return this.data[key];
		}
		return null;
	};
	this.setGlobalData = function (key, value) {
		key = key.toLowerCase();
		this.data[key] = value;
	};

	this.getCurrentFusionEventNumber = function (ext) {
		return ext.rh.rhEvtProg.rhEventGroup.evgLine || -1;
	};
	this.sdkVersion = 20;
	this.checkSupportsSDKVersion = function (sdkVer) {
		if (sdkVer < 16 || sdkVer > 20) {
			throw "HTML5 DarkEdif SDK does not support SDK version " + this.sdkVersion;
		}
	};

	// minifier will rename notMinified, so we can detect minifier simply
	this.minified = false;
	if (!this.hasOwnProperty('minified')) {
		this['minified'] = true;
	}

	this.consoleLog = function (ext, str) {
		// Log if DebugMode not defined, or true
		if (ext == null || ext['DebugMode'] == null || ext['DebugMode']) {
			// Exts will be renamed in minified
			if (this['minified'] && ext != null && ext['DebugMode'] == true) {
				console.warn("DebugMode left true for an extension in minified mode. Did the ext developer not set it false before distributing?");
				ext['DebugMode'] = false;
			}

			const extName = (ext == null || this['minified']) ? "Unknown DarkEdif ext" :
				ext['ExtensionName'] || ext.constructor.name.replaceAll('CRun', '').replaceAll('_',' ');
			console.log(extName + " - " + str);
		}
	};
	if (!this['minified']) {
		let that = this;
		// Use this for debugging to make sure objects are deleted.
		// Note they're not garbage collected when last holder releases it, but at any point after,
		// when the GC decides to.
		// On Chrome, it took half a minute or so, and delay was possibly affected by whether the page has focus.
		// GC is not required, remember - the cleanup may not happen at all in some browsers.
		this.finalizer = new FinalizationRegistry(function(desc) {
			that.consoleLog(null, "Noting the destruction of [" + desc + "].");
		});
	}
	else {
		this.finalizer = { register: function(desc) { } };
	}

	this['Properties'] = function(ext, edPtrFile, extVersion) {
		// DarkEdif SDK stores offset of DarkEdif props away from start of EDITDATA inside private data.
		// eHeader is 20 bytes, so this should be 20+ bytes.
		if (ext.ho.privateData < 20) {
			throw "Not smart properties - eHeader missing?";
		}
		// DarkEdif SDK header read:
		// header uint32, hash uint32, hashtypes uint32, numprops uint16, pad uint16, sizeBytes uint32 (includes whole EDITDATA)
		// if prop set v2, then uint64 editor checkbox ptr
		// then checkbox list, one bit per checkbox, including non-checkbox properties
		// so skip numProps / 8 bytes
		// then moving to Data list:
		// size uint32 (includes whole Data), propType uint16, propNameSize uint8, propname u8 (lowercased), then data bytes

		let bytes = edPtrFile.ccfBytes.slice(edPtrFile.pointer);
		
		edPtrFile.skipBytes(ext.ho.privateData - 20); // sub size of eHeader; edPtrFile won't start with eHeader
		const verBuff = new Uint8Array(edPtrFile.readBuffer(4));
		const verStr = String.fromCharCode.apply('', verBuff.reverse());
		let propVer;
		if (verStr == 'DAR2') {
			propVer = 2;
		} else if (verStr == 'DAR1') {
			propVer = 1;
		} else {
			throw "Version string " + verStr + " unknown. Did you restore the file offset?";
		}
		// Pull out hash, hashTypes, numProps, pad, sizeBytes, visibleEditorProps
		let header = new Uint8Array(edPtrFile.readBuffer(4 + 4 + 2 + 2 + 4 + (propVer > 1 ? 8 : 0)));
		let headerDV = new DataView(header.buffer);
		this.numProps = headerDV.getUint16(4 + 4, true); // Skip past hash and hashTypes
		this.sizeBytes = headerDV.getUint32(4 + 4 + 4, true); // skip past numProps and pad

		let editData = edPtrFile.readBuffer(
			this.sizeBytes -
			// skip area between eHeader -> Props
			(ext.ho.privateData - 20) -
			// Skip DarkEdif header
			header.byteLength
		);
		this.chkboxes = editData.slice(0, Math.ceil(this.numProps / 8));
		let that = this;
		let IsComboBoxProp = function(propTypeID) {
			// PROPTYPE_COMBOBOX, PROPTYPE_COMBOBOXBTN, PROPTYPE_ICONCOMBOBOX
			return propTypeID == 7 || propTypeID == 20 || propTypeID == 24;
		};
		let RuntimePropSet = function(data) {
			let rsDV = new DataView(data.propData.buffer);
			let rs = /* RuntimePropSet */ { 
				// Always 'S', compared with 'L' for non-set list.
				setIndicator: String.fromCharCode(rsDV.getUint8(0)),
				// Number of repeats of this set, 1 is minimum and means one of this set
				numRepeats: rsDV.getUint16(1, true),
				// Property that ends this set's data, as a JSON index, inclusive
				lastSetJSONPropIndex: rsDV.getUint16(1 + 2, true),
				// First property that begins this set's data, as a JSON index
				firstSetJSONPropIndex: rsDV.getUint16(1 + (2 * 2), true),
				// Name property JSON index that will appear in list when switching set entry
				setNameJSONPropIndex: rsDV.getUint16(1 + (2 * 3), true),
				// Current set index selected (0+), present at runtime too, but not used there
				getIndexSelected: function() {
					return rsDV.getUint16(1 + (2 * 4), true);
				},
				setIndexSelected: function(i) {
					rsDV.setUint16(1 + (2 * 4), i, true);
				},
				// Set name, as specified in JSON. Don't confuse with user-specified set name.
				setName: that.textDecoder.decode(data.propData.slice(1 + (2 * 5))),
			};
			if (rs.setIndicator != 'S')
				throw "Not a prop set!";
			return rs;
		};
		let GetPropertyIndex = function(chkIDOrName) {
			if (propVer > 1) {
				let jsonIdx = -1;
				if (typeof chkIDOrName == 'number') {
					const p = that.props.find(function(p) { return p.index == chkIDOrName; });
					if (p == null) {
						throw "Invalid property name \"" + chkIDOrName + "\"";
					}
					jsonIdx = p.propJSONIndex;
				} else {
					const p = that.props.find(function(p) { return p.propName == chkIDOrName; });
					if (p == null) {
						throw "Invalid property name \"" + chkIDOrName + "\"";
					}
					jsonIdx = p.propJSONIndex;
				}
				// Look up prop index from JSON index - DarkEdif::Properties::PropIdxFromJSONIdx
				let data = that.props[0], i = 0;
				while (data.propJSONIndex != jsonIdx) {
					if (i >= that.numProps) {
						throw "Couldn't find property of JSON ID " + jsonIdx + ", hit property " + i + " of " + that.numProps + " stored.\n";
					}
					if (IsComboBoxProp(data.propTypeID) && String.fromCharCode(data.propData[0]) == 'S') {
						let rs = new RuntimePropSet(data);
						let rsContainer = data;
						// We're beyond all of this set's JSON range: skip past all repeats
						if (jsonIdx > rs.lastSetJSONPropIndex) {
							while (data.propJSONIndex != rs.lastSetJSONPropIndex) {
								data = that.props[i++];
							}
							rs = rsContainer = null;
						}
						// It's within this set's range
						else if (jsonIdx >= rs.firstSetJSONPropIndex && jsonIdx <= rs.lastSetJSONPropIndex) {
							if (rs.getIndexSelected() > 0) {
								for (let j = 0; ;) {
									data = that.props[++i];
									
									// Skip until end of this entry, then move to next prop
									if (data.propJSONIndex == rs.lastSetJSONPropIndex) {
										if (++j == rs.getIndexSelected()) {
											data = that.props[++i];
											break;
										}
									}
								}
								continue;
							} else {
								data = that.props[++i];
								continue;
							}
						}
						// else it's not in this set: continue to standard loop
						else {
							rs = rsContainer = null;
						}
					}
					
					data = that.props[++i];
				}
				return data.index;
			}
			if (typeof chkIDOrName == 'number') {
				if (that.numProps <= chkIDOrName) {
					throw "Invalid property ID " + chkIDOrName + ", max ID is " + (that.numProps - 1) + ".";
				}
				return chkIDOrName;
			}
			const p = that.props.find(function(p) { return p.propName == chkIDOrName; });
			if (p == null) {
				throw "Invalid property name \"" + chkIDOrName + "\"";
			}
			return p.index;
		};
		this['IsPropChecked'] = function(chkIDOrName) {
			const idx = GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return 0;
			}
			return (that.chkboxes[Math.floor(idx / 8)] & (1 << idx % 8)) != 0;
		};
		this['GetPropertyStr'] = function(chkIDOrName) {
			const idx = GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return "";
			}
			const prop = that.props[idx];
			const textPropIDs = [
				5, // PROPTYPE_EDIT_STRING:
				22, // PROPTYPE_EDIT_MULTILINE:
				16, // PROPTYPE_FILENAME:
				19, // PROPTYPE_PICTUREFILENAME:
				26, // PROPTYPE_DIRECTORYNAME:
			];
			if (textPropIDs.indexOf(prop.propTypeID) != -1 || IsComboBoxProp(prop.propTypeID)) {
				// Prop ver 2 added repeating prop sets
				if (propVer == 2 && IsComboBoxProp(prop.propTypeID)) {
					const setIndicator = String.fromCharCode(prop.propData[0]);
					if (setIndicator == 'L') {
						return that.textDecoder.decode(prop.propData.slice(1));
					} else if (setIndicator == 'S') {
						throw "Property " + prop.propName + " is not textual.";
					}
					throw "Property " + prop.propName + " is not a valid list property.";
				}
				let t = that.textDecoder.decode(prop.propData);
				if (prop.propTypeID == 22) { //PROPTYPE_EDIT_MULTILINE
					t = t.replaceAll('\r', ''); // CRLF to LF
				}
				return t;
			}
			throw "Property " + prop.propName + " is not textual.";
		};
		this['GetPropertyNum'] = function(chkIDOrName) {
			const idx = that.GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return 0.0;
			}
			const prop = that.props[idx];
			const numPropIDsInteger = [
				6, // PROPTYPE_EDIT_NUMBER
				9, // PROPTYPE_COLOR
				11, // PROPTYPE_SLIDEREDIT
				12, // PROPTYPE_SPINEDIT
				13 // PROPTYPE_DIRCTRL
			];
			const numPropIDsFloat = [
				21, // PROPTYPE_EDIT_FLOAT
				27 // PROPTYPE_SPINEDITFLOAT
			];
			if (numPropIDsInteger.indexOf(prop.propTypeID) != -1) {
				return new DataView(prop.propData).getUint32(0, true);
			}
			if (numPropIDsFloat.indexOf(prop.propTypeID) != -1) {
				return new DataView(prop.propData).getFloat32(0, true);
			}
			throw "Property " + prop.propName + " is not numeric.";
		};
		this['GetPropertyImageID'] = function(chkIDOrName, imgID) {
			const idx = GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return -1;
			}
			const prop = that.props[idx];
			if (prop.propTypeID != 23) { // PROPTYPE_IMAGELIST
				throw "Property " + prop.propName + " is not an image list.";
			}
			
			if ((~~imgID != imgID) || imgID < 0) {
				throw "Image index " + imgID + " is invalid.";
			}
			const dv = new DataView(prop.propData.buffer);
			if (imgID >= dv.getUint16(0, true)) {
				return -1;
			}
			
			return imgID.getUint16(2 * (1 + idx), true)
		};
		this['GetPropertyNumImages'] = function(chkIDOrName, imgID) {
			const idx = GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return -1;
			}
			const prop = that.props[idx];
			if (prop.propTypeID != 23) { // PROPTYPE_IMAGELIST
				throw "Property " + prop.propName + " is not an image list.";
			}
			
			return new DataView(prop.propData.buffer).getUint16(0, true);
		};
		this['GetSizeProperty'] = function(chkIDOrName) {
			const idx = GetPropertyIndex(chkIDOrName);
			if (idx == -1) {
				return -1;
			}
			const prop = that.props[idx];
			if (prop.propTypeID != 8) { // PROPTYPE_SIZE
				throw "Property " + prop.propName + " is not an size property.";
			}
			
			const dv = new DataView(prop.propData.buffer);
			return { width: dv.getInt32(0, true), height: dv.getInt32(4, true) };
		};

		this['PropSetIterator'] = this.PropSetIterator = function(nameListJSONIdx, numSkippedSetsBefore, runSetEntry, props) {
			this.nameListJSONIdx = nameListJSONIdx;
			this.numSkippedSetsBefore = numSkippedSetsBefore;
			this.props = that.props;
			this.runSetEntry = runSetEntry;
			
			this.runPropSet = new RuntimePropSet(runSetEntry);
			this.runPropSet.setIndexSelected(0);
			this.firstIt = true;
			let thatToo = this;
			this.next = function() {
				// next() is called for first iterator
				if (thatToo.firstIt) {
					thatToo.firstIt = false;
				} else {
					thatToo.runPropSet.setIndexSelected(thatToo.runPropSet.getIndexSelected() + 1);
				}
				return {
					value: thatToo.runPropSet.getIndexSelected(),
					done: thatToo.runPropSet.getIndexSelected() >= thatToo.runPropSet.numRepeats
				};
			};
			this[Symbol.iterator] = function () { return this; };
		};
		this['LoopPropSet'] = this.LoopPropSet = function(setName, numSkips = 0) {
			let d;
			for (let i = 0, j = 0; i < that.numProps; ++i) {
				d = that.props[i];
				if (IsComboBoxProp(d.propTypeID) && String.fromCharCode(d.propData[0]) == 'S') {
					if (new RuntimePropSet(d).setName == setName && ++j > numSkips)
						return new that.PropSetIterator(i, j - 1, d, this);
				}
			}
			throw "No set found with name " + setName + ".";
		}

		this.props = [];
		const data = editData.slice(this.chkboxes.length);
		const dataDV = new DataView(new Uint8Array(data).buffer);

		this.textDecoder = null;
		if (globalThis['TextDecoder'] != null) {
			this.textDecoder = new globalThis['TextDecoder']();
		}
		else {
			// one byte = one char - should suffice for basic ASCII property names
			this.textDecoder = {
				decode: function(txt) {
					return String.fromCharCode.apply("", txt);
				}
			};
		}

		for (let i = 0, pt = 0, propSize, propEnd; i < this.numProps; ++i) {
			propSize = dataDV.getUint32(pt, true);
			propEnd = pt + propSize;
			pt += 4;
			const propTypeID = dataDV.getUint16(pt, true);
			pt += 2;
			// propJSONIndex does not exist in Data in DarkEdif smart props ver 1, so JSON index is same as prop index
			let propJSONIndex = i;
			if (propVer == 2) {
				propJSONIndex = dataDV.getUint16(pt, true);
				pt += 2;
			}
			const propNameLength = dataDV.getUint8(pt);
			pt += 1;
			const propName = this.textDecoder.decode(new Uint8Array(data.slice(pt, pt + propNameLength)));
			pt += propNameLength;
			const propData = new Uint8Array(data.slice(pt, propEnd));

			this.props.push({ index: i, propTypeID: propTypeID, propJSONIndex: propJSONIndex, propName: propName, propData: propData });
			pt = propEnd;
		}
	};
	this['Surface'] = function(rhPtr, needBitmapFuncs, needTextFuncs, width, height, alpha) {
		if (rhPtr == null || needBitmapFuncs == null || needTextFuncs == null || width == null || height == null || alpha == null)
			throw "Invalid Surface ctor arguments";
		this.rhPtr = rhPtr;
		this.hasGeometryCapacity = needBitmapFuncs;
		this.hasTextCapacity = needTextFuncs;
		this.canvas = document.createElement("canvas");
		this.context = this.canvas.getContext("2d");
		this.altered = false;
		this.canvas.width = width;
		this.canvas.height = height;
		this.mosaic = 0;
		this.xSpot = this.ySpot = 0;
		
		let surf = this;
		this.faux = { img: surf.canvas, mosaic: 0, xSpot: 0, ySpot: 0 };
		this['FillImageWith'] = function(sf) {
			if (sf.fillType == darkEdif['SurfaceFill']['FillType']['Flat']) {
				surf.context.rect(0, 0, surf.canvas.width, surf.canvas.height);
				surf.context.fillStyle = sf.color;
				surf.context.fill();
				this.altered = true;
				return true;
			}
		};
		this['GetAndResetAltered'] = function() {
			if (!this.altered) {
				return false;
			}
			this.altered = false;
			return true;
		}
		this.ext = null;
		this['SetAsExtensionDisplay'] = function(ext) {
			surf.ext = ext;
		};
		this['BlitToFrameWithExtEffects'] = function(renderer, pt) {
			const x = this.ext.ho.hoX + (pt ? pt.x : 0),
				y = this.ext.ho.hoY + (pt ? pt.y : 0);
			let angle = 0, scaleX = 1, scaleY = 1, inkEffect = 1, inkEffectParam = 0;
			if ((this.ext.ho.hoOEFlags & CObjectCommon.OEFLAG_SPRITES) != 0) {
				angle = this.ext.ho.roc.rcAngle;
				scaleX = this.ext.ho.roc.rcScaleX;
				scaleY = this.ext.ho.roc.rcScaleY;
				inkEffect = this.ext.ho.ros.rsEffect;
				inkEffectParam = this.ext.ho.ros.rsEffectParam;
				this.faux.xSpot = this.ext.ho.hoImgXSpot;
				this.faux.ySpot = this.ext.ho.hoImgYSpot;
			}
			
			surf.context.save();
			renderer._context.save();
			renderer.renderImage(this.faux, x, y, angle, scaleX, scaleY, inkEffect, inkEffectParam);
			renderer._context.restore();
			surf.context.restore();
		};
		this.img = surf.canvas;
		
		return this;
	};
	this['SurfaceFill'] = {
		'FillType': {
			'Flat': 0
		},
		'Solid': function(color) {
			this.fillType = darkEdif['SurfaceFill']['FillType']['Flat'];
			this.color = color;
			return this;
		}
	};
	this['ColorRGB'] = function(r,g,b) {
		return `rgba(${r}, ${g}, ${b}, 1.0)`;
	};
})();

/** @constructor */
function CRunGamejoltGameAPI() {
	/// <summary> Constructor of Fusion object. </summary>

	// DarkEdif SDK exts should have these four variables defined.
	// We need this[] and globalThis[] instead of direct because HTML5 Final Project minifies and breaks the names otherwise
	this['ExtensionVersion'] = 5; // To match C++ version
	this['SDKVersion'] = 20; // To match C++ version
	this['DebugMode'] = true;
	this['ExtensionName'] = 'GamejoltGameAPI';

	// Can't find DarkEdif wrapper
	if (!globalThis.hasOwnProperty('darkEdif')) {
		throw "a wobbly";
	}
	globalThis['darkEdif'].checkSupportsSDKVersion(this.SDKVersion);

	// ======================================================================================================
	// Actions
	// ======================================================================================================
	const joltBase = "https://api.gamejolt.com";
	this.triggerBuffer = [];
	this.latestResponse = null;

	function md5(string)
	{
		function RotateLeft(lValue, iShiftBits)
		{
			return (lValue << iShiftBits) | (lValue >>> (32 - iShiftBits));
		}

		function AddUnsigned(lX, lY)
		{
			var lX4, lY4, lX8, lY8, lResult;
			lX8 = (lX & 0x80000000);
			lY8 = (lY & 0x80000000);
			lX4 = (lX & 0x40000000);
			lY4 = (lY & 0x40000000);
			lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
			if (lX4 & lY4)
			{
				return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
			}
			if (lX4 | lY4)
			{
				if (lResult & 0x40000000)
				{
					return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
				}
				else
				{
					return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
				}
			}
			else
			{
				return (lResult ^ lX8 ^ lY8);
			}
		}

		function F(x, y, z)
		{
			return (x & y) | ((~x) & z);
		}

		function G(x, y, z)
		{
			return (x & z) | (y & (~z));
		}

		function H(x, y, z)
		{
			return (x ^ y ^ z);
		}

		function I(x, y, z)
		{
			return (y ^ (x | (~z)));
		}

		function FF(a, b, c, d, x, s, ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(F(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		}

		function GG(a, b, c, d, x, s, ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(G(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		}

		function HH(a, b, c, d, x, s, ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(H(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		}

		function II(a, b, c, d, x, s, ac)
		{
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(I(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		}

		function ConvertToWordArray(string)
		{
			var lWordCount;
			var lMessageLength = string.length;
			var lNumberOfWords_temp1 = lMessageLength + 8;
			var lNumberOfWords_temp2 = (lNumberOfWords_temp1 - (lNumberOfWords_temp1 % 64)) / 64;
			var lNumberOfWords = (lNumberOfWords_temp2 + 1) * 16;
			var lWordArray = new Array(lNumberOfWords - 1);
			var lBytePosition = 0;
			var lByteCount = 0;
			while (lByteCount < lMessageLength)
			{
				lWordCount = (lByteCount - (lByteCount % 4)) / 4;
				lBytePosition = (lByteCount % 4) * 8;
				lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount) << lBytePosition));
				lByteCount++;
			}
			lWordCount = (lByteCount - (lByteCount % 4)) / 4;
			lBytePosition = (lByteCount % 4) * 8;
			lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80 << lBytePosition);
			lWordArray[lNumberOfWords - 2] = lMessageLength << 3;
			lWordArray[lNumberOfWords - 1] = lMessageLength >>> 29;
			return lWordArray;
		}

		function WordToHex(lValue)
		{
			var WordToHexValue = "", WordToHexValue_temp = "", lByte, lCount;
			for (lCount = 0; lCount <= 3; lCount++)
			{
				lByte = (lValue >>> (lCount * 8)) & 255;
				WordToHexValue_temp = "0" + lByte.toString(16);
				WordToHexValue = WordToHexValue + WordToHexValue_temp.substr(WordToHexValue_temp.length - 2, 2);
			}
			return WordToHexValue;
		}

		function Utf8Encode(string)
		{
			var utftext = "";

			for (var n = 0; n < string.length; n++)
			{
				var c = string.charCodeAt(n);

				if (c < 128)
				{
					utftext += String.fromCharCode(c);
				}
				else if ((c > 127) && (c < 2048))
				{
					utftext += String.fromCharCode((c >> 6) | 192);
					utftext += String.fromCharCode((c & 63) | 128);
				}
				else
				{
					utftext += String.fromCharCode((c >> 12) | 224);
					utftext += String.fromCharCode(((c >> 6) & 63) | 128);
					utftext += String.fromCharCode((c & 63) | 128);
				}

			}

			return utftext;
		}

		var x;
		var k, AA, BB, CC, DD, a, b, c, d;
		var S11 = 7, S12 = 12, S13 = 17, S14 = 22;
		var S21 = 5, S22 = 9 , S23 = 14, S24 = 20;
		var S31 = 4, S32 = 11, S33 = 16, S34 = 23;
		var S41 = 6, S42 = 10, S43 = 15, S44 = 21;

		string = Utf8Encode(string);

		x = ConvertToWordArray(string);

		a = 0x67452301;
		b = 0xEFCDAB89;
		c = 0x98BADCFE;
		d = 0x10325476;

		for (k = 0; k < x.length; k += 16)
		{
			AA = a;
			BB = b;
			CC = c;
			DD = d;
			a = FF(a, b, c, d, x[k + 0], S11, 0xD76AA478);
			d = FF(d, a, b, c, x[k + 1], S12, 0xE8C7B756);
			c = FF(c, d, a, b, x[k + 2], S13, 0x242070DB);
			b = FF(b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
			a = FF(a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
			d = FF(d, a, b, c, x[k + 5], S12, 0x4787C62A);
			c = FF(c, d, a, b, x[k + 6], S13, 0xA8304613);
			b = FF(b, c, d, a, x[k + 7], S14, 0xFD469501);
			a = FF(a, b, c, d, x[k + 8], S11, 0x698098D8);
			d = FF(d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
			c = FF(c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
			b = FF(b, c, d, a, x[k + 11], S14, 0x895CD7BE);
			a = FF(a, b, c, d, x[k + 12], S11, 0x6B901122);
			d = FF(d, a, b, c, x[k + 13], S12, 0xFD987193);
			c = FF(c, d, a, b, x[k + 14], S13, 0xA679438E);
			b = FF(b, c, d, a, x[k + 15], S14, 0x49B40821);
			a = GG(a, b, c, d, x[k + 1], S21, 0xF61E2562);
			d = GG(d, a, b, c, x[k + 6], S22, 0xC040B340);
			c = GG(c, d, a, b, x[k + 11], S23, 0x265E5A51);
			b = GG(b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
			a = GG(a, b, c, d, x[k + 5], S21, 0xD62F105D);
			d = GG(d, a, b, c, x[k + 10], S22, 0x2441453);
			c = GG(c, d, a, b, x[k + 15], S23, 0xD8A1E681);
			b = GG(b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
			a = GG(a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
			d = GG(d, a, b, c, x[k + 14], S22, 0xC33707D6);
			c = GG(c, d, a, b, x[k + 3], S23, 0xF4D50D87);
			b = GG(b, c, d, a, x[k + 8], S24, 0x455A14ED);
			a = GG(a, b, c, d, x[k + 13], S21, 0xA9E3E905);
			d = GG(d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
			c = GG(c, d, a, b, x[k + 7], S23, 0x676F02D9);
			b = GG(b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
			a = HH(a, b, c, d, x[k + 5], S31, 0xFFFA3942);
			d = HH(d, a, b, c, x[k + 8], S32, 0x8771F681);
			c = HH(c, d, a, b, x[k + 11], S33, 0x6D9D6122);
			b = HH(b, c, d, a, x[k + 14], S34, 0xFDE5380C);
			a = HH(a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
			d = HH(d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
			c = HH(c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
			b = HH(b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
			a = HH(a, b, c, d, x[k + 13], S31, 0x289B7EC6);
			d = HH(d, a, b, c, x[k + 0], S32, 0xEAA127FA);
			c = HH(c, d, a, b, x[k + 3], S33, 0xD4EF3085);
			b = HH(b, c, d, a, x[k + 6], S34, 0x4881D05);
			a = HH(a, b, c, d, x[k + 9], S31, 0xD9D4D039);
			d = HH(d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
			c = HH(c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
			b = HH(b, c, d, a, x[k + 2], S34, 0xC4AC5665);
			a = II(a, b, c, d, x[k + 0], S41, 0xF4292244);
			d = II(d, a, b, c, x[k + 7], S42, 0x432AFF97);
			c = II(c, d, a, b, x[k + 14], S43, 0xAB9423A7);
			b = II(b, c, d, a, x[k + 5], S44, 0xFC93A039);
			a = II(a, b, c, d, x[k + 12], S41, 0x655B59C3);
			d = II(d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
			c = II(c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
			b = II(b, c, d, a, x[k + 1], S44, 0x85845DD1);
			a = II(a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
			d = II(d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
			c = II(c, d, a, b, x[k + 6], S43, 0xA3014314);
			b = II(b, c, d, a, x[k + 13], S44, 0x4E0811A1);
			a = II(a, b, c, d, x[k + 4], S41, 0xF7537E82);
			d = II(d, a, b, c, x[k + 11], S42, 0xBD3AF235);
			c = II(c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
			b = II(b, c, d, a, x[k + 9], S44, 0xEB86D391);
			a = AddUnsigned(a, AA);
			b = AddUnsigned(b, BB);
			c = AddUnsigned(c, CC);
			d = AddUnsigned(d, DD);
		}

		var temp = WordToHex(a) + WordToHex(b) + WordToHex(c) + WordToHex(d);
		return temp.toLowerCase();
	}

	function serializeUrl(url, privateKey)
	{
		return url + "&signature=" + md5(joltBase + url + privateKey);
	};

	const httpGet = async (url, responseType, trigger) =>
	{
		let responseTicket = new ResponseTicket(url, responseType);
		if (trigger != undefined)
		{
			responseTicket.hasTrigger = true;
			responseTicket.trigger = trigger;
		}

		try
		{
			const response = await fetch(joltBase + serializeUrl(url, this.privateKey));
			const data = await response.json();
			responseTicket.data = data;
		}
		catch (error)
		{
			responseTicket.hasError = true;
			responseTicket.error = error;
		}

		this.triggerBuffer.push(responseTicket)
	};

	this.Act_Auth = function (userName, userToken)
	{
		this.gameAuthData.userName = userName;
		this.gameAuthData.userToken = userToken;
		httpGet(
			"/api/game/v1_2/users/auth/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'Auth',
			0 // Cnd_AuthFinished
		)
	};

	this.Act_AuthCreds = function ()
	{
		// Not applicable for this runtime
		this.latestResponse = new ResponseTicket('', '');
		this.latestResponse.hasError = true;
		this.latestResponse.error = "Cannot use 'Authorize user via .gj-credentials' on HTML5";
	};

	this.Act_SetGameID = function (gameID)
	{
		this.gameID = gameID;
	};

	this.Act_SetPrivateKey = function (privateKey)
	{
		this.privateKey = privateKey;
	};

	this.Act_SetGuestName = function (guestName)
	{
		this.gameAuthData.guestName = guestName;
	};

	this.Act_FetchUsername = function (userName)
	{
		httpGet(
			"/api/game/v1_2/users/?game_id=" +
			this.gameID +
			"&username=" +
			userName,

			'FetchUsers',
			1 // Cnd_FetchFinished
		)
	};

	this.Act_FetchUserID = function (userID)
	{
		httpGet(
			"/api/game/v1_2/users/?game_id=" +
			this.gameID +
			"&user_id=" +
			userID,

			'FetchUsers',
			1 // Cnd_FetchFinished
		)
	};

	this.Act_OpenSession = function ()
	{
		httpGet(
			"/api/game/v1_2/sessions/open/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'OpenSession',
			2 // Cnd_OpenFinished
		)
	};

	this.Act_PingSession = function ()
	{
		httpGet(
			"/api/game/v1_2/sessions/ping/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'PingSession',
			3 // Cnd_PingFinished
		)
	};

	this.Act_PingStatusSession = function (status)
	{
		httpGet(
			"/api/game/v1_2/sessions/ping/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&status=" +
			status,

			'PingSession',
			3 // Cnd_PingFinished
		)
	};

	this.Act_CheckSession = function ()
	{
		httpGet(
			"/api/game/v1_2/sessions/check/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'CheckSession',
			4 // Cnd_CheckFinished
		)
	};

	this.Act_CloseSession = function ()
	{
		httpGet(
			"/api/game/v1_2/sessions/close/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'CloseSession',
			5 // Cnd_CloseFinished
		)
	};

	this.Act_AddUserScore = function (displayScore, sortScore, table, extraData)
	{
		let url = 	"/api/game/v1_2/scores/add/?game_id=" + 
					this.gameID +
					"&username=" +
					this.gameAuthData.userName + 
					"&user_token=" +
					this.gameAuthData.userToken +
					"&score=" +
					displayScore +
					"&sort=" + 
					sortScore;
		if (table != -1)
			url += "&table_id=" + table;
		if (extraData.length > 0)
			url += "&extra_data=" + extraData;

		httpGet(
			url,
			'AddScore',
			6 // Cnd_ScoreAdded
		)
	};

	this.Act_AddGuestScore = function (displayScore, sortScore, table, extraData)
	{
		let url = 	"/api/game/v1_2/scores/add/?game_id=" + 
					this.gameID +
					"&guest=" +
					this.gameAuthData.guestName + 
					"&score=" +
					displayScore +
					"&sort=" + 
					sortScore;
		if (table != -1)
			url += "&table_id=" + table;
		if (extraData.length > 0)
			url += "&extra_data=" + extraData;

		httpGet(
			url,
			'AddScore',
			6 // Cnd_ScoreAdded
		)
	};

	this.Act_GetScoreRanking = function (score, table)
	{
		let url = 	"/api/game/v1_2/scores/get-rank/?game_id=" + 
					this.gameID +
					"&sort=" + 
					score;
		if (table != -1)
			url += "&table_id=" + table;

		httpGet(
			url,
			'GetRank',
			7 // Cnd_RankingRetrieved
		)
	};

	this.Act_FetchScores = function (table, limit, betterThan, worseThan)
	{
		let url = "/api/game/v1_2/scores/?game_id=" + this.gameID;
		if (table != -1)
			url += "&table_id=" + table;
		if (limit != -1)
			url += "&limit=" + limit;
		if (betterThan != -1)
			url += "&better_than=" + betterThan;
		if (worseThan != -1)
			url += "&worse_than=" + worseThan;

		httpGet(
			url,
			'FetchScores',
			8 // Cnd_ScoresFetched
		)
	};

	this.Act_FetchUserScores = function (table, limit, betterThan, worseThan)
	{
		let url = "/api/game/v1_2/scores/?game_id=" + this.gameID;
		if (this.gameAuthData.userName.length > 0 && this.gameAuthData.userToken.length > 0)
			url += "&username=" + this.gameAuthData.userName + "&user_token=" + this.gameAuthData.userToken;
		else if (this.gameAuthData.guestName.length > 0)
			url += "&guest=" + this.gameAuthData.guestName;

		if (table != -1)
			url += "&table_id=" + table;
		if (limit != -1)
			url += "&limit=" + limit;
		if (betterThan != -1)
			url += "&better_than=" + betterThan;
		if (worseThan != -1)
			url += "&worse_than=" + worseThan;

		httpGet(
			url,
			'FetchScores',
			8 // Cnd_ScoresFetched
		)
	};
	
	this.Act_GetTables = function ()
	{
		httpGet(
			"/api/game/v1_2/scores/tables/?game_id=" +
			this.gameID,

			'ScoreTables',
			9 // Cnd_TablesRetrieved
		)
	};
	
	this.Act_GetTrophy = function (trophy)
	{
		httpGet(
			"/api/game/v1_2/trophies/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&trophy_id=" +
			trophy,

			'FetchTrophies',
			10 // Cnd_TrophiesRetrieved
		)
	};
	
	this.Act_GetTrophies = function ()
	{
		httpGet(
			"/api/game/v1_2/trophies/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'FetchTrophies',
			10 // Cnd_TrophiesRetrieved
		)
	};
	
	this.Act_GetUnlockedTrophies = function ()
	{
		httpGet(
			"/api/game/v1_2/trophies/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&achieved=true",

			'FetchTrophies',
			10 // Cnd_TrophiesRetrieved
		)
	};
	
	this.Act_UnlockTrophy = function (trophy)
	{
		httpGet(
			"/api/game/v1_2/trophies/add-achieved/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&trophy_id=" +
			trophy,

			'AchieveTrophy',
			11 // Cnd_TrophyAdded
		)
	};
	
	this.Act_LockTrophy = function (trophy)
	{
		httpGet(
			"/api/game/v1_2/trophies/remove-achieved/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&trophy_id=" +
			trophy,

			'RevokeTrophy',
			12 // Cnd_TrophyRemoved
		)
	};
	
	this.Act_GlobalStorageGetData = function (key)
	{
		httpGet(
			"/api/game/v1_2/data-store/?game_id=" +
			this.gameID +
			"&key=" +
			key,

			'FetchData',
			13 // Cnd_GSGetData
		)
	};
	
	this.Act_GlobalStorageGetKeys = function (pattern)
	{
		let url = "/api/game/v1_2/data-store/get-keys/?game_id=" + this.gameID;
		if (pattern.length > 0)
			url += "&pattern=" + pattern;

		httpGet(
			url,
			'GetDataKeys',
			14 // Cnd_GSGetKeys
		)
	};
	
	this.Act_GlobalStorageDeleteKey = function (key)
	{
		httpGet(
			"/api/game/v1_2/data-store/remove/?game_id=" +
			this.gameID +
			"&key=" +
			key,

			'RemoveData',
			15 // Cnd_GSDeleteKey
		)
	};
	
	this.Act_GlobalStorageSetKey = function (key, data)
	{
		httpGet(
			"/api/game/v1_2/data-store/set/?game_id=" +
			this.gameID +
			"&key=" +
			key +
			"&data=" +
			data,

			'SetData',
			16 // Cnd_GSSetKey
		)
	};
	
	this.Act_GlobalStorageUpdateKey = function (key, data, operation)
	{
		httpGet(
			"/api/game/v1_2/data-store/update/?game_id=" +
			this.gameID +
			"&key=" +
			key +
			"&value=" +
			data +
			"&operation=" +
			operation,

			'UpdateData',
			17 // Cnd_GSUpdateKey
		)
	};
	
	this.Act_UserStorageGetData = function (key)
	{
		httpGet(
			"/api/game/v1_2/data-store/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&key=" +
			key,

			'FetchData',
			18 // Cnd_USGetData
		)
	};
	
	this.Act_UserStorageGetKeys = function (pattern)
	{
		let url = 	"/api/game/v1_2/data-store/get-keys/?game_id=" + 
					this.gameID +
					"&username=" +
					this.gameAuthData.userName +
					"&user_token=" +
					this.gameAuthData.userToken;
		if (pattern.length > 0)
			url += "&pattern=" + pattern;

		httpGet(
			url,
			'GetDataKeys',
			19 // Cnd_USGetKeys
		)
	};
	
	this.Act_UserStorageDeleteKey = function (key)
	{
		httpGet(
			"/api/game/v1_2/data-store/remove/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&key=" +
			key,

			'RemoveData',
			20 // Cnd_USDeleteKey
		)
	};
	
	this.Act_UserStorageSetKey = function (key, data)
	{
		httpGet(
			"/api/game/v1_2/data-store/set/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&key=" +
			key +
			"&data=" +
			data,

			'SetData',
			21 // Cnd_USSetKey
		)
	};
	
	this.Act_UserStorageUpdateKey = function (key, data, operation)
	{
		httpGet(
			"/api/game/v1_2/data-store/update/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken +
			"&key=" +
			key +
			"&value=" +
			data +
			"&operation=" +
			operation,

			'UpdateData',
			22 // Cnd_USUpdateKey
		)
	};

	this.Act_GlobalFileStorageSaveData = function (key, filePath)
	{
		// No implementation
	};

	this.Act_GlobalFileStorageSetKey = function (key, filePath)
	{
		// No implementation
	};

	this.Act_GlobalFileStorageUpdateKey = function (key, filePath, operation)
	{
		// No implementation
	};

	this.Act_UserFileStorageSaveData = function (key, filePath)
	{
		// No implementation
	};

	this.Act_UserFileStorageSetKey = function (key, filePath)
	{
		// No implementation
	};

	this.Act_UserFileStorageUpdateKey = function (key, filePath, operation)
	{
		// No implementation
	};
	
	this.Act_GetFriendsList = function ()
	{
		httpGet(
			"/api/game/v1_2/friends/?game_id=" +
			this.gameID +
			"&username=" +
			this.gameAuthData.userName +
			"&user_token=" +
			this.gameAuthData.userToken,

			'Friends',
			29 // Cnd_GetFriendsList
		)
	};
	
	this.Act_GetCurrentTime = function ()
	{
		httpGet(
			"/api/game/v1_2/time/?game_id=" +
			this.gameID,

			'Time',
			30 // Cnd_GetCurrentTime
		)
	};

	// ======================================================================================================
	// Conditions
	// ======================================================================================================
	this.Cnd_AnyCallTriggered = function ()
	{
		return true;
	};

	this.Cnd_CallTriggered = function ()
	{
		return true;
	};

	// =============================
	// Expressions
	// =============================

	this.Exp_GetJsonResponse = function ()
	{
		if (this.latestResponse.hasError)
			return "";
		return JSON.stringify(this.latestResponse.data, null, 4);
	};

	this.Exp_GetResponseType = function ()
	{
		return this.latestResponse.type;
	};

	this.Exp_GetResponseStatus = function ()
	{
		if (this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.success == undefined)
			return "";

		return j.response.success;
	};

	this.Exp_GetResponseMessage = function ()
	{
		if (this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.message == undefined)
			return "";

		return j.response.message;
	};

	this.Exp_GetErrorMessage = function ()
	{
		if (!this.latestResponse.hasError)
			return "";
		return this.latestResponse.error;
	};

	this.Exp_GetGameID = function ()
	{
		return this.gameID;
	};

	this.Exp_GetPrivateKey = function ()
	{
		return this.privateKey;
	};

	this.Exp_GetRequestURL = function ()
	{
		return joltBase + this.latestResponse.url;
	};

	this.Exp_GetUserName = function ()
	{
		return this.gameAuthData.userName;
	};

	this.Exp_GetUserToken = function ()
	{
		return this.gameAuthData.userToken;
	};

	this.Exp_GetGuestName = function ()
	{
		return this.gameAuthData.guestName;
	};

	this.Exp_FetchedUserCount = function ()
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users))
			return 0;

		return j.response.users.length;
	};

	this.Exp_FetchedUserDisplayName = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.developer_name == undefined)
			return "";

		return user.developer_name;
	};

	this.Exp_FetchedUsername = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.username == undefined)
			return "";

		return user.username;
	};

	this.Exp_FetchedUserID = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return 0;
		let user = j.response.users[index];
		if (user.id == undefined)
			return 0;

		return parseInt(user.id);
	};

	this.Exp_FetchedUserDescription = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.developer_description == undefined)
			return "";

		return user.developer_description;
	};

	this.Exp_FetchedUserAvatar = function (index, resolution)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.avatar_url == undefined)
			return "";

		return "https://m.gjcdn.net/user-avatar/" + resolution + user.avatar_url.substring(34, user.avatar_url.lastIndexOf(".")) + ".png";
	};

	this.Exp_FetchedUserWebsite = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.developer_website == undefined)
			return "";

		return user.developer_website;
	};

	this.Exp_FetchedUserStatus = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.status == undefined)
			return "";

		return user.status;
	};

	this.Exp_FetchedUserType = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.type == undefined)
			return "";

		return user.type;
	};

	this.Exp_FetchedUserLastLoggedIn = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.last_logged_in == undefined)
			return "";

		return user.last_logged_in;
	};

	this.Exp_FetchedUserLastLoggedInTimestamp = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return 0;
		let user = j.response.users[index];
		if (user.last_logged_in_timestamp == undefined)
			return 0;

		return user.last_logged_in_timestamp;
	};

	this.Exp_FetchedUserSignedUp = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return "";
		let user = j.response.users[index];
		if (user.signed_up == undefined)
			return "";

		return user.signed_up;
	};

	this.Exp_FetchedUserSignedUpTimestamp = function (index)
	{
		if (this.latestResponse.type != 'FetchUsers' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.users == undefined || !Array.isArray(j.response.users) || j.response.users.length <= index)
			return 0;
		let user = j.response.users[index];
		if (user.signed_up_timestamp == undefined)
			return 0;

		return user.signed_up_timestamp;
	};

	this.Exp_ScoreRanking = function ()
	{
		if (this.latestResponse.type != 'GetRank' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.rank == undefined)
			return 0;

		return j.response.rank;
	};

	this.Exp_FetchedScoreCount = function ()
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores))
			return 0;

		return j.response.scores.length;
	};

	this.Exp_FetchedScoreUsername = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return "";
		let score = j.response.scores[index];
		if (score.user == undefined)
			return "";

		return score.user;
	};

	this.Exp_FetchedScoreUserID = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return 0;
		let score = j.response.scores[index];
		if (score.user_id == undefined)
			return 0;

		return parseInt(score.user_id);
	};

	this.Exp_FetchedScoreGuestName = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return "";
		let score = j.response.scores[index];
		if (score.guest == undefined)
			return "";

		return score.guest;
	};

	this.Exp_FetchedScoreScore = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return "";
		let score = j.response.scores[index];
		if (score.score == undefined)
			return "";

		return score.score;
	};

	this.Exp_FetchedScoreSort = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return 0;
		let score = j.response.scores[index];
		if (score.sort == undefined)
			return 0;

		return parseInt(score.sort);
	};

	this.Exp_FetchedScoreExtraData = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return "";
		let score = j.response.scores[index];
		if (score.extra_data == undefined)
			return "";

		return score.extra_data;
	};

	this.Exp_FetchedScoreSubmit = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return "";
		let score = j.response.scores[index];
		if (score.stored == undefined)
			return "";

		return score.stored;
	};

	this.Exp_FetchedScoreSubmitTimestamp = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.scores == undefined || !Array.isArray(j.response.scores) || j.response.scores.length <= index)
			return 0;
		let score = j.response.scores[index];
		if (score.stored_timestamp == undefined)
			return 0;

		return score.stored_timestamp;
	};

	this.Exp_FetchedScoreTableID = function (index)
	{
		if (this.latestResponse.type != 'FetchScores' || this.latestResponse.hasError)
			return 0;

		const match = this.latestResponse.url.match(/table_id=([0-9]+)/);
		return match ? parseInt(match[1]) : 0;
	};

	this.Exp_FetchedTableCount = function ()
	{
		if (this.latestResponse.type != 'ScoreTables' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.tables == undefined || !Array.isArray(j.response.tables))
			return 0;

		return j.response.tables.length;
	};

	this.Exp_FetchedTableName = function (index)
	{
		if (this.latestResponse.type != 'ScoreTables' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.tables == undefined || !Array.isArray(j.response.tables) || j.response.tables.length <= index)
			return "";
		let table = j.response.tables[index];
		if (table.name == undefined)
			return "";

		return table.name;
	};

	this.Exp_FetchedTableID = function (index)
	{
		if (this.latestResponse.type != 'ScoreTables' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.tables == undefined || !Array.isArray(j.response.tables) || j.response.tables.length <= index)
			return 0;
		let table = j.response.tables[index];
		if (table.id == undefined)
			return 0;

		return parseInt(table.id);
	};

	this.Exp_FetchedTableDescription = function (index)
	{
		if (this.latestResponse.type != 'ScoreTables' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.tables == undefined || !Array.isArray(j.response.tables) || j.response.tables.length <= index)
			return "";
		let table = j.response.tables[index];
		if (table.description == undefined)
			return "";

		return table.description;
	};

	this.Exp_FetchedTableIsPrimary = function (index)
	{
		if (this.latestResponse.type != 'ScoreTables' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.tables == undefined || !Array.isArray(j.response.tables) || j.response.tables.length <= index)
			return 0;
		let table = j.response.tables[index];
		if (table.primary == undefined)
			return 0;

		return table.primary;
	};

	this.Exp_FetchedTrophyCount = function ()
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies))
			return 0;

		return j.response.trophies.length;
	};

	this.Exp_FetchedTrophyTitle = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return "";
		let trophy = j.response.trophies[index];
		if (trophy.title == undefined)
			return "";

		return trophy.title;
	};

	this.Exp_FetchedTrophyID = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return 0;
		let trophy = j.response.trophies[index];
		if (trophy.id == undefined)
			return 0;

		return parseInt(trophy.id);
	};

	this.Exp_FetchedTrophyDescription = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return "";
		let trophy = j.response.trophies[index];
		if (trophy.description == undefined)
			return "";

		return trophy.description;
	};

	this.Exp_FetchedTrophyDifficulty = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return "";
		let trophy = j.response.trophies[index];
		if (trophy.difficulty == undefined)
			return "";

		return trophy.difficulty;
	};

	this.Exp_FetchedTrophyImageURL = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return "";
		let trophy = j.response.trophies[index];
		if (trophy.image_url == undefined)
			return "";

		return trophy.image_url;
	};

	this.Exp_FetchedTrophyAchieved = function (index)
	{
		if (this.latestResponse.type != 'FetchTrophies' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.trophies == undefined || !Array.isArray(j.response.trophies) || j.response.trophies.length <= index)
			return "";
		let trophy = j.response.trophies[index];
		if (trophy.achieved == undefined)
			return "";

		return trophy.achieved;
	};

	this.Exp_RetrievedKeyData = function ()
	{
		if (this.latestResponse.type != 'FetchData' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.data == undefined)
			return "";

		return j.response.data;
	};

	this.Exp_FetchedKeyCount = function ()
	{
		if (this.latestResponse.type != 'GetDataKeys' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.keys == undefined || !Array.isArray(j.response.keys))
			return 0;

		return j.response.keys.length;
	};

	this.Exp_FetchedKey = function (index)
	{
		if (this.latestResponse.type != 'GetDataKeys' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.keys == undefined || !Array.isArray(j.response.keys) || j.response.keys.length <= index)
			return "";
		let key = j.response.keys[index];
		if (key.key == undefined)
			return "";

		return key.key;
	};

	this.Exp_UpdatedKeyData = function ()
	{
		if (this.latestResponse.type != 'UpdateData' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.data == undefined)
			return "";

		return j.response.data;
	};

	this.Exp_FetchedFriendCount = function ()
	{
		if (this.latestResponse.type != 'Friends' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.friends == undefined || !Array.isArray(j.response.friends))
			return 0;

		return j.response.friends.length;
	};

	this.Exp_FetchedFriend = function (index)
	{
		if (this.latestResponse.type != 'Friends' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.friends == undefined || !Array.isArray(j.response.friends) || j.response.friends.length <= index)
			return 0;
		let friend = j.response.friends[index];
		if (friend.friend_id == undefined)
			return 0;

		return parseInt(friend.friend_id);
	};

	this.Exp_TimeYear = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.year == undefined)
			return 0;

		return parseInt(j.response.year);
	};

	this.Exp_TimeMonth = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.month == undefined)
			return 0;

		return parseInt(j.response.month);
	};

	this.Exp_TimeDay = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.day == undefined)
			return 0;

		return parseInt(j.response.day);
	};

	this.Exp_TimeHour = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.hour == undefined)
			return 0;

		return parseInt(j.response.hour);
	};

	this.Exp_TimeMinute = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.minute == undefined)
			return 0;

		return parseInt(j.response.minute);
	};

	this.Exp_TimeSecond = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.second == undefined)
			return 0;

		return parseInt(j.response.second);
	};

	this.Exp_TimeTimestamp = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return 0;

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.timestamp == undefined)
			return 0;

		return j.response.timestamp;
	};

	this.Exp_TimeTimezone = function ()
	{
		if (this.latestResponse.type != 'Time' || this.latestResponse.hasError)
			return "";

		let j = this.latestResponse.data;
		if (j.response == undefined || j.response.timezone == undefined)
			return "";

		return j.response.timezone;
	};

	// =============================
	// Function arrays
	// =============================

	this.$actionFuncs = [
		/* 0 */ this.Act_Auth,
		/* 1 */ this.Act_AuthCreds,
		/* 2 */ this.Act_SetGuestName,
		/* 3 */ this.Act_FetchUsername,
		/* 4 */ this.Act_FetchUserID,
		/* 5 */ this.Act_OpenSession,
		/* 6 */ this.Act_PingSession,
		/* 7 */ this.Act_PingStatusSession,
		/* 8 */ this.Act_CheckSession,
		/* 9 */ this.Act_CloseSession,
		/* 10 */ this.Act_AddUserScore,
		/* 11 */ this.Act_AddGuestScore,
		/* 12 */ this.Act_GetScoreRanking,
		/* 13 */ this.Act_FetchScores,
		/* 14 */ this.Act_GetTables,
		/* 15 */ this.Act_GetTrophy,
		/* 16 */ this.Act_GetTrophies,
		/* 17 */ this.Act_GetUnlockedTrophies,
		/* 18 */ this.Act_UnlockTrophy,
		/* 19 */ this.Act_LockTrophy,
		/* 20 */ this.Act_GlobalStorageGetData,
		/* 21 */ this.Act_GlobalStorageGetKeys,
		/* 22 */ this.Act_GlobalStorageDeleteKey,
		/* 23 */ this.Act_GlobalStorageSetKey,
		/* 24 */ this.Act_GlobalStorageUpdateKey,
		/* 25 */ this.Act_UserStorageGetData,
		/* 26 */ this.Act_UserStorageGetKeys,
		/* 27 */ this.Act_UserStorageDeleteKey,
		/* 28 */ this.Act_UserStorageSetKey,
		/* 29 */ this.Act_UserStorageUpdateKey,
		/* 30 */ this.Act_GlobalFileStorageSaveData,
		/* 31 */ this.Act_GlobalFileStorageSetKey,
		/* 32 */ this.Act_GlobalFileStorageUpdateKey,
		/* 33 */ this.Act_UserFileStorageSaveData,
		/* 34 */ this.Act_UserFileStorageSetKey,
		/* 35 */ this.Act_UserFileStorageUpdateKey,
		/* 36 */ this.Act_GetFriendsList,
		/* 37 */ this.Act_GetCurrentTime,
		/* 38 */ this.Act_SetGameID,
		/* 39 */ this.Act_SetPrivateKey,
		/* 40 */ this.Act_FetchUserScores
	];
	this.$conditionFuncs = [
		/* 0 */ this.Cnd_CallTriggered,		/* Cnd_AuthFinished      */
		/* 1 */ this.Cnd_CallTriggered,		/* Cnd_FetchFinished     */
		/* 2 */ this.Cnd_CallTriggered,		/* Cnd_OpenFinished      */
		/* 3 */ this.Cnd_CallTriggered,		/* Cnd_PingFinished      */
		/* 4 */ this.Cnd_CallTriggered,		/* Cnd_CheckFinished     */
		/* 5 */ this.Cnd_CallTriggered,		/* Cnd_CloseFinished     */
		/* 6 */ this.Cnd_CallTriggered,		/* Cnd_ScoreAdded        */
		/* 7 */ this.Cnd_CallTriggered,		/* Cnd_RankingRetrieved  */
		/* 8 */ this.Cnd_CallTriggered,		/* Cnd_ScoresFetched     */
		/* 9 */ this.Cnd_CallTriggered,		/* Cnd_TablesRetrieved   */
		/* 10 */ this.Cnd_CallTriggered,	/* Cnd_TrophiesRetrieved */
		/* 11 */ this.Cnd_CallTriggered,	/* Cnd_TrophyAdded       */
		/* 12 */ this.Cnd_CallTriggered,	/* Cnd_TrophyRemoved     */
		/* 13 */ this.Cnd_CallTriggered,	/* Cnd_GSGetData         */
		/* 14 */ this.Cnd_CallTriggered,	/* Cnd_GSGetKeys         */
		/* 15 */ this.Cnd_CallTriggered,	/* Cnd_GSDeleteKey       */
		/* 16 */ this.Cnd_CallTriggered,	/* Cnd_GSSetKey          */
		/* 17 */ this.Cnd_CallTriggered,	/* Cnd_GSUpdateKey       */
		/* 18 */ this.Cnd_CallTriggered,	/* Cnd_USGetData         */
		/* 19 */ this.Cnd_CallTriggered,	/* Cnd_USGetKeys         */
		/* 20 */ this.Cnd_CallTriggered,	/* Cnd_USDeleteKey       */
		/* 21 */ this.Cnd_CallTriggered,	/* Cnd_USSetKey          */
		/* 22 */ this.Cnd_CallTriggered,	/* Cnd_USUpdateKey       */
		/* 23 */ this.Cnd_CallTriggered,	/* Cnd_FGSSaveData       */
		/* 24 */ this.Cnd_CallTriggered,	/* Cnd_FGSSetKey         */
		/* 25 */ this.Cnd_CallTriggered,	/* Cnd_FGSUpdateKey      */
		/* 26 */ this.Cnd_CallTriggered,	/* Cnd_FUSSaveData       */
		/* 27 */ this.Cnd_CallTriggered,	/* Cnd_FUSSetKey         */
		/* 28 */ this.Cnd_CallTriggered,	/* Cnd_FUSUpdateKey      */
		/* 29 */ this.Cnd_CallTriggered,	/* Cnd_GetFriendsList    */
		/* 30 */ this.Cnd_CallTriggered,	/* Cnd_GetCurrentTime    */
		/* 31 */ this.Cnd_AnyCallTriggered,	/* Cnd_AnyCallFinished   */
		/* 32 */ this.Cnd_CallTriggered		/* Cnd_OnError			 */
	];
	this.$expressionFuncs = [
		/* 0 */ this.Exp_GetJsonResponse,
		/* 1 */ this.Exp_GetResponseType,
		/* 2 */ this.Exp_GetResponseStatus,
		/* 3 */ this.Exp_GetResponseMessage,
		/* 4 */ this.Exp_GetUserName,
		/* 5 */ this.Exp_GetUserToken,
		/* 6 */ this.Exp_GetGuestName,
		/* 7 */ this.Exp_FetchedUserCount,
		/* 8 */ this.Exp_FetchedUserDisplayName,
		/* 9 */ this.Exp_FetchedUsername,
		/* 10 */ this.Exp_FetchedUserID,
		/* 11 */ this.Exp_FetchedUserDescription,
		/* 12 */ this.Exp_FetchedUserAvatar,
		/* 13 */ this.Exp_FetchedUserWebsite,
		/* 14 */ this.Exp_FetchedUserStatus,
		/* 15 */ this.Exp_FetchedUserType,
		/* 16 */ this.Exp_FetchedUserLastLoggedIn,
		/* 17 */ this.Exp_FetchedUserLastLoggedInTimestamp,
		/* 18 */ this.Exp_FetchedUserSignedUp,
		/* 19 */ this.Exp_FetchedUserSignedUpTimestamp,
		/* 20 */ this.Exp_ScoreRanking,
		/* 21 */ this.Exp_FetchedScoreCount,
		/* 22 */ this.Exp_FetchedScoreUsername,
		/* 23 */ this.Exp_FetchedScoreUserID,
		/* 24 */ this.Exp_FetchedScoreGuestName,
		/* 25 */ this.Exp_FetchedScoreScore,
		/* 26 */ this.Exp_FetchedScoreSort,
		/* 27 */ this.Exp_FetchedScoreExtraData,
		/* 28 */ this.Exp_FetchedScoreSubmit,
		/* 29 */ this.Exp_FetchedScoreSubmitTimestamp,
		/* 30 */ this.Exp_FetchedTableCount,
		/* 31 */ this.Exp_FetchedTableName,
		/* 32 */ this.Exp_FetchedTableID,
		/* 33 */ this.Exp_FetchedTableDescription,
		/* 34 */ this.Exp_FetchedTableIsPrimary,
		/* 35 */ this.Exp_FetchedTrophyCount,
		/* 36 */ this.Exp_FetchedTrophyTitle,
		/* 37 */ this.Exp_FetchedTrophyID,
		/* 38 */ this.Exp_FetchedTrophyDescription,
		/* 39 */ this.Exp_FetchedTrophyDifficulty,
		/* 40 */ this.Exp_FetchedTrophyImageURL,
		/* 41 */ this.Exp_FetchedTrophyAchieved,
		/* 42 */ this.Exp_RetrievedKeyData,
		/* 43 */ this.Exp_FetchedKeyCount,
		/* 44 */ this.Exp_FetchedKey,
		/* 45 */ this.Exp_UpdatedKeyData,
		/* 46 */ this.Exp_FetchedFriendCount,
		/* 47 */ this.Exp_FetchedFriend,
		/* 48 */ this.Exp_TimeYear,
		/* 49 */ this.Exp_TimeMonth,
		/* 50 */ this.Exp_TimeDay,
		/* 51 */ this.Exp_TimeHour,
		/* 52 */ this.Exp_TimeMinute,
		/* 53 */ this.Exp_TimeSecond,
		/* 54 */ this.Exp_TimeTimestamp,
		/* 55 */ this.Exp_TimeTimezone,
		/* 56 */ this.Exp_GetGameID,
		/* 57 */ this.Exp_GetPrivateKey,
		/* 58 */ this.Exp_GetRequestURL,
		/* 59 */ this.Exp_FetchedScoreTableID,
		/* 60 */ this.Exp_GetErrorMessage
	];
}
//
CRunGamejoltGameAPI.prototype = CServices.extend(new CRunExtension(), {
	/// <summary> Prototype definition </summary>
	/// <description> This class is a sub-class of CRunExtension, by the mean of the
	/// CServices.extend function which copies all the properties of
	/// the parent class to the new class when it is created.
	/// As all the necessary functions are defined in the parent class,
	/// you only need to keep the ones that you actually need in your code. </description>

	getNumberOfConditions: function() {
		/// <summary> Returns the number of conditions </summary>
		/// <returns type="Number" isInteger="true"> Warning, if this number is not correct, the application _will_ crash</returns>
		return 33; // $conditionFuncs not available yet
	},

	createRunObject: function(file, cob, version) {
		/// <summary> Creation of the Fusion extension. </summary>
		/// <param name="file"> A CFile object, pointing to the object's data zone </param>
		/// <param name="cob"> A CCreateObjectInfo containing infos about the created object</param>
		/// <param name="version"> version : the version number of the object, as defined in the C code </param>
		/// <returns type="Boolean"> Always false, as it is unused. </returns>

		// Use the "file" parameter to call the CFile object, and
		// gather the data of the object in the order as they were saved
		// (same order as the definition of the data in the EDITDATA structure
		// of the C code).
		// Please report to the documentation for more information on the CFile object

		if (this.ho == null) {
			throw "HeaderObject not defined when needed to be.";
		}

		// DarkEdif properties are accessible as on other platforms: IsPropChecked(), GetPropertyStr(), GetPropertyNum()
		let props = new darkEdif['Properties'](this, file, version);

		this.gameID = props['GetPropertyStr'](0);
		this.privateKey = props['GetPropertyStr'](1);
		this.gameAuthData = globalThis['darkEdif'].getGlobalData("AuthInfo");
		if (this.gameAuthData == null)
		{
			this.gameAuthData = new GameAuth("", "", "");
			globalThis['darkEdif'].setGlobalData("AuthInfo", this.gameAuthData);
		}

		// The return value is not used in this version of the runtime: always return false.
		return false;
	},

	handleRunObject: function()
	{
		while (this.triggerBuffer.length > 0)
		{
			this.latestResponse = this.triggerBuffer.shift();

			// Error Handling!
			if (this.latestResponse.hasError)
			{
				this.ho.generateEvent(32, 0); // Cnd_OnError
				continue;
			}

			if (this.latestResponse.hasTrigger)
				this.ho.generateEvent(this.latestResponse.trigger, 0);
			this.ho.generateEvent(31, 0); // Cnd_AnyCallFinished
		}
		return 0;
	},

	condition: function(num, cnd) {
		/// <summary> Called when a condition of this object is tested. </summary>
		/// <param name="num" type="Number" integer="true"> The number of the condition; 0+. </param>
		/// <param name="cnd" type="CCndExtension"> a CCndExtension object, allowing you to retreive the parameters
		//			of the condition. </param>
		/// <returns type="Boolean"> True if the condition is currently true. </returns>

		const func = this.$conditionFuncs[~~num];
		if (func == null) {
			throw "Unrecognised condition ID " + (~~num) + " passed to GamejoltGameAPI.";
		}

		// Note: New Direction parameter is not supported by this, add a workaround based on condition and parameter index;
		// SDL Joystick's source has an example.
		const args = new Array(func.length);
		for (let i = 0; i < args.length; ++i) {
			args[i] = cnd.getParamExpString(this.rh, i);
		}

		return func.apply(this, args);
	},
	action: function(num, act) {
		/// <summary> Called when an action of this object is executed </summary>
		/// <param name="num" type="Number"> The ID/number of the action, as defined by
		///		its array index. </param>
		/// <param name="act" type="CActExtension"> A CActExtension object, allowing you to
		///		retrieve the parameters of the action </param>

		const func = this.$actionFuncs[~~num];
		if (func == null) {
			throw "Unrecognised action ID " + (~~num) + " passed to GamejoltGameAPI.";
		}

		// Note: New Direction parameter is not supported by this, add a workaround based on action and parameter index;
		// SDL Joystick's source has an example.
		const args = new Array(func.length);
		for (let i = 0; i < args.length; ++i) {
			args[i] = act.getParamExpression(this.rh, i);
		}

		func.apply(this, args);
	},
	expression: function(num) {
		/// <summary> Called during the evaluation of an expression. </summary>
		/// <param name="num" type="Number"> The ID/number of the expression. </param>
		/// <returns> The result of the calculation, a number or a string </returns>

		// Note that it is important that your expression function asks for
		// each and every one of the parameters of the function, each time it is
		// called. The runtime will crash if you miss parameters.

		const func = this.$expressionFuncs[~~num];
		if (func == null) {
			throw "Unrecognised expression ID " + (~~num) + " passed to GamejoltGameAPI.";
		}

		const args = new Array(func.length);
		for (let i = 0; i < args.length; ++i) {
			args[i] = this.ho.getExpParam();
		}

		return func.apply(this, args);
	}

	// No comma for the last function : the Google compiler
	// we use for final projects does not accept it
});

class GameAuth
{
  constructor(userName, userToken, guestName)
  {
    this.userName = userName;
    this.userToken = userToken;
    this.guestName = guestName;
  }
}

class ResponseTicket
{
	constructor(url, type)
	{
		this.url = url;
		this.type = type;
		this.data = null;
		this.trigger = 0;
		this.hasTrigger = false;

		this.hasError = false;
		this.error = "";
  	}
}
