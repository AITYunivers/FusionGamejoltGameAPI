package Extensions.DarkEdif
{
    import flash.utils.Endian;
    import flash.utils.ByteArray;

    public class DarkEdifPropSet
    {
        public var setIndicator:String;
        public var numRepeats:uint;
        public var lastSetJSONPropIndex:uint;
        public var firstSetJSONPropIndex:uint;
        public var setNameJSONPropIndex:uint;
        public var setName:String;
        
        private var rsDV:ByteArray;

        public function DarkEdifPropSet(rsDV:ByteArray)
        {
            rsDV.endian = Endian.LITTLE_ENDIAN;
            // Always 'S', compared with 'L' for non-set list.
            setIndicator = String.fromCharCode(rsDV.readUnsignedByte());
            // Number of repeats of this set, 1 is minimum and means one of this set
            numRepeats = rsDV.readUnsignedShort();
            // Property that ends this set's data, as a JSON index, inclusive
            lastSetJSONPropIndex = rsDV.readUnsignedShort();
            // First property that begins this set's data, as a JSON index
            firstSetJSONPropIndex = rsDV.readUnsignedShort();
            // Name property JSON index that will appear in list when switching set entry
            setNameJSONPropIndex = rsDV.readUnsignedShort();
            // Set name, as specified in JSON. Don't confuse with user-specified set name.
            setName = rsDV.readUTFBytes(rsDV.bytesAvailable);

            this.rsDV = rsDV;
        }

        public function getIndexSelected():uint
        {
            rsDV.position = 1 + (2 * 4);
            return rsDV.readUnsignedShort();
        }

        public function setIndexSelected(i:uint):void
        {
            rsDV.position = 1 + (2 * 4);
            rsDV.writeShort(i);
        }
    }
}