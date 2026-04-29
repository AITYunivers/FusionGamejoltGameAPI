package Extensions.DarkEdif
{
    import flash.utils.Endian;
    import flash.utils.ByteArray;

    public class DarkEdifProperty
    {
        public var index:uint;
        public var propTypeID:uint;
        public var propJSONIndex:uint;
        public var propName:String;
        public var propData:ByteArray;

        public function DarkEdifProperty(index:uint, propTypeID:uint, propJSONIndex:uint, propName:String, propData:ByteArray)
        {
            this.index = index;
            this.propTypeID = propTypeID;
            this.propJSONIndex = propJSONIndex;
            this.propName = propName;
            this.propData = propData;

            this.propData.endian = Endian.LITTLE_ENDIAN;
        }
    }
}