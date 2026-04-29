package Extensions.DarkEdif
{
	import Services.*;
	import Extensions.*;
    import flash.utils.Dictionary;

    public final class DarkEdif
    {
        private static var data:Dictionary = new Dictionary();
        private static var sdkVersion:int = 20;

        public function DarkEdif()
        {
            throw new Error("DarkEdif is a static class, you cannot initialize it!");
        }

        public static function getGlobalData(key:String):Object
        {
            key = key.toLowerCase();
            if (key in data)
            {
                return data[key];
            }
            return null;
        }

        public static function setGlobalData(key:String, value:Object):void
        {
            key = key.toLowerCase();
            data[key] = value;
        }
        
        // Could not implement getCurrentFusionEventNumber: evgLine is not in Flash

        public static function checkSupportsSDKVersion(sdkVer:int):void
        {
            if (sdkVer < 16 || sdkVer > 20)
            {
                throw new Error("Flash DarkEdif SDK does not support SDK version " + sdkVersion);
            }
        }

        public static function getProperties(ext:CRunExtension, edPtrFile:CBinaryFile, extVersion:int):DarkEdifProperties
        {
            return new DarkEdifProperties(ext, edPtrFile, extVersion);
        }
    }
}